using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwsWorkshop.Application.Settings
{
    public class ServiceApiSettings
    {
        public ApiClient TenantApi { get; set; }
        public ApiClient GeoLocationApi { get; set; }
        public ApiClient SharedApi { get; set; }
        public ApiClient SearchApi { get; set; }
        public ApiClient TransmitterApi { get; set; }
    }

    public class ApiClient
    {
        public string Name { get; set; }
        public string NameAuthorize { get; set; }
        public string Uri { get; set; }
    }
}
