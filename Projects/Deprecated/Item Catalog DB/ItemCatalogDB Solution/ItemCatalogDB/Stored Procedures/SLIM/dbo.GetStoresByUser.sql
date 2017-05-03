if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetStoresByUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetStoresByUser]
GO

CREATE PROCEDURE dbo.GetStoresByUser
    @Store_No int,
    @UserAccess int
AS
	-- **************************************************************************
	-- Procedure: GetStoresByUser()
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

	declare @Zone_ID int

	select @Zone_ID = zone_id from store where store_no = @Store_No

	if @UserAccess = 1
	begin
		SELECT Store_Name,Store.Store_No
		FROM Store (NOLOCK)
		where Store_no = @Store_no
		And (Mega_Store = 1 OR WFM_Store = 1) AND dbo.fn_GetCustomerType(Store.Store_No, Internal, BusinessUnit_ID) = 3 -- Regional
		ORDER BY Store_Name,Store_No
	end
	else if @UserAccess = 2
	begin
	SELECT Store_Name,Store.Store_No
		FROM Store (NOLOCK)
		INNER JOIN Zone (NOLOCK) ON Store.Zone_Id = Zone.Zone_ID
		where Zone.Zone_ID = @Zone_ID
		And (Mega_Store = 1 OR WFM_Store = 1) AND dbo.fn_GetCustomerType(Store.Store_No, Internal, BusinessUnit_ID) = 3 -- Regional
		ORDER BY Store_Name,Store_No
	end
	else
	begin
	SELECT Store_Name,Store.Store_No
		FROM Store (NOLOCK)
		where (Mega_Store = 1 OR WFM_Store = 1) AND dbo.fn_GetCustomerType(Store.Store_No, Internal, BusinessUnit_ID) = 3 -- Regional
		ORDER BY Store_Name,Store_No
	end

	COMMIT TRAN

	SET NOCOUNT OFF

END

GO

