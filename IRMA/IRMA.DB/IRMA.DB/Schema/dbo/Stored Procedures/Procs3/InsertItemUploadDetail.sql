CREATE PROCEDURE dbo.InsertItemUploadDetail
	@ItemUploadHeader_ID as int, 
	@ItemIdentifier as varchar(200),
	@POSDescription as varchar(200),
	@Description as varchar(200),
	@TaxClassID as varchar(200),
	@FoodStamps as varchar(200),
	@RestrictedHours as varchar(200),
	@EmployeeDiscountable as varchar(200),
	@Discontinued as varchar(200),
	@NationalClassID as varchar(200)
AS
BEGIN
    SET NOCOUNT ON
            
    DECLARE 
		@ItemIdentifierValid bit,
		@SubTeamAllowed bit,
		@SubTeam_No int,
		@User_ID as int, 
		@Item_Key as int
	
	SELECT @User_ID = (SELECT [User_ID] FROM ItemUploadHeader 
						WHERE ItemUploadHeader_ID = @ItemUploadHeader_ID)

	-- Is the Identifier Valid? ------------------------------------
	IF EXISTS (SELECT Identifier FROM ItemIdentifier II 
				WHERE II.Identifier = @ItemIdentifier AND II.Default_Identifier = 1 AND Deleted_Identifier = 0)
		BEGIN
			SET @ItemIdentifierValid = 1 
			-- Get the SubTeam Number -------------------------------------------
			SELECT @SubTeam_No = (SELECT SubTeam_No FROM Item I 
					INNER JOIN ItemIdentifier II ON I.Item_Key = II.Item_Key 
					WHERE II.Identifier = @ItemIdentifier AND II.Default_Identifier = 1 AND Deleted_Identifier = 0) 

			-- Get the Item_Key
			SELECT @Item_Key = (SELECT I.Item_Key FROM Item I 
					INNER JOIN ItemIdentifier II ON I.Item_Key = II.Item_Key 
					WHERE II.Identifier = @ItemIdentifier AND II.Default_Identifier = 1 AND Deleted_Identifier = 0)

			-- Is this user allowed to work with this SubTeam? ---------------------------
			IF (EXISTS (SELECT SubTeam_No FROM UsersSubTeam US 
						WHERE US.[User_ID] = @User_ID AND US.SubTeam_No = @SubTeam_No)
			OR EXISTS (SELECT UserName FROM Users WHERE [User_ID] = @User_ID AND SuperUser = 1))
					SET @SubTeamAllowed = 1 
			ELSE
					SET @SubTeamAllowed = 0
		END
	ELSE
		BEGIN
			SET @ItemIdentifierValid = 0
			-- If Identifier is invalid, then there will be no subteam
			SET @SubTeam_No = NULL
			SET @Item_Key = NULL
			SET @SubTeamAllowed = 0
		END

	
	--  Finally insert the record ---------------------------------------------
	INSERT INTO dbo.ItemUploadDetail (	
		ItemUploadHeader_ID, 
		ItemIdentifier, 
		POSDescription, 
		Description, 
		TaxClassID, 
		FoodStamps, 
		RestrictedHours, 
		EmployeeDiscountable,
		Discontinued, 
		NationalClassID,
		SubTeam_No,
		ItemIdentifierValid,
		SubTeamAllowed,
		Item_Key)
	VALUES (
		@ItemUploadHeader_ID, 
		@ItemIdentifier, 
		@POSDescription, 
		@Description, 
		@TaxClassID, 
		@FoodStamps, 
		@RestrictedHours, 
		@EmployeeDiscountable,
		@Discontinued, 
		@NationalClassID,
		@SubTeam_No,
		@ItemIdentifierValid,
		@SubTeamAllowed,
		@Item_Key)

    SET NOCOUNT OFF


END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertItemUploadDetail] TO [IRMAClientRole]
    AS [dbo];

