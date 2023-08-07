using Mammoth.Common.DataAccess;
using MammothWebApi.DataAccess.Models;
using MammothWebApi.Service.Models;
using System;
using System.Collections.Generic;

namespace MammothWebApi.Service.Extensions
{
	public static class StagingModelMapperExtensions
	{
		const string ONE = "1";
		const string ZERO = "0";

		public static List<StagingItemLocaleModel> ToStagingItemLocaleModel(this IEnumerable<ItemLocaleServiceModel> itemLocales, DateTime timestamp, Guid transactionId)
		{
			var staging = new List<StagingItemLocaleModel>();

			foreach (var itemLocale in itemLocales)
			{
				var row = new StagingItemLocaleModel
				{
					Region = itemLocale.Region,
					ScanCode = itemLocale.ScanCode,
					BusinessUnitID = itemLocale.BusinessUnitId,
					Authorized = itemLocale.Authorized,
					Discount_Case = itemLocale.CaseDiscount,
					Discount_TM = itemLocale.TMDiscount,
					Restriction_Age = itemLocale.AgeRestriction,
					Restriction_Hours = itemLocale.RestrictedHours,
					Discontinued = itemLocale.Discontinued,
					LocalItem = itemLocale.LocalItem,
					LabelTypeDesc = itemLocale.LabelTypeDescription,
					Locality = itemLocale.Locality,
					Product_Code = itemLocale.ProductCode,
					RetailUnit = itemLocale.RetailUnit,
					Sign_Desc = itemLocale.SignDescription,
					Sign_RomanceText_Long = itemLocale.SignRomanceLong,
					Sign_RomanceText_Short = itemLocale.SignRomanceShort,
					Msrp = itemLocale.Msrp,
					OrderedByInfor = itemLocale.OrderedByInfor ?? false,
					AltRetailSize = itemLocale.AltRetailSize,
					AltRetailUOM = itemLocale.AltRetailUOM,
					DefaultScanCode = itemLocale.DefaultScanCode,
					IrmaItemKey = itemLocale.IrmaItemKey,
					ScaleItem = itemLocale.IsNonRetailItem ? false : itemLocale.ScaleItem.GetValueOrDefault(false),
					Timestamp = timestamp,
					TransactionId = transactionId,
				};

				staging.Add(row);
			}

			return staging;
		}

		public static List<StagingPriceModel> ToStagingPriceModel(this IEnumerable<PriceServiceModel> prices, DateTime timestamp, Guid guid)
		{
			var priceStagingList = new List<StagingPriceModel>();

			foreach (var price in prices)
			{
				var stagingRow = new StagingPriceModel
				{
					Region = price.Region,
					BusinessUnitId = price.BusinessUnitId,
					ScanCode = price.ScanCode,
					Multiple = price.Multiple,
					Price = price.Price,
					PriceType = price.PriceType,
					PriceUom = price.PriceUom,
					StartDate = price.StartDate,
					EndDate = price.EndDate,
					CurrencyCode = price.CurrencyCode,
					Timestamp = timestamp,
					TransactionId = guid
				};

				priceStagingList.Add(stagingRow);
			}

			return priceStagingList;
		}

