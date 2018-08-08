using Icon.Common.DataAccess;
using Icon.Framework;
using System;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class UpdateVenueCommandHandler : ICommandHandler<UpdateVenueCommand>
    {
        private IconContext context;

        public UpdateVenueCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(UpdateVenueCommand data)
        {
            Locale existingLocale = context.Locale
                .Include(l => l.LocaleTrait.Select(lt => lt.Trait))
                .Single(l => l.localeID == data.LocaleId);

            ThrowExceptionIfVenueDataValidationFails(data, existingLocale);

            existingLocale.localeName = data.LocaleName;
            existingLocale.localeOpenDate = data.OpenDate;
            existingLocale.localeCloseDate = data.CloseDate;

            AddOrUpdateTraitValue(TraitCodes.ModifiedUser, existingLocale, data.UserName);
            AddOrUpdateTraitValue(TraitCodes.LocaleSubtype, existingLocale, data.LocaleSubType);

            if (!string.IsNullOrWhiteSpace(data.VenueCode))
            {
                AddOrUpdateTraitValue(TraitCodes.VenueCode, existingLocale, data.VenueCode);
            }

            if (!string.IsNullOrWhiteSpace(data.VenueOccupant))
            {
                AddOrUpdateTraitValue(TraitCodes.VenueOccupant, existingLocale, data.VenueOccupant);
            }

            context.SaveChanges();
        }

        private void ThrowExceptionIfVenueDataValidationFails(UpdateVenueCommand data, Locale existingLocale)
        {
            if (context.Locale
                .Where(l => l.localeID != existingLocale.localeID && l.parentLocaleID == data.ParentLocaleId) 
                .Any(l => l.localeName.ToLower() == data.LocaleName.ToLower()))
            {
                string parantLocaleName = context.Locale.Where(l => l.localeID == data.ParentLocaleId).Select(lc => lc.localeName).FirstOrDefault().ToString();
                throw new ArgumentException(String.Format("{0}: This venue name is already in use for store: {1}.", data.LocaleName, parantLocaleName));
            }

            if (!string.IsNullOrEmpty(data.VenueCode) && context.LocaleTrait
               .Where(lt => lt.localeID != existingLocale.localeID)
               .Any(lt => lt.Trait.traitCode == TraitCodes.VenueCode && lt.traitValue == data.VenueCode))
            {
                throw new ArgumentException(String.Format("{0}: This Venue Code is already in use for this Region.", data.VenueCode));
            }

            if (data.CloseDate.HasValue && data.CloseDate < data.OpenDate) 
            {
                throw new ArgumentException(String.Format("Close Date should be after Open Date for Venue: {0}.", data.LocaleName));
            }

        }

        private void AddOrUpdateTraitValue(string traitCode, Locale existingLocale, string traitValue)
        {
            LocaleTrait trait = existingLocale.LocaleTrait.SingleOrDefault(lt => lt.Trait.traitCode == traitCode);

            if (trait == null)
            {
                existingLocale.LocaleTrait.Add(new LocaleTrait
                {
                    localeID = existingLocale.localeID,
                    traitID = context.Trait.First(t => t.traitCode == traitCode).traitID,
                    traitValue = traitValue,
                    Trait = context.Trait.First(t => t.traitCode == traitCode)
                });
            }
            else
            {
                trait.traitValue = traitValue;
            }
        }
     
    }
}