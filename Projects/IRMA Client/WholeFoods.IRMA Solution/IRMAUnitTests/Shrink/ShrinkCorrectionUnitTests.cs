using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WholeFoods.IRMA;


namespace IRMAUnitTests
{
	[TestClass]
	public class ShrinkCorrectionUnitTests
	{
		[TestMethod]
		public void EditShrinkSameFiscalPeriodAsAdmin()
		{
			// User Role
			bool editRole = false;
			bool userShrinkAdmin = true;
			string currentUser = "John.Doe";
			string spoilageUser = "Jane.Smith";

			if (userShrinkAdmin)
			{
				editRole = true;
			}
			else if ((currentUser.ToLower()) == (spoilageUser.ToLower()))
			{
				editRole = true;
			}

			// Shrink Information
			int shrinkFP = 8;
			int shrinkFY = 2013;

			// Current Day Information
			DateTime today = new DateTime(2013, 5, 2);
			int todayFP = 8;
			int todayFW = 4;
			int todayFY = 2013;
			int todayDayOfYear = 4;

			bool editRow = false;

			if ((todayFY - shrinkFY == 1) & (todayDayOfYear == 1) & (shrinkFP == 13) & editRole)
			{
				editRow = true;
			}
			else if (todayFY == shrinkFY & editRole)
			{
				if (todayFP == shrinkFP)
				{
					editRow = true;
				}
				else if (todayFP - shrinkFP == 1)
				{
					if (today.DayOfWeek == DayOfWeek.Monday & todayFW == 1)
					{
						editRow = true;
					}
				}

			}

			Assert.AreEqual(true, editRow);
		}

		[TestMethod]
		public void CannotEditShrinkPreviousFiscalPeriodNotMondayWeek1AsAdmin()
		{
			// User Role
			bool editRole = false;
			bool userShrinkAdmin = true;
			string currentUser = "John.Doe";
			string spoilageUser = "Jane.Smith";

			if (userShrinkAdmin)
			{
				editRole = true;
			}
			else if ((currentUser.ToLower()) == (spoilageUser.ToLower()))
			{
				editRole = true;
			}

			// Shrink Information
			int shrinkFP = 7;
			int shrinkFY = 2013;

			// Current Day Information
			DateTime today = new DateTime(2013, 5, 2);
			int todayFP = 8;
			int todayFW = 4;
			int todayFY = 2013;
			int todayDayOfYear = 4;

			bool editRow = false;

			if ((todayFY - shrinkFY == 1) & (todayDayOfYear == 1) & (shrinkFP == 13) & editRole)
			{
				editRow = true;
			}
			else if (todayFY == shrinkFY & editRole)
			{
				if (todayFP == shrinkFP)
				{
					editRow = true;
				}
				else if (todayFP - shrinkFP == 1)
				{
					if (today.DayOfWeek == DayOfWeek.Monday & todayFW == 1)
					{
						editRow = true;
					}
				}

			}

			Assert.AreNotEqual(true, editRow);
		}

		[TestMethod]
		public void CanEditShrinkPreviousFPMondayWeek1AsAdmin()
		{
			// User Role
			bool editRole = false;
			bool userShrinkAdmin = true;
			string currentUser = "John.Doe";
			string spoilageUser = "Jane.Smith";

			if (userShrinkAdmin)
			{
				editRole = true;
			}
			else if ((currentUser.ToLower()) == (spoilageUser.ToLower()))
			{
				editRole = true;
			}

			// Shrink Information
			int shrinkFP = 7;
			int shrinkFY = 2013;

			// Current Day Information
			DateTime today = new DateTime(2013, 4, 15);
			int todayFP = 8;
			int todayFW = 1;
			int todayFY = 2013;
			int todayDayOfYear = 197;

			bool editRow = false;

			if ((todayFY - shrinkFY == 1) & (todayDayOfYear == 1) & (shrinkFP == 13) & editRole)
			{
				editRow = true;
			}
			else if (todayFY == shrinkFY & editRole)
			{
				if (todayFP == shrinkFP)
				{
					editRow = true;
				}
				else if (todayFP - shrinkFP == 1)
				{
					if (today.DayOfWeek == DayOfWeek.Monday & todayFW == 1)
					{
						editRow = true;
					}
				}

			}

			Assert.AreEqual(true, editRow);
		}

		[TestMethod]
		public void CannotEditShrinkSameFPDifferentFYAsAdmin()
		{
			// User Role
			bool editRole = false;
			bool userShrinkAdmin = true;
			string currentUser = "John.Doe";
			string spoilageUser = "Jane.Smith";

			if (userShrinkAdmin)
			{
				editRole = true;
			}
			else if ((currentUser.ToLower()) == (spoilageUser.ToLower()))
			{
				editRole = true;
			}

			// Shrink Information
			int shrinkFP = 8;
			int shrinkFY = 2012;

			// Current Day Information
			DateTime today = new DateTime(2013, 4, 18);
			int todayFP = 8;
			int todayFW = 1;
			int todayFY = 2013;
			int todayDayOfYear = 200;

			bool editRow = false;

			if ((todayFY - shrinkFY == 1) & (todayDayOfYear == 1) & (shrinkFP == 13) & editRole)
			{
				editRow = true;
			}
			else if (todayFY == shrinkFY & editRole)
			{
				if (todayFP == shrinkFP)
				{
					editRow = true;
				}
				else if (todayFP - shrinkFP == 1)
				{
					if (today.DayOfWeek == DayOfWeek.Monday & todayFW == 1)
					{
						editRow = true;
					}
				}

			}

			Assert.AreNotEqual(true, editRow);
		}

