if exists (select * from dbo.sysobjects where id = object_id(N'dbo.UpdateItemUploadDetail') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.UpdateItemUploadDetail
GO


CREATE PROCEDURE dbo.UpdateItemUploadDetail
	@ItemUploadDetail_ID as int, 
	@ItemIdentifier as varchar(200),
	@POSDescription as varchar(200),
	@Description as varchar(200),
	@TaxClassID as varchar(200),
	@FoodStamps as varchar(200),
	@RestrictedHours as varchar(200),
	@EmployeeDiscountable as varchar(200),
	@Discontinued as varchar(200),
	@NationalClassID as varchar(200),
	@Uploaded as bit
AS
BEGIN
    SET NOCOUNT ON
if  exists (select * from dbo.ItemUploadDetail where
 ItemUploadDetail_ID = @ItemUploadDetail_ID)

begin					
	UPDATE dbo.ItemUploadDetail SET 	
		ItemIdentifier = ISNULL(@ItemIdentifier, ItemIdentifier),
		POSDescription = ISNULL(@POSDescription, POSDescription),
		Description = ISNULL(@Description, Description),
		TaxClassID = ISNULL(@TaxClassID, TaxClassID),
		FoodStamps = ISNULL(@FoodStamps, FoodStamps),
		RestrictedHours = ISNULL(@RestrictedHours,RestrictedHours),
		EmployeeDiscountable = ISNULL (@EmployeeDiscountable,EmployeeDiscountable),
		Discontinued = ISNULL(@Discontinued, Discontinued),
		NationalClassID = ISNULL(@NationalClassID, NationalClassID),
		Uploaded = @Uploaded 
	WHERE 
		ItemUploadDetail_ID = @ItemUploadDetail_ID
end

else

begin 
INSERT INTO [dbo].[ItemUploadDetail]

           ([ItemIdentifier]
           ,[POSDescription]
           ,[Description]
           ,[TaxClassID]
           ,[FoodStamps]
           ,[RestrictedHours]
           ,[EmployeeDiscountable]
           ,[Discontinued]
           ,[NationalClassID]
           ,[Uploaded])
          
     VALUES
           (        
           @ItemIdentifier
           ,@POSDescription
           ,@Description
           ,@TaxClassID
           ,@FoodStamps
           ,@RestrictedHours
           ,@EmployeeDiscountable
           ,@Discontinued
           ,@NationalClassID
           ,@Uploaded    ) 
end
END


GO
