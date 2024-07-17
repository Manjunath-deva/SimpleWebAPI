using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using SimpleOtisAPI.Domain.DTOs;
using SimpleOtisAPI.Domain.Interfaces;
using SimpleOtisAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOtisAPI.Domain.Services
{
    public class DynamicMenu : IDynamicMenu
    {
        private readonly SimpleOtisAPIContext _context;
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _cacheExpire = TimeSpan.FromMinutes(5);
        public DynamicMenu(SimpleOtisAPIContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }
        public async Task<NavigationData> DynamicMenuDB(DynamicMenuDBDTO dynamic)
        {
            NavigationData navigationData = new();

            var cacheKey = $"DynamicMenu - {dynamic.Country_Code}";
            if (!_cache.TryGetValue(cacheKey, out List<DynamicMenuModel>? dynamicMenuData))
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

            return navigationData ?? new NavigationData();
        }
    }
}
