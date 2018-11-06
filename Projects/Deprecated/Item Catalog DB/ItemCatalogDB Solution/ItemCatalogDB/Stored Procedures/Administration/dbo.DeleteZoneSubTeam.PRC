SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteZoneSubTeam]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteZoneSubTeam]
GO


CREATE PROCEDURE dbo.DeleteZoneSubTeam
	@Zone_ID int,
	@SubTeam_No int,
	@Supplier_Store_No int
AS 

DELETE
FROM ZoneSubTeam
WHERE Zone_ID = @Zone_ID AND SubTeam_No = @SubTeam_No AND Supplier_Store_No = @Supplier_Store_No

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

