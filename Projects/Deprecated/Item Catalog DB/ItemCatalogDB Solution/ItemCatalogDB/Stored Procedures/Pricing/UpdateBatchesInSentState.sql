IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UpdateBatchesInSentState') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE dbo.UpdateBatchesInSentState
GO


CREATE PROCEDURE dbo.UpdateBatchesInSentState
	@PriceBatchHeaderIds_Update varchar(max),
	@PriceBatchHeaderIds_Rollback varchar(max)
AS

/*
############################################################################
Change History

2/15/10
Tom Lux
TFS 11981
Changed both parameters to varchar(max) so that lists of batch IDs coming in
would not be truncated.
############################################################################
*/

DECLARE @CurrentStatus int
DECLARE @RowIndex int
DECLARE @PriceBatchHeaderId int
DECLARE @tblPBH_IDs_Update table 
                   (
                          RowIndex int IDENTITY(1,1) PRIMARY KEY CLUSTERED,
                          PriceBatchHeaderID int
                   )
                   
DECLARE @tblPBH_IDs_Rollback table 
                   (
                          RowIndex int IDENTITY(1,1) PRIMARY KEY CLUSTERED,
                          PriceBatchHeaderID int
                   )                   
 
IF @PriceBatchHeaderIds_Update IS NOT NULL
	BEGIN
		INSERT INTO @tblPBH_IDs_Update(PriceBatchHeaderID)
			 SELECT Key_Value
			 FROM dbo.fn_Parse_List(@PriceBatchHeaderIds_Update, ',')
		     
		SELECT @RowIndex = 1

		WHILE EXISTS (SELECT * FROM @tblPBH_IDs_Update WHERE RowIndex = @RowIndex)
			BEGIN
				 SELECT @PriceBatchHeaderId = PBH.PriceBatchHeaderID,
						@CurrentStatus = PBH.PriceBatchStatusId
				 FROM dbo.PriceBatchHeader PBH
						INNER JOIN @tblPBH_IDs_Update LIST ON LIST.PriceBatchHeaderID = PBH.PriceBatchHeaderID
				 WHERE LIST.RowIndex = @RowIndex	
				 
				IF @CurrentStatus = 2 -- Only Package Batch
					EXEC dbo.UpdatePriceBatchPackage @PriceBatchHeaderId
				ELSE -- Package and put into Sent
					BEGIN
						EXEC dbo.UpdatePriceBatchPackage @PriceBatchHeaderId
						EXEC dbo.UpdatePriceBatchStatus @PriceBatchHeaderId,5
					END		 
		     
				  SELECT @RowIndex = @RowIndex + 1
			END
	END
	
	--DELETE FROM @tblPBH_IDs
	
IF @PriceBatchHeaderIds_Rollback IS NOT NULL
	BEGIN
		INSERT INTO @tblPBH_IDs_Rollback(PriceBatchHeaderID)
			 SELECT Key_Value
			 FROM dbo.fn_Parse_List(@PriceBatchHeaderIds_Rollback, ',')
		     
		SELECT @RowIndex = 1

		WHILE EXISTS (SELECT * FROM @tblPBH_IDs_Rollback WHERE RowIndex = @RowIndex)
			BEGIN
				 SELECT @PriceBatchHeaderId = PBH.PriceBatchHeaderID,
						@CurrentStatus = PBH.PriceBatchStatusId
				 FROM dbo.PriceBatchHeader PBH
						INNER JOIN @tblPBH_IDs_Rollback LIST ON LIST.PriceBatchHeaderID = PBH.PriceBatchHeaderID
				 WHERE LIST.RowIndex = @RowIndex	
				 
				 EXEC dbo.UpdatePriceBatchPackage @PriceBatchHeaderId
		     
				 SELECT @RowIndex = @RowIndex + 1
			END
	END	
GO 