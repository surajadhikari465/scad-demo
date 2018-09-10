using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using KitBuilderWebApi.Controllers;
using KitBuilderWebApi.DataAccess.Dto;
using KitBuilderWebApi.DataAccess.Repository;
using KitBuilderWebApi.DataAccess.UnitOfWork;
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
        private Mock<IRepository<InstructionList>> mockInstructionListRepository;
        private Mock<IRepository<InstructionListMember>> mockInstructionListMemberRepository;
        private Mock<IRepository<InstructionType>> mockInstructionTypeRespository;
        private Mock<IRepository<Status>> mockStatusRespository;
        private Mock<IUrlHelper> mockUrlHelper;
        private Mock<IHelper<InstructionListDto, InstructionListsParameters>> mockInstructionListHelper;
        private Mock<IUnitOfWork> mockUnitWork;

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
            mockUnitWork = new Mock<IUnitOfWork>();
            mockUrlHelper = new Mock<IUrlHelper>();  
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

            mockInstructionListMemberRepository.Setup(il => il.Find(It.IsAny<Expression<Func<InstructionListMember, bool>>>()))
                .Returns<Expression<Func<InstructionListMember, bool>>>(s => instructionListMembers.Where(s.Compile()).FirstOrDefault());

            mockInstructionListMemberRepository.Setup(il => il.FindAll(It.IsAny<Expression<Func<InstructionListMember, bool>>>()))
                .Returns<Expression<Func<InstructionListMember, bool>>>(s => instructionListMembers.Where(s.Compile()).ToList());

            mockInstructionListRepository.Setup(il => il.Find(It.IsAny<Expression<Func<InstructionList, bool>>>()))
                .Returns<Expression<Func<InstructionList, bool>>>(s => instructionLists.Where(s.Compile()).FirstOrDefault());


        }

        private void InitializeMapper()
        {
            MappingHelper.InitializeMapper();
        }

        [TestMethod]
        public void InstructionListMemberController_GetInstructionListMember_ValidILM()
        {
            var listId = 2;
            var listMemberId = 4;
            var response = instructionListMemberController.GetInstructionListMember(listId, listMemberId);

            Assert.IsInstanceOfType(response, typeof(OkObjectResult), "OK Expected");

        }

        [TestMethod]
        public void InstructionListMemberController_GetInstructionListMember_InvalidListId()
        {
            var listId = 9;
            var listMemberId = 4;
            var response = instructionListMemberController.GetInstructionListMember(listId, listMemberId);

            Assert.IsInstanceOfType(response, typeof(NotFoundResult), "Not Found Expected");

        }

        [TestMethod]
        public void InstructionListMemberController_GetInstructionListMember_InvalidListMemberId()
        {
            var listId = 2;
            var listMemberId = 14;
            var response = instructionListMemberController.GetInstructionListMember(listId, listMemberId);

            Assert.IsInstanceOfType(response, typeof(NotFoundResult), "Not Found Expected");
        }


        [TestMethod]
        public void InstructionListMemberController_AddInstructionListMember_ValidILM()
        {
            mockInstructionListRepository.SetupGet(s => s.UnitOfWork).Returns(mockUnitWork.Object);
            var instructionListId = 1;
            var InstructionListMemberDto = new InstructionListMemberDto { InstructionListId  = instructionListId, Group = "NewGroup", Member = "NewMember", Sequence = 0};

            //When
            
            var response = instructionListMemberController.AddInstructionListMember(instructionListId,InstructionListMemberDto);

            // Then
            Assert.IsInstanceOfType(response, typeof(StatusCodeResult), "HTTP 201 Expected");
            Assert.AreEqual(((StatusCodeResult)response).StatusCode, 201, "HTTP 201 Expected");
            mockUnitWork.Verify(m => m.Commit(), Times.Once);

        }

        [TestMethod]
        public void InstructionListMemberController_AddInstructionListMember_InvalidList()
        {
            mockInstructionListRepository.SetupGet(s => s.UnitOfWork).Returns(mockUnitWork.Object);
            var instructionListId = 99;
            var InstructionListMemberDto = new InstructionListMemberDto { InstructionListId = instructionListId, Group = "NewGroup", Member = "NewMember", Sequence = 0 };

            //When

            var response = instructionListMemberController.AddInstructionListMember(instructionListId, InstructionListMemberDto);

            // Then
            Assert.IsInstanceOfType(response, typeof(NotFoundResult), "Not Found Expected");
            mockUnitWork.Verify(m => m.Commit(), Times.Never);

        }


        [TestMethod]
        public void InstructionListMemberController_UpdateInstructionListMember_InvalidList()
        {

            mockInstructionListRepository.SetupGet(s => s.UnitOfWork).Returns(mockUnitWork.Object);
            var instructionListId = 99;
            var InstructionListMemberDto = new List<InstructionListMemberDto>
            {
                new InstructionListMemberDto { InstructionListId = instructionListId, Group = "NewGroup", Member = "NewMember", Sequence = 0 }
            };

            //When

            var response = instructionListMemberController.UpdateInstructionListMembers(instructionListId, InstructionListMemberDto);

            // Then
            Assert.IsInstanceOfType(response, typeof(NotFoundObjectResult), "Not Found Expected");
            mockUnitWork.Verify(m => m.Commit(), Times.Never);


        }

        [TestMethod]
        public void InstructionListMemberController_UpdateInstructionListMember_Valid_One()
        {

            mockInstructionListMemberRepository.SetupGet(s => s.UnitOfWork).Returns(mockUnitWork.Object);
            var instructionListId = 1;
            var InstructionListMemberDto = new List<InstructionListMemberDto>
            {
                new InstructionListMemberDto { InstructionListId = instructionListId, Group = "NewGroup", Member = "NewMember", Sequence = 0 }
            };

            //When

            var response = instructionListMemberController.UpdateInstructionListMembers(instructionListId, InstructionListMemberDto);

            // Then
            Assert.IsInstanceOfType(response, typeof(AcceptedResult), "Accepted Expected");
            mockUnitWork.Verify(m => m.Commit(), Times.Once);


        }

        [TestMethod]
        public void InstructionListMemberController_UpdateInstructionListMember_Valid_MoreThanOne()
        {

            mockInstructionListMemberRepository.SetupGet(s => s.UnitOfWork).Returns(mockUnitWork.Object);
            var instructionListId = 1;
            var InstructionListMemberDto = new List<InstructionListMemberDto>
            {
                new InstructionListMemberDto { InstructionListId = instructionListId, Group = "NewGroup", Member = "NewMember", Sequence = 0 },
                new InstructionListMemberDto { InstructionListId = instructionListId, Group = "NewGroup", Member = "NewMember2", Sequence = 1 }
            };

            //When

            var response = instructionListMemberController.UpdateInstructionListMembers(instructionListId, InstructionListMemberDto);

            // Then
            Assert.IsInstanceOfType(response, typeof(AcceptedResult), "Accepted Expected");
            mockUnitWork.Verify(m => m.Commit(), Times.Once);


        }

        [TestMethod]
        public void InstructionListMemberController_DeleteInstructionListMember_InvalidList()
        {
            mockInstructionListMemberRepository.SetupGet(s => s.UnitOfWork).Returns(mockUnitWork.Object);
            var instructionListId = 99;
            var instructionListMemberId = 1;
          

            //When

            var response = instructionListMemberController.DeleteInstructionListMember(instructionListId, instructionListMemberId);

            // Then
            Assert.IsInstanceOfType(response, typeof(NotFoundObjectResult), "Not Found Expected");
            mockUnitWork.Verify(m => m.Commit(), Times.Never);
        }


        [TestMethod]
        public void InstructionListMemberController_DeleteInstructionListMembers_InvalidList()
        {
            mockInstructionListMemberRepository.SetupGet(s => s.UnitOfWork).Returns(mockUnitWork.Object);
            var instructionListId = 99;
            var instructionListMemberId = new List<int>{1,2};


            //When

            var response = instructionListMemberController.DeleteInstructionListMembers(instructionListId, instructionListMemberId);

            // Then
            Assert.IsInstanceOfType(response, typeof(NotFoundObjectResult), "Not Found Expected");
            mockUnitWork.Verify(m => m.Commit(), Times.Never);
        }

        [TestMethod]
        public void InstructionListMemberController_DeleteInstructionListMember_Valid()
        {
            mockInstructionListMemberRepository.SetupGet(s => s.UnitOfWork).Returns(mockUnitWork.Object);
            var instructionListId = 1;
            var instructionListMemberId = 1;


            //When

            var response = instructionListMemberController.DeleteInstructionListMember(instructionListId, instructionListMemberId);

            // Then
            Assert.IsInstanceOfType(response, typeof(NoContentResult), "No Content Expected");
            mockUnitWork.Verify(m => m.Commit(), Times.Once);
        }

        [TestMethod]
        public void InstructionListMemberController_DeleteInstructionListMembers_Valid()
        {
            mockInstructionListMemberRepository.SetupGet(s => s.UnitOfWork).Returns(mockUnitWork.Object);
            var instructionListId = 1;
            var instructionListMemberId = new List<int> { 1, 2 };


            //When

            var response = instructionListMemberController.DeleteInstructionListMembers(instructionListId, instructionListMemberId);

            // Then
            Assert.IsInstanceOfType(response, typeof(NoContentResult), "No Content Expected");
            mockUnitWork.Verify(m => m.Commit(), Times.Once);
        }



    }
}
