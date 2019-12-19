﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WFM.Nexus.BTS.OOS.ScanProduct.UnitTests.ScanProductAsyncReference;

namespace WFM.Nexus.BTS.OOS.ScanProduct.UnitTests
{
    [TestClass()]
    public class ScanProductTests
    {
        private BTS_OOS_Orch_ScanProduct_ScanProductAsyncClient asyncClient;
        private List<IScanProductAsyncTest> scanProductAsyncTests;
        private const int Upper = 5;

        [TestInitialize()]
        public void Setup()
        {
            scanProductAsyncTests = CreateSystemUnderTest(Upper);
        }

        [TestMethod()]
        public void ScanProductDeployedLocalMachine()
        {
            ScanProductLocal();
        }

        private void ScanProductLocal()
        {
            var client = new BTS_OOS_Orch_ScanProduct_ScanProductAsyncClient();
            var sp = CreateScanProductRequest();
            client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Anonymous;
            client.ScanProductsByStoreAbbreviation(sp.ScanProductsByStoreAbbreviation);
        }


        [TestMethod()]
        public void ScanProductAsync()
        {
            asyncClient = new BTS_OOS_Orch_ScanProduct_ScanProductAsyncClient();
            var scanProductRequest = CreateScanProductRequest();
            object asyncState = null;
            var asyncResult = asyncClient.BeginScanProductsByStoreAbbreviation(scanProductRequest.ScanProductsByStoreAbbreviation, Callback, asyncState);
            asyncClient.EndScanProductsByStoreAbbreviation(asyncResult);
        }

        private void Callback(IAsyncResult ar)
        {
            Console.WriteLine("Completed=" + ar.IsCompleted);
        }

        private ScanProductsByStoreAbbreviationRequest CreateScanProductRequest()
        {
            ScanProductsByStoreAbbreviationRequest request = new ScanProductsByStoreAbbreviationRequest();
            request.ScanProductsByStoreAbbreviation = new ScanProductsByStoreAbbreviation
            {
                scanDate = DateTime.Now,
                scanDateSpecified = true,
                storeAbbrev = "BCA",
                regionAbbrev = "FL",
                upcs = new[] { "0009948241284", "0009948241487" }
            };
            return request;
        }

        private List<IScanProductAsyncTest> CreateSystemUnderTest(int upper)
        {
            var scanProductAsyncTests = new List<IScanProductAsyncTest>();
            for (var i = 0; i < upper; i++)
            {
                var request = CreateScanProductRequestForSut("NPL", "FL", ScannedUpcs());
                scanProductAsyncTests.Add(new ScanProductAsyncTest(i, request));
            }
            return scanProductAsyncTests;
        }

        private ScanProductsByStoreAbbreviationRequest CreateScanProductRequestForSut(string storeAbbrev, string regionAbbrev, string[] upcs)
        {
            ScanProductsByStoreAbbreviationRequest request = new ScanProductsByStoreAbbreviationRequest();
            request.ScanProductsByStoreAbbreviation = new ScanProductsByStoreAbbreviation
            {
                scanDate = DateTime.Now,
                scanDateSpecified = true,
                storeAbbrev = storeAbbrev,
                regionAbbrev = regionAbbrev,
                upcs = upcs
            };
            return request;
        }


