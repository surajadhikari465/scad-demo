SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CheckForDuplicateManifest]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CheckForDuplicateManifest]
GO

CREATE PROCEDURE dbo.CheckForDuplicateManifest
@DistLoc_ID int, 
@Customer_ID int 
AS 
SELECT COUNT(*) AS ManifestCount 
FROM Manifest 
WHERE Customer_ID = @Customer_ID AND 
DistLoc_ID = @DistLoc_ID AND 
ShipDate IS NULL 

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


