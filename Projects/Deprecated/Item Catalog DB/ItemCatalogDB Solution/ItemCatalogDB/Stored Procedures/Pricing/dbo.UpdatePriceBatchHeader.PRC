SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Catherine Ward>
-- Create date: <September 11, 2006>
-- Description:	<Updates price batch header fields>
-- =============================================
if exists (select * from dbo.sysobjects where id = object_id(N'dbo.UpdatePriceBatchHeader') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.UpdatePriceBatchHeader
GO

CREATE PROCEDURE dbo.[UpdatePriceBatchHeader]
    @PriceBatchHeaderID int,
	@BatchDescription as varchar(30),
	@AutoApplyFlag as bit,
	@ApplyDate as datetime,
	@POSBatchId as int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE PriceBatchHeader 
	SET
		BatchDescription = @BatchDescription,
		AutoApplyFlag = @AutoApplyFlag,
		ApplyDate = @ApplyDate,
		POSBatchId = @POSBatchId
	WHERE 
		PriceBatchHeaderID = @PriceBatchHeaderID

    SET NOCOUNT OFF;

END
GO

SET QUOTED_IDENTIFIER OFF 
GO