		/// <summary>
		/// Converts the ItemLocaleServiceModel to the model it needs to add it to the ItemLocaleExtended staging table.
		/// This table is a key-value row based table as opposed to a column table.
		/// This applies to all the 'Extended' attributes.
		/// </summary>
		/// <param name="itemLocales">List of ItemLocale data</param>
		/// <param name="timestamp">Timestamp for which the batch is added to the staging table.</param>
		/// <returns>List of items which can be copied to the ItemLocaleExtended staging table.</returns>
		public static List<StagingItemLocaleExtendedModel> ToStagingItemLocaleExtendedModel(
				this IEnumerable<ItemLocaleServiceModel> itemLocales,
				DateTime timestamp,
				Guid transactionId)
		{
			var itemLocalesExtended = new List<StagingItemLocaleExtendedModel>();
			var model = new StagingItemLocaleExtendedModel();

			foreach (var itemLocale in itemLocales)
			{
				// Create a StagingItemLocaleExtendedModel row for each extended attribute
				model = new StagingItemLocaleExtendedModel
				{
					AttributeId = Attributes.ColorAdded,
					AttributeValue = itemLocale.ColorAdded.HasValue ? itemLocale.ColorAdded.Value.BoolToString() : null,
					Region = itemLocale.Region,
					BusinessUnitId = itemLocale.BusinessUnitId,
					ScanCode = itemLocale.ScanCode
				};
				itemLocalesExtended.Add(model);

				model = new StagingItemLocaleExtendedModel
				{
					AttributeId = Attributes.CountryOfProcessing,
					AttributeValue = itemLocale.CountryOfProcessing,
					Region = itemLocale.Region,
					BusinessUnitId = itemLocale.BusinessUnitId,
					ScanCode = itemLocale.ScanCode
				};
				itemLocalesExtended.Add(model);

				model = new StagingItemLocaleExtendedModel
				{
					AttributeId = Attributes.Origin,
					AttributeValue = itemLocale.Origin,
					Region = itemLocale.Region,
					BusinessUnitId = itemLocale.BusinessUnitId,
					ScanCode = itemLocale.ScanCode
				};
				itemLocalesExtended.Add(model);

				model = new StagingItemLocaleExtendedModel
				{
					AttributeId = Attributes.ElectronicShelfTag,
					AttributeValue = itemLocale.ElectronicShelfTag.HasValue
								&& itemLocale.ElectronicShelfTag.Value ? itemLocale.ElectronicShelfTag.Value.BoolToString() : null,
					Region = itemLocale.Region,
					BusinessUnitId = itemLocale.BusinessUnitId,
					ScanCode = itemLocale.ScanCode
				};
				itemLocalesExtended.Add(model);

				model = new StagingItemLocaleExtendedModel
				{
					AttributeId = Attributes.Exclusive,
					AttributeValue = itemLocale.Exclusive.HasValue ? itemLocale.Exclusive.Value.ToString("o") : null,
					Region = itemLocale.Region,
					BusinessUnitId = itemLocale.BusinessUnitId,
					ScanCode = itemLocale.ScanCode
				};
				itemLocalesExtended.Add(model);

				model = new StagingItemLocaleExtendedModel
				{
					AttributeId = Attributes.NumberOfDigitsSentToScale,
					AttributeValue = itemLocale.NumberOfDigitsSentToScale.HasValue ? itemLocale.NumberOfDigitsSentToScale.Value.ToString() : null,
					Region = itemLocale.Region,
					BusinessUnitId = itemLocale.BusinessUnitId,
					ScanCode = itemLocale.ScanCode
				};
				itemLocalesExtended.Add(model);

				model = new StagingItemLocaleExtendedModel
				{
					AttributeId = Attributes.ChicagoBaby,
					AttributeValue = itemLocale.ChicagoBaby,
					Region = itemLocale.Region,
					BusinessUnitId = itemLocale.BusinessUnitId,
					ScanCode = itemLocale.ScanCode
				};
				itemLocalesExtended.Add(model);

				model = new StagingItemLocaleExtendedModel
				{
					AttributeId = Attributes.TagUom,
					AttributeValue = itemLocale.TagUom,
					Region = itemLocale.Region,
					BusinessUnitId = itemLocale.BusinessUnitId,
					ScanCode = itemLocale.ScanCode
				};
				itemLocalesExtended.Add(model);

				model = new StagingItemLocaleExtendedModel
				{
					AttributeId = Attributes.LinkedScanCode,
					AttributeValue = itemLocale.LinkedItem,
					Region = itemLocale.Region,
					BusinessUnitId = itemLocale.BusinessUnitId,
					ScanCode = itemLocale.ScanCode
				};
				itemLocalesExtended.Add(model);

				model = new StagingItemLocaleExtendedModel
				{
					AttributeId = Attributes.ScaleExtraText,
					AttributeValue = itemLocale.ScaleExtraText,
					Region = itemLocale.Region,
					BusinessUnitId = itemLocale.BusinessUnitId,
					ScanCode = itemLocale.ScanCode
				};
				itemLocalesExtended.Add(model);

				model = new StagingItemLocaleExtendedModel
				{
					AttributeId = Attributes.CfsSendToScale,
					AttributeValue = itemLocale.IsNonRetailItem ? "False" : itemLocale.SendtoCFS?.ToString(), //PBI 14888: NonRetail items should not be flagged as 'send to scale' since they are 0.00-priced items which causes an error in (On)ePlum, but we need to send this data to SLAW for any Non Retail Items with Extra Text data.
					Region = itemLocale.Region,
					BusinessUnitId = itemLocale.BusinessUnitId,
					ScanCode = itemLocale.ScanCode
				};
				itemLocalesExtended.Add(model);

				model = new StagingItemLocaleExtendedModel
				{
					AttributeId = Attributes.ShelfLife,
					AttributeValue = itemLocale.ShelfLife?.ToString(),
					Region = itemLocale.Region,
					BusinessUnitId = itemLocale.BusinessUnitId,
					ScanCode = itemLocale.ScanCode
				};
				itemLocalesExtended.Add(model);

				model = new StagingItemLocaleExtendedModel
				{
					AttributeId = Attributes.UseByEab,
					AttributeValue = itemLocale.UseBy,
					Region = itemLocale.Region,
					BusinessUnitId = itemLocale.BusinessUnitId,
					ScanCode = itemLocale.ScanCode
				};
				itemLocalesExtended.Add(model);

				model = new StagingItemLocaleExtendedModel
				{
					AttributeId = Attributes.ForceTare,
					AttributeValue = itemLocale.ForceTare?.ToString(),
					Region = itemLocale.Region,
					BusinessUnitId = itemLocale.BusinessUnitId,
					ScanCode = itemLocale.ScanCode
				};
				itemLocalesExtended.Add(model);

				model = new StagingItemLocaleExtendedModel
				{
					AttributeId = Attributes.WrappedTareWeight,
					AttributeValue = itemLocale.WrappedTareWeight,
					Region = itemLocale.Region,
					BusinessUnitId = itemLocale.BusinessUnitId,
					ScanCode = itemLocale.ScanCode
				};
				itemLocalesExtended.Add(model);

				model = new StagingItemLocaleExtendedModel
				{
					AttributeId = Attributes.UnwrappedTareWeight,
					AttributeValue = itemLocale.UnwrappedTareWeight,
					Region = itemLocale.Region,
					BusinessUnitId = itemLocale.BusinessUnitId,
					ScanCode = itemLocale.ScanCode
				};
				itemLocalesExtended.Add(model);

				//For Katherine
				model = new StagingItemLocaleExtendedModel
				{
					AttributeId = Attributes.PosScaleTare,
					AttributeValue = itemLocale.PosScaleTare.HasValue ? itemLocale.PosScaleTare.ToString(): null,
					Region = itemLocale.Region,
					BusinessUnitId = itemLocale.BusinessUnitId,
					ScanCode = itemLocale.ScanCode
				};
				itemLocalesExtended.Add(model);

				model = new StagingItemLocaleExtendedModel
				{
					AttributeId = Attributes.LockedForSale,
					AttributeValue = itemLocale.LockedForSale.HasValue ? itemLocale.LockedForSale.ToString() : null,
					Region = itemLocale.Region,
					BusinessUnitId = itemLocale.BusinessUnitId,
					ScanCode = itemLocale.ScanCode
				};
				itemLocalesExtended.Add(model);

                model = new StagingItemLocaleExtendedModel
                {
                    AttributeId = Attributes.QuantityRequired,
                    AttributeValue = itemLocale.QuantityRequired.ToString(),
                    Region = itemLocale.Region,
                    BusinessUnitId = itemLocale.BusinessUnitId,
                    ScanCode = itemLocale.ScanCode
                };
                itemLocalesExtended.Add(model);

                model = new StagingItemLocaleExtendedModel
                {
                    AttributeId = Attributes.PriceRequired,
                    AttributeValue = itemLocale.PriceRequired.ToString(),
                    Region = itemLocale.Region,
                    BusinessUnitId = itemLocale.BusinessUnitId,
                    ScanCode = itemLocale.ScanCode
                };
                itemLocalesExtended.Add(model);

                model = new StagingItemLocaleExtendedModel
                {
                    AttributeId = Attributes.QtyProhibit,
                    AttributeValue = itemLocale.QtyProhibit.HasValue ? itemLocale.QtyProhibit.ToString() : null,
                    Region = itemLocale.Region,
                    BusinessUnitId = itemLocale.BusinessUnitId,
                    ScanCode = itemLocale.ScanCode
                };
                itemLocalesExtended.Add(model);

                model = new StagingItemLocaleExtendedModel
                {
                    AttributeId = Attributes.CostedByWeight,
                    AttributeValue = itemLocale.CostedByWeight.ToString() ,
                    Region = itemLocale.Region,
                    BusinessUnitId = itemLocale.BusinessUnitId,
                    ScanCode = itemLocale.ScanCode
                };
                itemLocalesExtended.Add(model);

                model = new StagingItemLocaleExtendedModel
                {
                    AttributeId = Attributes.CatchweightRequired,
                    AttributeValue = itemLocale.CatchweightRequired.ToString(),
                    Region = itemLocale.Region,
                    BusinessUnitId = itemLocale.BusinessUnitId,
                    ScanCode = itemLocale.ScanCode
                };
                itemLocalesExtended.Add(model);

                model = new StagingItemLocaleExtendedModel
                {
                    AttributeId = Attributes.CatchWtReq,
                    AttributeValue = itemLocale.CatchWtReq.ToString(),
                    Region = itemLocale.Region,
                    BusinessUnitId = itemLocale.BusinessUnitId,
                    ScanCode = itemLocale.ScanCode
                };
                itemLocalesExtended.Add(model);

            }

			// Set timestamp and transactionId for all rows
			itemLocalesExtended.ForEach(il =>
			{
				il.Timestamp = timestamp;
				il.TransactionId = transactionId;
			});
			return itemLocalesExtended;
		}

