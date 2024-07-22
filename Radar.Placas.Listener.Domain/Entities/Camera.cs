using System.Text.Json.Serialization;

namespace Radar.Placas.Listener.Domain.Entities
{
    public class Camera
    {
        [JsonPropertyName("Numero")]
        public long Numero { get; set; } = 0;

        [JsonPropertyName("Latitude")]
        public double Latitude { get; set; } = 0;

        [JsonPropertyName("Longitude")]
        public double Longitude { get; set; } = 0;

        [JsonPropertyName("Id")]
        public int Id { get; set; } = 0;

        [JsonPropertyName("Raio")]
        public int Raio { get; set; } = 0;

        [JsonPropertyName("DescricaoLocalizacao")]
        public string DescricaoLocalizacao { get; set; } = string.Empty;

        [JsonPropertyName("CodigoLocalizacao")]
        public int CodigoLocalizacao { get; set; } = 0;

        [JsonPropertyName("Sistema")]
        public Sistema Sistema { get; set; } = new();

        [JsonPropertyName("DataInclusao")]
        public DateTime DataInclusao { get; set; } = new();

        [JsonPropertyName("Situacao")]
        public char Situacao { get; set; } = new();
    }
}