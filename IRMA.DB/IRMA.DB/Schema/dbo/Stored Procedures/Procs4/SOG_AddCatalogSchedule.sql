CREATE PROCEDURE dbo.SOG_AddCatalogSchedule
	@ManagedByID	tinyint,
	@StoreNo		int,
	@SubTeamNo		int,
	@Mon			bit,
	@Tue			bit,
	@Wed			bit,
	@Thu			bit,
	@Fri			bit,
	@Sat			bit,
	@Sun			bit
WITH RECOMPILE
AS

-- **************************************************************************
-- Procedure: SOG_AddCatalogSchedule()
--    Author: Billy Blackerby
--      Date: 6/26/2009
--
-- Description:
-- Utilized by StoreOrderGuide to insert a CatalogSchedule row
--
-- Modification History:
-- Date			Init	Comment
-- 06/26/2009	BBB		Creation
-- 07/01/2009	BBB		Added ManagedByID parameter
-- 08/03/2009	BBB		Added a lookup to the Schedule table so that schedules
--						are only created for stores without an existing one
-- **************************************************************************	
BEGIN
    SET NOCOUNT ON
	--**************************************************************************
	--Main SQL
	--**************************************************************************
	IF @StoreNo = 0
		--**************************************************************************
		--Insert a row for each store if storeno = 0
		--**************************************************************************
		INSERT INTO 
			CatalogSchedule
		(
			ManagedByID,
			StoreNo,
			SubTeamNo,
			Mon,
			Tue,
			Wed,
			Thu,
			Fri,
			Sat,
			Sun
		)
		(
			SELECT
				@ManagedByID,
				Store_No,
				@SubTeamNo,
				@Mon,
				@Tue,
				@Wed,
				@Thu,
				@Fri,
				@Sat,
				@Sun
			FROM 
				Store
			WHERE
				Store_No NOT IN (SELECT StoreNo FROM CatalogSchedule WHERE StoreNo = Store_No AND ManagedByID = @ManagedByID AND SubTeamNo = @SubTeamNo)
		)
	ELSE
		--**************************************************************************
		--Insert a row for each store if storeno = 0
		--**************************************************************************
		INSERT INTO 
			CatalogSchedule
		(
			ManagedByID,
			StoreNo,
			SubTeamNo,
			Mon,
			Tue,
			Wed,
			Thu,
			Fri,
			Sat,
			Sun
		)
		VALUES
		(
			@ManagedByID,
			@StoreNo,
			@SubTeamNo,
			@Mon,
			@Tue,
			@Wed,
			@Thu,
			@Fri,
			@Sat,
			@Sun
		)

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SOG_AddCatalogSchedule] TO [IRMASLIMRole]
    AS [dbo];