        private string[] ScannedUpcs()
        {
            return new[]
            {
                "1101374007",
                "1121002365",
                "1121002365",
                "1251121232",
                "1356230055",
                "1356230220",
                "1356230223",
                "1356230261",
                "1356240002",
                "1362860887",
                "1397100002",
                "1553210030",
                "1553210073",
                "1583900797",
                "1645920011",
                "1645920033",
                "18012700000",
                "18012700001",
                "18012700002",
                "18012700002",
                "18012700010",
                "18274100001",
                "18274100082",
                "18450000003",
                "18581400021",
                "1862741446",
                "1862755106",
                "1862774000",
                "1862774200",
                "18824400014",
                "18824400020",
                "1935655111",
                "2066200022",
                "2169806200",
                "2190829102",
                "2190840772",
                "2250610260",
                "2250613806",
                "2250613807",
                "2250620232",
                "2250631692",
                "2253105302",
                "2392310044",
                "2392310131",
                "2392320426",
                "2392320428",
                "2392333011",
                "2392390131",
                "2418249115",
                "2618110201",
                "2618110204",
                "2727111928",
                "2840009237",
                "3121592347",
                "3301700184",
                "3574222108",
                "3574222912",
                "3619212215",
                "3619212700",
                "3701426201",
                "3701427034",
                "3877882016",
                "3877882032",
                "3904700145",
                "3997800125",
                "3997800141",
                "3997800143",
                "3997800375",
                "3997800603",
                "3997800642",
                "3997800679",
                "3997800815",
                "3997800911",
                "3997801952",
                "3997802321",
                "4127734990",
                "4127850909",
                "4133112466",
                "4150880070",
                "4157005278",
                "4161700285",
                "4217519600",
                "4217519741",
                "4227200540",
                "4227200561",
                "4227200719",
                "4260847055",
                "4318200209",
                "4318200520",
                "4318200521",
                "4318200525",
                "4318200591",
                "4318200833",
                "4364620758",
                "4364768003",
                "4790001602",
                "4950800500",
                "4950800501",
                "5200050024",
                "5260304184",
                "5565361880",
                "5844945003",
                "5844962002",
                "5844977020",
                "5844986004",
                "5965417001",
                "60265214001",
                "60418312170",
                "60418312191",
                "60418312192",
                "62013300362",
                "6205881633",
                "63172300610",
                "63888200203",
                "64850514250",
                "64850530250",
                "64850531250",
                "64850550721",
                "65722700033",
                "65856423051",
                "6667682253",
                "66745000111",
                "67195900003",
                "67810817101",
                "67852301051",
                "6927607031",
                "69765840012",
                "69899780618",
                "70180930012",
                "7057366666",
                "7057366666",
                "7061700155",
                "7061700167",
                "70617301011",
                "7067000289",
                "7079621013",
                "70816311548",
                "70840631714",
                "7084400453",
                "70875800101",
                "7107201248",
                "7110500001",
                "71138100044",
                "7114600245",
                "71612312561",
                "71612312585",
                "71725600039",
                "71725600061",
                "72037950127",
                "72051602100",
                "72133282005",
                "72133282007",
                "72225219208",
                "72225238106",
                "7225100211",
                "7246300024",
                "72663512127",
                "72802800942",
                "72822901325",
                "72822978922",
                "72899713600",
                "7293497111",
                "73032306048",
                "7321400607",
                "7321400947",
                "73291322170",
                "73291322766",
                "73291322769",
                "73402790502",
                "7341600039",
                "73486510301",
                "7349013191",
                "73788006130",
                "73853773000",
                "73853776600",
                "73853776900",
                "73922300157",
                "73922300158",
                "7405311502",
                "7430505116",
                "74323107311",
                "7433347145",
                "7433347665",
                "7440161041",
                "74447391237",
                "74599890302",
                "7468210309",
                "7468210350",
                "7468210780",
                "7468210782",
                "7468210786",
                "7468211533",
                "74752520064",
                "74759960699",
                "74759962018",
                "7478000005",
                "7478000009",
                "7478545288",
                "74840400003",
                "74840400006",
                "74840428813",
                "74840431982",
                "7487396015",
                "7523900110",
                "75535500100",
                "7581000015",
                "7581000016",
                "7581002135",
                "7581002335",
                "75928300036",
                "75928300092",
                "75928310063",
                "75928320004",
                "75928320008",
                "75928320024",
                "7613205016",
                "7630123001",
                "7630184002",
                "76566750030",
                "76566762700",
                "76566762859",
                "7658000488",
                "76733501100",
                "76733501102",
                "76733502002",
                "7707523050",
                "7724150060",
                "7750212610",
                "78087207853",
                "78087208305",
                "78087209356",
                "78273300004",
                "78273300026",
                "78273300027",
                "78273301211",
                "78273301211",
                "78273301214",
                "7831421175",
                "79055501180",
                "79055501182",
                "79055506152",
                "79323211171",
                "79357360117",
                "79439800011",
                "79439800020",
                "79921066663",
                "79921082501",
                "80056570114",
                "80056570135",
                "80487923265",
                "80599300021",
                "80812411118",
                "80812412103",
                "80812414134",
                "80812414831",
                "81336900023",
                "81336900107",
                "81355100112",
                "81355100211",
                "81793900004",
                "81793900923",
                "8266670020",
                "827400009",
                "827411111",
                "82969600054",
                "82973900052",
                "83845500001",
                "8411411097",
                "8411411585",
                "8411411717",
                "8411412105",
                "8425322203",
                "8425322231",
                "8425322240",
                "8425324006",
                "8425324007",
                "8425326820",
                "84357100209",
                "84357100324",
                "8438095724",
                "8438095784",
                "8438095904",
                "8505361084",
                "8505361084",
                "85108700005",
                "85108700006",
                "85108700025",
                "85193700213",
                "85225800301",
                "85269700141",
                "85323100300",
                "85343400204",
                "85343400217",
                "85343400218",
                "85352200016",
                "85413700020",
                "85413700050",
                "85413700055",
                "85428550010",
                "85428550014",
                "85577200102",
                "85616100215",
                "85616100216",
                "85682000060",
                "85682000460",
                "85682016009",
                "85832000002",
                "85832876218",
                "85850300101",
                "8705271279",
                "8775412002",
                "87989000011",
                "88424400510",
                "88424411511",
                "88428404125",
                "88462348623",
                "8870201560",
                "89000000110",
                "89000000111",
                "89000000112",
                "89044400056",
                "89147500101",
                "89195300100",
                "89347000102",
                "89375100015",
                "89375100033",
                "89411200217",
                "89418500049",
                "89441000109",
                "89445500002",
                "89445500009",
                "89494300200",
                "89499100110",
                "89535200215",
                "89685900000",
                "89685900007",
                "89685900058",
                "89685900078",
                "89685900079",
                "89685900088",
                "89685900090",
                "89741500214",
                "89758000013",
                "89758000015",
                "89758000054",
                "89819500101",
                "8983618415",
                "8983618551",
                "8983618843",
                "89840300202",
                "89857500133",
                "89859600108",
                "89859600115",
                "89859600172",
                "89873900108",
                "89903300203",
                "89903300204",
                "89912100115",
                "89912100120",
                "9034124401",
                "9232533332",
                "9321580100",
                "9546900713",
                "9690602324",
                "9830800282",
                "9830800284",
                "9948240315",
                "9948240624",
                "9948240656",
                "9948240690",
                "9948240886",
                "9948241091",
                "9948241092",
                "9948241098",
                "9948241121",
                "9948241237",
                "9948241367",
                "9948241475",
                "9948241490",
                "9948241491",
                "9948241673",
                "9948241795",
                "9948241905",
                "9948241977",
                "9948242086",
                "9948242095",
                "9948242096",
                "9948242226",
                "9948242720",
                "9948242757",
                "9948242773",
                "9948242822",
                "9948242906",
                "9948243152",
                "9948243544",
                "9948243570",
                "9948243586",
                "9948243590",
                "9948243592",
                "9948243594",
                "9948243647",
                "9948243754",
                "9948243765",
                "9948243816",
                "9948243828",
                "9948243891",
                "9948243904",
                "9948243911",
                "9948243926",
                "9948244097",
                "1405406909",
                "1405408209",
                "1405413509",
                "15412",
                "2529360017",
                "2883306030",
                "4319231050",
                "4411531621",
                "4411533807",
                "4411533818",
                "4411533824",
                "4411533829",
                "5215900464",
                "5215953050",
                "5215959125",
                "60972268472",
                "64543301002",
                "65436765271",
                "65436765272",
                "65436765273",
                "65436765273",
                "65484481062",
                "66437270005",
                "66559601400",
                "68954408020",
                "69706859012",
                "7347200214",
                "74236520875",
                "74447393104",
                "75008870225",
                "75008870240",
                "75854315500",
                "76352830301",
                "7835551050",
                "7835554000",
                "79570902002",
                "8131210178",
                "8131250000",
                "8131250002",
                "8131250004",
                "81771901001",
                "81771901006",
                "81771901602",
                "81771901604",
                "81841100043",
                "85015700020",
                "85015700035",
                "85157000305",
                "85157000306",
                "85157000307",
                "8647950009",
                "8819434060",
                "89177000200",
                "9396600017",
                "9396600091",
                "9492211867",
                "9948240224",
                "9948240225",
                "9948241340",
                "9948241553",
                "9948241719",
                "9948241844",
                "9948242069",
                "9948243049",
                "9948243052",
                "9948243055",
                "9948243282",
                "9948243703",
                "1143311296",
                "2857100556",
                "2857100556",
                "4227200054",
                "4227200124",
                "5215900203",
                "71812281353",
                "71812281355",
                "7260973665",
                "7260974100",
                "74447344031",
                "74447344034",
                "74951248665",
                "7684020028",
                "79951201205",
                "81654601146",
                "8269111830",
                "83418300714",
                "83418300904",
                "8425344430",
                "8994730226",
                "89968400211",
                "9948240068",
                "9948240487",
                "9948240966",
                "9948241224",
                "9948243420",
                "9948243422",
                "9948243431",
                "9948243433",
                "9948243453",
                "3098500300",
                "60506920012",
                "68907692923",
                "71525620002",
                "72474200559",
                "79269200001",
                "79269200111",
                "79269200223",
                "79269233459",
                "79285000237",
                "79427125020",
                "89326800002",
                "2107801580",
                "3367415146",
                "3367415341",
                "3367415395",
                "3367415730",
                "60949271028",
                "60949272230",
                "61309883485",
                "63125752760",
                "63125753561",
                "65801011111",
                "72778304055",
                "73373903065",
                "73373903772",
                "74365000182",
                "74868923001",
                "7631430271",
                "77374350001",
                "79001103012",
                "79001116006",
                "79001116047",
                "79001121008",
                "81271101093",
                "83876600537",
                "83876600806",
                "83876600835",
                "85884700087",
                "9907100405",
                "9948228505",
                "9948242302",
                "85304900172",
                "2531710595",
                "73898527176",
                "61697309135",
                "9948241349",
                "9948241517",
                "1182610074",
                "1182630001",
                "1570021311",
                "3114200110",
                "325883102800",
                "4608512047",
                "4608542140",
                "6250510284",
                "6250511966",
                "7055170053",
                "75988531709",
                "76165790423",
                "77379802503",
                "7790167000",
                "800582501",
                "82764000105",
                "83571500105",
                "83571500107",
                "89175600001",
                "89175600004",
                "89221002079",
                "65625230016",
                "7017715412",
                "7073407060",
                "74267640053",
                "74267640059",
                "74267640066",
                "84610700790",
                "1432110300",
                "81242701032",
                "9948240767",
                "1820000833",
                "2556924161",
                "3876636120",
                "71155609010",
                "72383000017",
                "72695990120",
                "76317625022",
                "81994200013",
                "85308300302"
            };
        }

        

