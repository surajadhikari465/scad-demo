SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DeleteRetentionPolicy]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[DeleteRetentionPolicy]
GO

CREATE PROCEDURE  dbo.DeleteRetentionPolicy
(
	@RetentionPolicyId int
)
AS 
BEGIN
		DELETE RetentionPolicy 
		WHERE RetentionPolicyId = @RetentionPolicyId

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 