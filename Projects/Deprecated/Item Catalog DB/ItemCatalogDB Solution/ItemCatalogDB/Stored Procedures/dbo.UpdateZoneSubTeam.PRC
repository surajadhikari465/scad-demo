SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateZoneSubTeam]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateZoneSubTeam]
GO


CREATE PROCEDURE dbo.UpdateZoneSubTeam
@Zone_ID int,
@SubTeam_No int,
@Supplier_Store_No int
AS 

UPDATE ZoneSubTeam
SET Supplier_Store_No = @Supplier_Store_No
WHERE Zone_ID = @Zone_ID AND SubTeam_No = @SubTeam_No

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


