CREATE PROCEDURE [app].[InsertItemMovement]
	@ItemMovementTransactions	app.ItemMovementType	READONLY
AS
BEGIN
	DECLARE @ItemMovementTrans TABLE (
		[ESBMessageID] [VARCHAR](255) NULL,
		[BusinessUnitID] [INT] NULL,
		[RegisterNumber] [INT] NULL,
		[TransactionSequenceNumber] [INT] NULL,
		[LineItemNumber] [INT] NULL,
		[Identifier] [INT] NULL,
		[TransDate] [DATETIME2](7) NULL,
		[Quantity] [INT] NULL,
		[ItemVoid] [BIT] NULL,
		[ItemType] [INT] NULL,
		[BasePrice] [DECIMAL](9, 2) NULL,
		[Weight] [DECIMAL](10, 3) NULL,
		[MarkDownAmount] [DECIMAL](9, 2) NULL
		)

	INSERT INTO @ItemMovementTrans (
		ESBMessageID,
		BusinessUnitID,
		RegisterNumber,
		TransactionSequenceNumber,
		LineItemNumber,
		Identifier,
		TransDate,
		Quantity,
		ItemVoid,
		ItemType,
		BasePrice,
		Weight,
		MarkDownAmount
		)
	SELECT ESBMessageID,
		BusinessUnitID,
		RegisterNumber,
		TransactionSequenceNumber,
		LineItemNumber,
		Identifier,
		TransDate,
		Quantity,
		ItemVoid,
		ItemType,
		BasePrice,
		Weight,
		MarkDownAmount
	FROM @ItemMovementTransactions;

	INSERT INTO [app].[ItemMovementErrorQueue] (
		[ESBMessageID],
		[TransactionSequenceNumber],
		[BusinessUnitID],
		[RegisterNumber],
		[LineItemNumber],
		[Identifier],
		[TransDate],
		[Quantity],
		[ItemVoid],
		[ItemType],
		[BasePrice],
		[Weight],
		[MarkDownAmount],
		[InsertDate]
		)
	SELECT im.ESBMessageID,
		im.TransactionSequenceNumber,
		im.BusinessUnitID,
		im.RegisterNumber,
		im.LineItemNumber,
		im.Identifier,
		im.TransDate,
		im.Quantity,
		im.ItemVoid,
		im.ItemType,
		im.BasePrice,
		im.Weight,
		im.MarkDownAmount,
		GETDATE()
	FROM @ItemMovementTrans im
	WHERE NOT EXISTS (
			SELECT 1
			FROM Item i(NOLOCK)
			WHERE im.identifier = i.itemid
			)

	INSERT INTO [app].[ItemMovement] (
		[ESBMessageID],
		[TransactionSequenceNumber],
		[BusinessUnitID],
		[RegisterNumber],
		[LineItemNumber],
		[Identifier],
		[TransDate],
		[Quantity],
		[ItemVoid],
		[ItemType],
		[BasePrice],
		[Weight],
		[MarkDownAmount],
		[InsertDate]
		)
	SELECT im.ESBMessageID,
		im.TransactionSequenceNumber,
		im.BusinessUnitID,
		im.RegisterNumber,
		im.LineItemNumber,
		sc.scanCode,
		im.TransDate,
		im.Quantity,
		im.ItemVoid,
		im.ItemType,
		im.BasePrice,
		im.Weight,
		im.MarkDownAmount,
		GETDATE()
	FROM @ItemMovementTransactions im
	JOIN ScanCode sc(NOLOCK) ON im.Identifier = sc.itemID
END
