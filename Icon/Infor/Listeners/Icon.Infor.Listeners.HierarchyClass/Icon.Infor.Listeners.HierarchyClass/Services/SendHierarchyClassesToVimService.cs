using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Infor.Listeners.HierarchyClass.Models;
using Icon.Infor.Listeners.HierarchyClass.EsbService;
using Icon.Infor.Listeners.HierarchyClass.Commands;
using Icon.Common.DataAccess;
using Icon.Logging;
using Newtonsoft.Json;
using Icon.Infor.Listeners.HierarchyClass.Constants;
using Esb.Core.EsbServices;
using Icon.Infor.Listeners.HierarchyClass.Requests;
using Icon.Infor.Listeners.HierarchyClass.Extensions;

namespace Icon.Infor.Listeners.HierarchyClass.Services
{
    public class SendHierarchyClassesToVimService : IHierarchyClassService
    {
        private static readonly IEnumerable<string> hierarchiesToSendToVim = new[] { Framework.Hierarchies.Names.Brands, Framework.Hierarchies.Names.National };
        private IEsbService<HierarchyClassEsbServiceRequest> esbService;
        private ICommandHandler<ArchiveVimHierarchyClassesCommand> archiveVimHierarchyClassesCommandHandler;
        private ICommandHandler<ArchiveVimMessageCommand> archiveVimMessageCommandHandler;
        private ILogger<SendHierarchyClassesToVimService> logger;

        public SendHierarchyClassesToVimService(
            IEsbService<HierarchyClassEsbServiceRequest> esbService,
            ICommandHandler<ArchiveVimHierarchyClassesCommand> archiveVimHierarchyClassesCommandHandler,
            ICommandHandler<ArchiveVimMessageCommand> archiveVimMessageCommandHandler,
            ILogger<SendHierarchyClassesToVimService> logger)
        {
            this.esbService = esbService;
            this.archiveVimHierarchyClassesCommandHandler = archiveVimHierarchyClassesCommandHandler;
            this.archiveVimMessageCommandHandler = archiveVimMessageCommandHandler;
            this.logger = logger;
        }

        public void ProcessHierarchyClassMessages(IEnumerable<InforHierarchyClassModel> hierarchyClasses)
        {
            var hierarchyClassesToSendToVim = hierarchyClasses
                .Where(hc => hc.ErrorCode == null && hierarchiesToSendToVim.Contains(hc.HierarchyName))
                .Select(hc => new VimHierarchyClassModel(hc))
                .ToList();

            if (hierarchyClassesToSendToVim.Any())
            {
                var firstModel = hierarchyClassesToSendToVim.First();
                var request = new HierarchyClassEsbServiceRequest
                {
                    Action = firstModel.Action,
                    MessageId = firstModel.MessageId,
                    HierarchyName = firstModel.HierarchyName,
                    HierarchyLevelName = firstModel.HierarchyLevelName,
                    HierarchyClasses = hierarchyClassesToSendToVim
                };

                var response = esbService.Send(request);

                HandleResponse(hierarchyClassesToSendToVim, request, response);
                ArchiveResponse(hierarchyClassesToSendToVim, request, response);
            }
        }

        private void HandleResponse(IEnumerable<VimHierarchyClassModel> hierarchyClassesToSendToVim, HierarchyClassEsbServiceRequest request, EsbServiceResponse response)
        {
            if (response.Status == EsbServiceResponseStatus.Failed)
            {
                foreach (var hierarchyClass in hierarchyClassesToSendToVim)
                {
                    hierarchyClass.ErrorCode = response.ErrorCode;
                    hierarchyClass.ErrorDetails = response.ErrorDetails;
                }

                logger.Error(
                    new
                    {
                        ErrorCode = ApplicationErrors.Codes.UnableToSendHierarchyClassesToVim,
                        ErrorDescription = ApplicationErrors.Descriptions.UnableToSendHierarchyClassesToVim,
                        Request = request,
                        Response = response,
                    }.ToJson());
            }
        }

        private void ArchiveResponse(IEnumerable<VimHierarchyClassModel> hierarchyClassesToSendToVim, HierarchyClassEsbServiceRequest request, EsbServiceResponse response)
        {
            try
            {
                archiveVimHierarchyClassesCommandHandler.Execute(new ArchiveVimHierarchyClassesCommand { HierarchyClasses = hierarchyClassesToSendToVim });
                archiveVimMessageCommandHandler.Execute(new ArchiveVimMessageCommand { Response = response });
            }
            catch (Exception ex)
            {
                logger.Error(
                    new
                    {
                        ErrorCode = ApplicationErrors.Codes.UnableToArchiveVimMessage,
                        ErrorDescription = ApplicationErrors.Descriptions.UnableToArchiveVimMessage,
                        Request = request,
                        Response = response,
                        Exception = ex.ToString()
                    }.ToJson());
            }
        }
    }
}
