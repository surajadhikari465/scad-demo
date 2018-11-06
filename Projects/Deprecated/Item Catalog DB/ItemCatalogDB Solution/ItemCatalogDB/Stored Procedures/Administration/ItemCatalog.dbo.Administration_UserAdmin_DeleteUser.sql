IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Administration_UserAdmin_DeleteUser]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Administration_UserAdmin_DeleteUser]
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.Administration_UserAdmin_DeleteUser
@User_Id int
AS
BEGIN
-- Deleting a User account sets the AccountEnabled flag to False
UPDATE Users
   SET [AccountEnabled] = 0
 WHERE  User_ID = @User_Id

-- 8.25.08 V3.2 Robert Shurbet
-- Added second update statement to support SLIM access flags

UPDATE SlimAccess
	SET UserAdmin = 0,
		ItemRequest = 0,
		VendorRequest = 0,
		IRMAPush = 0,
		StoreSpecials = 0,
		RetailCost = 0,
		Authorizations = 0,
		WebQuery = 0,
		ScaleInfo = 0
WHERE User_ID = @User_ID

END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


