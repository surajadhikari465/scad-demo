
/****** Object:  StoredProcedure [dbo].[GetAvgCostForStores] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[GetAvgCostForStores]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetAvgCostForStores]
GO

CREATE PROCEDURE [dbo].[GetAvgCostForStores]
	@Item_Key int,
	@Store_No int,
	@AllWFM bit,
	@AllHFM bit,
	@Zone int,
	@State varchar(2),
	@SubTeam_No int,
	@Current bit,
	@Start_Date datetime,
	@End_Date datetime,
	@MaxRows int = 1000

---- ********************************************************************************************************
---- Procedure: GetAvgCostForStores()
----    Author: Mugdha Deshpande
----      Date: 07/03/2012
---- Description: This procedure is used to get current average cost or average cost history 
----              for an item, subtem and selected store criteria
----
---- ********************************************************************************************************
AS
BEGIN
	If (@current = 1)
		BEGIN 	
			DECLARE @Temp Table (AvgCostHistoryID int, Store_No int)
			INSERT @Temp
				SELECT (SELECT TOP 1 AvgCostHistoryID
                         FROM AvgCostHistory (nolock)
                         WHERE Item_Key = @Item_Key
                             AND Store_No = S.Store_No
                             AND SubTeam_No = @SubTeam_No
                         ORDER BY Effective_Date DESC), 
                         S.Store_No
				FROM 
					Store (nolock) S
				INNER JOIN
					Vendor (nolock) V ON V.Store_No = S.Store_No
				WHERE		S.Store_No =	ISNULL(@Store_No, S.Store_No)
				AND			S.WFM_Store =	ISNULL(@AllWFM, S.WFM_Store)
				AND			S.Mega_Store =	ISNULL(@AllHFM, Mega_Store)
				AND			S.Zone_ID =		ISNULL(@Zone, S.Zone_ID)
				AND			V.State =		ISNULL(@State, V.State)
				
			SELECT 
			S.Store_Name,
			ACH.AvgCost, 
			ACH.Effective_Date, 
			ISNULL(ACR.Description, '') As Reason, 
			ISNULL(Users.Username, 'PO#' + CONVERT(varchar(255),OI.Orderheader_ID)) As Source
			FROM @Temp T
			INNER JOIN AvgCostHistory (nolock) ACH ON T.AvgCostHistoryID = ACH.AvgCostHistoryId
			LEFT JOIN AvgCostAdjReason (nolock) ACR ON ACH.Reason = ACR.ID
			LEFT JOIN Users(nolock) ON ACH.User_ID = Users.User_ID
			LEFT JOIN ItemHistory(nolock) IH ON ACH.Item_Key = IH.Item_Key
						AND ACH.Store_No = IH.Store_No
						AND ACH.Item_Key = IH.Item_Key
						AND CONVERT(VARCHAR(10), IH.DateStamp, 101) = CONVERT(VARCHAR(10), ACH.Effective_Date, 101)
						AND IH.Adjustment_ID = 5
			LEFT JOIN OrderItem(nolock) OI ON OI.OrderItem_ID = IH.OrderItem_ID		
			INNER JOIN Store(nolock) S ON ACH.Store_No = S.Store_No

		END
	ELSE
		BEGIN
			SELECT Top (@MaxRows)
				S.Store_Name,
				ACH.AvgCost, 
				ACH.Effective_Date, 
				ISNULL(ACR.Description, '') As Reason, 
				ISNULL(Users.Username, 'PO#' + CONVERT(varchar(255),OI.Orderheader_ID)) As Source
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
			INNER JOIN 
				Store (nolock) S ON ACH.Store_No = S.Store_No
			INNER JOIN
				Vendor (nolock) V ON V.Store_No = S.Store_No
			WHERE		ACH.Item_Key = @Item_Key 
			AND			ACH.Store_No = ISNULL(@Store_No, ACH.Store_No)
			AND			ACH.SubTeam_No = @SubTeam_No
			AND			S.WFM_Store = ISNULL(@AllWFM, S.WFM_Store)
			AND			S.Mega_Store = ISNULL(@AllHFM, Mega_Store)
			AND			S.Zone_ID = ISNULL(@Zone, S.Zone_ID)
			AND			V.State = ISNULL(@State, V.State)
			AND			Effective_Date BETWEEN @Start_Date AND @End_Date
			ORDER BY S.Store_Name, ACH.Effective_Date Desc 	
		END
END	                       
GO