using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using SimpleOtisAPI.Domain.DTOs;
using SimpleOtisAPI.Domain.Interfaces;
using SimpleOtisAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SimpleOtisAPI.Domain.Services
{
    public class DynamicMenu : IDynamicMenu
    {
        private readonly SimpleOtisAPIContext _context;
        private readonly IMemoryCache _cache;
        private readonly IDistributedCache _distributedCache;
        private readonly IConfiguration _configuration;
        private readonly TimeSpan _cacheExpire = TimeSpan.FromMinutes(5);
        public DynamicMenu(SimpleOtisAPIContext context, IMemoryCache cache, IDistributedCache distributedCache, IConfiguration configuration)
        {
            _context = context;
            _cache = cache;
            _distributedCache = distributedCache;
            _configuration = configuration;
        }
        public async Task<NavigationData> DynamicMenuDB(DynamicMenuDBDTO dynamic)
        {
            var navigationData = new NavigationData();
            var dynamicMenuData = new List<DynamicMenuModel>();

            var cacheKey = $"DynamicMenu - {dynamic.Country_Code}";

            if (bool.Parse(_configuration["RedisCacheOptions:RedisCacheAvailable"]))
            {
                //var cachedData = await _distributedCache.GetAsync(cacheKey); //To fetch the data in bytes from cache
                var cachedData = await _distributedCache.GetStringAsync(cacheKey); //To fetch the data as string value from cache

                if (cachedData != null)
                {
                    //var cachedStringData = Encoding.UTF8.GetString(cachedData); // If the data is in bytes then use this 
                    dynamicMenuData = JsonSerializer.Deserialize<List<DynamicMenuModel>>(cachedData) ?? new List<DynamicMenuModel>();

                    foreach (var item in dynamicMenuData)
                    {
                        if (item.NavigationType == 1)
                            navigationData.LeftNav.Add(item);
                        else
                            navigationData.Footer.Add(item);
                    }
                }
                else
                {
                    //If Not Found Fetch the Data from DataBase
                    dynamicMenuData = await _context.tblDynamicMenus.FromSql($"EXEC usp_GetDynamicMenu {dynamic.Country_Code}, {dynamic.Language_Code}").ToListAsync();

                    foreach (var item in dynamicMenuData)
                    {
                        if (item.NavigationType == 1)
                            navigationData.LeftNav.Add(item);
                        else
                            navigationData.Footer.Add(item);
                    }

                    //Serialize the data to be stored in bytes use this below 2 lines
                    //var cachedDataString = JsonSerializer.Serialize(dynamicMenuData); 
                    //var newDataCache = Encoding.UTF8.GetBytes(cachedDataString);

                    var newDataCache = JsonSerializer.Serialize(dynamicMenuData);

                    //Set cache options 
                    var options = new DistributedCacheEntryOptions()
                                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

                    await _distributedCache.SetStringAsync(cacheKey, newDataCache, options);
                }
            }
            else
            {

                //Using In-Memory Cache to cache the data
                if (!_cache.TryGetValue(cacheKey, out dynamicMenuData))
                {

                    dynamicMenuData = await _context.tblDynamicMenus.FromSql($"EXEC usp_GetDynamicMenu {dynamic.Country_Code}, {dynamic.Language_Code}").ToListAsync();

                    foreach (var item in dynamicMenuData)
                    {
                        if (item.NavigationType == 1)
                            navigationData?.LeftNav?.Add(item);
                        else
                            navigationData?.Footer?.Add(item);
                    }

                    _cache.Set(cacheKey, navigationData, _cacheExpire);
                }
            }

            return navigationData ?? new NavigationData();
        }
    }
}
