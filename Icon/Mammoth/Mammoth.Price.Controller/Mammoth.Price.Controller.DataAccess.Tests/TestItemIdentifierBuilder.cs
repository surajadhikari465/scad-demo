using System;
using Irma.Framework;

namespace Irma.Testing.Builders
{
    public class TestItemIdentifierBuilder
    {
        private int identifier_ID;
        private int item_Key;
        private string identifier;
        private byte default_Identifier;
        private byte deleted_Identifier;
        private byte add_Identifier;
        private byte remove_Identifier;
        private byte national_Identifier;
        private string checkDigit;
        private string identifierType;
        private System.Nullable<int> numPluDigitsSentToScale;
        private System.Nullable<bool> scale_Identifier;

        public TestItemIdentifierBuilder()
        {
            this.identifier_ID = 0;
            this.item_Key = 0;
            this.identifier = "1234598760";
            this.default_Identifier = 0;
            this.deleted_Identifier = 0;
            this.add_Identifier = 0;
            this.remove_Identifier = 0;
            this.national_Identifier = 0;
            this.checkDigit = "0";
            this.identifierType = "B";
            this.numPluDigitsSentToScale = 0;
            this.scale_Identifier = false;
        }

        public TestItemIdentifierBuilder WithIdentifier_ID(int identifier_ID)
        {
            this.identifier_ID = identifier_ID;
            return this;
        }

        public TestItemIdentifierBuilder WithItem_Key(int item_Key)
        {
            this.item_Key = item_Key;
            return this;
        }

        public TestItemIdentifierBuilder WithIdentifier(string identifier)
        {
            this.identifier = identifier;
            return this;
        }

        public TestItemIdentifierBuilder WithDefault_Identifier(byte default_Identifier)
        {
            this.default_Identifier = default_Identifier;
            return this;
        }

        public TestItemIdentifierBuilder WithDeleted_Identifier(byte deleted_Identifier)
        {
            this.deleted_Identifier = deleted_Identifier;
            return this;
        }

        public TestItemIdentifierBuilder WithAdd_Identifier(byte add_Identifier)
        {
            this.add_Identifier = add_Identifier;
            return this;
        }

        public TestItemIdentifierBuilder WithRemove_Identifier(byte remove_Identifier)
        {
            this.remove_Identifier = remove_Identifier;
            return this;
        }

        public TestItemIdentifierBuilder WithNational_Identifier(byte national_Identifier)
        {
            this.national_Identifier = national_Identifier;
            return this;
        }

        public TestItemIdentifierBuilder WithCheckDigit(string checkDigit)
        {
            this.checkDigit = checkDigit;
            return this;
        }

        public TestItemIdentifierBuilder WithIdentifierType(string identifierType)
        {
            this.identifierType = identifierType;
            return this;
        }

        public TestItemIdentifierBuilder WithNumPluDigitsSentToScale(System.Nullable<int> numPluDigitsSentToScale)
        {
            this.numPluDigitsSentToScale = numPluDigitsSentToScale;
            return this;
        }

        public TestItemIdentifierBuilder WithScale_Identifier(System.Nullable<bool> scale_Identifier)
        {
            this.scale_Identifier = scale_Identifier;
            return this;
        }

        public ItemIdentifier Build()
        {
            ItemIdentifier itemIdentifier = new ItemIdentifier();

            itemIdentifier.Identifier_ID = this.identifier_ID;
            itemIdentifier.Item_Key = this.item_Key;
            itemIdentifier.Identifier = this.identifier;
            itemIdentifier.Default_Identifier = this.default_Identifier;
            itemIdentifier.Deleted_Identifier = this.deleted_Identifier;
            itemIdentifier.Add_Identifier = this.add_Identifier;
            itemIdentifier.Remove_Identifier = this.remove_Identifier;
            itemIdentifier.National_Identifier = this.national_Identifier;
            itemIdentifier.CheckDigit = this.checkDigit;
            itemIdentifier.IdentifierType = this.identifierType;
            itemIdentifier.NumPluDigitsSentToScale = this.numPluDigitsSentToScale;
            itemIdentifier.Scale_Identifier = this.scale_Identifier;

            return itemIdentifier;
        }

        public static implicit operator ItemIdentifier(TestItemIdentifierBuilder builder)
        {
            return builder.Build();
        }
    }
}