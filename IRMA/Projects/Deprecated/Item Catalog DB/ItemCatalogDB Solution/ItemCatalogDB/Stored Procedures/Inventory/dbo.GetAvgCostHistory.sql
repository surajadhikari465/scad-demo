SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetAvgCostHistory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetAvgCostHistory]
GO


CREATE PROCEDURE [dbo].[GetAvgCostHistory]
	@Item_Key int,
	@Store_No int,
	@SubTeam_No int,
	@MaxRows int = 1000
AS

DECLARE @SQL varchar(MAX)

SELECT @SQL = 'SELECT TOP ' + CONVERT(varchar, @MaxRows) + ' 
					ACH.Quantity, 
					ACH.AvgCost, 
					ACH.Effective_Date, 
					ISNULL(ACR.Description, '''') As Reason, 
					ISNULL(Users.Username, ''PO# '' + CONVERT(varchar(255),OI.Orderheader_ID)) As Source
				FROM 
					AvgCostHistory ACH(nolock)
				LEFT JOIN 
					AvgCostAdjReason ACR(nolock) ON ACH.Reason = ACR.ID
				LEFT JOIN 
					Users(nolock) ON ACH.User_ID = Users.User_ID
				LEFT JOIN
					ItemHistory(nolock) IH ON ACH.Item_Key = IH.Item_Key
						AND ACH.Store_No = IH.Store_No
						AND ACH.Item_Key = IH.Item_Key
						AND CONVERT(VARCHAR(10), IH.DateStamp, 101) = CONVERT(VARCHAR(10), ACH.Effective_Date, 101)
						AND IH.Adjustment_ID = 5
				LEFT JOIN
					OrderItem(nolock) OI ON OI.OrderItem_ID = IH.OrderItem_ID
				WHERE		(ACH.Item_Key = ' + CONVERT(varchar, @Item_Key) + ') 
				AND			(ACH.Store_No = ' + CONVERT(varchar, @Store_No) + ')
				AND			(ACH.SubTeam_No = ' + CONVERT(varchar, @SubTeam_No) + ')
				ORDER BY ACH.Effective_Date DESC '

EXEC (@SQL)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO