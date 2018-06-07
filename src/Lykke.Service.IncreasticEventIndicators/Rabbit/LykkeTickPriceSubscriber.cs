using System;
using System.Threading.Tasks;
using Autofac;
using Common;
using Common.Log;
using Lykke.RabbitMqBroker;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.IncreasticEventIndicators.Core.Domain.Model;
using Lykke.Service.IncreasticEventIndicators.Core.Services.Exchanges;
using Lykke.Service.IncreasticEventIndicators.Settings.ServiceSettings;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Lykke.Service.IncreasticEventIndicators.Rabbit
{
    public class LykkeTickPriceSubscriber : IStartable, IStopable
    {
        private readonly ExchangeSettings _settings;
        private readonly ILykkeTickPriceHandler _handler;
        private readonly ILog _log;
        private RabbitMqSubscriber<TickPrice> _subscriber;

        public LykkeTickPriceSubscriber(
            IncreasticEventIndicatorsSettings settings,
            ILykkeTickPriceHandler handler,
            ILog log)
        {
            _settings = settings.TickPrice;
            _handler = handler;
            _log = log;
        }

        public void Start()
        {
            var settings = RabbitMqSubscriptionSettings.CreateForSubscriber(_settings.ConnectionString, _settings.Exchange, _settings.QueueSuffix);
            settings.IsDurable = false;
            settings.DeadLetterExchangeName = null;
            settings.ExchangeName = _settings.Exchange;

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
                await _log.WriteErrorAsync(nameof(LykkeTickPriceSubscriber), nameof(ProcessMessageAsync), $"tickPrice {tickPrice.ToJson()}", ex);
            }
        }

        public void Dispose()
        {
            _subscriber?.Dispose();
        }

        public void Stop()
        {
            _subscriber?.Stop();
        }
    }

    public class TickPrice : ITickPrice
    {
        private bool Equals(TickPrice other)
        {
            return string.Equals(Source, other.Source)
                   && string.Equals(Asset, other.Asset)
                   && Ask == other.Ask && Bid == other.Bid;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TickPrice)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Source != null ? Source.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Asset != null ? Asset.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Ask.GetHashCode();
                hashCode = (hashCode * 397) ^ Bid.GetHashCode();
                return hashCode;
            }
        }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("asset")]
        public string Asset { get; set; }

        [JsonProperty("timestamp")]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime Timestamp { get; set; }

        [JsonProperty("ask")]
        public decimal Ask { get; set; }

        [JsonProperty("bid")]
        public decimal Bid { get; set; }

        //public static TickPrice FromOrderBook(OrderBook orderBook)
        //{
        //    return new TickPrice
        //    {
        //        Source = orderBook.Source,
        //        Asset = orderBook.Asset,
        //        Timestamp = orderBook.Timestamp,
        //        Ask = orderBook.Asks.Min(x => x.Price),
        //        Bid = orderBook.Bids.Max(x => x.Price)
        //    };
        //}
    }
}
