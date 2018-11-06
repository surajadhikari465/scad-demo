CREATE PROCEDURE dbo.SOG_AddOrder
	@CatalogID		int,
	@VendorID		int,
	@StoreID		int,
	@UserID			int,
	@FromSubTeamID	int,
	@ToSubTeamID	int,
	@ExpectedDate	smalldatetime,
	@CatalogOrderID	int OUTPUT
WITH RECOMPILE
AS

-- **************************************************************************
-- Procedure: SOG_AddOrder()
--    Author: Billy Blackerby
--      Date: 3/25/2009
--
-- Description:
-- Utilized by StoreOrderGuide to insert an Order
--
-- Modification History:
-- Date			Init	Comment
-- 03/25/2009	BBB		Creation
-- 03/27/2009	BBB		Added ExpectedDate and GroupBy for catalogs with multi
--						stores authorized
-- 04/23/2009	BBB		Added join to ItemManager to retrieve newly added
--						Vendor_ID column
-- 06/06/2009	BBB		Updated ExpectedDate, OrderDate, SentDate values to only
--						pass the date value to OrderHeader
-- **************************************************************************	
BEGIN
    SET NOCOUNT ON
	--**************************************************************************
	--Get ExpectedDate Variable Value
	--**************************************************************************
	IF @ExpectedDate = NULL OR @ExpectedDate = '1/1/1900'
		SET @ExpectedDate = DATEADD(hh, CONVERT(int, ISNULL((SELECT AdminValue FROM CatalogAdmin WHERE AdminKey = 'ExpectedDate'), 36)), GETDATE())
    
	--**************************************************************************
	--Main SQL
	--**************************************************************************
	INSERT INTO 
		CatalogOrder
	(
		CatalogID,
		VendorID,
		StoreID,
		UserID,
		FromSubTeamID,
		ToSubTeamID,
		ExpectedDate
	)
	(
		SELECT
			@CatalogID,
			im.Vendor_ID,
			s.Store_No,
			@UserID,
			@FromSubTeamID,
			c.SubTeam,
			CONVERT(varchar(10), @ExpectedDate, 101)
		FROM
			Catalog					(nolock) c
			INNER JOIN CatalogStore	(nolock) cs	ON c.CatalogID		= cs.CatalogID
			INNER JOIN Store		(nolock) s	ON s.Store_No		= @StoreID
			INNER JOIN ItemManager	(nolock) im	ON c.ManagedByID	= im.Manager_ID
		WHERE
			c.CatalogID = @CatalogID
		GROUP BY
			im.Vendor_ID,
			s.Store_No,
			c.SubTeam
	)

	--**************************************************************************
	--Return ID
	--**************************************************************************
	SET @CatalogOrderID = SCOPE_IDENTITY()
	
	RETURN
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SOG_AddOrder] TO [IRMASLIMRole]
    AS [dbo];

