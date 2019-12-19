-- =============================================
-- Author:		Anjana
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[InsertBulkUpload] 
	@UPC as varchar(25)
   ,@Reason as varchar(200)
   ,@ExpirationDate as datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    Insert into KNOWN_OOS_DETAIL (upc,ProductStatus,ExpirationDate) values 
    (@UPC,@Reason,@ExpirationDate)
    
    select SCOPE_IDENTITY() as RecordID
    
END
