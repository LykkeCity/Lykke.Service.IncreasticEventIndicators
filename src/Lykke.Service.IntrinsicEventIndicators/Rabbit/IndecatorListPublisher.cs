using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Autofac;
using Common;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker.Publisher;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.IntrinsicEventIndicators.Core.Services.LyciAssets;

namespace Lykke.Service.IntrinsicEventIndicators.Rabbit
{
    public class IndecatorListPublisher : IIndecatorListSender, IStartable, IStopable
    {
        private readonly string _rabbitConnectionString;
        private readonly ILogFactory _logFactory;
        private RabbitMqPublisher<IndecatorList> _publisher;

        public IndecatorListPublisher(string rabbitConnectionString, ILogFactory logFactory)
        {
            _rabbitConnectionString = rabbitConnectionString;
            _logFactory = logFactory;
        }

        public async Task SendAsync(IndecatorList message)
        {
            await _publisher.ProduceAsync(message);
        }

        public void Start()
        {
            var settings = RabbitMqSubscriptionSettings.CreateForPublisher(_rabbitConnectionString, "lyci.assets.intrinsicEventIndicators");

            _publisher = new RabbitMqPublisher<IndecatorList>(_logFactory, settings)
                .SetSerializer(new JsonMessageSerializer<IndecatorList>())
                .DisableInMemoryQueuePersistence()
                .Start();
        }

        public void Dispose()
        {
            _publisher?.Dispose();
        }

        public void Stop()
        {
            _publisher?.Stop();
        }
    }
}
