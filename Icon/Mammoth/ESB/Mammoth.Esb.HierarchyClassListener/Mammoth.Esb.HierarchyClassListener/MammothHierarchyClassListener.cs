using Icon.Common.Email;
using Icon.Esb;
using Icon.Esb.ListenerApplication;
using Icon.Esb.MessageParsers;
using Icon.Esb.Subscriber;
using Icon.Logging;
using Mammoth.Common.DataAccess;
using Mammoth.Esb.HierarchyClassListener.Models;
using Mammoth.Esb.HierarchyClassListener.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mammoth.Esb.HierarchyClassListener
{
    public class MammothHierarchyClassListener : ListenerApplication<MammothHierarchyClassListener, ListenerApplicationSettings>
    {
        private IMessageParser<List<HierarchyClassModel>> messageParser;
        private IHierarchyClassService<AddOrUpdateHierarchyClassRequest> hierarchyClassService;
        private IHierarchyClassService<DeleteBrandRequest> deleteBrandsService;
        private IHierarchyClassService<DeleteMerchandiseClassRequest> deleteMerchandiseService;
        private IHierarchyClassService<DeleteNationalClassRequest> deleteNationalService;

        public MammothHierarchyClassListener(ListenerApplicationSettings listenerApplicationSettings,
            EsbConnectionSettings esbConnectionSettings,
            IEsbSubscriber subscriber,
            IEmailClient emailClient,
            ILogger<MammothHierarchyClassListener> logger,
            IMessageParser<List<HierarchyClassModel>> messageParser,
            IHierarchyClassService<AddOrUpdateHierarchyClassRequest> hierarchyClassService,
            IHierarchyClassService<DeleteBrandRequest> deleteBrandsService,
            IHierarchyClassService<DeleteMerchandiseClassRequest> deleteMerchandiseService,
            IHierarchyClassService<DeleteNationalClassRequest> deleteNationalService)
            : base(listenerApplicationSettings,
                  esbConnectionSettings,
                  subscriber,
                  emailClient,
                  logger)
        {
            this.messageParser = messageParser;
            this.hierarchyClassService = hierarchyClassService;
            this.deleteBrandsService = deleteBrandsService;
            this.deleteMerchandiseService = deleteMerchandiseService;
            this.deleteNationalService = deleteNationalService;
        }

        public override void HandleMessage(object sender, EsbMessageEventArgs args)
        {
            List<HierarchyClassModel> hierarchyClasses = null;
            try
            {
                hierarchyClasses = messageParser.ParseMessage(args.Message);
            }
            catch (Exception e)
            {
                LogAndNotifyError(e);
            }

            if (hierarchyClasses != null)
            {
                try
                {
                    switch (hierarchyClasses.First().Action)
                    {
                        case Icon.Esb.Schemas.Wfm.Contracts.ActionEnum.AddOrUpdate:
                            hierarchyClassService.ProcessHierarchyClasses(new AddOrUpdateHierarchyClassRequest { HierarchyClasses = hierarchyClasses });
                            break;
                        case Icon.Esb.Schemas.Wfm.Contracts.ActionEnum.Delete:
                            switch (hierarchyClasses.First().HierarchyId)
                            {
                                case Hierarchies.Brands:
                                    deleteBrandsService.ProcessHierarchyClasses(new DeleteBrandRequest { HierarchyClasses = hierarchyClasses });
                                    break;
                                case Hierarchies.Merchandise:
                                    deleteMerchandiseService.ProcessHierarchyClasses(new DeleteMerchandiseClassRequest { HierarchyClasses = hierarchyClasses });
                                    break;
                                case Hierarchies.Tax:
                                    throw new ArgumentException($"No handler specified for Delete {Hierarchies.Names.Tax} Hierarchy.");
                                case Hierarchies.Browsing:
                                    throw new ArgumentException($"No handler specified for Delete {Hierarchies.Names.Browsing} Hierarchy.");
                                case Hierarchies.Financial:
                                    throw new ArgumentException($"No handler specified for Delete {Hierarchies.Names.Financial} Hierarchy.");
                                case Hierarchies.National:
                                    deleteNationalService.ProcessHierarchyClasses(new DeleteNationalClassRequest { HierarchyClasses = hierarchyClasses });
                                    break;
                                case Hierarchies.CertificationAgencyManagement:
                                    throw new ArgumentException($"No handler specified for Delete {Hierarchies.Names.CertificationAgencyManagement} Hierarchy.");
                                default:
                                    throw new ArgumentException($"No handler specified for Delete unknown hierarchy ID {hierarchyClasses.First().HierarchyClassId}.");
                            }

                            break;
                        default:
                            throw new ArgumentException($"No handler specified for Action {hierarchyClasses.First().Action}");
                    }

                }
                catch (Exception e)
                {
                    LogAndNotifyError(e);
                }
            }

            AcknowledgeMessage(args);
        }
    }
}
