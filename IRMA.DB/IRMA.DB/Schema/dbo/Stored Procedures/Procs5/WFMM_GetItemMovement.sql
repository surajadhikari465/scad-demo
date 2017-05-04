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
-- **************************************************************************
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT (quantity+weight1) as quantity, 
       week_number,
       Dateadd(day, 1 - Datepart(weekday, Getdate()), CONVERT(DATE,
                                                      Getdate() - 7 *
                                                      Isnull(week_number,
                                                      0))) as MovementDate
		FROM   (SELECT Sum(quantity)                                   quantity,
				Sum(weight)												weight1,
               Datediff(day, datestamp, Dateadd(day, 1 - Datepart(weekday,
                                                         Getdate()),
                                        CONVERT(
                                        DATE, Getdate()))) / 7 week_number
				FROM   itemhistory (nolock)
				WHERE  datestamp BETWEEN Dateadd(day, 1 - Datepart(weekday, Getdate()),
												 CONVERT(DATE, Getdate() - 41))
												 AND Dateadd(day, 1 - Datepart(weekday,
																	  Getdate
																	  ()),
													 CONVERT(DATE, Getdate()))
					   AND store_no = @Store_No
					   AND adjustment_id = @Adjustment_ID
					   and SubTeam_No = @Subteam_No
					   and Item_Key = (select Item_Key from ItemIdentifier where Identifier = @Identifier and deleted_identifier = 0 and remove_identifier = 0)
				GROUP  BY Datediff(day, datestamp, Dateadd(day, 1 - Datepart(weekday,
																	Getdate()
																	), CONVERT(
															  DATE, Getdate()))) / 7) a


END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[WFMM_GetItemMovement] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[WFMM_GetItemMovement] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[WFMM_GetItemMovement] TO [IRMAReportsRole]
    AS [dbo];

