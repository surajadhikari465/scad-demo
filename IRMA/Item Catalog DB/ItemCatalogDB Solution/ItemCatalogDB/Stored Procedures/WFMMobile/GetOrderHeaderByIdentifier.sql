SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetOrderHeaderByIdentifier')
	BEGIN
		DROP Procedure [dbo].GetOrderHeaderByIdentifier
	END
GO

CREATE PROCEDURE [dbo].[GetOrderHeaderByIdentifier] 
	@UPC			AS varchar(13), 
	@StoreNumber	AS int

AS

-- ****************************************************************************************************************
-- Procedure: GetOrderHeaderByIdentifier()
--    Author: Kyle Milner
--      Date: 2012-10-22
--
-- Description:
-- This procedure is called from the IRMA Plugin form FindOrderByItem.vb.  It returns orders which are
-- associated with a particular identifier.
--
--	Conditions:
--		>> Order must be sent
--		>> Order must be open
--		>> Order must originate from the current user's store
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2012-10-22	KM		8319	Initial creation;
-- 2012-10-25	KM		8319	Select st.SubteamName instead of oh.Transfer_To_Subteam;
-- 2012-10-25	KM		8319	Remove @Subteam input parameter; Simplify joins;
-- 2012-10-29	KM		8319	Join with Vendor instead of using a subquery in the WHERE clause;
-- 2013-05-10	KM		12037	Add ORDER BY clause;
-- ****************************************************************************************************************

BEGIN
	SELECT
		oh.OrderHeader_ID,
		st.SubTeam_Name,
		oh.OrderedCost,
		oh.Expected_Date,
		oh.eInvoice_Id

	FROM
		OrderHeader				(NOLOCK) oh
		JOIN OrderItem			(NOLOCK) oi		ON oh.OrderHeader_ID		= oi.OrderHeader_ID
		JOIN ItemIdentifier		(NOLOCK) ii		ON oi.Item_Key				= ii.Item_Key
		JOIN SubTeam			(NOLOCK) st		ON oh.Transfer_To_SubTeam	= st.SubTeam_No
		JOIN Vendor				(NOLOCK) v		ON oh.PurchaseLocation_ID	= v.Vendor_ID
	
	WHERE
		ii.Identifier = @UPC
		AND oh.SentDate	IS NOT NULL
		AND oh.CloseDate IS NULL
		AND v.Store_no = @StoreNumber

	ORDER BY
		oh.OrderHeader_ID DESC
END
GO