        [TestMethod()]
        public void ScanProductAsyncManyTimes()
        {
            for (var i = 0; i < Upper; i++)
            {
                scanProductAsyncTests[i].ScanProduct();
            }
            WaitUnless();
        }


        private void WaitUnless()
        {
            while (true)
            {
                var done = true;
                for (var i = 0; i < Upper; i++)
                {
                    if (done)
                        done = scanProductAsyncTests[i].IsCompleted;
                }
                if (done) break;
            }
        }

    }

    internal interface IScanProductAsyncTest
    {
        void ScanProduct();
        bool IsCompleted { get; }
    }

    internal class ScanProductAsyncTest : IScanProductAsyncTest
    {
        private ScanProductsByStoreAbbreviationRequest scanProductRequest;
        private IAsyncResult asyncResult;
        private int testOrder;

        internal ScanProductAsyncTest(int testOrder, ScanProductsByStoreAbbreviationRequest scanProductRequest)
        {
            this.testOrder = testOrder;
            this.scanProductRequest = scanProductRequest;
        }

        public void ScanProduct()
        {
            var dateTime = DateTime.Now;
            Console.WriteLine("Test " + testOrder + " has Started at " + dateTime);

            var asyncClient = new BTS_OOS_Orch_ScanProduct_ScanProductAsyncClient();
            asyncResult = asyncClient.BeginScanProductsByStoreAbbreviation(scanProductRequest.ScanProductsByStoreAbbreviation, Callback, asyncClient);
        }

