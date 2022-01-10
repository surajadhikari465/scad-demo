using Icon.Esb.Schemas.Wfm.Contracts;

namespace Icon.Services.ItemPublisher.Infrastructure.MessageQueue
{
    public interface ITraitMessageBuilder
    {
        TraitType BuildTrait(string traitCode, string traitDescription, bool value);

        TraitType BuildTrait(string traitCode, string traitDescription, bool? value);

        TraitType BuildTrait(string traitCode, string traitDescription, decimal? value);

        TraitType BuildTrait(string traitCode, string traitDescription, double? value);

        TraitType BuildTrait(string traitCode, string traitDescription, int? value);

        TraitType BuildTraitLeaveBlankIfNull(string traitCode, string traitDescription, bool? value);

        TraitType BuildTrait(string traitCode, string traitDescription, string value);

        TraitType BuildTrait(string traitCode, string traitDescription, string value, UomType uom);
    }
}