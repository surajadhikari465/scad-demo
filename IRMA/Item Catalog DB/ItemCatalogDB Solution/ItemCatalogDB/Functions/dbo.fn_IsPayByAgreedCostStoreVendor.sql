if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fn_IsPayByAgreedCostStoreVendor]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[fn_IsPayByAgreedCostStoreVendor]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

Create FUNCTION dbo.fn_IsPayByAgreedCostStoreVendor
(
	@Store_No int
	,@Vendor_ID int
	,@Date smalldatetime
)
RETURNS bit
AS

BEGIN 

RETURN
	CASE
		WHEN EXISTS 
			   (SELECT * FROM PayOrderedCost PC (nolock) 
			   WHERE PC.Vendor_ID = @Vendor_ID 
			   AND PC.Store_No = @Store_No 
			   AND isnull(@Date, getdate()) >= PC.BeginDate) 
		THEN 1
		ELSE 0
	END			        
	 
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

 