CREATE TABLE [dbo].[InstructionListMember]
(
	[InstructionListMemberId] INT NOT NULL IDENTITY PRIMARY KEY, 
    [InstructionListId] INT NOT NULL, 
    [Group] NVARCHAR(60) NOT NULL, 
    [Sequence] INT NOT NULL, 
    [Member] NVARCHAR(15) NULL, 
    CONSTRAINT [FK_InstructionListMember_InstructionList] FOREIGN KEY ([InstructionListId]) REFERENCES [InstructionList]([InstructionListId])
)

GO

CREATE INDEX [IX_InstructionListMember_InstructionListId] ON [dbo].[InstructionListMember] ([InstructionListId])
