using TestD.Services.IServices;

namespace TestD.Services
{
    public class TimeServices : ITimeServices
    {
        private readonly ILogger<TimeServices> _logger;

        public TimeServices(ILogger<TimeServices> logger)
        {
            _logger = logger;
        }

        public void PrintNow()
        {
            _logger.LogInformation(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
        }
    }
}
