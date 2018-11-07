
CREATE PROCEDURE  dbo.DeleteRetentionPolicy
(
	@RetentionPolicyId int
)
AS 
BEGIN
		DELETE RetentionPolicy 
		WHERE RetentionPolicyId = @RetentionPolicyId

END