		public static List<StagingItemLocaleSupplierModel> ToStagingItemLocaleSupplierModel(this IEnumerable<ItemLocaleServiceModel> itemLocales, DateTime timestamp, Guid transactionId)
		{
			var staging = new List<StagingItemLocaleSupplierModel>();

			foreach (var itemLocale in itemLocales)
			{
				var row = new StagingItemLocaleSupplierModel
				{
					Region = itemLocale.Region,
					ScanCode = itemLocale.ScanCode,
					BusinessUnitID = itemLocale.BusinessUnitId,
					IrmaVendorKey = itemLocale.IrmaVendorKey,
					SupplierCaseSize = itemLocale.SupplierCaseSize,
					SupplierItemId = itemLocale.SupplierItemId,
					SupplierName = itemLocale.SupplierName,
					Timestamp = timestamp,
					TransactionId = transactionId
				};

				staging.Add(row);
			}

			return staging;
		}

		public static List<StagingItemLocaleSupplierDeleteModel> ToStagingItemLocaleSupplierDeleteModel(this IEnumerable<ItemLocaleServiceModel> itemLocales, DateTime timestamp, Guid transactionId)
		{
			var staging = new List<StagingItemLocaleSupplierDeleteModel>();

			foreach (var itemLocale in itemLocales)
			{
				var row = new StagingItemLocaleSupplierDeleteModel
				{
					Region = itemLocale.Region,
					ScanCode = itemLocale.ScanCode,
					BusinessUnitID = itemLocale.BusinessUnitId,
					Timestamp = timestamp,
					TransactionId = transactionId
				};

				staging.Add(row);
			}

			return staging;
		}

		/// <summary>
		/// Convert boolean to a string of "1" or "0"
		/// </summary>
		/// <param name="value">any boolean</param>
		/// <returns>"1" or "0"</returns>
		public static string BoolToString(this bool value)
		{
			return value ? ONE : ZERO;
		}

		public static string ToLogString(this ItemLocaleServiceModel itemLocaleModel)
		{
			return string.Format("ScanCode: {0}, BusinessUnit: {1}, Region: {2}", itemLocaleModel.ScanCode, itemLocaleModel.BusinessUnitId, itemLocaleModel.Region);
		}

		public static string ToLogString(this PriceServiceModel priceModel)
		{
			return string.Format("ScanCode: {0}, BusinessUnit: {1}, Region: {2}", priceModel.ScanCode, priceModel.BusinessUnitId, priceModel.Region);
		}
	}
}