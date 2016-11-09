using Icon.Testing.CustomModels;
namespace Icon.Testing.Builders
{
    public class TestItemMovementTransactionBuilder
    {
        private string esbMessageID;
        private int firstItemMovementToIrmaIndex;
        private int lastItemMovementToIrmaIndex;
        private bool? processed;

        public TestItemMovementTransactionBuilder()
        {
            this.esbMessageID = "TestESBMessageID1";
            this.firstItemMovementToIrmaIndex = 0;
            this.lastItemMovementToIrmaIndex = 0;
        }

        public TestItemMovementTransactionBuilder WithESBMessageID(string esbMessageID)
        {
            this.esbMessageID = esbMessageID;
            return this;
        }

        public TestItemMovementTransactionBuilder WithFirstItemMovementToIrmaIndex(int firstItemMovementToIrmaIndex)
        {
            this.firstItemMovementToIrmaIndex = firstItemMovementToIrmaIndex;
            return this;
        }

        public TestItemMovementTransactionBuilder WithLastItemMovementToIrmaIndex(int lastItemMovementToIrmaIndex)
        {
            this.lastItemMovementToIrmaIndex = lastItemMovementToIrmaIndex;
            return this;
        }

        public TestItemMovementTransactionBuilder WithProcessedByController(bool processed)
        {
            this.processed = processed;
            return this;
        }
        public ItemMovementTransaction Build()
        {
            ItemMovementTransaction itemMovementTransaction = new ItemMovementTransaction();
            itemMovementTransaction.ESBMessageID = this.esbMessageID;
            itemMovementTransaction.LastItemMovementToIrmaIndex = this.lastItemMovementToIrmaIndex;
            itemMovementTransaction.FirstItemMovementToIrmaIndex = this.firstItemMovementToIrmaIndex;
            itemMovementTransaction.Processed  = this.processed;

            return itemMovementTransaction;
        }

        public static implicit operator ItemMovementTransaction(TestItemMovementTransactionBuilder builder)
        {
            return builder.Build();
        }
    }
}
