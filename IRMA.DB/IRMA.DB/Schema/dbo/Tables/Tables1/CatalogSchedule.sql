CREATE TABLE [dbo].[CatalogSchedule] (
    [CatalogScheduleID] INT          IDENTITY (1, 1) NOT NULL,
    [ManagedByID]       TINYINT      NULL,
    [StoreNo]           INT          NULL,
    [SubTeamNo]         INT          NULL,
    [Mon]               BIT          CONSTRAINT [DF_CatalogSchedule_Mon] DEFAULT ((0)) NULL,
    [Tue]               BIT          CONSTRAINT [DF_CatalogSchedule_Tue] DEFAULT ((0)) NULL,
    [Wed]               BIT          CONSTRAINT [DF_CatalogSchedule_Wed] DEFAULT ((0)) NULL,
    [Thu]               BIT          CONSTRAINT [DF_CatalogSchedule_Thu] DEFAULT ((0)) NULL,
    [Fri]               BIT          CONSTRAINT [DF_CatalogSchedule_Fri] DEFAULT ((0)) NULL,
    [Sat]               BIT          CONSTRAINT [DF_CatalogSchedule_Sat] DEFAULT ((0)) NULL,
    [Sun]               BIT          CONSTRAINT [DF_CatalogSchedule_Sun] DEFAULT ((0)) NULL,
    [InsertDate]        DATETIME     CONSTRAINT [DF_CatalogSchedule_InsertDate] DEFAULT (getdate()) NULL,
    [UpdateDate]        DATETIME     CONSTRAINT [DF_CatalogSchedule_UpdateDate] DEFAULT (getdate()) NULL,
    [InsertUser]        VARCHAR (50) NULL,
    [UpdateUser]        VARCHAR (50) NULL,
    CONSTRAINT [PK_CatalogSchedule] PRIMARY KEY CLUSTERED ([CatalogScheduleID] ASC),
    CONSTRAINT [FK_ItemManager_CatalogSchedule] FOREIGN KEY ([ManagedByID]) REFERENCES [dbo].[ItemManager] ([Manager_ID]),
    CONSTRAINT [FK_Store_CatalogSchedule] FOREIGN KEY ([StoreNo]) REFERENCES [dbo].[Store] ([Store_No]),
    CONSTRAINT [FK_SubTeam_CatalogSchedule] FOREIGN KEY ([SubTeamNo]) REFERENCES [dbo].[SubTeam] ([SubTeam_No])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'FK - ItemCatalog.ItemManager that the schedule corresponds to ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CatalogSchedule', @level2type = N'COLUMN', @level2name = N'ManagedByID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'FK - ItemCatalog.Store that the schedule corresponds to ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CatalogSchedule', @level2type = N'COLUMN', @level2name = N'StoreNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'FK - ItemCatalog.SubTeam that the schedule corresponds to ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CatalogSchedule', @level2type = N'COLUMN', @level2name = N'SubTeamNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Monday delivery', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CatalogSchedule', @level2type = N'COLUMN', @level2name = N'Mon';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tuesday delivery', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CatalogSchedule', @level2type = N'COLUMN', @level2name = N'Tue';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Wednesday delivery', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CatalogSchedule', @level2type = N'COLUMN', @level2name = N'Wed';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Thursday delivery', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CatalogSchedule', @level2type = N'COLUMN', @level2name = N'Thu';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Friday delivery', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CatalogSchedule', @level2type = N'COLUMN', @level2name = N'Fri';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Saturday delivery', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CatalogSchedule', @level2type = N'COLUMN', @level2name = N'Sat';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sunday delivery', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CatalogSchedule', @level2type = N'COLUMN', @level2name = N'Sun';

