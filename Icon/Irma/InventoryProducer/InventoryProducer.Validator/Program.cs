/**
 * This is a tool that can test the producer XML output with older XML messages to confirm mapping logic
 * 
 * Comments needed to run code
 * 1. InventoryProducer.Producer.Publish.MessagePublisher.PublishMessage sending message to both AMQ and ESB
 * 2. InventoryProducer.Producer.ProducerBuilders.ProducerBuilder.ComposeProducer AMQ and ESB producer connection
*/

using Icon.DbContextFactory;
using InventoryProducer.Common;
using InventoryProducer.Producer.ProducerBuilders;
using InventoryProducer.Validator;
using InventoryProducer.Validator.DAL;
using Irma.Framework;
using System.Xml;

string region = "MW";
string[] eventTypeCodes = new string[] { "PO_MOD", "PO_CRE", "PO_LINE_DEL", "PO_DEL" };
string eventTableName = "amz.OrderQueue";
int maxTestSamples = 300;
string producerType = "PurchaseOrder";
string expectedTimeZone = "-05:00";
bool deleteOldResult = true;
string folderPrefix = @".\validator";

IDbContextFactory<IrmaContext> irmaContextFactory = new IrmaDbContextFactory();
IProducerBuilder producerBuilder = new InventoryPurchaseOrderProducerBuilder();
InventoryProducerSettings settings = InventoryProducerSettings.CreateFromConfig("IRMA", ProducerType.Instance);
settings.DequeueMaxRecords = maxTestSamples;
var producer = producerBuilder.ComposeProducer(settings);
int successCounter = 0;
int jumpOverCounter = 0;

using (var irmaContext = irmaContextFactory.CreateContext($"IRMA_{region}"))
{
    var messageArchiveEventDAL = new MessageArchiveEventDAL(irmaContext.Database);
    var eventDAL = new EventDAL(eventTableName, irmaContext.Database);
    var messageDAL = new MessageDAL(irmaContext.Database);

    Console.WriteLine("Fetching list...");
    var messageArchiveEvents = messageArchiveEventDAL.GetList(eventTypeCodes, maxTestSamples);
    var numberOfTestSamples = messageArchiveEvents.Count;
    Console.WriteLine($"Got: {numberOfTestSamples}");

    //Creating a tracking list of all the tests and creating new events too
    var tests = messageArchiveEvents.Select(a => new TestInstance()
    {
        OldArchivedEvent = a,
        CreatedEvent = a       //HACK!! Dont try this at home
    }).ToList();
    eventDAL.Truncate();

    //Inserting new events into Producer queue
    Console.WriteLine($"Creating events in {eventTableName} table");
    foreach (var test in tests)
    {
        eventDAL.Create(test.CreatedEvent);
    }

    //Running producer
    producer.Execute();

    if (deleteOldResult && Directory.Exists(folderPrefix))
    {
        Directory.Delete(folderPrefix, true);
    }

    //Filling up test data for analysis
    foreach (var test in tests)
    {
        test.NewArchivedEvent = messageArchiveEventDAL.Get(test.CreatedEvent.KeyID, test.CreatedEvent.SecondaryKeyID, test.CreatedEvent.EventTypeCode);

        test.OldMessage = messageDAL.GetMessage(test.OldArchivedEvent.MessageHeaders.MessageNumber, test.OldArchivedEvent.KeyID);
        test.NewMessage = messageDAL.GetMessage(test.NewArchivedEvent.MessageHeaders.MessageNumber, test.NewArchivedEvent.KeyID);

        if (test.OldMessage != null)
        {
            test.OldMessage.RawMessage = updateKnownTestExceptions(test.OldMessage.RawMessage, expectedTimeZone);
        }
        if (test.NewMessage != null)
        {
            test.NewMessage.RawMessage = updateKnownTestExceptions(test.NewMessage.RawMessage, expectedTimeZone);
        }

        test.OldMessage?.LoadXML();
        test.NewMessage?.LoadXML();

        if (test.OldMessage == null)
        {
            test.AreEqual = null;
            jumpOverCounter++;
            //Console.WriteLine($"Jumping, Old: {test.OldMessage?.MessageNumber}, New: {test.NewMessage?.MessageNumber}");
        }
        else
        {
            test.MessageDiff = new Microsoft.XmlDiffPatch.XmlDiff(Microsoft.XmlDiffPatch.XmlDiffOptions.None);
            test.MessageDiff.Algorithm = Microsoft.XmlDiffPatch.XmlDiffAlgorithm.Fast;
            test.MessageDiff.IgnoreWhitespace = true;
            test.MessageDiff.IgnoreChildOrder = true;
            test.MessageDiff.IgnoreComments = true;
            test.MessageDiff.IgnorePrefixes = true;
            test.MessageDiff.IgnoreNamespaces = true;
            test.MessageDiff.IgnoreXmlDecl = true;

            string folderName = $"{folderPrefix}/{producerType}/{test.NewMessage?.MessageNumber}_{test.OldMessage?.MessageNumber}";

            test.DiffFileName = $"{folderName}/diff.xml";
            var di = Directory.CreateDirectory(folderName);
            using (XmlWriter writer = XmlWriter.Create(test.DiffFileName))
            {
                test.AreEqual = test.Successful = test.MessageDiff.Compare(test.NewMessage?.XMLMessage, test.OldMessage?.XMLMessage, writer);
                File.WriteAllText($"{folderName}/New_{test.NewMessage?.MessageNumber}.xml", test.NewMessage?.RawMessage);
                File.WriteAllText($"{folderName}/Old_{test.OldMessage?.MessageNumber}.xml", test.OldMessage?.RawMessage);
            }
            Console.WriteLine($"Success: {test.Successful}, Old: {test.OldMessage?.MessageNumber}, New: {test.NewMessage?.MessageNumber}");
            successCounter += test.Successful ? 1 : 0;
        }
    }
    Console.WriteLine($"Jumpver percentage: {((float)jumpOverCounter / numberOfTestSamples) * 100}%");
    Console.WriteLine($"Success percentage: {((float)successCounter / (numberOfTestSamples - jumpOverCounter)) * 100}%");
}

string updateKnownTestExceptions(string rawMessage, string expectedTimeZone)
{
    string correctedMessage = rawMessage;
    int messageNumberStart = correctedMessage.IndexOf("<messageNumber>");
    int messageNumberEnd = correctedMessage.IndexOf("</messageNumber>") + "</messageNumber>".Length;
    correctedMessage = correctedMessage.Remove(messageNumberStart, messageNumberEnd - messageNumberStart);
    correctedMessage = correctedMessage.Replace("+05:30", expectedTimeZone);
    correctedMessage = correctedMessage.Replace(".0000", "");
    return correctedMessage;
}