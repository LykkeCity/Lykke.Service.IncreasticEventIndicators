using System;

namespace Lykke.Service.IntrinsicEventIndicators.Core.Domain
{
    public interface IMatrixHistory
    {
        string Data { get; }
        DateTime Created { get; }
    }

    public class MatrixHistory : IMatrixHistory
    {
        public string Data { get; set; }
        public DateTime Created { get; set; }
    }
}
