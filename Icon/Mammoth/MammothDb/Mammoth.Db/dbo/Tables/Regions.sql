CREATE TABLE [dbo].[Regions] (
    [regionID]     TINYINT        IDENTITY (1, 1) NOT NULL,
    [Region]       NCHAR (2)      NOT NULL,
    [RegionName]   NVARCHAR (255) NOT NULL,
    [AddedDate]    DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifiedDate] DATETIME       NULL,
    CONSTRAINT [PK_Regions] PRIMARY KEY CLUSTERED ([Region] ASC) WITH (FILLFACTOR = 100)
);