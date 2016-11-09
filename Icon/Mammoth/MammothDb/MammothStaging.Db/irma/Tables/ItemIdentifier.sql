CREATE TABLE [irma].[ItemIdentifier] (
    [Region]                  NCHAR (2)    NOT NULL,
    [Identifier_ID]           INT          NOT NULL,
    [Item_Key]                INT          NOT NULL,
    [Identifier]              VARCHAR (13) NOT NULL,
    [Default_Identifier]      TINYINT      CONSTRAINT [DF__ItemIdent__Defau__47C20D6C] DEFAULT ((0)) NOT NULL,
    [Deleted_Identifier]      TINYINT      CONSTRAINT [DF__ItemIdent__Delet__48B631A5] DEFAULT ((0)) NOT NULL,
    [Add_Identifier]          TINYINT      CONSTRAINT [DF__ItemIdent__Add_I__49AA55DE] DEFAULT ((0)) NOT NULL,
    [Remove_Identifier]       TINYINT      CONSTRAINT [DF__ItemIdent__Remov__4A9E7A17] DEFAULT ((0)) NOT NULL,
    [National_Identifier]     TINYINT      CONSTRAINT [DF_ItemIdentifier_National_Identifier] DEFAULT ((0)) NOT NULL,
    [CheckDigit]              CHAR (1)     NULL,
    [IdentifierType]          CHAR (1)     NULL,
    [NumPluDigitsSentToScale] INT          NULL,
    [Scale_Identifier]        BIT          CONSTRAINT [DF_ItemIdentifier_Scale_Identifier] DEFAULT ((0)) NULL
);



