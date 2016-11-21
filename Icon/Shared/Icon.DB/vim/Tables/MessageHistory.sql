CREATE TABLE vim.MessageHistory
(
	MessageHistoryId INT PRIMARY KEY IDENTITY(1,1),
	EsbMessageId UNIQUEIDENTIFIER,
	MessageTypeId INT,
	MessageStatusId INT,
	Message XML,
	InsertDate DATETIME2(7) constraint DF_VimMessageHistory_InsertDate default (sysdatetime()) not null
)