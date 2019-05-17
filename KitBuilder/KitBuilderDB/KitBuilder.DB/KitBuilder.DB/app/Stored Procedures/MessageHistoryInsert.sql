CREATE PROCEDURE app.MessageHistoryInsert @MessageTypeId INT
	,@Message XML
	,@MessageHeader NVARCHAR(max)
AS
BEGIN
	INSERT INTO app.MessageHistory (
		MessageTypeId
		,Message
		,MessageHeader
		)
	VALUES (
		@MessageTypeId
		,@Message
		,@MessageHeader
		)
END