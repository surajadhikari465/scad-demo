using Amazon.Lambda.SQSEvents;
using Amazon.Lambda.TestUtilities;
using Xunit;

namespace Warp.ProcessPrices.Lambda.Tests
{
    public class FunctionTest
    {
        [Fact]
        public void TestToUpperFunction()
        {

            // Invoke the lambda function and confirm the string was upper cased.
            var context = new TestLambdaContext();
            var upperCase = Function.FunctionHandler(new SQSEvent(), context);

            Assert.Equal("HELLO WORLD", upperCase);
        }
    }
}
