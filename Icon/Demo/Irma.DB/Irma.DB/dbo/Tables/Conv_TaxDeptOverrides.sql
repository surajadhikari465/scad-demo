CREATE TABLE [dbo].[Conv_TaxDeptOverrides] (
    [Item_Key]             INT          NOT NULL,
    [upcno]                VARCHAR (13) NULL,
    [Store_No]             INT          NULL,
    [TaxJurisdictionID]    INT          NOT NULL,
    [TaxFlagKey]           CHAR (1)     NOT NULL,
    [TaxFlagValue]         BIT          NULL,
    [OriginalClassId]      INT          NULL,
    [FinalClassId]         INT          NULL,
    [MatchedJurisdictions] INT          NULL,
    [CompleteMatch]        BIT          NULL,
    CONSTRAINT [PK_Conv_TaxDeptOverrides] PRIMARY KEY CLUSTERED ([Item_Key] ASC, [TaxJurisdictionID] ASC, [TaxFlagKey] ASC)
);