		[TestMethod]
		public void CanEditShrinkPreviousFPFirstDayOfFYAsAdmin()
		{
			// User Role
			bool editRole = false;
			bool userShrinkAdmin = true;
			string currentUser = "John.Doe";
			string spoilageUser = "Jane.Smith";

			if (userShrinkAdmin)
			{
				editRole = true;
			}
			else if ((currentUser.ToLower()) == (spoilageUser.ToLower()))
			{
				editRole = true;
			}

			// Shrink Information
			int shrinkFP = 13;
			int shrinkFY = 2013;

			// Current Day Information
			DateTime today = new DateTime(2013, 9, 30);
			int todayFP = 1;
			int todayFW = 1;
			int todayFY = 2014;
			int todayDayOfYear = 1;

			bool editRow = false;

			if ((todayFY - shrinkFY == 1) & (todayDayOfYear == 1) & (shrinkFP == 13) & editRole)
			{
				editRow = true;
			}
			else if (todayFY == shrinkFY & editRole)
			{
				if (todayFP == shrinkFP)
				{
					editRow = true;
				}
				else if (todayFP - shrinkFP == 1)
				{
					if (today.DayOfWeek == DayOfWeek.Monday & todayFW == 1)
					{
						editRow = true;
					}
				}

			}

			Assert.AreEqual(true, editRow);
		}


		// Shrink Only Role
		[TestMethod]
		public void EditShrinkSameFiscalPeriodAsShrinkOnly()
		{
			// User Role
			bool editRole = false;
			bool userShrinkAdmin = false;
			string currentUser = "John.Doe";
			string spoilageUser = "John.Doe";

			if (userShrinkAdmin)
			{
				editRole = true;
			}
			else if ((currentUser.ToLower()) == (spoilageUser.ToLower()))
			{
				editRole = true;
			}

			// Shrink Information
			int shrinkFP = 8;
			int shrinkFY = 2013;

			// Current Day Information
			DateTime today = new DateTime(2013, 5, 2);
			int todayFP = 8;
			int todayFW = 4;
			int todayFY = 2013;
			int todayDayOfYear = 4;

			bool editRow = false;

			if ((todayFY - shrinkFY == 1) & (todayDayOfYear == 1) & (shrinkFP == 13) & editRole)
			{
				editRow = true;
			}
			else if (todayFY == shrinkFY & editRole)
			{
				if (todayFP == shrinkFP)
				{
					editRow = true;
				}
				else if (todayFP - shrinkFP == 1)
				{
					if (today.DayOfWeek == DayOfWeek.Monday & todayFW == 1)
					{
						editRow = true;
					}
				}

			}

			Assert.AreEqual(true, editRow);
		}

		[TestMethod]
		public void EditShrinkDifferentUserSameFiscalPeriodAsShrinkOnly()
		{
			// User Role
			bool editRole = false;
			bool userShrinkAdmin = false;
			string currentUser = "John.Doe";
			string spoilageUser = "Jane.Smith";

			if (userShrinkAdmin)
			{
				editRole = true;
			}
			else if ((currentUser.ToLower()) == (spoilageUser.ToLower()))
			{
				editRole = true;
			}

			// Shrink Information
			int shrinkFP = 8;
			int shrinkFY = 2013;

			// Current Day Information
			DateTime today = new DateTime(2013, 5, 2);
			int todayFP = 8;
			int todayFW = 4;
			int todayFY = 2013;
			int todayDayOfYear = 4;

			bool editRow = false;

			if ((todayFY - shrinkFY == 1) & (todayDayOfYear == 1) & (shrinkFP == 13) & editRole)
			{
				editRow = true;
			}
			else if (todayFY == shrinkFY & editRole)
			{
				if (todayFP == shrinkFP)
				{
					editRow = true;
				}
				else if (todayFP - shrinkFP == 1)
				{
					if (today.DayOfWeek == DayOfWeek.Monday & todayFW == 1)
					{
						editRow = true;
					}
				}

			}

			Assert.AreNotEqual(true, editRow);
		}

