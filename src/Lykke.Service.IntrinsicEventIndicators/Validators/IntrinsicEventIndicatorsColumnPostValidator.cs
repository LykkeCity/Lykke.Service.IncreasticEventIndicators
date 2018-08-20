using FluentValidation;
using JetBrains.Annotations;
using Lykke.Service.IntrinsicEventIndicators.Client.Models;

namespace Lykke.Service.IntrinsicEventIndicators.Validators
{
    [UsedImplicitly]
    public class IntrinsicEventIndicatorsColumnPostValidator : AbstractValidator<IntrinsicEventIndicatorsColumnPost>
    {
        public IntrinsicEventIndicatorsColumnPostValidator()
        {
            RuleFor(o => o.Delta)
                .GreaterThan(0)
                .WithMessage($"{nameof(IntrinsicEventIndicatorsColumnPost.Delta)} should be greater than zero");
        }
    }
}
