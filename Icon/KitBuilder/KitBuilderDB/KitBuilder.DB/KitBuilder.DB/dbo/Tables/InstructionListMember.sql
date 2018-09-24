CREATE TABLE [dbo].[InstructionListMember]
(
	[InstructionListMemberId] INT NOT NULL IDENTITY PRIMARY KEY, 
    [InstructionListId] INT NOT NULL, 
    [Group] NVARCHAR(60) NOT NULL, 
    [Sequence] INT NOT NULL, 
    [Member] NVARCHAR(15) NULL, 
	[InsertDate] DATETIME2 NOT NULL DEFAULT getDate(), 
	[LastUpdatedDate] DATETIME2 NOT NULL DEFAULT getDate(), 
    CONSTRAINT [FK_InstructionListMember_InstructionList] FOREIGN KEY ([InstructionListId]) REFERENCES [InstructionList]([InstructionListId])
)

GO

CREATE INDEX [IX_InstructionListMember_InstructionListId] ON [dbo].[InstructionListMember] ([InstructionListId])
