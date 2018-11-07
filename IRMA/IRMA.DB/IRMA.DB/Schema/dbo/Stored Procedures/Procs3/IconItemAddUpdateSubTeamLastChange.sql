
CREATE PROCEDURE [dbo].[IconItemAddUpdateSubTeamLastChange]
	-- Add the parameters for the stored procedure here
	@updatedItemList dbo.IconItemWithSubteamType READONLY
AS
BEGIN
	SET NOCOUNT ON;

	-- =====================================================
	-- Declare Variables
	-- =====================================================
	DECLARE @now datetime;
	SET @now = (SELECT GETDATE());
	DECLARE @distinctList dbo.IconItemWithSubteamType;
	DECLARE @defaultNonAlignedPosDepNo int;
	set @defaultNonAlignedPosDepNo = 294;

	INSERT INTO @distinctList
	SELECT DISTINCT vi.*
	FROM @updatedItemList vi
	INNER JOIN ItemIdentifier ii on vi.ScanCode = ii.Identifier 
	                            AND ii.Deleted_Identifier = 0
								AND ii.Default_Identifier = 1
	INNER JOIN Item  i     on i.Item_Key = ii.Item_Key
	INNER JOIN SubTeam ist on ist.SubTeam_No = i.SubTeam_No
	 LEFT JOIN SubTeam st  on st.Dept_No = vi.DeptNo
	WHERE (st.SubTeam_No is NULL or ist.SubTeam_No  <> st.SubTeam_No) 


	-- =====================================================
	-- IconItemLastChange
	-- =====================================================
	BEGIN TRY
		-- Update existing rows
		UPDATE lc
		SET
			Subteam_No			= st.SubTeam_No,
			InsertDate			= @now
		FROM
			IconItemLastChange		lc
			JOIN @distinctList			di on lc.Identifier = di.ScanCode and di.SubTeamNotAligned = 0			
			JOIN SubTeam				st on	di.DeptNo = st.Dept_No


		-- Insert new rows if they don't exist yet
		INSERT INTO IconItemLastChange(Identifier, Subteam_No, InsertDate)
		SELECT
			di.ScanCode								as Identifier,
			st.SubTeam_No							as Subteam_No,			
			@now									as InsertDate
		FROM
			@distinctList	di
			JOIN SubTeam	st on	di.DeptNo = st.Dept_No  and di.SubTeamNotAligned = 0	
		WHERE NOT EXISTS (SELECT * FROM IconItemLastChange lc WHERE lc.Identifier = di.ScanCode)
		
		--Update non aligned sub team updates

		UPDATE lc
		SET		
			Subteam_No			= st.SubTeam_No,
			InsertDate			= @now
		FROM
			IconItemLastChange			lc
			JOIN @distinctList			di on lc.Identifier = di.ScanCode and di.SubTeamNotAligned = 1
			JOIN ItemIdentifier			ii on	di.ScanCode = ii.Identifier
			JOIN Item					i on ii.Item_Key = i.Item_Key			
			JOIN SubTeam			oldSub on   i.SubTeam_No = oldSub.SubTeam_No and oldSub.AlignedSubTeam = 1
			JOIN SubTeam				st on	st.Dept_No = @defaultNonAlignedPosDepNo

		-- Insert new rows if they don't exist yet
		INSERT INTO IconItemLastChange(Identifier, Subteam_No, InsertDate)
		SELECT
			di.ScanCode								as Identifier,
			st.SubTeam_No							as Subteam_No,
			@now									as InsertDate
		FROM
			@distinctList	di
			JOIN ItemIdentifier			ii on	di.ScanCode = ii.Identifier
			JOIN Item					i on ii.Item_Key = i.Item_Key
			JOIN SubTeam			oldSub on   i.SubTeam_No = oldSub.SubTeam_No and oldSub.AlignedSubTeam = 1
			JOIN SubTeam				st on	st.Dept_No = @defaultNonAlignedPosDepNo
		WHERE NOT EXISTS (SELECT * FROM IconItemLastChange lc WHERE lc.Identifier = di.ScanCode)
				and di.SubTeamNotAligned = 1

	END TRY
	BEGIN CATCH
		DECLARE @err_no int, @err_sev int, @err_msg varchar(MAX)
		SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
		RAISERROR ('IconItemAddUpdateSubTeamLastChange failed with error no: %d and message: %s', @err_sev, 1, @err_no, @err_msg)
	END CATCH

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[IconItemAddUpdateSubTeamLastChange] TO [IConInterface]
    AS [dbo];

