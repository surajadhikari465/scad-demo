/*
This table is not a core piece of the base-scan facility; it was added to support that solution and any others apps or processes
that want to use it, because every system should have this.

When this was written, it was checked into Azure repo here:
https://dev.azure.com/wholefoods/Supply%20Chain%20Application%20Development/_git/SCAD?path=/OutOfStock/DEV/OOSDatabase/dbo/Tables/App.sql&version=GBmaster

Main tech doc(s) here: https://dev.azure.com/wholefoods/Supply%20Chain%20Application%20Development/_git/SCAD?path=/OutOfStock/DEV/OOSDatabase/_documentation/
*/
CREATE TABLE [dbo].[App] (
    [AppID]   INT            IDENTITY (1, 1) NOT NULL,
    [AppName] NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_App] PRIMARY KEY CLUSTERED ([AppID] ASC) WITH (FILLFACTOR = 100),
	CONSTRAINT UQ_App_AppName UNIQUE(AppName)
);

