CREATE PROCEDURE [dbo].[UpdateInstanceData] 
	@PluDigitsSentToScale varchar(20),
	@UGCulture varchar(5),
	@UGDateMask varchar(12)
AS
BEGIN
    SET NOCOUNT ON

	UPDATE dbo.InstanceData SET PluDigitsSentToScale = @PluDigitsSentToScale,  UG_Culture = @UGCulture,  UG_DateMask = @UGDateMask
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateInstanceData] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateInstanceData] TO [IRMAClientRole]
    AS [dbo];

