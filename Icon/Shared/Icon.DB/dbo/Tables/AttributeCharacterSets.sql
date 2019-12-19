CREATE TABLE dbo.AttributeCharacterSets
(   
	AttributeCharacterSetId INT  IDENTITY (1, 1) NOT NULL,
	AttributeId  INT  NOT NULL,
	CharacterSetId INT  NOT NULL,
	CONSTRAINT [PK_AttributeCharacterSets] PRIMARY KEY CLUSTERED (AttributeCharacterSetId ASC),
    CONSTRAINT [FK_AttributeCharacterSets_Attributes_AttributeId] FOREIGN KEY ([AttributeId]) REFERENCES [dbo].[Attributes] ([AttributeId]) ON DELETE CASCADE,
    CONSTRAINT [FK_AttributeCharacterSets_CharacterSets_CharacterSetId] FOREIGN KEY ([CharacterSetId]) REFERENCES [dbo].CharacterSets ([CharacterSetId])   
)