using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SimpleOtisAPI.Domain.DTOs
{
    public class WidgetDBDTO
    {
        public string User { get; set; }
        public string Country_Code { get; set; }
        public string Language_Code { get; set; }
    }
}
