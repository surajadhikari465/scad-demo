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
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsRegionWithMutipleJurisdiction] TO [IRMAClientRole]
    AS [dbo];

