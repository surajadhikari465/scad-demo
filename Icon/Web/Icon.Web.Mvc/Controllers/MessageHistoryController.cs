using Icon.Common.DataAccess;
using Icon.Ewic.Transmission.Producers;
using Icon.Framework;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Controllers
{
    [Authorize(Roles = "WFM\\IRMA.Developers")]
    public class MessageHistoryController : Controller
    {
        private IQueryHandler<GetFailedMessagesParameters, List<MessageModel>> getFailedMessagesQueryHandler;
        private IQueryHandler<GetMessageHistoryParameters, List<MessageHistory>> getMessageHistoryQueryHandler;
        private ICommandHandler<ReprocessFailedMessagesCommand> reprocessFailedMessagesCommandHandler;
        private ICommandHandler<UpdateMessageHistoryStatusCommand> updateMessageHistoryStatusCommandHandler;
        private IMessageProducer esbMessageProducer;

        public MessageHistoryController(
            IQueryHandler<GetFailedMessagesParameters, List<MessageModel>> getFailedMessagesQueryHandler,
            IQueryHandler<GetMessageHistoryParameters, List<MessageHistory>> getMessageHistoryQueryHandler,
            ICommandHandler<ReprocessFailedMessagesCommand> reprocessFailedMessagesCommandHandler,
            ICommandHandler<UpdateMessageHistoryStatusCommand> updateMessageHistoryStatusCommandHandler,
            IMessageProducer esbMessageProducer)
        {
            this.getFailedMessagesQueryHandler = getFailedMessagesQueryHandler;
            this.getMessageHistoryQueryHandler = getMessageHistoryQueryHandler;
            this.reprocessFailedMessagesCommandHandler = reprocessFailedMessagesCommandHandler;
            this.updateMessageHistoryStatusCommandHandler = updateMessageHistoryStatusCommandHandler;
            this.esbMessageProducer = esbMessageProducer;
        }

        public ActionResult Index()
        {
            var failedMessageHistoryEntries = getFailedMessagesQueryHandler.Search(new GetFailedMessagesParameters())
                .Select(mh => new FailedMessageViewModel
                {
                    Id = mh.Id,
                    InsertDate = mh.InsertDate,
                    MessageStatus = "Failed",
                    MessageStatusId = mh.MessageStatusId,
                    MessageType = GetMessageTypeId(mh.MessageTypeId),
                    MessageTypeId = mh.MessageTypeId
                })
                .ToList();

            return View(failedMessageHistoryEntries);
        }

        private string GetMessageTypeId(int messageTypeId)
        {
            switch (messageTypeId)
            {
                case MessageTypes.CchTaxUpdate: return "CCH Tax Update";
                case MessageTypes.DepartmentSale: return "Department Sale";
                case MessageTypes.Hierarchy: return "Hierarchy";
                case MessageTypes.ItemLocale: return "Item Locale";
                case MessageTypes.Locale: return "Locale";
                case MessageTypes.Price: return "Price";
                case MessageTypes.Product: return "Product";
                case MessageTypes.Ewic: return "eWIC";
                default: throw new ArgumentException("MessageTypeId " + messageTypeId + " is not known.");
            }
        }

        public ActionResult Reprocess(List<FailedMessageViewModel> viewModels)
        {
            if (viewModels == null || viewModels.Count == 0 || viewModels.Any(vm => vm == null))
            {
                return Json(new { Success = false, Error = "Please select rows to reprocess." });
            }

            // eWIC messages aren't managed by a controller, so we'll transmit those directly and pass the others to the Reprocess command.

            var ewicMessages = viewModels.Where(vm => vm.MessageTypeId == MessageTypes.Ewic).ToList();
            var messagesManagedByController = viewModels.Where(vm => vm.MessageTypeId != MessageTypes.Ewic).ToList();

            if (ewicMessages.Count > 0)
            {
                try
                {
                    ProcessEwicMessages(ewicMessages);
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, Error = String.Format("Error occurred while attempting to send eWIC messages: {0}", ex) });
                }
            }

            if (messagesManagedByController.Count > 0)
            {
                try
                {
                    reprocessFailedMessagesCommandHandler.Execute(new ReprocessFailedMessagesCommand
                    {
                        MessageHistoriesById = messagesManagedByController.Select(vm => vm.Id).ToList()
                    });
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, Error = String.Format("Error occurred while attempting to reprocess messages: {0}", ex) });
                }
            }

            return Json(new { Success = true, ReprocessedData = viewModels });
        }

        private void ProcessEwicMessages(List<FailedMessageViewModel> ewicMessages)
        {
            List<MessageHistory> messages = GetMessages(ewicMessages);

            TransmitEwicMessages(messages);

            UpdateMessageHistoryStatus(ewicMessages.Select(m => m.Id).ToList());
        }

        private void UpdateMessageHistoryStatus(List<int> messageHistoriesById)
        {
            var command = new UpdateMessageHistoryStatusCommand { MessageHistoriesById = messageHistoriesById, MessageStatusId = MessageStatusTypes.Sent };

            updateMessageHistoryStatusCommandHandler.Execute(command);
        }

        private List<MessageHistory> GetMessages(List<FailedMessageViewModel> ewicMessages)
        {
            var query = new GetMessageHistoryParameters { MessageHistoriesById = ewicMessages.Select(m => m.Id).ToList() };

            List<MessageHistory> messages = getMessageHistoryQueryHandler.Search(query);

            return messages;
        }

        private void TransmitEwicMessages(List<MessageHistory> messages)
        {
            esbMessageProducer.SendMessages(messages);
        }
    }
}