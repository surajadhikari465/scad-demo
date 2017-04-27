---Kasthuri
-- Updates IRMA item subteam with ICON item POS dept using subteam to pos dept association in IRMA
---04/04/2015 MZ
-- Update SP by matching SubTeam.Dept_No with the POSDeptNo from Icon to determine an IRMA item's new subteam.

CREATE PROCEDURE [dbo].[IconItemSubTeamUpdate]
	-- Add the parameters for the stored procedure here	
	@updatedItemList dbo.IconItemWithSubteamType READONLY,
	@UserName varchar(25)
AS
BEGIN
	SET NOCOUNT ON;

	-- =====================================================
	-- Declare Variables
	-- =====================================================
	DECLARE @userId int;
	DECLARE @now datetime;
	DECLARE @distinctList dbo.IconItemWithSubteamType;
	DECLARE @subTeamAligned varchar(25);
	DECLARE @defaultNonAlignedPosDepNo int;
	set @defaultNonAlignedPosDepNo = 294;
		
	CREATE TABLE #tempCategory (Category_ID int, Category_Name varchar(35), SubTeam_No int, UserID int,  SubTeam_Type_ID int );

	SET @userId = (SELECT u.User_ID FROM Users u WHERE u.UserName = @UserName);
	SET @now = (SELECT GETDATE());
	set @subTeamAligned = 'SubTeam Aligned';

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
	-- Update Item
	-- =====================================================
	BEGIN TRY
		--Add category if new sub team association does not exists..
		insert into ItemCategory ([Category_Name], [SubTeam_No], [User_ID])
		select distinct @subTeamAligned, st.SubTeam_No, @userId 
		from  @distinctList		di 
		join SubTeam st on	di.DeptNo = st.Dept_No and  di.SubTeamNotAligned = 0
		where 
		NOT EXISTS (SELECT 1 
					FROM ItemCategory ic
					WHERE ic.[Category_Name] = @subTeamAligned
					AND ic.[SubTeam_No] = st.SubTeam_No)
		
		--Update item subteam association and set the category to 'SubTeam Aligned'
		UPDATE i
		SET
			i.SubTeam_No = st.SubTeam_No,
			i.Category_ID = ic.Category_ID,
			i.LastModifiedUser_ID = @userId,
			i.LastModifiedDate = @now
		FROM
			Item i
			JOIN ItemIdentifier		ii on	i.Item_Key = ii.Item_Key
			JOIN @distinctList		di on	ii.Identifier = di.ScanCode and di.SubTeamNotAligned = 0
			JOIN SubTeam		    st on	di.DeptNo = st.Dept_No
			LEFT JOIN ItemCategory  ic on	st.SubTeam_No = ic.SubTeam_No			
									   and  ic.Category_Name = @subTeamAligned

		UPDATE i
		SET
			i.SubTeam_No = st.SubTeam_No,
			i.Category_ID = ic.Category_ID,
			i.LastModifiedUser_ID = @userId,
			i.LastModifiedDate = @now
		FROM
			Item i
			JOIN ItemIdentifier		ii on	i.Item_Key = ii.Item_Key
			JOIN @distinctList		di on	ii.Identifier = di.ScanCode and di.SubTeamNotAligned = 1
			JOIN SubTeam		oldSub on   i.SubTeam_No = oldSub.SubTeam_No and oldSub.AlignedSubTeam = 1
			JOIN SubTeam		    st on	st.Dept_No = @defaultNonAlignedPosDepNo
			LEFT JOIN ItemCategory  ic on	st.SubTeam_No = ic.SubTeam_No			
									and ic.Category_Name = @subTeamAligned

	END TRY
	BEGIN CATCH
		DECLARE @err_no int, @err_sev int, @err_msg varchar(MAX)
		SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
		RAISERROR ('IconItemSubTeamUpdate failed with error no: %d and message: %s', @err_sev, 1, @err_no, @err_msg)
	END CATCH
	drop table #tempCategory

END


GO


