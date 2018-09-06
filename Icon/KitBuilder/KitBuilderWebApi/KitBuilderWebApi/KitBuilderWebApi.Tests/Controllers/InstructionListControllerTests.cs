using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using KitBuilderWebApi.Controllers;
using KitBuilderWebApi.DataAccess.Repository;
using KitBuilderWebApi.DatabaseModels;
using KitBuilderWebApi.Helper;
using KitBuilderWebApi.QueryParameters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq.Expressions;
using KitBuilderWebApi.DataAccess.Dto;
using InstructionType = KitBuilderWebApi.DatabaseModels.InstructionType;


namespace KitBuilderWebApi.Tests.Controllers
{

  [TestClass]
  public class InstructionListControllerTests
    {

        private InstructionListController instructionListController;
        private Mock<ILogger<InstructionListController>> mockLogger;
        private Mock<IRepository<InstructionList>> mockInstructionListRepository;
        private Mock<IRepository<InstructionListMember>> mockInstructionListMemberRepository;
        private Mock<IRepository<InstructionType>> mockInstructionTypeRespository;
        private Mock<IRepository<Status>> mockStatusRespository;
        private Mock<IUrlHelper> mockUrlHelper;
        private Mock<IHelper<InstructionListDto, InstructionListsParameters>> mockLinkGroupHelper;
        private List<InstructionList> instructionLists;
        private List<InstructionType> instructionTypes;
        private List<Status> statuses;

        [TestInitialize]
        public void InitializeTest()
        {

            string locationUrl = "http://localhost:55873/api/InstructionList/";
            mockLogger = new Mock<ILogger<InstructionListController>>();
            mockInstructionListRepository = new Mock<IRepository<InstructionList>>();
            mockInstructionListMemberRepository = new Mock<IRepository<InstructionListMember>>();
            mockInstructionTypeRespository = new Mock<IRepository<InstructionType>>();
            mockStatusRespository = new Mock<IRepository<Status>>();
            mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns(locationUrl);


            mockLinkGroupHelper = new Mock<IHelper<InstructionListDto, InstructionListsParameters>>();

            instructionListController = new InstructionListController(mockInstructionListRepository.Object,
                mockInstructionListMemberRepository.Object,
                mockInstructionTypeRespository.Object,
                mockStatusRespository.Object,
                mockLogger.Object,
                mockLinkGroupHelper.Object);
        }

        private void SetUpDataAndRepository()
        {
            MappingHelper.InitializeMapper();

            instructionLists = new List<InstructionList>
            {
                new InstructionList {InstructionListId = 1, InstructionTypeId = 1, StatusId = 1, Name = "List 1"},
                new InstructionList {InstructionListId = 2, InstructionTypeId = 1, StatusId = 1, Name = "List 1"}
            };

            instructionTypes = new List<InstructionType>
            {
                new InstructionType {InstructionTypeId = 1, Name = "Type1"}
            };

            statuses = new List<Status>
            {
                new Status {StatusId = 1, StatusCode = "ENA", StatusDescription = "Enabled"}
            };

            mockInstructionListRepository.Setup(m => m.GetAll()).Returns(instructionLists.AsQueryable());
            mockInstructionTypeRespository.Setup(m => m.GetAll()).Returns(instructionTypes.AsQueryable());
            mockStatusRespository.Setup(m => m.GetAll()).Returns(statuses.AsQueryable());


        }

        [TestCleanup]
        public void Cleanup()
        {
            Mapper.Reset();
        }

        [TestMethod]
        public void InstructionListController_GetInstructionsList_NoParameters_Returns_BadRequest()
        {
            var response =  instructionListController.GetInstructionsList(null);
            Assert.IsInstanceOfType(response,typeof(BadRequestObjectResult), "Bad Request Expected");
            Assert.IsNotNull(response, "The response is null");
        }

        [TestMethod]
        public void InstructionListController_GetInstructionsList_ValidParameters_Returns_Ok()
        {
            var parameters = new InstructionListsParameters();

            var response = instructionListController.GetInstructionsList(parameters);
            Assert.IsInstanceOfType(response, typeof(OkObjectResult), "Ok Request Expected");
            Assert.IsNotNull(response, "The response is null");
        }

        [TestMethod]
        public void InstructionListController_GetInstructionsList_InvalidOrderBy_Returns_BadRequest()
        {
            mockLinkGroupHelper.Setup(s => s.SetOrderBy(It.IsAny<IQueryable<InstructionListDto>>(), It.IsAny<InstructionListsParameters>())).Throws(new Exception("Invalid Order By"));

            var parameters = new InstructionListsParameters {OrderBy = "ThisValueDoesntExist"};

            var response = instructionListController.GetInstructionsList(parameters);
            Assert.IsInstanceOfType(response, typeof(BadRequestObjectResult), "Bad Request Expected");
            Assert.IsNotNull(response, "The response is null");
        }
        [TestMethod]
        public void InstructionListController_UpdateInstructionsList_ListDoesntExist_Returns_NotFound()
        {
            var parameters = new InstructionList {InstructionListId = -1};
            var response = instructionListController.UpdateInstructionList(parameters);

            Assert.IsInstanceOfType(response, typeof(NotFoundResult), "Not Found Expected");
            Assert.IsNotNull(response, "The response is null");
        }

        [TestMethod]
        public void InstructionListController_UpdateInstructionsList_Null_Returns_BadRequest()
        {
            
            var response = instructionListController.UpdateInstructionList(null);

            Assert.IsInstanceOfType(response, typeof(BadRequestObjectResult), "Bad Request Expected");
            Assert.IsNotNull(response, "The response is null");
        }

        [TestMethod]
        public void InstructionListController_AddInstructionsList_Null_Returns_BadRequest()
        {

            var response = instructionListController.AddInstructionList(null);

            Assert.IsInstanceOfType(response, typeof(BadRequestObjectResult), "Bad Request Expected");
            Assert.IsNotNull(response, "The response is null");
        }


    }
}