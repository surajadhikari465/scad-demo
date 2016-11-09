using GlobalEventController.Common;
using SubteamEventController.DataAccess.Commands;
using GlobalEventController.DataAccess.Infrastructure;
using GlobalEventController.DataAccess.DataServices;
using SubteamEventController.DataAccess.DataServices;
using GlobalEventController.DataAccess.Queries;
using GlobalEventController.DataAccess.Commands;
using GlobalEventController.Controller.EventServices;
using SubteamEventController.DataAccess.Queries;
using Icon.Framework;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;


namespace SubteamEventController.Controller.EventServices
{
    public class ItemSubTeamEventService : IEventService
    {
        private readonly IrmaContext context;
        private IQueryHandler<GetScanCodeQuery, List<ScanCode>> getScanCodeHandler;
        private IQueryHandler<GetItemIdentifiersQuery, List<ItemIdentifier>> getItemIdentifierHandler;
        private IDataService<UpdateItemSubTeamDataService> updateItemServiceHandler;
        private IQueryHandler<GetSubTeamHierarchyQuery, HierarchyClass> getSubTeamHierarchyQueryHandlercs;
        private ICommandHandler<AddItemCategoryCommand> addItemCategoryCommandHandler;
        private IQueryHandler<GetUserQuery, Users> getUserHandler;

        public int? ReferenceId { get; set; }
        public string Message { get; set; }
        public string Region { get; set; }
        public int EventTypeId { get; set; }
        public List<ScanCode> ScanCodes { get; set; }

        public ItemSubTeamEventService(IrmaContext context,
            IQueryHandler<GetScanCodeQuery, List<ScanCode>> getScanCodeHandler,
            IQueryHandler<GetItemIdentifiersQuery, List<ItemIdentifier>> getItemIdentifierHandler,
            IDataService<UpdateItemSubTeamDataService> updateItemServiceHandler,
            IQueryHandler<GetSubTeamHierarchyQuery, HierarchyClass> getSubTeamHierarchyQueryHandlercs,
            ICommandHandler<AddItemCategoryCommand> addItemCategoryCommandHandler,
            IQueryHandler<GetUserQuery, Users> getUserHandler)
        {
            this.context = context;
            this.getScanCodeHandler = getScanCodeHandler;
            this.getItemIdentifierHandler = getItemIdentifierHandler;
            this.updateItemServiceHandler = updateItemServiceHandler;
            this.getSubTeamHierarchyQueryHandlercs = getSubTeamHierarchyQueryHandlercs;
            this.addItemCategoryCommandHandler = addItemCategoryCommandHandler;
            this.getUserHandler = getUserHandler;
        }

