using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
using Radar.Placas.Listener.Domain.Enums;
using Radar.Placas.Listener.Domain.Interfaces.Requests;
using System.Text.Json;
using Radar.Placas.Listener.Domain.Entities.Responses;

namespace Radar.Placas.Listener.Infra.Requests
{
    public class SendDadosDeteccaoRadarRequest : ISendDadosDeteccaoRadarRequest
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SendDadosDeteccaoRadarRequest> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string? _urlPonte;

        public SendDadosDeteccaoRadarRequest(
            IConfiguration configuration,
            ILogger<SendDadosDeteccaoRadarRequest> logger,
            IHttpClientFactory httpClientFactory
            )
        {
            _configuration = configuration;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _urlPonte = _configuration.GetSection("Url:Ponte").Value;
        }
        public async Task SendAsync(DadosDeteccaoRadarResponse dadosDeteccaoRadarResponse)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var bodyRequest = new StringContent(
                                        JsonSerializer.Serialize(dadosDeteccaoRadarResponse),
                                        Encoding.UTF8,
                                        Application.Json);


                var httpResponseMessage = await httpClient.PostAsync($"{_urlPonte}", bodyRequest);

                if (!httpResponseMessage.IsSuccessStatusCode)
                    throw new Exception("API não retornou statusCode 200 ao tentar enviar o bodyRequest");
            }
            catch (Exception ex)
            {
                string message = $"Ocorreu um erro durante a chamada do método Radar.Placas.Listener.Infra.Requests.SendDadosDeteccaoRadarRequest.SendAsync()";
                StatusCode statusCode = StatusCode.RECURSO_EXTERNO_ERRO_WEBSERVICE;

                _logger.LogError($"StatusCode: {statusCode}, Message: {message}, StackTrace: {ex}");
                throw;
            }
        }
    }
}