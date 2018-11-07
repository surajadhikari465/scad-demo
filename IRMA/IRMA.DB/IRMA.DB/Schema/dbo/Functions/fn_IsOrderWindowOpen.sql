CREATE FUNCTION [dbo].[fn_IsOrderWindowOpen]
(
    @ZoneList varchar(500),
    @SubTeamList varchar(500),
    @StoreList varchar(500)
)
RETURNS VARCHAR(5000)
AS
BEGIN
    DECLARE @tblZone TABLE (Zone_Id int)
	DECLARE @tblSubTeam TABLE (SubTeam_No int)
	DECLARE @tblStore TABLE (Store_No int)
	DECLARE @Zone_Id int
	DECLARE @Store_No int
	DECLARE @SubTeam_No int
	DECLARE @OrderWindowOpen datetime
	DECLARE @OrderWindowEnd datetime
	DECLARE @OrderWindowEndTransfers datetime
	DECLARE @ErrorString varchar(5000)
	DECLARE @CurrentTime datetime
	DECLARE @Zone_Name varchar(50)
	DECLARE @SubTeam_Name varchar(50)
	DECLARE @Store_Name varchar(50)

	SELECT @ErrorString = ''
	
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
		
			SELECT @OrderWindowOpen = OrderStart FROM ZoneSubTeam WHERE Zone_Id = @Zone_Id AND SubTeam_No = @SubTeam_No AND Supplier_Store_No = @Store_No
			SELECT @OrderWindowEnd = OrderEnd FROM ZoneSubTeam WHERE Zone_Id = @Zone_Id AND SubTeam_No = @SubTeam_No AND Supplier_Store_No = @Store_No
			SELECT @OrderWindowEndTransfers = OrderEndTransfers FROM ZoneSubTeam WHERE Zone_Id = @Zone_Id AND SubTeam_No = @SubTeam_No AND Supplier_Store_No = @Store_No
			SELECT @CurrentTime = GETDATE()

			IF CONVERT(VARCHAR,@CurrentTime,108) > CONVERT(VARCHAR,@OrderWindowOpen,108) AND ((CONVERT(VARCHAR,@CurrentTime,108) < CONVERT(VARCHAR,@OrderWindowEnd,108)) OR (CONVERT(VARCHAR,@CurrentTime,108) < CONVERT(VARCHAR,@OrderWindowEndTransfers,108)))
				BEGIN
					SELECT @Zone_Name = Zone_Name FROM Zone WHERE Zone_Id = @Zone_Id
					SELECT @SubTeam_Name = SubTeam_Name FROM SubTeam WHERE SubTeam_No = @SubTeam_No
					SELECT @Store_Name = Store_Name FROM Store WHERE Store_No = @Store_No

					IF @ErrorString = '' 
						SELECT @ErrorString = ',' + @Zone_Name + '|' + @SubTeam_Name + '|' + @Store_Name
					ELSE
						SELECT @ErrorString = @ErrorString + ',' + @Zone_Name + '|' + @SubTeam_Name + '|' + @Store_Name
				END

			FETCH NEXT FROM c1
			INTO @Zone_ID, @SubTeam_No, @Store_No
		END
	CLOSE c1
	DEALLOCATE c1	
	
	RETURN @ErrorString
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsOrderWindowOpen] TO [IRMAClientRole]
    AS [dbo];

