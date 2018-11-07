if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetStoreItemAuths]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetStoreItemAuths]
GO
/****** Object:  StoredProcedure [dbo].[GetStoreItemAuths]    Script Date: 07/04/2007 10:10:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetStoreItemAuths]
		@Item_Key int

AS

-- =============================================
-- Author:		Greg Bowles
-- Create date: 7-4-07
-- Description:	Retrieves Store Auths for a passed in Item_Key
--
-- Date			TFS		Init		Comment
-- 2013-04-09	11745	KM			Add StoreJurisdictionID to the result set;
-- 2013-09-10   13661	FA      	Add transaction isolation level
-- =============================================

BEGIN

	SET NOCOUNT ON;
	
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	
	BEGIN TRAN

	SELECT S.Store_No,
		   S.Store_Name,
		   S.StoreJurisdictionID,
		   	SI.Authorized,
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