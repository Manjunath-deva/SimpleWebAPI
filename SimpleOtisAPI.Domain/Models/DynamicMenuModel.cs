using Azure.Core.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOtisAPI.Domain.Models
{
    public class DynamicMenuModel
    {
        public string Name { get; set; }
        public string Application { get; set; }
        public int Order { get; set; }
        public string BasePath { get; set; }
        public string RelativePath { get; set; }
        public string DisplayName { get; set; }
        public int NavigationType { get; set; }
    }

    public class NavigationData
    {
        public List<DynamicMenuModel> LeftNav { get; set; } = new List<DynamicMenuModel>();
        public List<DynamicMenuModel> Footer { get; set; } = new List<DynamicMenuModel>();
    }

}