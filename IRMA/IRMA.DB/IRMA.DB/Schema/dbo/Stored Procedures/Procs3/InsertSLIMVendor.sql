CREATE PROCEDURE dbo.InsertSLIMVendor
    @CompanyName varchar(50),
    @PS_Vendor_ID varchar(10),
    @PS_Address_Sequence varchar(2),
    @PS_Export_Vendor_ID varchar(10)
AS
	-- **************************************************************************
	-- Procedure: InsertSLIMVendor()
	--    Author: 
	--      Date: 
	--
	-- Modification History:
	-- Date			Init	Comment
	-- 2013-09-10   FA		Add transaction isolation level
	-- **************************************************************************
BEGIN
    SET NOCOUNT ON

	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	
	BEGIN TRAN

    INSERT INTO Vendor (CompanyName, PS_Vendor_ID, PS_Export_Vendor_ID, PS_Address_Sequence, AddVendor)
    VALUES (@CompanyName, @PS_Vendor_ID, @PS_Export_Vendor_ID, @PS_Address_Sequence, 1)

	select Scope_Identity()
    
	COMMIT TRAN

	SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertSLIMVendor] TO [IRMASLIMRole]
    AS [dbo];

