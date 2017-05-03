SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertImportCycleCount]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertImportCycleCount]
GO

CREATE PROCEDURE dbo.InsertImportCycleCount
@Store_No int,
@Start_Scan datetime,
@End_Scan datetime
AS 
BEGIN
    SET NOCOUNT ON

    INSERT INTO CycleImportHeader ( Store_No, Start_Scan, End_Scan)
    VALUES ( @Store_No, @Start_Scan, @End_Scan)

    SELECT MAX(CycleImportHeader_ID) AS CycleImportHeaderID
    FROM CycleImportHeader

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


