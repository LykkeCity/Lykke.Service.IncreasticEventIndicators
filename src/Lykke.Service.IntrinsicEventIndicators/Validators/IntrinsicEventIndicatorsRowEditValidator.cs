using FluentValidation;
using JetBrains.Annotations;
using Lykke.Service.IntrinsicEventIndicators.Client.Models;

namespace Lykke.Service.IntrinsicEventIndicators.Validators
{
    [UsedImplicitly]
    public class IntrinsicEventIndicatorsRowEditValidator : AbstractValidator<IntrinsicEventIndicatorsRowEdit>
    {
        public IntrinsicEventIndicatorsRowEditValidator()
        {
            RuleFor(o => o.RowId)
                .NotEmpty()
                .WithMessage($"{nameof(IntrinsicEventIndicatorsRowEdit.RowId)} required");

            RuleFor(o => o.PairName)
                .NotEmpty()
                .WithMessage($"{nameof(IntrinsicEventIndicatorsRowEdit.PairName)} required");
        }
    }
}
