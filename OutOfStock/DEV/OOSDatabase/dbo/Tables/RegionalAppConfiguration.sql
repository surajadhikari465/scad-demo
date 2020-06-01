CREATE TABLE [dbo].[RegionalAppConfiguration] (
    [RegionId]          INT           NOT NULL,
    [UseBiztalk]        BIT           CONSTRAINT [DF_RegionalAppConfiguration_UseBiztalk] DEFAULT ((1)) NULL,
    [BiztalkServiceURI] VARCHAR (255) NULL,
    [OOSServiceURI]     VARCHAR (255) NOT NULL,
    CONSTRAINT [PK_RegionalAppConfiguration] PRIMARY KEY CLUSTERED ([RegionId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_RegionalAppConfiguration_Region_Id] FOREIGN KEY ([RegionId]) REFERENCES [dbo].[REGION] ([ID])
);

