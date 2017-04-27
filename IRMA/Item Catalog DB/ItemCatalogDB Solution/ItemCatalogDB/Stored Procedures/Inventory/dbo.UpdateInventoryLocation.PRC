SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateInventoryLocation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateInventoryLocation]
GO


CREATE PROCEDURE dbo.UpdateInventoryLocation
	@LocationID int,
	@StoreID int,
	@SubTeamID int,
	@Name varchar(50),
	@Desc varchar(50),
	@Notes varchar(250)

AS

SET NOCOUNT ON

--See if there is a current record.
IF EXISTS (SELECT InvLoc_ID FROM InventoryLocation (nolock) WHERE InvLoc_ID = @LocationID)
	BEGIN
		--Current record found, UPDATE.
		UPDATE InventoryLocation SET InvLoc_Name = @Name, InvLoc_Desc = @Desc, Store_No = @StoreID, SubTeam_No = @SubTeamID, Notes = @Notes
		WHERE InvLoc_ID = @LocationID
		SELECT @LocationID
	END
ELSE
	BEGIN
		--No current record, INSERT.
		INSERT INTO InventoryLocation (InvLoc_Name, InvLoc_Desc, Store_No, SubTeam_No, Notes) 
		VALUES(@Name, @Desc, @StoreID, @SubTeamID, @Notes)
		SELECT scope_identity();
	END

SET NOCOUNT OFF
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


