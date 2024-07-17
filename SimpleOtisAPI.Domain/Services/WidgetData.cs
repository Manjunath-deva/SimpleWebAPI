using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using SimpleOtisAPI.Domain.DTOs;
using SimpleOtisAPI.Domain.Interfaces;
using SimpleOtisAPI.Domain.Models;
using System.Linq;


namespace SimpleOtisAPI.Domain.Services
{
    public class WidgetData : IWidgetData
    {
        private readonly SimpleOtisAPIContext _context;
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _cacheExpire = TimeSpan.FromMinutes(3);
        public WidgetData(SimpleOtisAPIContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }
        public async Task<List<tblWidget>> WidgetDBData(WidgetDBDTO widget)
        {
            //var widgetDetails = context.tblWidgets.FromSql($"SELECT Widget_Id, Emergency_Callbacks, Non_Emergency_Callbacks, Total_Callbacks, CD.Country_Code, LD.Language_Code, w.Country_Id, w.Language_Id FROM tblWidget w INNER JOIN tblCountryDetails CD ON w.Country_Id = CD.Country_Id INNER JOIN tblLanguageDetails LD ON w.Language_Id = LD.Language_Id WHERE CD.Country_Code = {widget.Country_Code} AND LD.Language_Code = {widget.Language_Code}");

            var cacheKey = $"WidgetData - {widget.User} - {widget.Country_Code}";

            if(!_cache.TryGetValue(cacheKey, out List<tblWidget>? widgetDetails))
            {
                widgetDetails = await _context.tblWidgets.FromSql($"EXEC usp_GetWidgetData {widget.User}, {widget.Country_Code}, {widget.Language_Code}").ToListAsync();
                _cache.Set(cacheKey, widgetDetails, _cacheExpire);
            }

            return widgetDetails ?? new List<tblWidget>();
        }
    }
}
