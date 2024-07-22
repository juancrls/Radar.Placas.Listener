using Microsoft.Extensions.Logging;
using Radar.Placas.Listener.Domain.Enums;
using Radar.Placas.Listener.Domain.Interfaces.MessageConsumers;
using Radar.Placas.Listener.Domain.Interfaces.Services;

namespace Radar.Placas.Listener.App.Services
{
    public class DadosDeteccaoRadarService : IDadosDeteccaoRadarService
    {
        private readonly ILogger<DadosDeteccaoRadarService> _logger;
        private readonly IDadosDeteccaoRadarMessageConsumer _placaMessageConsumer;

        public DadosDeteccaoRadarService(IDadosDeteccaoRadarMessageConsumer placaMessageConsumer, ILogger<DadosDeteccaoRadarService> logger) 
        {
            _placaMessageConsumer = placaMessageConsumer;
            _logger = logger;
        }

        public async Task ProcessAsync()
        {
            try
            {
                await _placaMessageConsumer.ProcessQueueAsync();
            }
            catch (Exception ex)
            {
                string message = "Ocorreu um erro durante a chamada do método Radar.Placas.Listener.App.Services.DadosDeteccaoRadarService.ProcessAsync()";
                StatusCode statusCode = StatusCode.ERRO_GERAL;

                _logger.LogError($"StatusCode: {statusCode}, Message: {message}, StackTrace: {ex}");
            }
        }
    }
}