using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Radar.Placas.Listener.Domain.Interfaces.MessageConsumers;
using Radar.Placas.Listener.Domain.Interfaces.Requests;
using RabbitMQ.Client.Events;
using Radar.Placas.Listener.Domain.Enums;
using System.Text;
using System.Text.Json;
using Radar.Placas.Listener.Domain.Entities.DTOs;
using Radar.Placas.Listener.Domain.Entities.Responses;

namespace Radar.Placas.Listener.Infra.MessageConsumers
{
    public class DadosDeteccaoRadarMessageConsumer : IDadosDeteccaoRadarMessageConsumer
{
        private readonly string? _rabbitQueue;
        private readonly string? _routingKey;
        private readonly string? _rabbitExchange;
        private readonly bool _autoDelete;
        private readonly ISendDadosDeteccaoRadarRequest _sendDadosDeteccaoRadarRequest;
        private readonly ILogger<DadosDeteccaoRadarMessageConsumer> _logger;

        public ConnectionFactory _connectionFactory { get; set; }

        public DadosDeteccaoRadarMessageConsumer(
            IConfiguration configuration,
            ISendDadosDeteccaoRadarRequest sendDetectaRequest,
            ILogger<DadosDeteccaoRadarMessageConsumer> logger)
        {
            _logger = logger;

            _rabbitQueue = configuration.GetSection("RabbitMQConfig:QueueName").Value;
            _rabbitExchange = configuration.GetSection("RabbitMQConfig:Exchange").Value;
            _routingKey = configuration.GetSection("RabbitMQConfig:RoutingKey").Value;
            _autoDelete = Convert.ToBoolean(configuration.GetSection("RabbitMQConfig:QueueAutoAck").Value);

            _connectionFactory = new ConnectionFactory();
            _connectionFactory.Uri = new Uri(configuration.GetConnectionString("RabbitMQConnection"));

            _sendDadosDeteccaoRadarRequest = sendDetectaRequest;
        }

        public Task ProcessQueueAsync()
        {
            AsyncEventingBasicConsumer consumer = null;
            try
            {
                _connectionFactory.DispatchConsumersAsync = true;
                _connectionFactory.AutomaticRecoveryEnabled = true;

                var connection = _connectionFactory.CreateConnection();
                var model = connection.CreateModel();


                model.QueueBind(
                    queue: _rabbitQueue,
                    exchange: _rabbitExchange,
                    routingKey: _routingKey
                );

                consumer = new AsyncEventingBasicConsumer(model);

                consumer.Received += async (channel, eventArgs) =>
                {
                    try
                    {
                        string json = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

                        _logger.LogInformation($"Processando {json}");

                        var jsonDadosDeteccaoRadarDTO = JsonSerializer.Deserialize<DadosDeteccaoRadarDTO>(json) ?? throw new Exception("Falha ao tentar deserializar o objeto da fila.");

                        var dadosDeteccaoRadarResponse = new DadosDeteccaoRadarResponse(jsonDadosDeteccaoRadarDTO);

                        await _sendDadosDeteccaoRadarRequest.SendAsync(dadosDeteccaoRadarResponse);

                        model.BasicAck(eventArgs.DeliveryTag, true);
                    }
                    catch (Exception ex)
                    {
                        string message = "Ocorreu um erro durante a chamada do método Radar.Placas.Listener.Infra.Repositories.DadosDeteccaoRadarMessageConsumer.ProcessQueueAsync()";
                        StatusCode statusCode = StatusCode.RECURSO_EXTERNO_ERRO_FILA;

                        _logger.LogError($"StatusCode: {statusCode}, Message: {message}, StackTrace: {ex}");

                        model.BasicReject(eventArgs.DeliveryTag, false);
                    }
                };

                var props = model.CreateBasicProperties();

                string id = model.BasicConsume(
                    queue: _rabbitQueue,
                    autoAck: _autoDelete,
                    consumer: consumer
                );
            }
            catch (Exception ex)
            {
                string message = "Ocorreu um erro durante a chamada do método Radar.Placas.Listener.Infra.Repositories.DadosDeteccaoRadarMessageConsumer.ProcessQueueAsync()";
                StatusCode statusCode = StatusCode.RECURSO_EXTERNO_ERRO_FILA;

                _logger.LogError($"StatusCode: {statusCode}, Message: {message}, StackTrace: {ex}");
            }

            return Task.CompletedTask;
        }
    }
}