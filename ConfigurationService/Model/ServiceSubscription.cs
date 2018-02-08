namespace ConfigurationService.Model
{
    public class ServiceSubscription
    {
        public string ServiceName { get; set; }
        public string BaseUrl { get; set; }
        public string HealthCheckRoute { get; set; }
    }
}
