CREATE PROCEDURE [dbo].[GetGLUploadInventoryAdjustment]
    @Store_No int,
    @CurrDate datetime = null,
	@StartDate datetime = null,
	@EndDate datetime = null,
	@IsUploaded bit = 0
AS

BEGIN

    SET NOCOUNT ON
	IF @StartDate IS NULL OR @EndDate IS NULL
		BEGIN
			-- previous Monday thru Sunday
			SELECT @StartDate = CONVERT(datetime, CONVERT(varchar(255), DATEADD("day", 1 - DATEPART(dw, ISNULL(@CurrDate, GETDATE())) - 6, ISNULL(@CurrDate, GETDATE())), 101)),
				   @EndDate = CONVERT(datetime, CONVERT(varchar(255), DATEADD("day", 2 - DATEPART(dw, ISNULL(@CurrDate, GETDATE())), ISNULL(@CurrDate, GETDATE())), 101))
		END

IF @IsUploaded = 0 
BEGIN
	SELECT 
		IH.Subteam_No			as SubTeam,
		IAC.GLAccount			as Account,
		S.BusinessUnit_Id		as Unit,
		ISNULL(SST.PS_Team_No, '') As DeptID,
		ISNULL(SST.PS_SubTeam_No, '') As [Product],
		-- Weight or Quantity is always zero; adding is faster than a case statement
		ROUND(SUM((IH.Weight + IH.Quantity) * IH.ExtCost), 2) as Amount,
		left(IAC.AdjustmentDescription,30) as Description
	FROM ItemHistory IH
	INNER JOIN ItemHistoryUpload IHU
		on IH.ItemHistoryId = IHU.ItemHistoryId
	INNER JOIN 
		InventoryAdjustmentCode IAC
		ON IH.InventoryAdjustmentCode_Id = IAC.InventoryAdjustmentCode_Id
	INNER JOIN 
		Store S
		ON IH.Store_No = S.Store_No
	INNER JOIN
		StoreSubTeam SST
		ON IH.Store_No = SST.Store_No
		AND IH.SubTeam_No = SST.SubTeam_No
	WHERE IHU.AccountingUploadDate IS NULL 
	  AND DATEDIFF(day, @StartDate, DateStamp) >= 0
	  AND DATEDIFF(day, DateStamp, @EndDate) >= 0
-- This might be a problem depending on time of day.  
	GROUP BY IH.Subteam_No, IAC.GLAccount, S.BusinessUnit_Id, SST.PS_Team_No, SST.PS_SubTeam_No, left(IAC.AdjustmentDescription,30)

-- Same query, with an equal and opposite reaction in account 120000
	SELECT 
		IH.Subteam_No			as SubTeam,
		120000					as Account,
		S.BusinessUnit_Id		as Unit,
		ISNULL(SST.PS_Team_No, '') As DeptID,
		ISNULL(SST.PS_SubTeam_No, '') As [Product],
		-- Weight or Quantity is always zero; adding is faster than a case statement
		-1 * ROUND(SUM((IH.Weight + IH.Quantity) * IH.ExtCost), 2) as Amount,
		left(IAC.AdjustmentDescription,30) as Description
	FROM ItemHistory IH
	INNER JOIN ItemHistoryUpload IHU
		on IH.ItemHistoryId = IHU.ItemHistoryId
	INNER JOIN 
		InventoryAdjustmentCode IAC
		ON IH.InventoryAdjustmentCode_Id = IAC.InventoryAdjustmentCode_Id
	INNER JOIN 
		Store S
		ON IH.Store_No = S.Store_No
	INNER JOIN
		StoreSubTeam SST
		ON IH.Store_No = SST.Store_No
		AND IH.SubTeam_No = SST.SubTeam_No
	WHERE IHU.AccountingUploadDate IS NULL 
	  AND DATEDIFF(day, @StartDate, DateStamp) >= 0
	  AND DATEDIFF(day, DateStamp, @EndDate) >= 0
-- This might be a problem depending on time of day.  
	GROUP BY IH.Subteam_No, IAC.GLAccount, S.BusinessUnit_Id, SST.PS_Team_No, SST.PS_SubTeam_No, left(IAC.AdjustmentDescription,30)
END
ELSE
	SELECT 
		IH.Subteam_No			as SubTeam,
		IAC.GLAccount			as Account,
		S.BusinessUnit_Id		as Unit,
		ISNULL(SST.PS_Team_No, '') As DeptID,
		ISNULL(SST.PS_SubTeam_No, '') As [Product],
		-- Weight or Quantity is always zero; adding is faster than a case statement
		ROUND(SUM((IH.Weight + IH.Quantity) * IH.ExtCost), 2) as Amount,
		left(IAC.AdjustmentDescription,30) as Description
	FROM ItemHistory IH
	INNER JOIN ItemHistoryUpload IHU
		on IH.ItemHistoryId = IHU.ItemHistoryId
	INNER JOIN 
		InventoryAdjustmentCode IAC
		ON IH.InventoryAdjustmentCode_Id = IAC.InventoryAdjustmentCode_Id
	INNER JOIN 
		Store S
		ON IH.Store_No = S.Store_No
	INNER JOIN
		StoreSubTeam SST
		ON IH.Store_No = SST.Store_No
		AND IH.SubTeam_No = SST.SubTeam_No
	WHERE IHU.AccountingUploadDate IS NOT NULL 
	  AND DATEDIFF(day, @StartDate, DateStamp) >= 0
	  AND DATEDIFF(day, DateStamp, @EndDate) >= 0
-- This might be a problem depending on time of day.  
	GROUP BY IH.Subteam_No, IAC.GLAccount, S.BusinessUnit_Id, SST.PS_Team_No, SST.PS_SubTeam_No, left(IAC.AdjustmentDescription,30)


    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetGLUploadInventoryAdjustment] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetGLUploadInventoryAdjustment] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetGLUploadInventoryAdjustment] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetGLUploadInventoryAdjustment] TO [IRMAReportsRole]
    AS [dbo];

