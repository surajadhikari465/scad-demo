CREATE TABLE infor.MessageArchiveLocale (
	MessageArchiveId INT NOT NULL identity(1, 1)
	,LocaleId INT NULL
	,BusinessUnitId INT NULL
	,LocaleName NVARCHAR(255) NOT NULL
	,LocaleTypeCode NVARCHAR(3) NOT NULL
	,InforMessageId UNIQUEIDENTIFIER NULL
	,Action NVARCHAR(30) NOT NULL
	,Context NVARCHAR(max) NOT NULL
	,ErrorCode NVARCHAR(30) NULL
	,ErrorDetails NVARCHAR(max) NULL
	,InsertDate DATETIME2(7) CONSTRAINT DF_MessageArchiveLocale_InsertDate DEFAULT(sysdatetime()) NOT NULL
	)
GO
ALTER TABLE infor.MessageArchiveLocale ADD CONSTRAINT MessageArchiveLocale_PK PRIMARY KEY CLUSTERED (MessageArchiveId)