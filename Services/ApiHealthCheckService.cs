public class ApiHealthCheckService : IHostedService, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ApiHealthCheckService> _logger;
    private Timer? _timer;

    public ApiHealthCheckService(HttpClient httpClient, ILogger<ApiHealthCheckService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(CheckApiHealth, null, TimeSpan.Zero, TimeSpan.FromMinutes(14)); // Execute a cada 14 minutos
        return Task.CompletedTask;
    }

    private async void CheckApiHealth(object? state)
    {
        try
        {
            _logger.LogInformation("Checking API health...");
            var response = await _httpClient.GetAsync($"{Environment.GetEnvironmentVariable("MY_API_URL")}/health");

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("API is healthy");
            }
            else
            {
                _logger.LogError($"API is unhealthy. Status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error checking API health: {ex.Message}");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}