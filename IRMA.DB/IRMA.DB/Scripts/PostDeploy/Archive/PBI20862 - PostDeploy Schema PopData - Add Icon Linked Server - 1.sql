-- ===========================================
-- Populate Infor/Icon ItemID in IRMA
-- VSTS 20862
-- Summary:
-- Adds a new column to the dbo.ValidatedScanCode table
-- and populates it with the ItemID from Icon's DB.

-- Basic Summary of Steps for PBI 20862:
-- NOTE:  these will have to be run outside the normal data pop script
-- link svr
-- schema & pop data
-- bring db up and validate
-- after OK, drop backup tbl
--
-- ===========================================
PRINT 'Adding Icon Linked Server (' + CONVERT(nvarchar, GETDATE(), 121) +')...';
DECLARE @dataSource nvarchar(4000);
DECLARE @regionInstance nvarchar(50);

SET @regionInstance = (SELECT @@SERVICENAME)

IF @regionInstance LIKE N'%P'
	SET @dataSource = N'IDP-ICON\SHARED3P'
IF @regionInstance LIKE N'%Q'
	SET @dataSource = N'IDQ-ICON\SQLSHARED3Q'
IF @regionInstance LIKE N'%T' OR @regionInstance LIKE N'%D'
	SET @dataSource = N'CEWD1815\SQLSHARED2012D'

IF EXISTS (select 1 from sys.servers where name = N'ICON')
	EXEC master.dbo.sp_dropserver @server=N'ICON', @droplogins='droplogins'

EXEC master.dbo.sp_addlinkedserver @server = N'ICON', @srvproduct=N'', @provider=N'SQLNCLI10', @datasrc=@dataSource, @catalog=N'icon' 
GO
EXEC master.dbo.sp_addlinkedsrvlogin @rmtsrvname=N'ICON',@useself=N'True',@locallogin=NULL,@rmtuser=NULL,@rmtpassword=NULL
GO

EXEC master.dbo.sp_serveroption @server=N'ICON', @optname=N'collation compatible', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'ICON', @optname=N'data access', @optvalue=N'true'
GO

EXEC master.dbo.sp_serveroption @server=N'ICON', @optname=N'dist', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'ICON', @optname=N'pub', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'ICON', @optname=N'rpc', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'ICON', @optname=N'rpc out', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'ICON', @optname=N'sub', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'ICON', @optname=N'connect timeout', @optvalue=N'0'
GO

EXEC master.dbo.sp_serveroption @server=N'ICON', @optname=N'collation name', @optvalue=null
GO

EXEC master.dbo.sp_serveroption @server=N'ICON', @optname=N'lazy schema validation', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'ICON', @optname=N'query timeout', @optvalue=N'0'
GO

EXEC master.dbo.sp_serveroption @server=N'ICON', @optname=N'use remote collation', @optvalue=N'true'
GO

EXEC master.dbo.sp_serveroption @server=N'ICON', @optname=N'remote proc transaction promotion', @optvalue=N'true'
GO