using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SimpleOtisWebAPI.Models;

public partial class Widget
{
    [JsonIgnore]
    public int WidgetId { get; set; }

    public int EmergencyCallbacks { get; set; }

    public int NonEmergencyCallbacks { get; set; }

    public int TotalCallbacks { get; set; }

    public string? Country { get; set; }

    public string? LanguageId { get; set; }
}
