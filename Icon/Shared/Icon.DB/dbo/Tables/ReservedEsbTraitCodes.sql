CREATE TABLE [dbo].[ReservedEsbTraitCodes] (
[ReservedTraitCodesId] INT  NOT NULL IDENTITY,
[TraitCode] NVARCHAR(3)  NOT NULL,
[Description] NVARCHAR(Max)  NULL,
[AttributeGroupId] INT NULL,
[TraitGroupId] INT  NULL,
CONSTRAINT [PK_ReservedEsbTraitCodes] PRIMARY KEY CLUSTERED ([ReservedTraitCodesId] ASC),
CONSTRAINT [FK_ReservedEsbTraitCodes_AttributeGroup_AttributeGroupId] FOREIGN KEY ([AttributeGroupId]) REFERENCES [dbo].[AttributeGroup] ([AttributeGroupId]),
CONSTRAINT [FK_ReservedEsbTraitCodes_TraitGroup_traitGroupID] FOREIGN KEY ([traitGroupID]) REFERENCES [dbo].TraitGroup (traitGroupID)
)