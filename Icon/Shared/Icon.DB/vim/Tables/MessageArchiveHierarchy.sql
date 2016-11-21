CREATE TABLE vim.MessageArchiveHierarchy
(
	MessageArchiveId INT PRIMARY KEY IDENTITY(1,1),
	HierarchyClassId int not null,
	HierarchyClassName nvarchar(255) not null,
	HierarchyName nvarchar(255),
	EsbMessageId uniqueidentifier not null,
	Context nvarchar(max) not null,
	ErrorCode nvarchar(100) null,
	ErrorDetails nvarchar(max) null,
    InsertDate datetime2(7) constraint DF_VimMessageArchiveHierarchy_InsertDate default (sysdatetime()) not null
)