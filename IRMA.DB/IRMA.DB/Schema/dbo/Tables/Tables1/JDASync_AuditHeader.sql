CREATE TABLE [dbo].[JDASync_AuditHeader] (
    [JDASync_AuditHeader_ID] INT      IDENTITY (1, 1) NOT NULL,
    [StartDateTime]          DATETIME NULL,
    [EndDateTime]            DATETIME NULL,
    CONSTRAINT [PK_JDASync_AuditHeader] PRIMARY KEY CLUSTERED ([JDASync_AuditHeader_ID] ASC)
);

