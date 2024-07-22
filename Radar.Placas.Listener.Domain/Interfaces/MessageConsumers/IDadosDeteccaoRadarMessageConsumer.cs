namespace Radar.Placas.Listener.Domain.Interfaces.MessageConsumers
{
    public interface IDadosDeteccaoRadarMessageConsumer
    {
        public Task ProcessQueueAsync();
    }
}