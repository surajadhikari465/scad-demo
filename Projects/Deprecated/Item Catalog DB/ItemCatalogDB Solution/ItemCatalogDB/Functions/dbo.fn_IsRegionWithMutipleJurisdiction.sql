
IF Exists (Select * From dbo.sysobjects where id = object_id(N'[dbo].[fn_IsRegionWithMutipleJurisdiction]') and xtype in (N'FN', N'IF', N'TF'))
DROP FUNCTION [dbo].[fn_IsRegionWithMutipleJurisdiction]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE FUNCTION [dbo].[fn_IsRegionWithMutipleJurisdiction]
(
)
RETURNS Bit
AS
  -- **************************************************************************
   -- Procedure: fn_IsRegionWithMutipleJurisdiction()
   --    Author: Faisal Ahmed
   --      Date: 04/24/2013
   --
   -- Description:
   -- This function returns true if the region has multiple jurisdiction
   --
   -- Modification History:
   -- Date        Init	Comment
   -- 04/24/2013  FA	Initial Version
   -- **************************************************************************
BEGIN
    DECLARE @Count As Integer	

    SELECT @Count = Count (*) 
    FROM StoreJurisdiction
    
    IF @Count > 1 RETURN 1
    Return 0
END
GO