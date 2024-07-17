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
using System.Text.Json;
using System.Threading.Tasks;

namespace SimpleOtisAPI.Domain.Services
{
    public class UserPreferenceData : IUserPreference
    {
        private readonly SimpleOtisAPIContext _context;
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _cacheExpire = TimeSpan.FromMinutes(5);
        public UserPreferenceData(SimpleOtisAPIContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }
        public async Task<List<tblPreference>> UserPreference(UserPreferenceDBDTO userPreference)
        {
            var cacheKey = $"UserPreference - {userPreference.User_Id}";

            if (!_cache.TryGetValue(cacheKey, out List<tblPreference>? userPreferences))
            {
                userPreferences = await _context.tblPreferences.FromSql($"EXEC usp_GetUserPreferences {userPreference.User_Id}").ToListAsync();

                userPreferences[0].LanguageList = JsonSerializer.Deserialize<List<LanguageList>>(userPreferences[0].Language_List);

                _cache.Set(cacheKey, userPreferences, _cacheExpire);
            }

            return userPreferences ?? new List<tblPreference>();
        }
    }
}
