if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[StoreItemAttribute_InsertUpdateAttribute]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[StoreItemAttribute_InsertUpdateAttribute]
GO

CREATE PROCEDURE dbo.StoreItemAttribute_InsertUpdateAttribute
	@ID int,
	@Store_No int,
	@Item_Key int,
	@Exempt bit,
	@User_ID int
AS 
BEGIN 
	IF @ID > 0 
		BEGIN
			UPDATE 
				StoreItemAttribute 
			SET 
				Exempt = @Exempt,
				ModifyDate = GETDATE(),
				UserID = @User_ID
			WHERE 
				StoreItemAttribute_ID = @ID
		END
	ELSE
		BEGIN
			INSERT INTO StoreItemAttribute
				(Store_No, Item_Key, Exempt, CreateDate, ModifyDate, UserID)
			VALUES 
				(@Store_No, @Item_Key, @Exempt, GETDATE(), GETDATE(), @User_ID)
		END
END
GO

