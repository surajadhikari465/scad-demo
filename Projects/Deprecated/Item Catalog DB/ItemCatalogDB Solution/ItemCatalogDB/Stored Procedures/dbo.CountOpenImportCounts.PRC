SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CountOpenImportCounts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CountOpenImportCounts]
GO

CREATE PROCEDURE dbo.CountOpenImportCounts
@Store_No int
AS 
SELECT COUNT(*) AS CountImportCounts
FROM CycleImportHeader
WHERE  Store_No = @Store_No AND Closed = 0

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


