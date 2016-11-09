using GlobalEventController.Common;
using GlobalEventController.DataAccess.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubteamEventController.Tests.DataAccess
{
    public class TestItemSubTeamModelBuilder
    {
        private int itemId;
        private string validationDate;
        private string scanCode;
        private string scanCodeType;
        private string subTeamName;
        private int subTeamNo;
        private int deptNo;
        private bool subTeamNotAligned;

        public TestItemSubTeamModelBuilder()
        {
            this.itemId = 1;
            this.validationDate = DateTime.Now.ToString();
            this.scanCode = "99881122771";
            this.scanCodeType = "UPC";
            this.subTeamName = "test (4242)";
            this.subTeamNo = 4242;
            this.deptNo = 100;
        }

        public TestItemSubTeamModelBuilder WithItemId(int itemId)
        {
            this.itemId = itemId;
            return this;
        }

        public TestItemSubTeamModelBuilder WithValidationDate(string validationDate)
        {
            this.validationDate = validationDate;
            return this;
        }

        public TestItemSubTeamModelBuilder WithScanCode(string scanCode)
        {
            this.scanCode = scanCode;
            return this;
        }

        public TestItemSubTeamModelBuilder WithScanCodeType(string scanCodeType)
        {
            this.scanCodeType = scanCodeType;
            return this;
        }

        public TestItemSubTeamModelBuilder WithSubTeamName(string SubTeamName)
        {
            this.subTeamName = SubTeamName;
            return this;
        }
        public TestItemSubTeamModelBuilder WithSubTeamNo(int SubTeamNo)
        {
            this.subTeamNo = SubTeamNo;
            return this;
        }

        public TestItemSubTeamModelBuilder WithDeptNo(int DeptNo)
        {
            this.deptNo = DeptNo;
            return this;
        }

        public TestItemSubTeamModelBuilder WithSubTeamNotAligned(bool subTeamNotAligned)
        {
            this.subTeamNotAligned = subTeamNotAligned;
            return this;
        }

        public ItemSubTeamModel Build()
        {
            ItemSubTeamModel validatedItem = new ItemSubTeamModel
            {
                ItemId = this.itemId,
                ValidationDate = this.validationDate,
                ScanCode = this.scanCode,
                ScanCodeType = this.scanCodeType,
                SubTeamName = this.subTeamName,
                SubTeamNo = this.subTeamNo,
                DeptNo = this.deptNo,
                SubTeamNotAligned = this.subTeamNotAligned
            };

            return validatedItem;
        }
    }
}
