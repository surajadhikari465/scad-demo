using Icon.Common.Context;
using Icon.Common.DataAccess;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Framework;
using Icon.Infor.Listeners.HierarchyClass.Commands;
using Icon.Infor.Listeners.HierarchyClass.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Icon.Infor.Listeners.HierarchyClass.Tests.Services
{
    public class BaseHierarchyClassesServiceTest
    {
        protected const int hierarchyClassIdForUpdate = 87654321;

        protected IHierarchyClassService service;

        private Mock<IHierarchyClassListenerSettings> _mockHierarchyClassListenerSettings;
        protected Mock<IHierarchyClassListenerSettings> MockHierarchyClassListenerSettings
        {
            get
            {
                if (_mockHierarchyClassListenerSettings == null)
                {
                    _mockHierarchyClassListenerSettings = new Mock<IHierarchyClassListenerSettings>();
                }
                return _mockHierarchyClassListenerSettings;
            }
        }

        private Mock<ICommandHandler<GenerateHierarchyClassEventsCommand>> _mockGenerateEventsCommandHandler;
        protected Mock<ICommandHandler<GenerateHierarchyClassEventsCommand>> MockGenerateEventsCommandHandler
        {
            get
            {
                if (_mockGenerateEventsCommandHandler == null)
                {
                    _mockGenerateEventsCommandHandler = new Mock<ICommandHandler<GenerateHierarchyClassEventsCommand>>();
                }
                return _mockGenerateEventsCommandHandler;
            }
        }

        private Mock<ICommandHandler<GenerateHierarchyClassMessagesCommand>> _mockGenerateMessagesCommandHandler;
        protected Mock<ICommandHandler<GenerateHierarchyClassMessagesCommand>> MockGenerateMessagesCommandHandler
        {
            get
            {
                if(_mockGenerateMessagesCommandHandler == null)
                {
                    _mockGenerateMessagesCommandHandler = new Mock<ICommandHandler<GenerateHierarchyClassMessagesCommand>>();
                }
                return _mockGenerateMessagesCommandHandler;
            }
        }

        protected IEnumerable<InforHierarchyClassModel> CreateInforHierarchyClassesForUpdate(
            int hierarchyClassId,
            string hierarchyName,
            ActionEnum action = ActionEnum.AddOrUpdate,
            Dictionary<string, string> hierarchyClassTraits = null)
        {
            return new List<InforHierarchyClassModel>
            {
                CreateInforHierarchyClassModel(hierarchyClassId, hierarchyName, action, hierarchyClassTraits)
            };
        }

        protected IEnumerable<InforHierarchyClassModel> CreateInforHierarchyClassesForAdd(
            string hierarchyName,
            ActionEnum action = ActionEnum.AddOrUpdate,
            Dictionary<string, string> hierarchyClassTraits = null)
        {
            return new List<InforHierarchyClassModel>
            {
                CreateInforHierarchyClassModel(0, hierarchyName, action, hierarchyClassTraits)
            };
        }

        protected IEnumerable<InforHierarchyClassModel> CreateInforHierarchyClassesForDelete(
            int hierarchyClassId,
            string hierarchyName,
            ActionEnum action = ActionEnum.Delete,
            Dictionary<string, string> hierarchyClassTraits = null)
        {
            return new List<InforHierarchyClassModel>
            {
                CreateInforHierarchyClassModel(hierarchyClassId, hierarchyName, action, hierarchyClassTraits)
            };
        }

        protected InforHierarchyClassModel CreateInforHierarchyClassModel(
            int hierarchyClassId,
            string hierarchyName,
            ActionEnum action,
            Dictionary<string, string> hierarchyClassTraits = null)
        {
            var testModel = new InforHierarchyClassModel
            {
                HierarchyClassId = hierarchyClassId,
                HierarchyName = hierarchyName,
                HierarchyLevelName = GetDefaultHierarchyLevelNameForTest(hierarchyName),
                Action = action,
                HierarchyClassName = GetDefaultHierarchyClassNameForTest(hierarchyName),
                HierarchyClassTraits = hierarchyClassTraits ?? new Dictionary<string, string>(),
            };
            return testModel;
        }

        protected void VerifyMockAddOrUpdateCall(
            Mock<ICommandHandler<AddOrUpdateHierarchyClassesCommand>> mockCommandHandler,
            string hierarchyName,
            Times times,
            int hierarchyClassId = 0)
        {
            VerifyMockAddOrUpdateHierarchyClassesExecute(mockCommandHandler,
                hierarchyName,
                GetDefaultHierarchyLevelNameForTest(hierarchyName),
                ActionEnum.AddOrUpdate,
                GetDefaultHierarchyClassNameForTest(hierarchyName),
                times,
                hierarchyClassId);
        }

        protected void VerifyMockAddOrUpdateHierarchyClassesExecute(
            Mock<ICommandHandler<AddOrUpdateHierarchyClassesCommand>> mockCommandHandler,
            string hierarchyName,
            string hierarchyLevelName,
            ActionEnum action,
            string hierarchyClassName,
            Times times,
            int hierarchyClassId = 0)
        {
            mockCommandHandler.Verify(
                m => m.Execute(It.Is<AddOrUpdateHierarchyClassesCommand>(
                    c => c.HierarchyClasses.All(
                        hc =>  hc.HierarchyClassId == hierarchyClassId
                            && hc.HierarchyName == hierarchyName
                            && hc.HierarchyLevelName == hierarchyLevelName
                            && hc.Action == action
                            && hc.HierarchyClassName == hierarchyClassName)))
                    , times);
        }

        protected void VerifyMockDeleteCall(
            Mock<ICommandHandler<DeleteHierarchyClassesCommand>> mockCommandHandler,
            string hierarchyName,
            Times times,
            int hierarchyClassId = 0)
        {
            VerifyMockDeleteHierarchyClassesExecute(mockCommandHandler,
                hierarchyName,
                GetDefaultHierarchyLevelNameForTest(hierarchyName),
                ActionEnum.Delete,
                GetDefaultHierarchyClassNameForTest(hierarchyName),
                times,
                hierarchyClassId);
        }

        protected void VerifyMockDeleteHierarchyClassesExecute(
            Mock<ICommandHandler<DeleteHierarchyClassesCommand>> mockCommandHandler,
            string hierarchyName,
            string hierarchyLevelName,
            ActionEnum action,
            string hierarchyClassName,
            Times times,
            int hierarchyClassId = 0)
        {
            mockCommandHandler.Verify(
                m => m.Execute(It.Is<DeleteHierarchyClassesCommand>(
                    c => c.HierarchyClasses.All(
                        hc => hc.HierarchyClassId == hierarchyClassId
                            && hc.HierarchyName == hierarchyName
                            && hc.HierarchyLevelName == hierarchyLevelName
                            && hc.Action == action
                            && hc.HierarchyClassName == hierarchyClassName
                            )))
                    , times);
        }

        protected void VerifyMockGenerateEventsCall(
            Mock<ICommandHandler<GenerateHierarchyClassEventsCommand>> mockCommandHandler,
            string hierarchyName,
            ActionEnum action,
            Times times,
            int hierarchyClassId = 0)
        {
            VerifyMockGenerateHierarchyClassEventsExecute(mockCommandHandler,
                hierarchyName,
                GetDefaultHierarchyLevelNameForTest(hierarchyName),
                action,
                GetDefaultHierarchyClassNameForTest(hierarchyName),
                times,
                hierarchyClassId);
        }

        protected void VerifyMockGenerateHierarchyClassEventsExecute(
            Mock<ICommandHandler<GenerateHierarchyClassEventsCommand>> mockCommandHandler,
            string hierarchyName,
            string hierarchyLevelName,
            ActionEnum action,
            string hierarchyClassName,
            Times times,
            int hierarchyClassId = 0)
        {
            mockCommandHandler.Verify(
                m => m.Execute(It.Is<GenerateHierarchyClassEventsCommand>(
                    c => c.HierarchyClasses.All(
                        hc =>  hc.HierarchyClassId == hierarchyClassId
                            && hc.HierarchyName == hierarchyName
                            && hc.HierarchyLevelName == hierarchyLevelName
                            && hc.Action == action
                            && hc.HierarchyClassName == hierarchyClassName)))
                    , times);
        }

        protected static string GetDefaultHierarchyLevelNameForTest(string hierarchyName)
        {
            var hierarchyLevelName = String.Empty;

            switch (hierarchyName)
            {
                case HierarchyNames.Brands:
                    hierarchyLevelName = HierarchyLevelNames.Brand;
                    break;
                case HierarchyNames.National:
                    hierarchyLevelName = HierarchyLevelNames.NationalClass;
                    break;
                case HierarchyNames.Financial:
                    hierarchyLevelName = HierarchyLevelNames.Financial;
                    break;
                case HierarchyNames.Tax:
                    hierarchyLevelName = HierarchyLevelNames.Tax;
                    break;
                case HierarchyNames.Merchandise:
                    hierarchyLevelName = HierarchyLevelNames.Segment;
                    break;
                default:
                    break;
            }
            return hierarchyLevelName;
        }

        protected string GetDefaultHierarchyClassNameForTest(string hierarchyName)
        {
            switch (hierarchyName)
            {
                case HierarchyNames.Financial:
                    return "Test Financial HierarchyClass(1234)";
                case HierarchyNames.Brands:
                case HierarchyNames.National:
                default:
                    return $"Test {hierarchyName} 1";
            }
        }
    }
}