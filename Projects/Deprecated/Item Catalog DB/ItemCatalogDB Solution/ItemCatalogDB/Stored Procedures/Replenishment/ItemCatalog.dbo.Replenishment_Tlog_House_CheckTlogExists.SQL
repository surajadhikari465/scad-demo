/****** Object:  StoredProcedure [dbo].[Replenishment_Tlog_House_CheckTlogExists]    Script Date: 02/28/2013 15:33:14 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Replenishment_Tlog_House_CheckTlogExists]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Replenishment_Tlog_House_CheckTlogExists]
GO
/****** Object:  StoredProcedure [dbo].[Replenishment_Tlog_House_CheckTlogExists]    Script Date: 09/25/2006 08:33:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.Replenishment_Tlog_House_CheckTlogExists
	@storeNo int,
    @tlogDate datetime
AS

BEGIN
	SET NOCOUNT ON;
	
	SELECT Tlog_Count = COUNT(*)
      FROM dbo.Sales_SumByItem (NOLOCK)
     WHERE Store_No = @storeNo
       AND Date_Key = CONVERT(date, @tlogDate)
     GROUP BY Date_Key, Store_No
		
	SET NOCOUNT OFF;		
END
GO
