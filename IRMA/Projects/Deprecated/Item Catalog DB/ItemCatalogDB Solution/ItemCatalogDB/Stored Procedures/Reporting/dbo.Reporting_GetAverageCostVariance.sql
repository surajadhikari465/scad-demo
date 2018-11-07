/****** Object:  StoredProcedure [dbo].[Reporting_GetAverageCostVariance]    Script Date: 07/11/2012 14:07:55 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Reporting_GetAverageCostVariance]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Reporting_GetAverageCostVariance]
GO

/****** Object:  StoredProcedure [dbo].[Reporting_GetAverageCostVariance]    Script Date: 07/11/2012 14:07:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- **************************************************************************
-- Procedure: Reporting_GetAverageCostVariance
-- Author: trey d'amico
-- Date: 07/03/2012
--
-- Description: added in 4.6 to be used with [Average Cost Variance.rdl]. 
-- **************************************************************************
CREATE PROCEDURE [dbo].[Reporting_GetAverageCostVariance]
	@StartDate              DATETIME,
	@EndDate                DATETIME,
	@Store_No				NVARCHAR(MAX),
	@Subteam_No             NVARCHAR(MAX),
	@Variance_Pct           DECIMAL(7,4) = 10  -- minimum variance, also in the .rdl's subheader expression
AS
BEGIN
	SET NOCOUNT ON;
	
SELECT	@Variance_Pct				= ( @Variance_Pct/100 ) -- to match the format in SAC table
	
SELECT	[Category]					= ( CAST(i.subteam_no AS NVARCHAR(10)) + ' - '  + ISNULL(ic.category_name, '') ),
		[Item]						= ii.identifier,
		[Description]				= i.item_description,
		[Store]						= sv.companyname,
		[Prior Cost]				= sac.prioravgcost,
		[Big Change Date]			= sac.effective_date,
		[Big Change]				= sac.newavgcost,
		[Dollar Change]				= ( sac.newavgcost - sac.prioravgcost ),
		[Percent Change]			= sac.variance_pct,
		[Current Avg Cost]			= ach.avgcost,
		[Current Avg Cost Date]		= ach.effective_date,
		[Retail]					= p.price / ( CASE WHEN p.multiple > 1 THEN p.multiple ELSE 1 END ),
		[PO]						= sac.orderheader_id
		
FROM	dbo.suspendedavgcost (nolock) sac
		INNER JOIN dbo.orderheader (nolock) oh			ON oh.orderheader_id			=	sac.orderheader_id
		INNER JOIN dbo.item (nolock) i					ON i.item_key					=	sac.item_key
		INNER JOIN dbo.itemidentifier (nolock) ii		ON ii.item_key					=	i.item_key
															AND ii.Default_Identifier	=	1
		INNER JOIN dbo.itemcategory (nolock) ic			ON ic.category_id				=	i.category_id
															AND ic.subteam_no			=	i.subteam_no
		INNER JOIN dbo.vendor (nolock) sv				ON sv.vendor_id					=	oh.receivelocation_id
		INNER JOIN dbo.price (nolock) p					ON p.store_no					=	sv.store_no
															AND p.item_key				=	i.item_key
		CROSS APPLY (	SELECT TOP 1 ach2.Effective_Date, ach2.AvgCost
						FROM dbo.avgcosthistory (nolock) ach2			
						WHERE ach2.item_key = sac.item_key
							AND ach2.store_no = sv.store_no
						ORDER BY ach2.Effective_Date DESC	) ach
WHERE	sac.Effective_Date			BETWEEN		@StartDate AND @EndDate
		AND	ABS(sac.Variance_pct)	>=			ABS(@Variance_Pct)
		AND ( sv.store_no			IN			( SELECT key_value FROM dbo.Fn_parse_list(@Store_No, ',')) )
		AND ( sac.subteam_no		IN			( SELECT key_value FROM dbo.Fn_parse_list (@Subteam_No, ',')) )
END

GO


