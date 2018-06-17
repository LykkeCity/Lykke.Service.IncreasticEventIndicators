// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

using Newtonsoft.Json;

namespace Lykke.Service.IntrinsicEventIndicators.Client.AutorestClient.Models
{
    public partial class IntrinsicEventIndicatorsColumnDto
    {
        /// <summary>
        /// Initializes a new instance of the IntrinsicEventIndicatorsColumnDto
        /// class.
        /// </summary>
        public IntrinsicEventIndicatorsColumnDto()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the IntrinsicEventIndicatorsColumnDto
        /// class.
        /// </summary>
        public IntrinsicEventIndicatorsColumnDto(double delta, string columnId = default(string))
        {
            ColumnId = columnId;
            Delta = delta;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ColumnId")]
        public string ColumnId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Delta")]
        public double Delta { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            //Nothing to validate
        }
    }
}
