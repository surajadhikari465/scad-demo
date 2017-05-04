SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertZoneSubTeam]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertZoneSubTeam]
GO


CREATE PROCEDURE dbo.InsertZoneSubTeam
@Zone_ID int,
@SubTeam_No int,
@Supplier_Store_No int
AS 

INSERT INTO ZoneSubTeam (Zone_ID, SubTeam_No, Supplier_Store_No)
VALUES (@Zone_ID, @SubTeam_No, @Supplier_Store_No)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


