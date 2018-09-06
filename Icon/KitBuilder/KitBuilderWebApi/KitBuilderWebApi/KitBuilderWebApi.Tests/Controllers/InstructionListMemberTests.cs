using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using KitBuilderWebApi.Controllers;
using KitBuilderWebApi.DataAccess.Dto;
using KitBuilderWebApi.DataAccess.Repository;
using KitBuilderWebApi.DatabaseModels;
using KitBuilderWebApi.Helper;
using KitBuilderWebApi.QueryParameters;
using KitBuilderWebApi.Helper;
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
        private Mock<IRepository<InstructionList>> mockInstructionListRepository;
        private Mock<IRepository<InstructionListMember>> mockInstructionListMemberRepository;
        private Mock<IRepository<InstructionType>> mockInstructionTypeRespository;
        private Mock<IRepository<Status>> mockStatusRespository;
        private Mock<IUrlHelper> mockUrlHelper;
        private Mock<IHelper<InstructionListDto, InstructionListsParameters>> mockInstructionListHelper;

        private IList<InstructionListDto> instructionListsDto;
        private IList<InstructionList> instructionLists;
        private List<InstructionListMember> instructionListMembers;


        [TestCleanup]
        public void TestCleanup()
        {
            Mapper.Reset();
        }


        [TestInitialize]
        public void InitializeTests()
        {
            string locationUrl = "http://localhost:55873/api/InstructionListMember/";
            mockLogger = new Mock<ILogger<InstructionListMemberController>>();
            mockInstructionListRepository = new Mock<IRepository<InstructionList>>();
            mockInstructionListMemberRepository = new Mock<IRepository<InstructionListMember>>();
            mockInstructionTypeRespository = new Mock<IRepository<InstructionType>>();
            mockStatusRespository = new Mock<IRepository<Status>>();
            mockUrlHelper= new Mock<IUrlHelper>();
            mockUrlHelper.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>()))
                .Returns(locationUrl);


            mockInstructionListHelper =
                new Mock<IHelper<InstructionListDto, InstructionListsParameters>>();


               instructionListMemberController = new InstructionListMemberController(mockLogger.Object,
                mockInstructionListHelper.Object, 
                mockInstructionListRepository.Object,
                mockInstructionListMemberRepository.Object,
                mockInstructionTypeRespository.Object,
                mockStatusRespository.Object);


            InitializeMapper();
            SetUpDataAndRepository();

        }

        private void SetUpDataAndRepository()
        {
            instructionListsDto = new List<InstructionListDto>();

            instructionLists = new List<InstructionList>
            {
                new InstructionList{ InstructionListId = 1, Name = "Instruction List 1", StatusId = 1},
                new InstructionList{ InstructionListId = 2, Name = "Instruction List 2", StatusId = 1}
            };

            instructionListMembers = new List<InstructionListMember>
            {
                new InstructionListMember {Group="Cooking Temp", InstructionListId = 1, Member = "Rare", Sequence = 0, InstructionListMemberId = 1},
                new InstructionListMember {Group="Cooking Temp", InstructionListId = 1, Member = "Medium", Sequence = 1, InstructionListMemberId = 2},
                new InstructionListMember {Group="Cooking Temp", InstructionListId = 1, Member = "Well Done", Sequence = 2, InstructionListMemberId = 3},
                new InstructionListMember {Group="Seasoning", InstructionListId = 2, Member = "Salt", Sequence = 0, InstructionListMemberId = 4},
                new InstructionListMember {Group="Seasoning", InstructionListId = 2, Member = "Pepper", Sequence = 1, InstructionListMemberId = 5}
            };

        }

        private void InitializeMapper()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<LinkGroup, LinkGroupDto>()
                    .ForMember(dest => dest.LinkGroupItemDto, conf => conf.MapFrom(src => src.LinkGroupItem));

                cfg.CreateMap<LinkGroupDto, LinkGroup>();
                cfg.CreateMap<LinkGroupItem, LinkGroupItemDto>();
                cfg.CreateMap<LinkGroupItemDto, LinkGroupItem>();
                cfg.CreateMap<Items, ItemsDto>();
                cfg.CreateMap<ItemsDto, Items>();
                cfg.CreateMap<InstructionList, InstructionListDto>();
                cfg.CreateMap<InstructionListDto, InstructionList>();
            });

        }



        [TestMethod]
        public void InstructionListMemberController_GetInstructionListMember()
        {
            mockInstructionListMemberRepository.Setup(il => il.Find(It.IsAny<Expression<Func<InstructionListMember, bool>>>()))
                .Returns<Expression<Func<InstructionListMember, bool>>>(s => instructionListMembers.Where(s.Compile()).First());

          var resutls=    instructionListMemberController.GetInstructionListMember(2, 4);
        }

        [TestMethod]
        public void InstructionListMemberController_AddInstructionListMember_NoParameters_Returns_BadRequest()
        {


            //var response = instructionListMemberController.AddInstructionListMember(null);
            //Assert.IsInstanceOfType(response, typeof(BadRequestObjectResult), "Bad Request Expected");
            //Assert.IsNotNull(response, "The response is null");

        }


        [TestMethod]
        public void InstructionListMemberController_UpdateInstructionListMember_NoParameters_Returns_BadRequest()
        {
            //var response = instructionListMemberController.UpdateInstructionListMember(null);
            //Assert.IsInstanceOfType(response, typeof(BadRequestObjectResult), "Bad Request Expected");
            //Assert.IsNotNull(response, "The response is null");

        }

        [TestMethod]
        public void InstructionListMemberController_DeleteInstructionListMember_NoParameters_Returns_BadRequest()
        {
            //var response = instructionListMemberController.DeleteInstructionListMember(null);
            //Assert.IsInstanceOfType(response, typeof(BadRequestObjectResult), "Bad Request Expected");
            //Assert.IsNotNull(response, "The response is null");

        }

        [TestMethod]
        public void InstructionListMemberController_DeleteInstructionListMember_InvalidInstructionList_Returns_NotFound()
        {

            //instructionListRepository.Setup(il => il.Find(It.IsAny<Expression<Func<InstructionList, bool>>>()))
            //    .Returns((InstructionList) null);

            //var parameters = new DeleteInstructionListMembersParameters();
            //parameters.InstructionListId = 99; 
            //parameters.InstructionListMemberIds = new List<int>() {1,2,3};
            
            //var response = instructionListMemberController.DeleteInstructionListMember(parameters);
            //Assert.IsInstanceOfType(response, typeof(NotFoundObjectResult), "Not Found Expected");
            //Assert.IsNotNull(response, "The response is null");

        }

    }
}
