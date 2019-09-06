CREATE PROCEDURE dbo.AuditGPMMessageSequence
	@action VARCHAR(25),
	@groupSize INT = 250000,
	@groupId INT = 0
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

	IF IsNull(@groupSize, 0) <= 0 
		SET @groupSize = 250000;

	IF @action = 'Initilize'
	BEGIN
		SELECT Count(*) [RowCount] FROM gpm.MessageSequence;
		RETURN;
	END

	IF @action = 'Get'
	BEGIN
		DECLARE @minId INT = (@groupId * @groupSize) + (CASE WHEN @groupID = 0 THEN 0 ELSE 1 END);

		CREATE TABLE #group (SequenceID INT INDEX ix_ID NONCLUSTERED);

		WITH cte AS(SELECT MessageSequenceID, Row_Number() OVER(ORDER BY MessageSequenceID) rowID FROM gpm.MessageSequence)
		  INSERT INTO #group (SequenceID)
		  SELECT TOP (@groupSize) MessageSequenceID
		  FROM cte
		  WHERE rowID >= @minId

		SELECT MessageSequenceID
			,ItemID
			,BusinessUnitID
			,PatchFamilyID
			,PatchFamilySequenceID
			,MessageID
			,Convert(varchar, InsertDateUtc, 120) InsertDateUtc
			,Convert(varchar, ModifiedDateUtc, 120) ModifiedDateUtc
		FROM gpm.MessageSequence ms WITH(NOLOCK)
		INNER JOIN #group g ON g.SequenceID = ms.MessageSequenceID 
		ORDER BY MessageSequenceID;

		IF (object_id('tempdb..#group') IS NOT NULL) DROP TABLE #group;
	END
END
GO

GRANT EXECUTE ON dbo.AuditGPMMessageSequence TO [MammothRole]
GO