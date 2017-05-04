using GlobalEventController.Common;
using Icon.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.Controller
{
    public static class ArchiveExtensions
    {
        public static EventQueueArchive ToEventArchive(this ArchiveEventModelWrapper<EventQueue> wrapper)
        {
            return new EventQueueArchive
            {
                EventId = wrapper.EventModel.EventId,
                Context = JsonConvert.SerializeObject(wrapper),
                EventMessage = wrapper.EventModel.EventMessage,
                EventQueueInsertDate = wrapper.EventModel.InsertDate,
                EventReferenceId = wrapper.EventModel.EventReferenceId,
                QueueId = wrapper.EventModel.QueueId,
                RegionCode = wrapper.EventModel.RegionCode,
                ErrorCode = wrapper.ErrorCode,
                ErrorDetails = wrapper.ErrorDetails
            };
        }

        public static EventQueueArchive ToEventArchive(this ArchiveEventModelWrapper<FailedEvent> wrapper)
        {
            return new EventQueueArchive
            {
                EventId = wrapper.EventModel.Event.EventId,
                Context = JsonConvert.SerializeObject(wrapper.EventModel),
                EventMessage = wrapper.EventModel.Event.EventMessage,
                EventQueueInsertDate = wrapper.EventModel.Event.InsertDate,
                EventReferenceId = wrapper.EventModel.Event.EventReferenceId,
                QueueId = wrapper.EventModel.Event.QueueId,
                RegionCode = wrapper.EventModel.Event.RegionCode,
                ErrorCode = wrapper.ErrorCode,
                ErrorDetails = wrapper.ErrorDetails
            };
        }

        public static IEnumerable<EventQueueArchive> ToEventArchiveList(this IEnumerable<EventQueue> events, string errorCode = null, string errorDetails = null)
        {
            var archiveModels = events.Select(e => new ArchiveEventModelWrapper<EventQueue>(e)).ToList();

            if (errorCode != null)
            {
                foreach (var archiveModel in archiveModels)
                {
                    archiveModel.ErrorCode = errorCode;
                    archiveModel.ErrorDetails = errorDetails;
                }
            }

            return archiveModels.ToEventArchiveList();
        }

        public static IEnumerable<EventQueueArchive> ToEventArchiveList(this IEnumerable<FailedEvent> failedEvents, string errorCode = null, string errorDetails = null)
        {
            var archiveModels = failedEvents.Select(e => new ArchiveEventModelWrapper<FailedEvent>(e)).ToList();

            if (errorCode != null)
            {
                foreach (var archiveModel in archiveModels)
                {
                    archiveModel.ErrorCode = errorCode;
                    archiveModel.ErrorDetails = errorDetails;
                }
            }

            return archiveModels.ToEventArchiveList();
        }

        public static IEnumerable<EventQueueArchive> ToEventArchiveList(this IEnumerable<NutriFactsModel> nutriFactsModels, IEnumerable<EventQueue> events, string errorCode = null, string errorDetails = null)
        {
            var archiveModels = nutriFactsModels
                .Join(
                    events,
                    n => n.Plu,
                    e => e.EventMessage,
                    (n, e) => new ArchiveEventModelWrapper<ArchiveNutriFactsEventModel>(new ArchiveNutriFactsEventModel(n, e)))
                .ToList();

            if (errorCode != null)
            {
                foreach (var archiveModel in archiveModels)
                {
                    archiveModel.ErrorCode = errorCode;
                    archiveModel.ErrorDetails = errorDetails;
                }
            }

            return archiveModels.ToEventArchiveList();
        }

        public static IEnumerable<EventQueueArchive> ToEventArchiveList(this IEnumerable<ValidatedItemModel> itemModels, IEnumerable<EventQueue> events, IEnumerable<NutriFactsModel> nutriFactsModels, string errorCode = null, string errorDetails = null)
        {
            var archiveModels = itemModels
                .Join(
                    events,
                    i => i.ScanCode,
                    e => e.EventMessage,
                    (i, e) => new ArchiveEventModelWrapper<ArchiveItemEventModel>(new ArchiveItemEventModel(i, e)))
                .ToList();

            if(nutriFactsModels != null && nutriFactsModels.Any())
            {
                foreach (var archiveModel in archiveModels)
                {
                    archiveModel.EventModel.NutriFactsModel = nutriFactsModels.FirstOrDefault(n => n.Plu == archiveModel.EventModel.ItemModel.ScanCode);
                }
            }

            if(errorCode != null)
            {
                foreach (var archiveModel in archiveModels)
                {
                    archiveModel.ErrorCode = errorCode;
                    archiveModel.ErrorDetails = errorDetails;
                }
            }

            return archiveModels.ToEventArchiveList();
        }

        private static IEnumerable<EventQueueArchive> ToEventArchiveList(this IEnumerable<ArchiveEventModelWrapper<EventQueue>> archiveModels)
        {
            return archiveModels.Select(e => e.ToEventArchive()).ToList();
        }

        private static IEnumerable<EventQueueArchive> ToEventArchiveList(this IEnumerable<ArchiveEventModelWrapper<FailedEvent>> archiveModels)
        {
            return archiveModels.Select(e => e.ToEventArchive()).ToList();
        }

        private static IEnumerable<EventQueueArchive> ToEventArchiveList(this IEnumerable<ArchiveEventModelWrapper<ArchiveNutriFactsEventModel>> archiveModels)
        {
            return archiveModels.Select(e => new EventQueueArchive
            {
                EventId = e.EventModel.Event.EventId,
                Context = JsonConvert.SerializeObject(e.EventModel),
                EventMessage = e.EventModel.Event.EventMessage,
                EventQueueInsertDate = e.EventModel.Event.InsertDate,
                EventReferenceId = e.EventModel.Event.EventReferenceId,
                QueueId = e.EventModel.Event.QueueId,
                RegionCode = e.EventModel.Event.RegionCode,
                ErrorCode = e.ErrorCode,
                ErrorDetails = e.ErrorDetails
            }).ToList();
        }

        private static IEnumerable<EventQueueArchive> ToEventArchiveList(this IEnumerable<ArchiveEventModelWrapper<ArchiveItemEventModel>> archiveModels)
        {
            return archiveModels.Select(e => new EventQueueArchive
            {
                EventId = e.EventModel.Event.EventId,
                Context = JsonConvert.SerializeObject(e.EventModel),
                EventMessage = e.EventModel.Event.EventMessage,
                EventQueueInsertDate = e.EventModel.Event.InsertDate,
                EventReferenceId = e.EventModel.Event.EventReferenceId,
                QueueId = e.EventModel.Event.QueueId,
                RegionCode = e.EventModel.Event.RegionCode,
                ErrorCode = e.ErrorCode,
                ErrorDetails = e.ErrorDetails
            }).ToList();
        }
    }
}
