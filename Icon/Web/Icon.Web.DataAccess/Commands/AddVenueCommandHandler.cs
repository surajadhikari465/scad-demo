using Icon.Common.DataAccess;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class AddVenueCommandHandler : ICommandHandler<AddVenueCommand>
    {
        private IconContext context;

        public AddVenueCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(AddVenueCommand data)
        {
            // Verify store name is unique.
            if (context.Locale.Any(l => l.localeName.ToLower() == data.LocaleName.ToLower() && l.parentLocaleID == data.ParentLocaleId))
            {
                throw new ArgumentException(String.Format("{0}: This venue name is already in use for this store.", data.LocaleName));
            }

            if (!string.IsNullOrEmpty(data.VenueCode) && context.LocaleTrait
              .Any(lt => lt.Trait.traitCode == TraitCodes.VenueCode && lt.traitValue == data.VenueCode))
            {
                throw new ArgumentException(String.Format("{0}: This Venue Code is already in use.", data.VenueCode));
            }

            var locale = new Locale
            {
                localeName = data.LocaleName,
                localeTypeID = data.LocaleTypeId,
                parentLocaleID = data.ParentLocaleId,
                localeOpenDate = data.OpenDate,
                ownerOrgPartyID = data.OwnerOrgPartyId,
                LocaleTrait = new List<LocaleTrait>
                {
                
                    new LocaleTrait { traitID = Traits.ModifiedUser, traitValue = data.UserName },
                    new LocaleTrait { traitID = Traits.TouchPointGroupId, traitValue = data.TouchPointGroupId },
                }
            };

            AddTraitValue(TraitCodes.LocaleSubtype, locale, data.LocaleSubType);

            if (!string.IsNullOrWhiteSpace(data.VenueCode))
            {
                AddTraitValue(TraitCodes.VenueCode, locale, data.VenueCode);
            }

            if (!string.IsNullOrWhiteSpace(data.VenueOccupant))
            {
                AddTraitValue(TraitCodes.VenueOccupant, locale, data.VenueOccupant);
            }

            context.Locale.Add(locale);
            context.SaveChanges();

            // Set 'output' parameter for Manager to consume.
            data.LocaleId = locale.localeID;
        }

        private void AddTraitValue(string traitCode, Locale locale, string traitValue)
        {

            locale.LocaleTrait.Add(new LocaleTrait
                {
                    traitID = context.Trait.First(t => t.traitCode == traitCode).traitID,
                    traitValue = traitValue,
                    Trait = context.Trait.First(t => t.traitCode == traitCode)
                });
            }
    
    }
}