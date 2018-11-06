SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'WFMM_GetItemMovement')
	BEGIN
		DROP Procedure [dbo].WFMM_GetItemMovement
	END
GO


CREATE PROCEDURE [dbo].[WFMM_GetItemMovement]
	@Store_No				int,
    @Subteam_No				int,
	@Identifier				varchar(13),
	@Adjustment_ID			int
AS

-- **************************************************************************
-- Procedure:WFMM_GetItemMovement()
--    Author: Hui Kou
--      Date: 09.19.12
--
-- Description:
-- This procedure is called from the WFM Mobile app to return item MOVEMENT
-- 
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 09.19.12		Hk   	7427	Creation
-- 04.24.17		EDM    23880	Updating to use Sales_SumByItem table 
--								instead of ItemHistory
-- **************************************************************************
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT	a.[quantity] + a.[weight1] as quantity,  
			a.[week_number],
			Dateadd(day,
				1 - Datepart(weekday, Convert(date, Getdate())),
				Dateadd(day, -7 * Isnull(a.[week_number], 0), Convert(date, Getdate()))) as MovementDate
	FROM (	
			SELECT	Sum([Sales_Quantity])	AS [quantity],
					Sum([Weight])			AS [weight1],
					Datediff(day,
						[Date_Key],
						Dateadd(day, 1 - Datepart(weekday, Convert(date, Getdate())), Convert(date, Getdate()))) / 7
											AS [week_number]
			FROM   [dbo].[Sales_SumByItem]
			WHERE  [Date_Key] BETWEEN Dateadd(day, 1 - Datepart(weekday, Convert(date, Getdate())), Dateadd(day, -41, Convert(date, Getdate())))
								AND Dateadd(day, 1 - Datepart(weekday, Convert(date, Getdate())), Convert(date, Getdate()))
				AND [Store_No] = @Store_No
				AND [SubTeam_No] = @Subteam_No
				AND [Item_Key] = (
					SELECT Item_Key 
					FROM ItemIdentifier
					WHERE Identifier = @Identifier  and deleted_identifier = 0 and remove_identifier = 0
				)
			GROUP BY Datediff(day,
						[Date_Key],
						Dateadd(day, 1 - Datepart(weekday, Convert(date, Getdate())), Convert(date, Getdate()))) / 7
		) a


END
go
