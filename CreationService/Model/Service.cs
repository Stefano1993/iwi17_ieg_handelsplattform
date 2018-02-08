namespace CreationService.Model
{
    public class Service
    {
        public string ServiceName { get; set; }
        public string BaseUrl { get; set; }
        public string HealthCheckRoute { get; set; }
        public bool Healthy { get; set; }
    }
}
