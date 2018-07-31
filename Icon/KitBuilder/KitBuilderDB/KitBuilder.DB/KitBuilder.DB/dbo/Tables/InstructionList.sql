CREATE TABLE [dbo].[InstructionList]
(
	[InstructionListId] INT NOT NULL IDENTITY PRIMARY KEY, 
    [Name] NVARCHAR(10) NOT NULL, 
    [InstructionTypeId] INT NOT NULL, 
    [StatusId] INT NOT NULL, 
    CONSTRAINT [FK_InstructionList_InstructionType_InstructionTypeId] FOREIGN KEY ([InstructionTypeId]) REFERENCES [dbo].[InstructionType]([InstructionTypeId])
				ON DELETE NO ACTION
                ON UPDATE NO ACTION,
    CONSTRAINT [FK_InstructionList_Status_StatusId] FOREIGN KEY ([StatusId]) REFERENCES [Status]([StatusID])
				ON DELETE NO ACTION
                ON UPDATE NO ACTION
)

GO

CREATE INDEX [IX_InstructionList_InstructionTypeId] ON [dbo].[InstructionList] ([InstructionTypeId])

GO

CREATE INDEX [IX_InstructionList_StatusId] ON [dbo].[InstructionList] ([StatusId])
