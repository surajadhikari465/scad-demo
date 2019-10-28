CREATE PROCEDURE  dbo.DeleteRetentionPolicy
	@RetentionPolicyId int
AS 
BEGIN
		DELETE RetentionPolicy 
		WHERE RetentionPolicyId = @RetentionPolicyId
END
GO

GRANT EXECUTE ON OBJECT::dbo.DeleteRetentionPolicy TO [IRSUser] AS [dbo];
GO
GRANT EXECUTE ON OBJECT::dbo.DeleteRetentionPolicy TO [IRMAClientRole] AS [dbo];
GO