        public void Run()
        {
            try
            {
                // Get Data from Icon
                ScanCode scanCode = GetScanCode(Message);
                // Get ItemIdentifier Information from IRMA (determine if it's a Default Identifier).
                GetItemIdentifiersQuery getItemIdentifier = new GetItemIdentifiersQuery { Predicate = ii => ii.Identifier == scanCode.scanCode && ii.Deleted_Identifier == 0 && ii.Default_Identifier == 1 };
                List<ItemIdentifier> itemIdentifiers = getItemIdentifierHandler.Handle(getItemIdentifier);
                //Process only if it is default identifer item update
                if (itemIdentifiers.Count > 0)
                {
                    ItemSubTeamModel itemSubTeamModel = new ItemSubTeamModel(scanCode);
                    int subTeamNo = GetIRMASubTeamNumberForPosDept(itemSubTeamModel);

                    if (subTeamNo != -1)
                    {
                        itemSubTeamModel.SubTeamNo = subTeamNo;                       
                        UpdateItemInIrma(itemSubTeamModel, itemIdentifiers);
                        this.context.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets ScanCode object.  It will check the Dictionary cache first, and then Icon Db.
        /// It will add the ScanCode object to the cache if it gets the information from the Db.
        /// </summary>
        /// <param name="identifier">UPC/ScanCode/Identifier</param>
        /// <returns>ScanCode object which also includes navigation properties (ItemTrait, ItemHierarchyClass, HierarchyClass)</returns>
        private ScanCode GetScanCode(string identifier)
        {
            ScanCode scanCode;
            if (Cache.IdentifierToScanCode.TryGetValue(identifier, out scanCode))
            {
                return scanCode;
            }
            else
            {
                GetScanCodeQuery getScanCode = new GetScanCodeQuery();
                getScanCode.ScanCodes = new List<string>();
                getScanCode.ScanCodes.Add(Message);
                scanCode = getScanCodeHandler.Handle(getScanCode).First();
                Cache.IdentifierToScanCode.Add(identifier, scanCode);
                return scanCode;
            }
        }


        private int GetIRMASubTeamNumberForPosDept(ItemSubTeamModel itemSubTeamModel)
        {
            HierarchyClass subTeamHierarchyClass = getSubTeamHierarchyQueryHandlercs.Handle(new GetSubTeamHierarchyQuery() { HierarchyName = itemSubTeamModel.SubTeamName });

            var eventGenrationtQuery = subTeamHierarchyClass.HierarchyClassTrait.Where(ht => ht.Trait.traitCode == TraitCodes.NonAlignedSubteam);
            bool newSubTeamDisableEventFlag = eventGenrationtQuery.Any();
            int newSubTeamNo = -1;
            int currentItemSubTeamNo = -1;
            if (!newSubTeamDisableEventFlag)
            {
                //item moved to aligned sub team : Get irma sub team no for current item pos dept no
                newSubTeamNo = GetSubTeamNumberForHierarchy(subTeamHierarchyClass);
                currentItemSubTeamNo = GetItemSubTeamNumberInIrma(itemSubTeamModel.ScanCode);
                newSubTeamNo = newSubTeamNo == currentItemSubTeamNo ? -1 : newSubTeamNo;
            }
            else
            {
                //Item moved to not-aligned sub team
                //If current item sub team in IRMA is no not aligned. then do nothing.. no update should happen to item in IRMA
                //otherwise update item to Default sub team
               
                   var item = this.context.Item
                          .Include(i => i.ItemIdentifier)
                          .SingleOrDefault(i => i.ItemIdentifier
                          .Any(ii => ii.Identifier == itemSubTeamModel.ScanCode && ii.Deleted_Identifier == 0 && ii.Default_Identifier == 1));

                   if (item != null)
                   {
                       int itemSubTeamNo = item.SubTeam_No;

                       bool? isItemCurrentSubTeamAligend = this.context.SubTeam.Where(st => st.SubTeam_No == itemSubTeamNo).Select(st => st.AlignedSubTeam).FirstOrDefault();

                       if (isItemCurrentSubTeamAligend.HasValue && isItemCurrentSubTeamAligend.Value)
                       {
                           return GetIRMASubTeamNumber(SubTeamConstants.DefaultNonAlignedPosDeptNo);
                       }
                       //else subTeamNo = -1; no update to item
                   }
                
            }
            return newSubTeamNo;

        }

        private int GetItemSubTeamNumberInIrma(string identifier)
        {
            int subTeamNumber = -1;
            subTeamNumber = (from i in context.Item
                                 join ii in context.ItemIdentifier on i.Item_Key equals ii.Item_Key
                                 where ii.Remove_Identifier == 0 && ii.Deleted_Identifier == 0 && ii.Identifier == identifier
                                 select i.SubTeam_No).FirstOrDefault();

            if (subTeamNumber <= 0)
                subTeamNumber = -1;

            return subTeamNumber;
        }

        private int GetSubTeamNumberForHierarchy( HierarchyClass subTeamHierarchyClass)
        {
            var posDeptQuery = subTeamHierarchyClass.HierarchyClassTrait.Where(ht => ht.Trait.traitCode == TraitCodes.PosDepartmentNumber);
            var posDeptCode = posDeptQuery.Count() == 0 ? String.Empty : posDeptQuery.Single().traitValue;
            int posDeptNumber = -1;
            
            //Look for subteam from subteam table for posDeptNumber
            int subTeamNo = -1;
            if(Int32.TryParse(posDeptCode, out posDeptNumber))
            {
                return GetIRMASubTeamNumber(posDeptNumber);
            }
            return subTeamNo;
        }

        private int GetIRMASubTeamNumber(int posDeptNumber)
        { 
            int subTeamNo = -1;

            List<SubTeam> subteams = this.context.SubTeam.Where(st => st.Dept_No == posDeptNumber).ToList();
            if (subteams == null || subteams.Count == 0)
            {
                throw new Exception(String.Format("The Sub team was not found for PosDeptNo: {0}.", posDeptNumber));
            }
            else if (subteams.Count > 1)
            {
                throw new Exception(String.Format("More than one sub team found for PosDeptNo: {0}.", posDeptNumber));
            }
            else
            {
                subTeamNo = subteams[0].SubTeam_No;
            }
            return subTeamNo;
        }


        /// <summary>
        /// Updates or Adds entry to IconItemLastChange if necessary.
        /// Updates Item in IRMA with canonical information.
        /// Adds entry to ValidatedScanCode table if necessary.
        /// </summary>
        /// <param name="validatedItem"></param>
        private void UpdateItemInIrma(ItemSubTeamModel itemSubTeamModel, List<ItemIdentifier> itemIdentifiers)
        {
            int subTeamAlignedClassID = AddItemCategoryForSubTeamAligend(itemSubTeamModel.SubTeamNo);
            List<string> scanCodes = new List<string>();
            scanCodes.Add(itemSubTeamModel.ScanCode);

            UpdateItemSubTeamDataService updateItemService = new UpdateItemSubTeamDataService();
            updateItemService.LastChangeCommand = new AddUpdateItemSubTeamLastChangeCommand { UpdatedItem = itemSubTeamModel };
            updateItemService.ItemCommand = new UpdateItemSubTeamCommand { UpdatedItemSubTeam = itemSubTeamModel, Category_ID = subTeamAlignedClassID };
            updateItemService.ItemIdentifiers = itemIdentifiers;
            updateItemService.Region = Region;

            updateItemServiceHandler.Process(updateItemService);
        }

        private int AddItemCategoryForSubTeamAligend(int subTeamNo)
        {   
            AddItemCategoryCommand addItemCategoryCommand = new AddItemCategoryCommand();
            addItemCategoryCommand.SubTeamNo = subTeamNo;
            addItemCategoryCommand.UserId =  GetUserId(Region);
            addItemCategoryCommandHandler.Handle(addItemCategoryCommand);
            return addItemCategoryCommand.ItemCategoryId;
        }

        private int GetUserId(string region)
        {
            int userId = Cache.InterfaceControllerUserId[region];
            if (userId == -1)
            {
                Users user = getUserHandler.Handle(new GetUserQuery { UserName = Cache.InterfaceControllerUserName });

                if (user == null)
                {
                    throw new NullReferenceException(String.Format("The User: {0} was not found for region: {1}.", Cache.InterfaceControllerUserName, region));
                }

                Cache.InterfaceControllerUserId[region] = user.User_ID;
                userId = user.User_ID;
            }
            return userId;
        }
    }
}
