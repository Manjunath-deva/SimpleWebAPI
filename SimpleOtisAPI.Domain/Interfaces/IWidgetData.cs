using SimpleOtisAPI.Domain.DTOs;
using SimpleOtisAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOtisAPI.Domain.Interfaces
{
    public interface IWidgetData
    {
        Task<List<tblWidget>> WidgetDBData(WidgetDBDTO widget);
    }
}
