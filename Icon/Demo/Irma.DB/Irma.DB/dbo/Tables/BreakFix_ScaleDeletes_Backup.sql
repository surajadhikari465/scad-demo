CREATE TABLE [dbo].[BreakFix_ScaleDeletes_Backup] (
    [Identifier_ID]           INT          IDENTITY (1, 1) NOT NULL,
    [Item_Key]                INT          NOT NULL,
    [Identifier]              VARCHAR (13) NOT NULL,
    [Default_Identifier]      TINYINT      NOT NULL,
    [Deleted_Identifier]      TINYINT      NOT NULL,
    [Add_Identifier]          TINYINT      NOT NULL,
    [Remove_Identifier]       TINYINT      NOT NULL,
    [National_Identifier]     TINYINT      NOT NULL,
    [CheckDigit]              CHAR (1)     NULL,
    [IdentifierType]          CHAR (1)     NULL,
    [NumPluDigitsSentToScale] INT          NULL,
    [Scale_Identifier]        BIT          NULL
);

