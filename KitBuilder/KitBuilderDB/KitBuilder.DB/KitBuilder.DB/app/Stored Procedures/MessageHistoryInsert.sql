create procedure app.MessageHistoryInsert
	@MessageTypeId int, @Message xml, @MessageHeader nvarchar(max)
as
begin
	insert into app.MessageHistory (MessageTypeId, Message, MessageHeader) values (@MessageTypeId, @Message,@MessageHeader)
end