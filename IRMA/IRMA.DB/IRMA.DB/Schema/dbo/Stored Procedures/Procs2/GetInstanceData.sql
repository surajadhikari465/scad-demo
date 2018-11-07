CREATE PROCEDURE dbo.GetInstanceData 
AS
BEGIN
    SET NOCOUNT ON

     --20100215 - Dave Stacey - Add UG Culture, DateMask to facilitate UK EIM 
     
     
	SELECT PrimaryRegionName, PrimaryRegionCode, PluDigitsSentToScale, UG_Culture, UG_DateMask
    FROM dbo.InstanceData (NOLOCK)
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInstanceData] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInstanceData] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInstanceData] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInstanceData] TO [IRMAExcelRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInstanceData] TO [IRMASLIMRole]
    AS [dbo];

