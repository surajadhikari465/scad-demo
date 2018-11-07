CREATE TABLE [dbo].[InstructionType] (
    [InstructionTypeId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]              NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_InstructionType] PRIMARY KEY CLUSTERED ([InstructionTypeId] ASC)
);




