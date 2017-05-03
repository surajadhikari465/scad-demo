CREATE PROCEDURE dbo.CheckVersion
@Version varchar(20),
@AppType int
AS 

--APP TYPES ARE AS FOLLOWS:
-- 1 = CLIENT
-- 2 = ADMIN TOOL

IF @AppType = 1
	-- USE AppVersion_Client COLUMN
	SELECT Upgrade_Path 
	FROM [Version]
	WHERE (NOT EXISTS (SELECT AppVersion_Client FROM [Version] WHERE AppVersion_Client = @Version)) AND Current_Release = 1
ELSE	
	IF @AppType = 2
		-- USE AppVersion_Admin COLUMN
		SELECT Upgrade_Path 
		FROM [Version] 
		WHERE (NOT EXISTS (SELECT AppVersion_Admin FROM [Version] WHERE AppVersion_Admin = @Version)) AND Current_Release = 1
	ELSE
		-- USE VERSION COLUMN
		SELECT Upgrade_Path 
		FROM [Version] 
		WHERE (NOT EXISTS (SELECT [Version] FROM [Version] WHERE [Version] = @Version)) AND Current_Release = 1
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckVersion] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckVersion] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckVersion] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckVersion] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckVersion] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckVersion] TO [IRMAExcelRole]
    AS [dbo];

