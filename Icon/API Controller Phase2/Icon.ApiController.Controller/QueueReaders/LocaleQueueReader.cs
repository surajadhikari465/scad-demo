using Icon.ApiController.Common;
using Icon.ApiController.DataAccess.Commands;
using Icon.ApiController.DataAccess.Queries;
using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Framework;
using Icon.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace Icon.ApiController.Controller.QueueReaders
{
	public class LocaleQueueReader : IQueueReader<MessageQueueLocale, Contracts.LocaleType>
	{
		private ILogger<LocaleQueueReader> logger;
		private IEmailClient emailClient;
		private IQueryHandler<GetMessageQueueParameters<MessageQueueLocale>, List<MessageQueueLocale>> getMessageQueueQuery;
		private IQueryHandler<GetLocaleLineageParameters, LocaleLineageModel> getLocaleLineageQueryHandler;
		private ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueLocale>> updateMessageQueueStatusCommandHandler;
		private Dictionary<string, string> timeZoneDictionary;

		public LocaleQueueReader(
			ILogger<LocaleQueueReader> logger,
			IEmailClient emailClient,
			IQueryHandler<GetMessageQueueParameters<MessageQueueLocale>, List<MessageQueueLocale>> getMessageQueueQuery,
			IQueryHandler<GetLocaleLineageParameters, LocaleLineageModel> getLocaleLineageQueryHandler,
			ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueLocale>> updateMessageQueueStatusCommandHandler)
		{
			this.logger = logger;
			this.emailClient = emailClient;
			this.getMessageQueueQuery = getMessageQueueQuery;
			this.getLocaleLineageQueryHandler = getLocaleLineageQueryHandler;
			this.updateMessageQueueStatusCommandHandler = updateMessageQueueStatusCommandHandler;

			timeZoneDictionary = new Dictionary<string, string>();

			foreach (var timeZone in TimeZoneInfo.GetSystemTimeZones())
			{
				timeZoneDictionary.Add(timeZone.DisplayName, timeZone.StandardName);
			}
		}

		public List<MessageQueueLocale> GetQueuedMessages()
		{
			var parameters = new GetMessageQueueParameters<MessageQueueLocale>
			{
				Instance = ControllerType.Instance,
				MessageQueueStatusId = MessageStatusTypes.Ready
			};
			return getMessageQueueQuery.Search(parameters);
			// adding this filter --will remove it when esb is ready to accept venue types)
			//return getMessageQueueQuery.Search(parameters).Where(m => m.LocaleTypeId != LocaleTypes.Venue).ToList();
		}

		public List<MessageQueueLocale> GroupMessagesForMiniBulk(List<MessageQueueLocale> messages)
		{
			if (messages == null || messages.Count == 0)
			{
				throw new ArgumentException("GroupMessages() was called with invalid arguments.  The parameter 'messages' must be a non-empty, non-null list.");
			}

			// Unlike the other types, locale messages aren't bundled.  They are sent one at a time.
			var groupedMessages = new List<MessageQueueLocale> { messages.First() };

			logger.Info(string.Format("Grouped {0} queued messages to be included in the mini-bulk.", groupedMessages.Count));

			return groupedMessages;
		}

		public Contracts.LocaleType BuildMiniBulk(List<MessageQueueLocale> messages)
		{
			if (messages == null || messages.Count == 0)
			{
				throw new ArgumentException("BuildMiniBulk() was called with invalid arguments.  Parameter 'messages' must be a non-null and non-empty list.");
			}

			MessageQueueLocale message = messages.Single();

			var parameters = new GetLocaleLineageParameters
			{
				LocaleId = message.LocaleId,
				LocaleTypeId = message.LocaleTypeId
			};

			var localeLineage = getLocaleLineageQueryHandler.Search(parameters);

			var miniBulk = new Contracts.LocaleType();

			try
			{
				BuildCompanyElement(localeLineage, miniBulk);

				BuildChainElement(localeLineage, miniBulk, message.LocaleTypeId);

				BuildRegionElement(localeLineage, miniBulk, message.LocaleTypeId);

				BuildMetroAndStoreElements(localeLineage, miniBulk, message.LocaleTypeId);

				if (message.LocaleTypeId == 5)
				{ 
					BuildVenueElements(localeLineage, miniBulk,message.LocaleTypeId);
				}
			}
			catch (Exception ex)
			{
				HandleMiniBulkException(message, ex);
			}

			return miniBulk;
		}

		private void HandleMiniBulkException(MessageQueueLocale message, Exception ex)
		{
			logger.Error(string.Format("MessageQueueId: {0}.  An error occurred when adding the message to the mini-bulk.  The message status will be marked as Failed.",
			message.MessageQueueId));

			ExceptionLogger<LocaleQueueReader> exceptionLogger = new ExceptionLogger<LocaleQueueReader>(logger);
			exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());

			var command = new UpdateMessageQueueStatusCommand<MessageQueueLocale>
			{
				QueuedMessages = new List<MessageQueueLocale> { message },
				MessageStatusId = MessageStatusTypes.Failed,
				ResetInProcessBy = true
			};

			updateMessageQueueStatusCommandHandler.Execute(command);

			string errorMessage = string.Format(Resource.FailedToAddQueuedMessageToMiniBulkMessage, ControllerType.Type, ControllerType.Instance);
			string emailSubject = Resource.FailedToAddQueuedMessageToMiniBulkEmailSubject;
			string emailBody = EmailHelper.BuildMessageBodyForMiniBulkError(errorMessage, message.MessageQueueId, ex.ToString());

			try
			{
				emailClient.Send(emailBody, emailSubject);
			}
			catch (Exception mailEx)
			{
				string mailErrorMessage = "A failure occurred while attempting to send the alert email.";
				exceptionLogger.LogException(mailErrorMessage, mailEx, this.GetType(), MethodBase.GetCurrentMethod());
			}
		}

		private void BuildVenueElements(LocaleLineageModel localeLineage, Contracts.LocaleType miniBulk, int localeTypeId)
		{
			for (int regionIndex = 0; regionIndex < localeLineage.DescendantLocales.Count; regionIndex++)
			{ 
				var region = localeLineage.DescendantLocales[regionIndex];
				var regionMiniBulk = miniBulk.locales[0].locales[regionIndex];
				for (int metroIndex = 0; metroIndex < region.DescendantLocales.Count; metroIndex++)
				{
					var metro = region.DescendantLocales[metroIndex];
					for (int storeIndex = 0; storeIndex < metro.DescendantLocales.Count; storeIndex++)
					{
						var store = metro.DescendantLocales[storeIndex];
						var storeMiniBulk = regionMiniBulk.locales[metroIndex].locales[storeIndex];
						for (int venueIndex = 0; venueIndex < store.DescendantLocales.Count; venueIndex++)
						{
							var venue = store.DescendantLocales[venueIndex];
							storeMiniBulk.locales[venueIndex] = new Contracts.LocaleType
							{
								Action = localeTypeId == LocaleTypes.Venue ? Contracts.ActionEnum.Inherit : Contracts.ActionEnum.AddOrUpdate,
								ActionSpecified = true,
								id = venue.DescendantLocales[venueIndex].LocaleId.ToString(),
								name = venue.DescendantLocales[venueIndex].LocaleName,
								type = new Contracts.LocaleTypeType
								{
									code = Contracts.LocaleCodeType.VNU,
									description = Contracts.LocaleDescType.Venue
								},
								traits = CreateVenueLocaleTraits(venue.DescendantLocales[venueIndex]),
							};
						}
					}
				}
			}
		}

		private void BuildMetroAndStoreElements(LocaleLineageModel localeLineage, Contracts.LocaleType miniBulk, int localeTypeId)
		{
			for (int regionIndex = 0; regionIndex < localeLineage.DescendantLocales.Count; regionIndex++)
			{
				var region = localeLineage.DescendantLocales[regionIndex];
				var regionMiniBulk = miniBulk.locales[0].locales[regionIndex];
				for (int metroIndex = 0; metroIndex < region.DescendantLocales.Count; metroIndex++)
				{
					regionMiniBulk.locales[metroIndex] = new Contracts.LocaleType
					{
						Action = localeTypeId == LocaleTypes.Store ? Contracts.ActionEnum.Inherit : Contracts.ActionEnum.AddOrUpdate,
						ActionSpecified = true,
						id = region.DescendantLocales[metroIndex].LocaleId.ToString(),
						name = region.DescendantLocales[metroIndex].LocaleName,
						type = new Contracts.LocaleTypeType
						{
							code = Contracts.LocaleCodeType.MTR,
							description = Contracts.LocaleDescType.Metro
						},
						locales = new Contracts.LocaleType[region.DescendantLocales[metroIndex].DescendantLocales.Count]
					};

					for (int storeIndex = 0; storeIndex < region.DescendantLocales[metroIndex].DescendantLocales.Count; storeIndex++)
					{
						var storeLocaleLineage = region.DescendantLocales[metroIndex].DescendantLocales[storeIndex];
						var storeLocaleType = new Contracts.LocaleType
						{
							Action = localeTypeId == LocaleTypes.Store ? Contracts.ActionEnum.AddOrUpdate : Contracts.ActionEnum.Inherit,
							ActionSpecified = true,
							name = region.DescendantLocales[metroIndex].DescendantLocales[storeIndex].LocaleName,
							type = new Contracts.LocaleTypeType
							{
								code = Contracts.LocaleCodeType.STR,
								description = Contracts.LocaleDescType.Store
							},
						};
						if (localeTypeId == 4)
						{
							storeLocaleType.id = region.DescendantLocales[metroIndex].DescendantLocales[storeIndex].BusinessUnitId.ToString();
							storeLocaleType.addresses = new Contracts.AddressType[]
							{
								CreateLocaleAddress(region.DescendantLocales[metroIndex].DescendantLocales[storeIndex])
							};
							storeLocaleType.traits = CreateStoreLocaleTraits(region.DescendantLocales[metroIndex].DescendantLocales[storeIndex]);
						}
						if (localeTypeId == 5)
						{
							storeLocaleType.id = region.DescendantLocales[metroIndex].DescendantLocales[storeIndex].LocaleId.ToString();
							storeLocaleType.locales = new Contracts.LocaleType[region.DescendantLocales[metroIndex].DescendantLocales[storeIndex].DescendantLocales.Count];
						}
						if (storeLocaleLineage.LocaleOpenDate.HasValue)
						{
							storeLocaleType.openDate = storeLocaleLineage.LocaleOpenDate.Value;
							storeLocaleType.openDateSpecified = true;
						}
						if (storeLocaleLineage.LocaleCloseDate.HasValue)
						{
							storeLocaleType.closeDate = storeLocaleLineage.LocaleCloseDate.Value;
							storeLocaleType.closeDateSpecified = true;
						}
						regionMiniBulk.locales[metroIndex].locales[storeIndex] = storeLocaleType;
					}
				}
			}
		}

		private void BuildRegionElement(LocaleLineageModel localeLineage, Contracts.LocaleType miniBulk, int localeTypeId)
		{
			for (int regionIndex = 0; regionIndex < localeLineage.DescendantLocales.Count; regionIndex++)
			{
				var region = localeLineage.DescendantLocales[regionIndex];
				miniBulk.locales[0].locales = new Contracts.LocaleType[]
				{
				new Contracts.LocaleType
				{
					Action = localeTypeId == LocaleTypes.Region ? Contracts.ActionEnum.AddOrUpdate : Contracts.ActionEnum.Inherit,
					ActionSpecified = true,
					id = region.LocaleId.ToString(),
					name = region.LocaleName,
					type = new Contracts.LocaleTypeType
					{
						code = Contracts.LocaleCodeType.REG,
						description = Contracts.LocaleDescType.Region
					},
					locales = new Contracts.LocaleType[region.DescendantLocales.Count]
				}
				};
			}
		}

		private void BuildChainElement(LocaleLineageModel localeLineage, Contracts.LocaleType miniBulk, int localeTypeId)
		{
			miniBulk.locales = new Contracts.LocaleType[]
			{
				new Contracts.LocaleType
				{
					Action = localeTypeId == LocaleTypes.Chain ? Contracts.ActionEnum.AddOrUpdate : Contracts.ActionEnum.Inherit,
					ActionSpecified = true,
					id = localeLineage.LocaleId.ToString(),
					name = localeLineage.LocaleName,
					type = new Contracts.LocaleTypeType
					{
						code = Contracts.LocaleCodeType.CHN,
						description = Contracts.LocaleDescType.Chain
					},
					locales = new Contracts.LocaleType[localeLineage.DescendantLocales.Count]
				}
			};
		}

		private void BuildCompanyElement(LocaleLineageModel localeLineage, Contracts.LocaleType miniBulk)
		{
			miniBulk.Action = Contracts.ActionEnum.Inherit;
			miniBulk.ActionSpecified = true;
			miniBulk.id = default(int).ToString();
			miniBulk.name = "Global";
			miniBulk.type = new Contracts.LocaleTypeType()
			{
				code = Contracts.LocaleCodeType.CMP,
				description = Contracts.LocaleDescType.Company
			};
		}

		private Contracts.TraitType[] CreateVenueLocaleTraits(LocaleLineageModel localeLineage)
		{
			return new Contracts.TraitType[]
			{
				new Contracts.TraitType
				{
					code = TraitCodes.VenueCode,
					type = new Contracts.TraitTypeType
					{
						description = TraitDescriptions.VenueCode,
						value = new Contracts.TraitValueType[]
						{
							new Contracts.TraitValueType
							{
								value = localeLineage.VenueCode,
							}
						}
					}
				},
				new Contracts.TraitType
				{
					code = TraitCodes.VenueOccupant,
					type = new Contracts.TraitTypeType
					{
						description = TraitDescriptions.VenueOccupant,
						value = new Contracts.TraitValueType[]
						{
							new Contracts.TraitValueType
							{
								value = localeLineage.VenueOccupant,
							}
						}
					}
				},
				new Contracts.TraitType
				{
					code = TraitCodes.LocaleSubtype,
					type = new Contracts.TraitTypeType
					{
						description = TraitDescriptions.LocaleSubtype,
						value = new Contracts.TraitValueType[]
						{
							new Contracts.TraitValueType
							{
								value = localeLineage.VenueSubType,
							}
						}
					}
				}
			};
		}

		private Contracts.TraitType[] CreateStoreLocaleTraits(LocaleLineageModel localeLineage)
		{
			return new Contracts.TraitType[]
			{
				new Contracts.TraitType
				{
					code = TraitCodes.StoreAbbreviation,
					type = new Contracts.TraitTypeType
					{
						description = TraitDescriptions.StoreAbbreviation,
						value = new Contracts.TraitValueType[]
						{
							new Contracts.TraitValueType
							{
								value = localeLineage.StoreAbbreviation,
							}
						}
					}
				},
				new Contracts.TraitType
				{
					code = TraitCodes.PhoneNumber,
					type = new Contracts.TraitTypeType
					{
						description = TraitDescriptions.PhoneNumber,
						value = new Contracts.TraitValueType[]
						{
							new Contracts.TraitValueType
							{
								value = localeLineage.PhoneNumber,
							}
						}
					}
				}
			};
		}

		private Contracts.AddressType CreateLocaleAddress(LocaleLineageModel localeLineage)
		{
			return new Contracts.AddressType
			{
				id = localeLineage.AddressId,
				idSpecified = true,
				type = new Contracts.AddressTypeType
				{
					code = AddressTypeCodes.PhysicalAddress,
					description = Contracts.AddressDescType.physical,
					ItemElementName = Contracts.ItemChoiceType.physical,
					Item = new Contracts.PhysicalAddressType
					{
						addressLine1 = (localeLineage.AddressLine1 + " " + localeLineage.AddressLine2 + " " + localeLineage.AddressLine3).Trim(),
						cityType = new Contracts.CityType
						{
							name = localeLineage.CityName
						},
						territoryType = new Contracts.TerritoryType
						{
							code = localeLineage.TerritoryCode,
							name = localeLineage.TerritoryName
						},
						country = new Contracts.CountryType
						{
							code = localeLineage.CountryCode,
							name = localeLineage.CountryName
						},
						postalCode = localeLineage.PostalCode,
						timezone = new Contracts.TimezoneType
						{
							// ESB is expecting the Time Zone standard name to be in the timezoneCode element.
							code = timeZoneDictionary[localeLineage.TimezoneName],
							name = GetTimezoneName(localeLineage.TimezoneName)
						}
					}
				},
				usage = new Contracts.AddressUsageType
				{
					code = localeLineage.AddressUsageCode,
					description = Contracts.AddressUsgDescType.street
				}
			};
		}

		private Contracts.TimezoneNameType GetTimezoneName(string timezoneName)
		{
			switch (timezoneName)
			{
				case "(UTC-10:00) Hawaii":
					return Contracts.TimezoneNameType.USHawaii;
				case "(UTC-08:00) Pacific Time (US & Canada)":
					return Contracts.TimezoneNameType.USPacific;
				case "(UTC-07:00) Mountain Time (US & Canada)":
					return Contracts.TimezoneNameType.USMountain;
				case "(UTC-06:00) Central Time (US & Canada)":
					return Contracts.TimezoneNameType.USCentral;
				case "(UTC-05:00) Eastern Time (US & Canada)":
					return Contracts.TimezoneNameType.USEastern;
				case "(UTC-04:00) Atlantic Time (Canada)":
					return Contracts.TimezoneNameType.CanadaAtlantic;
				case "(UTC) Dublin, Edinburgh, Lisbon, London":
					return Contracts.TimezoneNameType.GMT;
				default:
					return Contracts.TimezoneNameType.USCentral;
			}
		}
	}
}
