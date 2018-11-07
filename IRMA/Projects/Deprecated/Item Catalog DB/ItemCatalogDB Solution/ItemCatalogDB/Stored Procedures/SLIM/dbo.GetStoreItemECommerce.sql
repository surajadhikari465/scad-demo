
IF exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetStoreItemECommerce]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetStoreItemECommerce]
GO

CREATE PROCEDURE [dbo].[GetStoreItemECommerce]
		@Item_Key int
AS

-- =============================================
-- Author:		Faisal Ahmed
-- Create date: 7-4-07
-- Description:	Retrieves Store ECommerce for a passed in Item_Key
--
-- Date			TFS		Init		Comment
-- 2013-10-17	14298   FA			Initial Version
-- =============================================

BEGIN

	SET NOCOUNT ON;
	
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	
	BEGIN TRAN

	SELECT S.Store_No,
		   S.Store_Name,
		   S.StoreJurisdictionID,
		   SI.ECommerce,
			(SELECT TOP 1 
				StoreItemVendorID
			FROM
				StoreItemVendor
			WHERE
				Store_No = S.Store_No
				AND
				Item_Key = @Item_Key
				AND
				PrimaryVendor = 1) AS StoreItemVendorID
	FROM Store S (NOLOCK)
		LEFT OUTER JOIN StoreItem SI (NOLOCK) ON S.Store_No = Si.Store_No
	WHERE Item_Key = @Item_Key
		AND Distribution_Center = 0
		AND Regional = 0
		AND (EXEWarehouse = 0 OR EXEWarehouse IS NULL)
		AND (WFM_Store = 1 OR Mega_Store = 1)
	ORDER BY S.Store_Name

	COMMIT TRAN
END

GO
