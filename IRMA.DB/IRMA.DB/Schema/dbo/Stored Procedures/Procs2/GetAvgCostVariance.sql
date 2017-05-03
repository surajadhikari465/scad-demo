CREATE PROCEDURE [dbo].[GetAvgCostVariance]
	@Identifier varchar(13) = null,
	@Store_No int,
	@SubTeam_No int = null,
	@StartDate smalldatetime,
	@EndDate smalldatetime,
	@Tolerance decimal(7,4),
	@LimitToOutOfTolerance bit
AS

DECLARE @SQL varchar(MAX)

SELECT @SQL = 'SELECT
					  *
				FROM
					  (
							SELECT            
								[Identifier]		=	ii.Identifier,
								[Item_Description]	=	i.Item_Description,
								[Qty]				=   ach.Quantity, 
								[AvgCost]			=   ach.AvgCost,
								[PrevAvgCost]		=   ISNULL(dbo.fn_GetPreviousAvgCost(ach.Item_Key, ach.Store_No, ach.SubTeam_No, ach.Effective_Date),0),
								[PrevAvgCostDt]		=	ISNULL(dbo.fn_GetPreviousAvgCostEffDate(ach.Item_Key, ach.Store_No, ach.SubTeam_No, ach.Effective_Date),''''),
								[VariancePct]		=   ((ach.AvgCost - dbo.fn_GetPreviousAvgCost(ach.Item_Key, ach.Store_No, ach.SubTeam_No, ach.Effective_Date)) / dbo.fn_GetPreviousAvgCost(ach.Item_Key, ach.Store_No, ach.SubTeam_No, ach.Effective_Date)) * 100,
								[VarianceDol]	    =   ach.AvgCost - dbo.fn_GetPreviousAvgCost(ach.Item_Key, ach.Store_No, ach.SubTeam_No, ach.Effective_Date),
								[EffDate]			=	ach.Effective_Date,
								[Reason]			=   ISNULL(acr.Description, ''''), 
								[Comments]			=	ach.Comments,
								[Source]			=	ISNULL(u.Username, ''PO''),
								[PrevSource]		=	ISNULL(dbo.fn_GetPreviousAvgCostSource(ach.Item_Key, ach.Store_No, ach.SubTeam_No, ach.Effective_Date), ''PO''),
								[SubTeam]			=	ach.SubTeam_No			
							FROM
								AvgCostHistory              (nolock) ach
								INNER JOIN	Item			(nolock) i		ON	i.Item_Key		=	ach.Item_Key
								INNER JOIN	ItemIdentifier	(nolock) ii		ON	ii.Item_Key		=	i.Item_Key
								LEFT JOIN AvgCostAdjReason  (nolock) acr    ON ach.Reason		=	acr.ID
								LEFT JOIN Users             (nolock) u      ON ach.User_ID		=	u.User_ID
								INNER JOIN OnHand			(nolock) oh		ON ach.Item_Key		=	oh.Item_Key
																			AND ach.Store_No	=	oh.Store_No
																			AND ach.SubTeam_No	=	oh.SubTeam_No
							WHERE 
								ach.Store_No			=	' + CONVERT(varchar, @Store_No) + '
							AND
								ach.Effective_Date		>=	''' + CONVERT(varchar, @StartDate, 120) + ''' 
							AND 
								ach.Effective_Date		<	''' + CONVERT(varchar, DATEADD(d,1,@EndDate)) + '''
							AND
								(oh.Quantity				>	0
									OR oh.weight			>	0) '
									
BEGIN			
	IF @Identifier IS NOT NULL
		SELECT @SQL = @SQL + 'AND (ii.Identifier		=	''' + @Identifier + ''') '
	ELSE
		SELECT @SQL = @SQL + 'AND ii.Default_Identifier	=	1 '
END

BEGIN
	IF @Subteam_No IS NOT NULL
		SELECT @SQL = @SQL + 'AND ach.SubTeam_No		=	' + CONVERT(varchar, @SubTeam_No) + ''
END

SELECT @SQL = @SQL + ') inner_result '

IF @LimitToOutOfTolerance = 1
	BEGIN
		SELECT @SQL = @SQL + 'WHERE	VariancePct	> ' + CONVERT(varchar, @Tolerance) + ' OR VariancePct < -' + CONVERT(varchar, @Tolerance)
	END			
	
SELECT @SQL = @SQL + 'ORDER BY SubTeam, Item_Description, EffDate DESC '

EXEC (@SQL)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAvgCostVariance] TO [IRMAReportsRole]
    AS [dbo];

