using System;
using System.Threading.Tasks;
using Autofac;
using Common;
using Common.Log;
using Lykke.Common.ExchangeAdapter.Contracts;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.IntrinsicEventIndicators.Core.Services.Exchanges;
using Lykke.Service.IntrinsicEventIndicators.Settings.ServiceSettings;

namespace Lykke.Service.IntrinsicEventIndicators.Rabbit
{
    public abstract class TickPriceSubscriber : IStartable, IStopable
    {
        private readonly TickPriceExchangeSettings _settings;
        private readonly ITickPriceHandler _handler;
        private readonly ILog _log;
        private RabbitMqSubscriber<TickPrice> _subscriber;

        protected TickPriceSubscriber(
            TickPriceExchangeSettings settings,
            ITickPriceHandler handler,
            ILogFactory logFactory)
        {
            _settings = settings;
            _handler = handler;
            _log = logFactory.CreateLog(this);
        }

        public void Start()
        {
            var settings = RabbitMqSubscriptionSettings.CreateForSubscriber(_settings.Exchange.ConnectionString, _settings.Exchange.Exchange, _settings.Exchange.QueueSuffix);
            settings.IsDurable = false;
            settings.DeadLetterExchangeName = null;
            settings.ExchangeName = _settings.Exchange.Exchange;

            _subscriber = new RabbitMqSubscriber<TickPrice>(settings,
                    new ResilientErrorHandlingStrategy(_log, settings, TimeSpan.FromSeconds(10)))
                .SetMessageDeserializer(new JsonMessageDeserializer<TickPrice>())
                .SetMessageReadStrategy(new MessageReadQueueStrategy())
                .Subscribe(ProcessMessageAsync)
                .CreateDefaultBinding()
                .SetLogger(_log)
                .Start();
        }

        private async Task ProcessMessageAsync(TickPrice tickPrice)
        {
            try
            {
                await _handler.Handle(tickPrice);
            }
            catch (Exception ex)
            {
                await _log.WriteErrorAsync(nameof(TickPriceSubscriber), nameof(ProcessMessageAsync), $"tickPrice {tickPrice.ToJson()}", ex);
            }
        }        

        public void Stop()
        {
            _subscriber?.Stop();
        }

        public void Dispose()
        {
            _subscriber?.Dispose();
        }
    }
}
