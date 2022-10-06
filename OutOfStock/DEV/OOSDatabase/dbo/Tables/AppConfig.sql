/*
This table is not a core piece of the base-scan facility; it was added to support that solution and any others apps or processes
that want to use it, because every system should have this.

When this was written, it was checked into Azure repo here:
https://dev.azure.com/wholefoods/Supply%20Chain%20Application%20Development/_git/SCAD?path=/OutOfStock/DEV/OOSDatabase/dbo/Tables/AppConfig.sql&version=GBmaster

Main tech doc(s) here: https://dev.azure.com/wholefoods/Supply%20Chain%20Application%20Development/_git/SCAD?path=/OutOfStock/DEV/OOSDatabase/_documentation/
*/
CREATE TABLE [dbo].[AppConfig](
  [AppConfgID]  INT             IDENTITY (1, 1) NOT NULL,
  [AppID]       INT             NOT NULL,
  [Key]         NVARCHAR (128)  NOT NULL,
  [Value]       NVARCHAR (max)  NULL,
  CONSTRAINT [PK_AppConfig] PRIMARY KEY CLUSTERED ([AppConfgID] ASC) WITH (FILLFACTOR = 100),
  CONSTRAINT [FK_AppConfig_App] FOREIGN KEY ([AppID]) REFERENCES [dbo].[App] ([AppID]),
  CONSTRAINT UQ_AppConfig_AppID_Key UNIQUE(AppID, [Key])
); -- Small table, so not doing other indexes for now.

