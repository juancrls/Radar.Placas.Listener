using Radar.Placas.Listener.Domain.Entities.Responses;

namespace Radar.Placas.Listener.Domain.Interfaces.Requests
{
    public interface ISendDadosDeteccaoRadarRequest
    {
        public Task SendAsync(DadosDeteccaoRadarResponse placaResponse);
    }
}
