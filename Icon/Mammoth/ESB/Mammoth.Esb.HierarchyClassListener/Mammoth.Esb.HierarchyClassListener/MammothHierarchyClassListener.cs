using Icon.Common.Email;
using Icon.Dvs;
using Icon.Dvs.ListenerApplication;
using Icon.Dvs.MessageParser;
using Icon.Dvs.Model;
using Icon.Dvs.Subscriber;
using Icon.Logging;
using Mammoth.Common.DataAccess;
using Mammoth.Esb.HierarchyClassListener.Models;
using Mammoth.Esb.HierarchyClassListener.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mammoth.Esb.HierarchyClassListener
{
    public class MammothHierarchyClassListener : ListenerApplication<MammothHierarchyClassListener>
    {
        private IMessageParser<List<HierarchyClassModel>> messageParser;
        private IHierarchyClassService<AddOrUpdateHierarchyClassRequest> hierarchyClassService;
        private IHierarchyClassService<DeleteBrandRequest> deleteBrandsService;
        private IHierarchyClassService<DeleteMerchandiseClassRequest> deleteMerchandiseService;
        private IHierarchyClassService<DeleteNationalClassRequest> deleteNationalService;
        private IHierarchyClassService<DeleteManufacturerRequest> deleteManufacturerService;

        public MammothHierarchyClassListener(
            DvsListenerSettings listenerSettings,
            IDvsSubscriber subscriber,
            IEmailClient emailClient,
            ILogger<MammothHierarchyClassListener> logger,
            IMessageParser<List<HierarchyClassModel>> messageParser,
            IHierarchyClassService<AddOrUpdateHierarchyClassRequest> hierarchyClassService,
            IHierarchyClassService<DeleteBrandRequest> deleteBrandsService,
            IHierarchyClassService<DeleteMerchandiseClassRequest> deleteMerchandiseService,
            IHierarchyClassService<DeleteNationalClassRequest> deleteNationalService,
            IHierarchyClassService<DeleteManufacturerRequest> deleteManufacturerService)
            : base(listenerSettings,
                  subscriber,
                  emailClient,
                  logger)
        {
            this.messageParser = messageParser;
            this.hierarchyClassService = hierarchyClassService;
            this.deleteBrandsService = deleteBrandsService;
            this.deleteMerchandiseService = deleteMerchandiseService;
            this.deleteNationalService = deleteNationalService;
            this.deleteManufacturerService = deleteManufacturerService;
        }

        public override void HandleMessage(DvsMessage message)
        {
            // Exceptions are not needed to be handled in most cases, It'll be handled by ListenerApplication
            List<HierarchyClassModel> hierarchyClasses = messageParser.ParseMessage(message);

            if (hierarchyClasses != null)
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
                            case Hierarchies.Manufacturer:
                                deleteManufacturerService.ProcessHierarchyClasses(new DeleteManufacturerRequest { HierarchyClasses = hierarchyClasses });
                                break;
                            default:
                                throw new ArgumentException($"No handler specified for Delete unknown hierarchy ID {hierarchyClasses.First().HierarchyClassId}.");
                        }

                        break;
                    default:
                        throw new ArgumentException($"No handler specified for Action {hierarchyClasses.First().Action}");
                }
            }
        }
    }
}
