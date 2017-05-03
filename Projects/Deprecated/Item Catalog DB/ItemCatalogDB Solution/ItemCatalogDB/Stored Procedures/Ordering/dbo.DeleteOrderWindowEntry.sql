SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteOrderWindowEntry]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteOrderWindowEntry]
GO


CREATE PROCEDURE [dbo].[DeleteOrderWindowEntry]
	@ZoneList varchar(500),
	@StoreList varchar(500),
	@SubTeamList varchar(500),
	@All bit
AS 
BEGIN    
    SET NOCOUNT ON    
    
    DECLARE @tblZone TABLE (Zone_Id int)
	DECLARE @tblSubTeam TABLE (SubTeam_No int)
	DECLARE @tblStore TABLE (Store_No int)
	DECLARE @Zone_Id int
	DECLARE @Store_No int
	DECLARE @SubTeam_No int
	
	IF @All = 1
		DELETE FROM dbo.ZoneSubTeam
	ELSE
		BEGIN

		INSERT INTO @tblZone
			SELECT Key_Value FROM dbo.fn_Parse_List(@ZoneList,'|')
			
		INSERT INTO @tblSubTeam
			SELECT Key_Value FROM dbo.fn_Parse_List(@SubTeamList,'|')
			
		INSERT INTO @tblStore
			SELECT Key_Value FROM dbo.fn_Parse_List(@StoreList,'|')		
			
		DECLARE c1 CURSOR READ_ONLY
			FOR
				SELECT Zone_Id, SubTeam_No, Store_No
				FROM
					(SELECT *
					FROM @tblZone
					CROSS JOIN @tblSubTeam) AS A
					CROSS JOIN @tblStore

			OPEN c1

			FETCH NEXT FROM c1
			INTO @Zone_Id, @SubTeam_No, @Store_No

			WHILE @@FETCH_STATUS = 0
			BEGIN

				DELETE FROM dbo.ZoneSubTeam WHERE Zone_Id = @Zone_Id AND SubTeam_No = @SubTeam_No AND Supplier_Store_No = @Store_No

				FETCH NEXT FROM c1
				INTO @Zone_ID, @SubTeam_No, @Store_No
			END
		CLOSE c1
		DEALLOCATE c1	
	END
        
    SET NOCOUNT OFF            
END 
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO  