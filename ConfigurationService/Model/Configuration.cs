using System.Collections.Generic;

namespace ConfigurationService.Model
{
    public class Configuration
    {
        public string ServiceName { get; set; }
        public Dictionary<string, string> Configurations { get; set; }
    }
}
