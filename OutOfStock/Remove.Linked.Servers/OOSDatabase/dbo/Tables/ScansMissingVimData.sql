CREATE TABLE [dbo].[ScansMissingVimData] (
    [Report_Header_Id] INT          NOT NULL,
    [UPC]              VARCHAR (25) NOT NULL,
    [ScanCount]        INT          CONSTRAINT [DF__ScansMiss__ScanC__4979DDF4] DEFAULT ((0)) NOT NULL,
    [id]               INT          IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_ScansMissingVimData] PRIMARY KEY NONCLUSTERED ([id] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE CLUSTERED INDEX [IX_ScansMissingVimData_Clustered]
    ON [dbo].[ScansMissingVimData]([Report_Header_Id] ASC, [UPC] ASC) WITH (FILLFACTOR = 80);

