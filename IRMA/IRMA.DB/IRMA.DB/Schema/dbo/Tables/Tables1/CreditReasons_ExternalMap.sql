CREATE TABLE [dbo].[CreditReasons_ExternalMap] (
    [OrderExternalSourceID]    INT          NOT NULL,
    [ExternalSourceReasonName] VARCHAR (20) NOT NULL,
    [CreditReason_ID]          INT          NOT NULL,
    CONSTRAINT [PK_CreditReasons_ExternalMap] PRIMARY KEY CLUSTERED ([OrderExternalSourceID] ASC, [ExternalSourceReasonName] ASC) WITH (FILLFACTOR = 90)
);

