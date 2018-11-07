 if exists (select * from dbo.sysobjects where id = object_id(N'dbo.GetScanGunStoreSubTeam') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.GetScanGunStoreSubTeam
GO

CREATE PROCEDURE dbo.GetScanGunStoreSubTeam
    @User_ID int,
    @Store_No int
AS

BEGIN
    SET NOCOUNT ON

	DECLARE @SubTeam_No int

	SELECT 
		@SubTeam_No = SubTeam_No
	FROM
		ScanGunStoreSubTeam
	WHERE
		User_ID = @User_ID 
		AND 
		Store_No = @Store_No

	EXEC dbo.GetSubTeam @SubTeam_No=@SubTeam_No

    SET NOCOUNT OFF
END
GO