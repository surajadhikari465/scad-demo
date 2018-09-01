using KitBuilderWebApi.Controllers;
using KitBuilderWebApi.DataAccess.Repository;
using KitBuilderWebApi.DatabaseModels;
using KitBuilderWebApi.Helper;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace KitBuilderWebApi.Tests.Controllers
{

  [TestClass]
  public class InstructionListControllerTests
    {

        private InstructionListController instructionListController;
        //private Mock<ILogger<InstructionListController>> mockLogger;
        //private Mock<IRepository<InstructionList>> instructionListRepository { get; set; }
        //private Mock<IRepository<InstructionListMember>> instructionListMemberRepository { get; set; }
        //private Mock<IRepository<InstructionType>> instructionTypeRespository { get; set; }
        //private Mock<IRepository<Status>> statusRespository { get; set; }
        //private Mock<InstructionListHelper> instructionListHelper;

        [TestInitialize]
        public void InitializeTest()
        {
            //this.mockLogger = new Mock<ILogger<InstructionListController>>();
            //this.instructionListRepository = new Mock<IRepository<InstructionList>>();
            //this.instructionListMemberRepository = new Mock<IRepository<InstructionListMember>>();
            //this.instructionTypeRespository = new Mock<IRepository<InstructionType>>();
            //this.statusRespository = new Mock<IRepository<Status>>();
            //this.instructionListHelper = new Mock<InstructionListHelper>();
        }

        [TestMethod]
        public void InstructionListController_GetInstructionsList_NoParameters()
        {
            //instructionListController = new InstructionListController(instructionListRepository.Object,
            //    instructionListMemberRepository.Object,
            //    instructionTypeRespository.Object,
            //    statusRespository.Object,
            //    mockLogger.Object,
            //    instructionListHelper.Object);

            //var response = instructionListController.GetInstructionsList(null);

        }
    }
}