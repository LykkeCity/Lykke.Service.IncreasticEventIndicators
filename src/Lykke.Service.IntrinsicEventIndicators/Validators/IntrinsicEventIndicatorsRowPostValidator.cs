using System;
using FluentValidation;
using JetBrains.Annotations;
using Lykke.Service.IntrinsicEventIndicators.Client.Models;

namespace Lykke.Service.IntrinsicEventIndicators.Validators
{
    [UsedImplicitly]
    public class IntrinsicEventIndicatorsRowPostValidator : AbstractValidator<IntrinsicEventIndicatorsRowPost>
    {
        public IntrinsicEventIndicatorsRowPostValidator()
        {
            ApplyDefault();

            RuleSet("lykke", () =>
            {
                ApplyDefault();

                RuleFor(o => o.Exchange)
                    .Equal("lykke", StringComparer.InvariantCultureIgnoreCase)
                    .WithMessage($"{nameof(IntrinsicEventIndicatorsRowPost.Exchange)} must be 'lykke'");
            });
        }

        private void ApplyDefault()
        {
            RuleFor(o => o.Exchange)
                .NotEmpty()
                .WithMessage($"{nameof(IntrinsicEventIndicatorsRowPost.Exchange)} required");

            RuleFor(o => o.AssetPair)
                .NotEmpty()
                .WithMessage($"{nameof(IntrinsicEventIndicatorsRowPost.AssetPair)} required");

            RuleFor(o => o.PairName)
                .NotEmpty()
                .WithMessage($"{nameof(IntrinsicEventIndicatorsRowPost.PairName)} required");
        }
    }
}
