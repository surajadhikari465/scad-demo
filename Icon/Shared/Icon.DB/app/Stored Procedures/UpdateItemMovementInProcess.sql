CREATE PROCEDURE [app].[UpdateItemMovementInProcess]
	@MaxTransaction int,
	@Instance int
AS
BEGIN
/*
-- ===============================================================
-- Modification History
--
-- Create date: 2014-11-18
-- Description:	
-- Mark the InProcessBy column on the app.ItemMovement table 
-- with the TlogCon job instance ID, and remove duplicate entries.

   Name:			TFS:			Modified Date:
   Min Zhao			5810			2014-11-26
   Tom Lux			18724			2016-10-12
	   1) New var @instRef for integer param @Instance cast as varchar (because column in ItemMovement tbl compared to this var is varchar).
	   2) Changed @TansactionTable table var to #moveTrx.
	   3) Changed TransDate type from datetime to datetime2(7) to match ItemMovement tbl def.
	   4) Added PK to #moveTrx because it's joined to ItemMovement tbl.
	   5) Change @TansactionIdTable table var to #moveId.
	   6) Added PK to #moveId because it's joined to ItemMovement tbl.
	   7) Removed "ORDER BY MIN(InsertDate)" from retrieval of main set of item-move entries.
	   8) Removed DISTINCT and MIN(InsertDate) from selection when pulling item-move entries into #moveTrx, as they do not contribute to unique result set (group-by clause handles this).
   ===============================================================
*/

	SET NOCOUNT ON;

	-- Cast instance ID to varchar to match ItemMovement tbl InProcessBy column type.
	declare @instRef varchar(30) = cast(@Instance as varchar)

	create table #moveTrx (
		BusinessUnitID int not null,
		RegisterNumber int not null,
		TransDate datetime2(7) not null,
		TransactionSequenceNumber int not null
		primary key (
			BusinessUnitID,
			RegisterNumber,
			TransDate,
			TransactionSequenceNumber
		)
	);

	create table #moveId (
		ItemMovementID int not null
		primary key (
			ItemMovementID
		)
	);

	/*
	=====================================================
	Clear in-process-by for current @instance.
	=====================================================
	*/
	UPDATE [app].[ItemMovement]
	SET [InProcessBy] = null
	WHERE [InProcessBy] = @instRef 

	  
	/*
	=====================================================
	Grab target number of entries from ItemMovement tbl.
	=====================================================
	*/
	;with ItemMovementTemp as
	(
		SELECT TOP (@MaxTransaction)
			BusinessUnitID, 
			RegisterNumber, 
			TransDate, 
			TransactionSequenceNumber
		FROM [app].[ItemMovement]
			with (rowlock, readpast, updlock)
		WHERE
			[InProcessBy] is null 
			AND [ProcessFailedDate] is null
		GROUP BY
			BusinessUnitID, 
			RegisterNumber, 
			TransDate, 
			TransactionSequenceNumber
	)
	INSERT INTO #moveTrx
	SELECT * FROM ItemMovementTemp


	/*
	=====================================================
	Retrieve primary key ID for targeted entries.
	=====================================================
	*/
	INSERT INTO #moveId
		SELECT ItemMovementID
		FROM [app].[ItemMovement] a
		JOIN #moveTrx b 
			ON a.BusinessUnitID = b.BusinessUnitID
			AND a.RegisterNumber = b.RegisterNumber
			AND a.TransDate = b.TransDate
			AND a.TransactionSequenceNumber = b.TransactionSequenceNumber

	
	/*
	=====================================================
	Mark in-process-by.
	=====================================================
	*/
	;with ItemMovement as
	( 
		select InProcessBy
		from [app].[ItemMovement] a 
			with (rowlock, readpast, updlock)
		JOIN #moveId b 
			on a.ItemMovementID = b.ItemMovementID
		WHERE
			[ProcessFailedDate] is null
			and [InProcessBy] is null
	)
	UPDATE ItemMovement 
	SET [InProcessBy] = @instRef


	/*
	=====================================================
	Delete duplicate transactions while the duplicates are in the ItemMovement table that haven't been successfully processed.
	=====================================================
	*/
	DELETE a 
	  FROM app.ItemMovement a
	  join app.ItemMovement b 
	    on a.BusinessUnitID = b.BusinessUnitID 
	   and a.RegisterNumber = b.RegisterNumber 
	   and a.TransDate = b.TransDate
	   and a.TransactionSequenceNumber = b.TransactionSequenceNumber 
	   and a.LineItemNumber = b.LineItemNumber
	   and a.ItemMovementID > b.ItemMovementID
	 WHERE a.InProcessBy = @instRef 

	/*
	=====================================================
	Delete duplicate transactions if the transactions have been processed.
	=====================================================
	*/
	DELETE a 
	  FROM app.ItemMovement a
	  join app.ItemMovementTransactionHistory b
	    on a.BusinessUnitID = b.BusinessUnitID 
	   and a.RegisterNumber = b.RegisterNumber 
	   and a.TransDate = b.TransDate
	   and a.TransactionSequenceNumber = b.TransactionSequenceNumber 
	 WHERE a.InProcessBy = @instRef

	/*
	=====================================================
	Return rows marked in-process-by for this @instance.
	=====================================================
	*/
	SELECT 
		ItemMovementID,
		ESBMessageID,
		TransactionSequenceNumber,
		BusinessUnitID,
		RegisterNumber,
		LineItemNumber,
		Identifier,
		TransDate,
		Quantity,
		ItemVoid,
		ItemType,
		BasePrice,
		Weight,
		MarkDownAmount,
		InsertDate,
		InProcessBy,
		ProcessFailedDate
	FROM
		app.ItemMovement
   WHERE
		InProcessBy = @instRef
	AND ProcessFailedDate IS NULL
  ORDER BY
		BusinessUnitID,
		RegisterNumber,
		TransactionSequenceNumber,
		TransDate,
		LineItemNumber

END
go
