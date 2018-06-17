// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Service.IntrinsicEventIndicators.Client.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public partial class IntrinsicEventIndicatorsDto
    {
        /// <summary>
        /// Initializes a new instance of the IntrinsicEventIndicatorsDto
        /// class.
        /// </summary>
        public IntrinsicEventIndicatorsDto()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the IntrinsicEventIndicatorsDto
        /// class.
        /// </summary>
        public IntrinsicEventIndicatorsDto(IList<IntrinsicEventIndicatorsColumnDto> columns = default(IList<IntrinsicEventIndicatorsColumnDto>), IList<IntrinsicEventIndicatorsRowDto> rows = default(IList<IntrinsicEventIndicatorsRowDto>), IList<IList<double?>> data = default(IList<IList<double?>>))
        {
            Columns = columns;
            Rows = rows;
            Data = data;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Columns")]
        public IList<IntrinsicEventIndicatorsColumnDto> Columns { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Rows")]
        public IList<IntrinsicEventIndicatorsRowDto> Rows { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Data")]
        public IList<IList<double?>> Data { get; set; }

    }
}