CREATE PROCEDURE [dbo].[InsertUpdateOrderWindow]
	@ZoneList varchar(500),
	@StoreList varchar(500),
	@SubTeamList varchar(500),
	@OrderStart varchar(10),
	@OrderEnd varchar(10),
	@OrderEndTransfers varchar(10)
AS 
BEGIN    
    SET NOCOUNT ON    
    
    DECLARE @tblZone TABLE (Zone_Id int)
	DECLARE @tblSubTeam TABLE (SubTeam_No int)
	DECLARE @tblStore TABLE (Store_No int)
	DECLARE @Zone_Id int
	DECLARE @Store_No int
	DECLARE @SubTeam_No int
	
	INSERT INTO @tblZone
		SELECT Key_Value FROM dbo.fn_Parse_List(@ZoneList,'|')
		
	INSERT INTO @tblSubTeam
		SELECT Key_Value FROM dbo.fn_Parse_List(@SubTeamList,'|')
		
	INSERT INTO @tblStore
		SELECT Key_Value FROM dbo.fn_Parse_List(@StoreList,'|')		
		
	DECLARE c1 CURSOR READ_ONLY
		FOR
			SELECT S.Zone_Id, SubTeam_No, S1.Store_No
			FROM
				(SELECT *
				FROM @tblZone
				CROSS JOIN @tblSubTeam) AS A
				CROSS JOIN @tblStore s1
				JOIN Store S ON S.Store_No = s1.Store_No
				WHERE A.Zone_Id = S.Zone_Id

		OPEN c1

		FETCH NEXT FROM c1
		INTO @Zone_Id, @SubTeam_No, @Store_No

		WHILE @@FETCH_STATUS = 0
		BEGIN

			IF EXISTS(SELECT * FROM ZoneSubTeam WHERE Zone_Id = @Zone_Id AND Supplier_Store_No = @Store_No AND SubTeam_No = @SubTeam_No)
				BEGIN
					UPDATE ZoneSubTeam SET OrderStart = @OrderStart, OrderEnd = @OrderEnd, OrderEndTransfers = @OrderEndTransfers
					WHERE Zone_Id = @Zone_Id AND Supplier_Store_No = @Store_No AND SubTeam_No = @SubTeam_No
				END
			ELSE
				BEGIN
					INSERT INTO ZoneSubTeam (Zone_Id, SubTeam_No, Supplier_Store_No, OrderStart, OrderEnd, OrderEndTransfers) 
						VALUES (@Zone_Id, @SubTeam_No, @Store_No, @OrderStart, @OrderEnd, @OrderEndTransfers) 
				END

			FETCH NEXT FROM c1
			INTO @Zone_ID, @SubTeam_No, @Store_No
		END
	CLOSE c1
	DEALLOCATE c1	
        
    SET NOCOUNT OFF            
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertUpdateOrderWindow] TO [IRMAClientRole]
    AS [dbo];

