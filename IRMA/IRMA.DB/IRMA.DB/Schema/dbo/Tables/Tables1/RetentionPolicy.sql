CREATE TABLE [dbo].[RetentionPolicy] (
    [RetentionPolicyId]    INT           IDENTITY (1, 1) NOT NULL,
    [Schema]               NVARCHAR (50) NOT NULL,
    [Table]                NVARCHAR (64) NOT NULL,
    [ReferenceColumn]      NVARCHAR (50) NOT NULL,
    [DaysToKeep]           INT           NOT NULL,
    [TimeToStart]          INT           NOT NULL,
    [TimeToEnd]            INT           NOT NULL,
    [IncludedInDailyPurge] BIT           NOT NULL,
    [DailyPurgeCompleted]  BIT           NOT NULL,
    [PurgeJobName]         NVARCHAR (50) NOT NULL,
    [LastPurgedDateTime]   DATETIME      NULL,
    CONSTRAINT [PK_RetentionPolicy] PRIMARY KEY CLUSTERED ([RetentionPolicyId] ASC) WITH (FILLFACTOR = 80)
);
GO

GRANT SELECT ON OBJECT::dbo.RetentionPolicy TO [IRSUser];
GO
GRANT UPDATE ON OBJECT::dbo.RetentionPolicy TO [IRSUser];
GO