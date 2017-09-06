using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSupport.ViewModels;

namespace WebSupport.Tests.TestData
{
    public class PriceResetTestData
    {
        protected PriceResetRequestViewModel FakePriceResetRequestViewModelA = null;
        protected PriceResetRequestViewModel FakePriceResetRequestViewModelB = null;

        public List<PriceResetRequestViewModel> AllPriceResetRequestViewModels
        {
            get
            {
                return new List<PriceResetRequestViewModel>() {
                    FakePriceResetRequestViewModelA,
                    FakePriceResetRequestViewModelB
                };
            }
        }

        public StoreViewModel[] StoreViewModels = new StoreViewModel[5] {
                new StoreViewModel { BusinessUnit="10000", Abbreviation="XYZ", Name="Test Store 1" },
                new StoreViewModel { BusinessUnit="10010", Abbreviation="ABC", Name="the abc (ABC)" },
                new StoreViewModel { BusinessUnit="10100", Abbreviation="NAA", Name="nnnnnnnnnn" },
                new StoreViewModel { BusinessUnit="11411", Abbreviation="ANH", Name="ahed31" },
                new StoreViewModel { BusinessUnit="10555", Abbreviation="BTH", Name="blah blah (BTH)" }
            };

        public const string NewlineSeparatedScanCodes =
@"46000063225
4282342774
81387901082
481501
4171
5958415578
488898
46000051606
1069932045204
9232500006
7341640241
27939900000
89849500107
9948228724
23717050";

        public const string NewlineSeparatedScanCodesWithBlankLinesAndWhitespace =
@"46000063225
4282342774 
81387901082
481501
4171

5958415578 
488898
46000051606
1069932045204
9232500006
    7341640241
27939900000

89849500107
9948228724
23717050";
    }
}