        private void Callback(IAsyncResult asyncResult)
        {
            var asyncClient = (BTS_OOS_Orch_ScanProduct_ScanProductAsyncClient)asyncResult.AsyncState;
            asyncClient.EndScanProductsByStoreAbbreviation(asyncResult);

            var dateTime = DateTime.Now;
            Console.WriteLine("Test " + testOrder + " has Completed=" + asyncResult.IsCompleted + " at " + dateTime);
        }

        public bool IsCompleted { get { return asyncResult.IsCompleted; } }

    }

    internal class ScanProductAsyncTester
    {
        private int upper;
        private List<IScanProductAsyncTest> scanProductAsyncTests;

        public ScanProductAsyncTester(int upper)
        {
            this.upper = upper;
        }

        public ScanProductAsyncTester(List<IScanProductAsyncTest> tests)
        {
            scanProductAsyncTests = tests;
        }

        public void ScanProductAsync()
        {
            for (var i = 0; i < upper; i++)
            {
                scanProductAsyncTests[i].ScanProduct();
            }
            WaitUnless();

        }

        private void WaitUnless()
        {
            while (true)
            {
                var done = true;
                for (var i = 0; i < upper; i++)
                {
                    if (done)
                        done = scanProductAsyncTests[i].IsCompleted;
                }
                if (done) break;
            }
        }

    }
}
