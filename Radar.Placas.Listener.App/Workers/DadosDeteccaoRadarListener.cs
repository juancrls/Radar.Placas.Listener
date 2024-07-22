using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Radar.Placas.Listener.Domain.Enums;
using Radar.Placas.Listener.Domain.Interfaces.Services;

namespace Radar.Placas.Listener.App.Workers
{
    public class DadosDeteccaoRadarListener : BackgroundService, IHostedService
    {
        private readonly ILogger<DadosDeteccaoRadarListener> _logger;
        private readonly IDadosDeteccaoRadarService _occurrenceService;

        public DadosDeteccaoRadarListener(ILogger<DadosDeteccaoRadarListener> logger, IDadosDeteccaoRadarService occurrenceService)
        {
            _logger = logger;
            _occurrenceService = occurrenceService;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"PlacaListener rodando em: {DateTimeOffset.Now}");

            try
            {
                await _occurrenceService.ProcessAsync();
                _logger.LogInformation("Serviço iniciado com sucesso!");
            }
            catch (Exception ex)
            {
                string message = $"Ocorreu um erro durante a chamada do método Radar.Placas.Listener.App.Workers.DadosDeteccaoRadarListener.ExecuteAsync()";
                StatusCode statusCode = StatusCode.ERRO_INICIALIZACAO;

                _logger.LogError($"StatusCode: {statusCode}, Message: {message}, StackTrace: {ex}");
            }
        }
    }
}