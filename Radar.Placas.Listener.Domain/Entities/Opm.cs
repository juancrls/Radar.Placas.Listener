using System.Text.Json.Serialization;

namespace Radar.Placas.Listener.Domain.Entities
{
    public class Opm
    {
        [JsonPropertyName("Codigo")]
        public long Codigo { get; set; } = 0;

        [JsonPropertyName("Descricao")]
        public string Descricao { get; set; } = string.Empty;
    }
}