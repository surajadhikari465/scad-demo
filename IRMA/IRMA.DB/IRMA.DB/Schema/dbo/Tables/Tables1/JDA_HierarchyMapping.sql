CREATE TABLE [dbo].[JDA_HierarchyMapping] (
    [JDA_HierarchyMapping_ID] INT IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Subteam_No]              INT NOT NULL,
    [Category_ID]             INT NOT NULL,
    [ProdHierarchyLevel3_ID]  INT NOT NULL,
    [ProdHierarchyLevel4_ID]  INT NOT NULL,
    [JDA_Dept]                INT NOT NULL,
    [JDA_SubDept]             INT NOT NULL,
    [JDA_Class]               INT NOT NULL,
    [JDA_SubClass]            INT NOT NULL,
    CONSTRAINT [PK_JDA_HierarchyMapping] PRIMARY KEY CLUSTERED ([JDA_HierarchyMapping_ID] ASC)
);