		[TestMethod]
		public void CannotEditShrinkPreviousFiscalPeriodNotMondayWeek1AsShrinkOnly()
		{
			// User Role
			bool editRole = false;
			bool userShrinkAdmin = false;
			string currentUser = "John.Doe";
			string spoilageUser = "John.Doe";

			if (userShrinkAdmin)
			{
				editRole = true;
			}
			else if ((currentUser.ToLower()) == (spoilageUser.ToLower()))
			{
				editRole = true;
			}

			// Shrink Information
			int shrinkFP = 7;
			int shrinkFY = 2013;

			// Current Day Information
			DateTime today = new DateTime(2013, 5, 2);
			int todayFP = 8;
			int todayFW = 4;
			int todayFY = 2013;
			int todayDayOfYear = 4;

			bool editRow = false;

			if ((todayFY - shrinkFY == 1) & (todayDayOfYear == 1) & (shrinkFP == 13) & editRole)
			{
				editRow = true;
			}
			else if (todayFY == shrinkFY & editRole)
			{
				if (todayFP == shrinkFP)
				{
					editRow = true;
				}
				else if (todayFP - shrinkFP == 1)
				{
					if (today.DayOfWeek == DayOfWeek.Monday & todayFW == 1)
					{
						editRow = true;
					}
				}

			}

			Assert.AreNotEqual(true, editRow);
		}

		[TestMethod]
		public void CanEditShrinkPreviousFPMondayWeek1AsShrinkOnly()
		{
			// User Role
			bool editRole = false;
			bool userShrinkAdmin = false;
			string currentUser = "John.Doe";
			string spoilageUser = "John.Doe";

			if (userShrinkAdmin)
			{
				editRole = true;
			}
			else if ((currentUser.ToLower()) == (spoilageUser.ToLower()))
			{
				editRole = true;
			}

			// Shrink Information
			int shrinkFP = 7;
			int shrinkFY = 2013;

			// Current Day Information
			DateTime today = new DateTime(2013, 4, 15);
			int todayFP = 8;
			int todayFW = 1;
			int todayFY = 2013;
			int todayDayOfYear = 197;

			bool editRow = false;

			if ((todayFY - shrinkFY == 1) & (todayDayOfYear == 1) & (shrinkFP == 13) & editRole)
			{
				editRow = true;
			}
			else if (todayFY == shrinkFY & editRole)
			{
				if (todayFP == shrinkFP)
				{
					editRow = true;
				}
				else if (todayFP - shrinkFP == 1)
				{
					if (today.DayOfWeek == DayOfWeek.Monday & todayFW == 1)
					{
						editRow = true;
					}
				}

			}

			Assert.AreEqual(true, editRow);
		}

		[TestMethod]
		public void CannotEditShrinkSameFPDifferentFYAsShrinkOnly()
		{
			// User Role
			bool editRole = false;
			bool userShrinkAdmin = false;
			string currentUser = "John.Doe";
			string spoilageUser = "John.Doe";

			if (userShrinkAdmin)
			{
				editRole = true;
			}
			else if ((currentUser.ToLower()) == (spoilageUser.ToLower()))
			{
				editRole = true;
			}

			// Shrink Information
			int shrinkFP = 8;
			int shrinkFY = 2012;

			// Current Day Information
			DateTime today = new DateTime(2013, 4, 18);
			int todayFP = 8;
			int todayFW = 1;
			int todayFY = 2013;
			int todayDayOfYear = 200;

			bool editRow = false;

			if ((todayFY - shrinkFY == 1) & (todayDayOfYear == 1) & (shrinkFP == 13) & editRole)
			{
				editRow = true;
			}
			else if (todayFY == shrinkFY & editRole)
			{
				if (todayFP == shrinkFP)
				{
					editRow = true;
				}
				else if (todayFP - shrinkFP == 1)
				{
					if (today.DayOfWeek == DayOfWeek.Monday & todayFW == 1)
					{
						editRow = true;
					}
				}

			}

			Assert.AreNotEqual(true, editRow);
		}

		[TestMethod]
		public void CanEditShrinkPreviousFPFirstDayOfFYAsShrinkOnly()
		{
			// User Role
			bool editRole = false;
			bool userShrinkAdmin = false;
			string currentUser = "John.Doe";
			string spoilageUser = "John.Doe";

			if (userShrinkAdmin)
			{
				editRole = true;
			}
			else if ((currentUser.ToLower()) == (spoilageUser.ToLower()))
			{
				editRole = true;
			}

			// Shrink Information
			int shrinkFP = 13;
			int shrinkFY = 2013;

			// Current Day Information
			DateTime today = new DateTime(2013, 9, 30);
			int todayFP = 1;
			int todayFW = 1;
			int todayFY = 2014;
			int todayDayOfYear = 1;

			bool editRow = false;

			if ((todayFY - shrinkFY == 1) & (todayDayOfYear == 1) & (shrinkFP == 13) & editRole)
			{
				editRow = true;
			}
			else if (todayFY == shrinkFY & editRole)
			{
				if (todayFP == shrinkFP)
				{
					editRow = true;
				}
				else if (todayFP - shrinkFP == 1)
				{
					if (today.DayOfWeek == DayOfWeek.Monday & todayFW == 1)
					{
						editRow = true;
					}
				}

			}

			Assert.AreEqual(true, editRow);
		}
	}
}
