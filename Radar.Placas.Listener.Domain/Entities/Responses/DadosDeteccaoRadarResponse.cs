using Radar.Placas.Listener.Domain.Entities.DTOs;

namespace Radar.Placas.Listener.Domain.Entities.Responses
{
    /// <summary>
    /// Objeto processado que será enviado para a api ponte
    /// </summary>
    public class DadosDeteccaoRadarResponse
    {
        public DadosDeteccaoRadarResponse(DadosDeteccaoRadarDTO dadosDeteccaoRadarDTO)
        { 
            Id = dadosDeteccaoRadarDTO.Id;
            Placa = dadosDeteccaoRadarDTO.Placa ?? string.Empty;
            DataHora = dadosDeteccaoRadarDTO.DataHora;
            Camera = dadosDeteccaoRadarDTO.Camera ?? new Camera();
            IdRequisicao = dadosDeteccaoRadarDTO.IdRequisicao;
        }

        public long Id { get; private set; }
        public string Placa { get; private set; }
        public DateTime DataHora { get; private set; }
        public Camera Camera { get; private set; }
        public Guid IdRequisicao { get; private set; }
    }
}