CREATE PROCEDURE [dbo].[IsItemSubTeamAligned]
	-- Add the parameters for the stored procedure here
	@scanCode varchar(255)
AS
BEGIN
	SET NOCOUNT ON;

	-- =====================================================
	-- Declare Variables
	-- =====================================================
	DECLARE @subTeamAligned varchar(25);
	DECLARE @SQLString varchar(255);

	set @subTeamAligned = 'SubTeam Aligned';




	IF EXISTS(SELECT * FROM sys.columns WHERE name = 'AlignedSubTeam' AND [object_id] = OBJECT_ID('SubTeam'))
		BEGIN
			SET @SQLString = 'SELECT COALESCE(Sub.AlignedSubTeam, 0)
				FROM
					Item i
					JOIN ItemIdentifier		ii on	i.Item_Key = ii.Item_Key
													AND ii.Deleted_Identifier = 0
													AND ii.Default_Identifier = 1
													AND	ii.Identifier =' + @scanCode +
					'JOIN SubTeam		Sub on i.SubTeam_No = Sub.SubTeam_No'
			      


			EXECUTE sp_executesql @SQLString
			
				
		END
		ELSE
			select 0;		
END;

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[IsItemSubTeamAligned] TO [IConInterface]
    AS [dbo];

