using KitBuilderWebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.Extensions.Configuration;
using KitBuilderWebApi.Models;

namespace KitBuilderWebApi.Tests.Controllers
{
	[TestClass]
	public class AuthenticationTest
	{

		private Authentication authentication;
		private Mock<ILogger<Authentication>> mockLogger;
		private Mock<IConfiguration> mockConfiguration;
		private LoginParameters loginParameters;

		[TestInitialize]
		public void InitializeTest()
		{
			mockLogger = new Mock<ILogger<Authentication>>();
			mockConfiguration = new Mock<IConfiguration>();

			authentication = new Authentication(mockConfiguration.Object, mockLogger.Object);
		}

		private void SetUpData()
		{
			loginParameters = new LoginParameters()
			{
				Username = "WFM\\KBTestUser",
				Password = "TestPwd123"
			};

		}

		[TestMethod]
		public void Authentication_UserNotAuthenticated_ReturnsUnauthenticated()
		{
			SetUpData();

			var response = authentication.Login(loginParameters);
			Assert.IsTrue(((OkObjectResult)response).Value.ToString().Contains("Unauthenticated"), "Unauthenticated User Issue");
		}
	}
}