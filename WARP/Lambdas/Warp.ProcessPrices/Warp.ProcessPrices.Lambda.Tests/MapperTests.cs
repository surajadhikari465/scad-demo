using Amazon.Lambda.SQSEvents;
using Xunit;

namespace Warp.ProcessPrices.Lambda.Tests
{

 
    public class MapperTests
    {
        [Fact]
        public void Mapper_ValidPriceMessage_ReturnsValidResponse()
        {
            var expectedGpmId = "0cb3d64f-48d1-4a32-8648-41b832731408";
            var mapper = new SqsToPriceMapper();
            var message = new SQSEvent.SQSMessage
            {
                Body = @"
                {
	                ""ItemId"": 1,
	                ""BusinessUnitId"": 1,
	                ""GpmId"": ""0cb3d64f-48d1-4a32-8648-41b832731408"",
	                ""ScanCode"": ""4023"",
	                ""StartDate"": ""2020-06-09 00:00:00"",
	                ""EndDate"": ""2020-06-09 23:59:59"",
	                ""Price"": 3.99,
	                ""PercentOff"": 10.00,
	                ""PriceType"": ""REG"",
	                ""PriceTypeAttribute"": ""REG"",
	                ""SellableUom"": ""EA"",
	                ""CurrencyCode"": ""USD"",
	                ""Multiple"": 1,
	                ""TagExpirationDate"": ""2020-06-09"",
	                ""InsertDateUtc"": ""2020-06-09 14:00:00.000000"",
	                ""ModifiedDateUtc"": ""2020-06-09 14:00:00.000000""
                }
"
            };


            var priceObject = mapper.Map(message);

            Assert.True(priceObject.IsValid);
            Assert.True(string.IsNullOrEmpty(priceObject.Message), $"priceObject.Message was expected to be null. value was {priceObject.Message}");
            Assert.Equal(expectedGpmId, priceObject.PriceModel.GpmId.ToString());

        }

        [Fact]
        public void Mapper_InValidPriceMessage_ReturnsInvalidResponse()
        {

            var mapper = new SqsToPriceMapper();
            var message = new SQSEvent.SQSMessage { Body = @"[]" };

            var priceObject = mapper.Map(message);

            Assert.False(priceObject.IsValid);
            Assert.False(string.IsNullOrEmpty(priceObject.Message), $"priceObject.Message was expected to be null. value was {priceObject.Message}");
        }

    }
}