
-- =============================================
-- Author:		Kyle Milner
-- Create date: 2015-11-12
-- Description:	Returns batch header names that match
--				the input.
-- =============================================

CREATE PROCEDURE [dbo].[GetBatchDescriptionMatches]
	@BatchDescription nvarchar(30),
	@StoreNumber int
AS
BEGIN
	set nocount on

    select distinct
		pbh.BatchDescription as [Batch Description],
		pbh.StartDate as [Start Date]
	from
		PriceBatchHeader (nolock) pbh
		join PriceBatchDetail (nolock) pbd on pbh.PriceBatchHeaderID = pbd.PriceBatchHeaderID
	where
		pbh.BatchDescription like '%' + @BatchDescription + '%'
		and pbd.Store_No = @StoreNumber
	order by
		pbh.StartDate desc
END

