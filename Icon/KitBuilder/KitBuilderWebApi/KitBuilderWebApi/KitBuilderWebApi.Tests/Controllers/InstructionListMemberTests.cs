using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using KitBuilderWebApi.Controllers;
using KitBuilderWebApi.DataAccess.Repository;
using KitBuilderWebApi.DatabaseModels;
using KitBuilderWebApi.Helper;
using KitBuilderWebApi.QueryParameters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace KitBuilderWebApi.Tests.Controllers
{
    [TestClass]
    public class InstructionListMemberTests
    {
        private InstructionListMemberController instructionListMemberController;
        private Mock<ILogger<InstructionListMemberController>> mockLogger;
        private Mock<IRepository<InstructionList>> instructionListRepository;
        private Mock<IRepository<InstructionListMember>> instructionListMemberRepository;
        private Mock<IRepository<InstructionType>> instructionTypeRespository;
        private Mock<IRepository<Status>> statusRespository;
        private Mock<IUrlHelper> urlHelper;
        private Mock<InstructionListHelper> instructionListHelper;

        [TestInitialize]
        public void InitializeTests()
        {
            mockLogger = new Mock<ILogger<InstructionListMemberController>>();
            instructionListRepository = new Mock<IRepository<InstructionList>>();
            instructionListMemberRepository = new Mock<IRepository<InstructionListMember>>();
            instructionTypeRespository = new Mock<IRepository<InstructionType>>();
            statusRespository = new Mock<IRepository<Status>>();
            instructionListHelper = new Mock<InstructionListHelper>(urlHelper);

            instructionListMemberController = new InstructionListMemberController(mockLogger.Object, instructionListHelper.Object, instructionListRepository.Object,
                instructionListMemberRepository.Object,
                instructionTypeRespository.Object,
                statusRespository.Object);
        }

        [TestMethod]
        public void InstructionListMemberController_AddInstructionListMember_NoParameters_Returns_BadRequest()
        {
            var response = instructionListMemberController.AddInstructionListMember(null);
            Assert.IsInstanceOfType(response, typeof(BadRequestObjectResult), "Bad Request Expected");
            Assert.IsNotNull(response, "The response is null");

        }


        [TestMethod]
        public void InstructionListMemberController_UpdateInstructionListMember_NoParameters_Returns_BadRequest()
        {
            var response = instructionListMemberController.UpdateInstructionListMember(null);
            Assert.IsInstanceOfType(response, typeof(BadRequestObjectResult), "Bad Request Expected");
            Assert.IsNotNull(response, "The response is null");

        }

        [TestMethod]
        public void InstructionListMemberController_DeleteInstructionListMember_NoParameters_Returns_BadRequest()
        {
            var response = instructionListMemberController.DeleteInstructionListMember(null);
            Assert.IsInstanceOfType(response, typeof(BadRequestObjectResult), "Bad Request Expected");
            Assert.IsNotNull(response, "The response is null");

        }

        [TestMethod]
        public void InstructionListMemberController_DeleteInstructionListMember_InvalidInstructionList_Returns_NotFound()
        {

            instructionListRepository.Setup(il => il.Find(It.IsAny<Expression<Func<InstructionList, bool>>>()))
                .Returns((InstructionList) null);

            var parameters = new DeleteInstructionListMembersParameters();
            parameters.InstructionListId = 99; 
            parameters.InstructionListMemberIds = new List<int>() {1,2,3};
            
            var response = instructionListMemberController.DeleteInstructionListMember(parameters);
            Assert.IsInstanceOfType(response, typeof(NotFoundObjectResult), "Not Found Expected");
            Assert.IsNotNull(response, "The response is null");

        }

    }
}
