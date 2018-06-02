namespace Lykke.Service.IncreasticEventIndicators.Core.Domain
{
    public interface IRunnerState
    {
        int Event { get; }
        decimal Extreme { get; }
        decimal ExpectedDcLevel { get; }
        decimal ExpectedOsLevel { get; }
        decimal Reference { get; }
        ExpectedDirectionalChange ExpectedDirectionalChange { get; }
        decimal DirectionalChangePrice { get; }
        decimal Delta { get; }
        string AssetPair { get; }
    }
}
