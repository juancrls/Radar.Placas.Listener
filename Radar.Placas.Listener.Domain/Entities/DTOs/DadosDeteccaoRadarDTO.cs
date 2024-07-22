using System.Text.Json.Serialization;

namespace Radar.Placas.Listener.Domain.Entities.DTOs
{
    /// <summary>
    /// Objeto que vai vir da fila do Rabbit (dados brutos).
    /// </summary>
    public class DadosDeteccaoRadarDTO
    {
        [JsonPropertyName("Id")]
        public long Id { get; set; } = 0;

        [JsonPropertyName("Placa")]
        public string Placa { get; set; } = string.Empty;

        [JsonPropertyName("DataHora")]
        public DateTime DataHora { get; set; } = new DateTime();

        [JsonPropertyName("Camera")]
        public Camera Camera { get; set; } = new();

        [JsonPropertyName("IdRequisicao")]
        public Guid IdRequisicao { get; set; } = Guid.Empty;
    }
}