using Icon.Esb.Schemas.Wfm.Contracts;
using System.Linq;

namespace Icon.Services.ItemPublisher.Infrastructure.MessageQueue
{
    /// <summary>
    /// TraitMessageBuilder handles translating traits into the ESB TraitType class
    /// </summary>
    public class TraitMessageBuilder : ITraitMessageBuilder
    {
        /// <summary>
        /// Build traits for a boolean value
        /// </summary>
        public TraitType BuildTrait(string traitCode, string traitDescription, bool value)
        {
            return BuildTrait(traitCode, traitDescription, value ? "1" : "0");
        }

        /// <summary>
        /// Build traits for a nullable boolean value
        /// </summary>
        public TraitType BuildTrait(string traitCode, string traitDescription, bool? value)
        {
            return BuildTrait(traitCode, traitDescription, value.GetValueOrDefault(false));
        }

        /// <summary>
        /// Build traits for a nullable boolean value and return a blank string if it's null
        /// </summary>
        public TraitType BuildTraitLeaveBlankIfNull(string traitCode, string traitDescription, bool? value)
        {
            return BuildTrait(traitCode,
                traitDescription,
                value.HasValue
                    ? value.Value
                        ? "1" : "0"
                    : string.Empty);
        }

        /// <summary>
        /// Build traits for a nullable int value
        /// </summary>
        public TraitType BuildTrait(string traitCode, string traitDescription, int? value)
        {
            return BuildTrait(traitCode, traitDescription, value?.ToString());
        }

        /// <summary>
        /// Build traits for a nullable double value
        /// </summary>
        /// </summary>
        public TraitType BuildTrait(string traitCode, string traitDescription, double? value)
        {
            return BuildTrait(traitCode, traitDescription, value?.ToString());
        }

        /// <summary>
        /// Build traits for a nullable decimal value
        /// </summary>
        public TraitType BuildTrait(string traitCode, string traitDescription, decimal? value)
        {
            return BuildTrait(traitCode, traitDescription, value?.ToString());
        }

        /// <summary>
        /// Build traits for a value and UOM
        /// </summary>
        public TraitType BuildTrait(string traitCode, string traitDescription, string value, UomType uom)
        {
            var trait = BuildTrait(traitCode, traitDescription, value);
            trait.type.value.FirstOrDefault().uom = uom;
            return trait;
        }

        /// <summary>
        /// Build traits for a string
        /// </summary>
        public TraitType BuildTrait(string traitCode, string traitDescription, string value)
        {
            var trait = new TraitType
            {
                code = traitCode,
                type = new TraitTypeType
                {
                    description = traitDescription,
                    value = new TraitValueType[]
                    {
                        new TraitValueType
                        {
                            value = value ?? string.Empty,
                        }
                    }
                }
            };
            return trait;
        }
    }
}