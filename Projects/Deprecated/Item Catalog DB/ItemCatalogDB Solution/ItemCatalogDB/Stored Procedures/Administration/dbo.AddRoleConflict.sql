SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[AddRoleConflict]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[AddRoleConflict]
GO

CREATE PROCEDURE dbo.AddRoleConflict
	@Role1 varchar(50),
	@Role2 varchar(50)
AS 
	INSERT INTO dbo.RoleConflicts (Role1, Role2) VALUES (@Role1, @Role2)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO  