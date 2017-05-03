CREATE TABLE [dbo].[JDASync_AuditDetail] (
    [JDASync_AuditDetail_ID] INT           IDENTITY (1, 1) NOT NULL,
    [JDASync_AuditHeader_ID] INT           NOT NULL,
    [Table_Name]             VARCHAR (75)  NULL,
    [Key_Value]              INT           NULL,
    [Is_InJDA]               BIT           DEFAULT ((1)) NULL,
    [Column_Name]            VARCHAR (75)  NULL,
    [IRMA_Value]             VARCHAR (150) NULL,
    [JDA_Value]              VARCHAR (150) NULL,
    [Is_Queued]              BIT           DEFAULT ((0)) NULL,
    [ScanDateTime]           DATETIME      NULL,
    CONSTRAINT [PK_JDASync_AuditDetail] PRIMARY KEY CLUSTERED ([JDASync_AuditDetail_ID] ASC),
    CONSTRAINT [FK_JDASync_AuditDetail_JDASync_AuditHeader] FOREIGN KEY ([JDASync_AuditHeader_ID]) REFERENCES [dbo].[JDASync_AuditHeader] ([JDASync_AuditHeader_ID])
);

