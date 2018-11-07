-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ItemDefaultAttributes_UpdateItemDefaultAttribute]
	@itemDefaultAttribute_ID int,
	@attributeName VARCHAR(50),
	@active BIT,
	@controlOrder TINYINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE ItemDefaultAttribute SET
    AttributeName = @attributeName,
    Active = @active,
    ControlOrder = @controlOrder
    WHERE 
    ItemDefaultAttribute_ID = @itemDefaultAttribute_ID
    
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemDefaultAttributes_UpdateItemDefaultAttribute] TO [IRMAClientRole]
    AS [dbo];

