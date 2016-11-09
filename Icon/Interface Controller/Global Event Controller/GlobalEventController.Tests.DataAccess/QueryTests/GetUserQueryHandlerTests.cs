using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using Icon.Testing.Builders;
using GlobalEventController.DataAccess.Queries;
using Irma.Framework;
using System.Collections.Generic;

namespace GlobalEventController.Tests.DataAccess.QueryTests
{
    [TestClass]
    public class GetUserQueryHandlerTests
    {
        private IrmaContext context;
        private GetUserQueryHandler queryHandler;

        [TestInitialize]
        public void InitializeData()
        {
            this.context = new IrmaContext();
            this.queryHandler = new GetUserQueryHandler(this.context);
        }

        [TestCleanup]
        public void CleanupData()
        {
            this.context.Dispose();
        }

        [TestMethod]
        public void GetUserQuery_ExistingUsername_ReturnsUserWithMatchingUsername()
        {
            string targetUser = "SYSTEM";
            // Given
            GetUserQuery query = new GetUserQuery { UserName = targetUser };

            // When
            Users actual = queryHandler.Handle(query);

            // Then
            Assert.IsNotNull(actual);
            // Correct user?
            
            Assert.AreEqual(
                actual.UserName.ToLower(),
                targetUser.ToLower(),
                string.Format("Expected username [{0}] does not match returned username [{1}].", targetUser, actual.UserName)
            );
        }

        [TestMethod]
        public void GetUserQuery_InvalidUser_NoUserObjectReturned()
        {
            string targetUser = "not.found.user";
            // Given
            GetUserQuery query = new GetUserQuery { UserName = targetUser };

            // When
            Users actual = queryHandler.Handle(query);

            // Then
            Assert.IsNull(actual,
                string.Format("We expected no user object to be returned for invalid username [{0}], but we got a user object.", targetUser)
            );
        }
    }
}
