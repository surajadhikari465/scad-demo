using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Irma.Framework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using GlobalEventController.DataAccess.Commands;

namespace GlobalEventController.Tests.DataAccess.CommandTests
{
	[TestClass]
	public class AddValidatedScanCodeCommandHandlerTests
	{
		private IrmaContext context;
		private AddValidatedScanCodeCommand command;
		private AddValidatedScanCodeCommandHandler handler;

		[TestInitialize]
		public void InitializeData()
		{
			this.context = new IrmaContext(); // empty constructor uses idd-fl\fld
			this.command = new AddValidatedScanCodeCommand();
			this.handler = new AddValidatedScanCodeCommandHandler(this.context);
		}

		[TestMethod]
		public void AddValidatedScanCode_ScanCodesAlreadyExistInValidatedScanCodeTable_NoScanCodesAreAdded()
		{
			// Given
			List<string> scanCodes = FindScanCodesInValidatedScanCodeTable(3);
			this.command.ScanCodes = scanCodes;
			List<ValidatedScanCode> expectedList = this.context.ValidatedScanCode.Where(vsc => scanCodes.Contains(vsc.ScanCode)).ToList();

			// When
			this.handler.Handle(this.command);
			this.context.SaveChanges();

			// Then
			IEnumerable<ValidatedScanCode> actualList = this.context.ValidatedScanCode.Where(vsc => scanCodes.Contains(vsc.ScanCode));
			Assert.IsTrue(actualList.Count() == scanCodes.Count);
			for (int i = 0; i < actualList.Count(); i++)
			{
				ValidatedScanCode expected = expectedList[i];
				ValidatedScanCode actual = actualList.First(a => a.ScanCode == expected.ScanCode);
				Assert.AreEqual(expected.InsertDate, actual.InsertDate, "The InsertDate of the existing ValidatedScanCode changed when it was not supposed to.");
				Assert.AreEqual(expected.Id, actual.Id, "The Id of the existing ValidatedScanCode changed.");
			}
		}

		[TestMethod]
		public void AddValidatedScanCode_SubsetOfScanCodesAlreadyExistInValidatedScanCodeTable_NonExistingScanCodesAreAdded()
		{
			// Given
			List<string> existingValidated = FindScanCodesInValidatedScanCodeTable(2);
			List<string> needValidation = FindScanCodesNotInValidatedScanCodeTable(2);
			List<ValidatedScanCode> expectedExistingValidated = this.context.ValidatedScanCode.Where(vsc => existingValidated.Contains(vsc.ScanCode)).ToList();

			this.command.ScanCodes = existingValidated.Union(needValidation).ToList();
			DateTime preTestDateTime = DateTime.Now;
			System.Threading.Thread.Sleep(1000);

			// When
			this.handler.Handle(this.command);
			this.context.SaveChanges();

			// Then
			for (int i = 0; i < existingValidated.Count; i++)
			{
				ValidatedScanCode expectedExisting = expectedExistingValidated[i];
				ValidatedScanCode actualExisting = this.context.ValidatedScanCode.First(vsc => vsc.ScanCode == expectedExisting.ScanCode);
				Assert.AreEqual(expectedExisting.InsertDate, actualExisting.InsertDate, "The InsertDate of the ValidatedScanCode unexpectedly changed.");
				Assert.AreEqual(expectedExisting.Id, actualExisting.Id, "The Id of the ValidatedScanCode is unexpectedly different.");

			}

			for (int i = 0; i < needValidation.Count; i++)
			{
				string expectedNeedValidation = needValidation[i];
				ValidatedScanCode actualNeedValidation = this.context.ValidatedScanCode.First(vsc => vsc.ScanCode == expectedNeedValidation);
				Assert.AreEqual(expectedNeedValidation, actualNeedValidation.ScanCode, "The ScanCode does not match the expected value.");
				Assert.IsTrue(actualNeedValidation.InsertDate > preTestDateTime, "The InsertDate was not added as expected.");
			}
		}

		[TestMethod]
		public void AddValidatedScanCode_ScanCodesDoNotExistInValidatedScanCodeTable_AllScanCodesAreAdded()
		{
			// Given
			List<string> needValidation = FindScanCodesNotInValidatedScanCodeTable(3);
			this.command.ScanCodes = needValidation;
			DateTime preTestDateTime = DateTime.Now;
			System.Threading.Thread.Sleep(1000);

			// When
			this.handler.Handle(this.command);
			this.context.SaveChanges();

			// Then
			for (int i = 0; i < needValidation.Count; i++)
			{
				string expected = needValidation[i];
				ValidatedScanCode actual = this.context.ValidatedScanCode.First(vsc => vsc.ScanCode == expected);
				Assert.AreEqual(expected, actual.ScanCode, "The ScanCode does not match the expected value.");
				Assert.IsTrue(actual.InsertDate > preTestDateTime, "The InsertDate was not added with the expected value.");
			}
		}

		private ValidatedScanCode BuildValidatedScanCode(string scanCode)
		{
			ValidatedScanCode validatedScanCode = new ValidatedScanCode();
			validatedScanCode.ScanCode = scanCode;
			validatedScanCode.InsertDate = DateTime.Now;

			return validatedScanCode;
		}

		private List<string> FindScanCodesNotInValidatedScanCodeTable(int maxRows)
		{
			IEnumerable<string> existingValidatedScanCodes = this.context.ValidatedScanCode.Select(vsc => vsc.ScanCode);
			List<string> scanCodes = this.context.ItemIdentifier
				.Where(ii => ii.Deleted_Identifier == 0 && ii.Default_Identifier == 1 && ii.Item.Retail_Sale == true)
				.Select(ii => ii.Identifier)
				.Except(existingValidatedScanCodes)
				.Take(maxRows)
				.ToList();

			return scanCodes;
		}

		private List<string> FindScanCodesInValidatedScanCodeTable(int maxRows)
		{
			List<string> scanCodes = new List<string>();
			scanCodes = this.context.ValidatedScanCode.Select(vsc => vsc.ScanCode).Take(maxRows).ToList();

			if (scanCodes.Count == 0)
			{
				List<ValidatedScanCode> validatedScanCodeList = new List<ValidatedScanCode>();
				for (int i = 1; i < maxRows; i++)
				{
					ValidatedScanCode validatedScanCode = new ValidatedScanCode();
					validatedScanCode.ScanCode = "44995588667" + i.ToString();
					validatedScanCode.InsertDate = DateTime.Now;
					validatedScanCodeList.Add(validatedScanCode);
				}

				scanCodes = validatedScanCodeList.Select(vsc => vsc.ScanCode).ToList();
			}

			return scanCodes;
		}
	}
}