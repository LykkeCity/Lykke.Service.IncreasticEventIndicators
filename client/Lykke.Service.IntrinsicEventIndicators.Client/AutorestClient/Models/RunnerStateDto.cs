// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Service.IntrinsicEventIndicators.Client.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class RunnerStateDto
    {
        /// <summary>
        /// Initializes a new instance of the RunnerStateDto class.
        /// </summary>
        public RunnerStateDto()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the RunnerStateDto class.
        /// </summary>
        /// <param name="expectedDirectionalChange">Possible values include:
        /// 'Upward', 'Downward'</param>
        public RunnerStateDto(int eventProperty, double extreme, double expectedDcLevel, double expectedOsLevel, double reference, ExpectedDirectionalChange expectedDirectionalChange, double directionalChangePrice, double delta, string assetPair = default(string), string exchange = default(string))
        {
            EventProperty = eventProperty;
            Extreme = extreme;
            ExpectedDcLevel = expectedDcLevel;
            ExpectedOsLevel = expectedOsLevel;
            Reference = reference;
            ExpectedDirectionalChange = expectedDirectionalChange;
            DirectionalChangePrice = directionalChangePrice;
            Delta = delta;
            AssetPair = assetPair;
            Exchange = exchange;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Event")]
        public int EventProperty { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Extreme")]
        public double Extreme { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ExpectedDcLevel")]
        public double ExpectedDcLevel { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ExpectedOsLevel")]
        public double ExpectedOsLevel { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Reference")]
        public double Reference { get; set; }

        /// <summary>
        /// Gets or sets possible values include: 'Upward', 'Downward'
        /// </summary>
        [JsonProperty(PropertyName = "ExpectedDirectionalChange")]
        public ExpectedDirectionalChange ExpectedDirectionalChange { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "DirectionalChangePrice")]
        public double DirectionalChangePrice { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Delta")]
        public double Delta { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "AssetPair")]
        public string AssetPair { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Exchange")]
        public string Exchange { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
        }
    }
}
