CREATE TABLE [dbo].[LocaleTrait] (
[traitID] INT  NOT NULL  
, [localeID] INT  NOT NULL  
, [uomID] int  NULL  
, [traitValue] NVARCHAR(255)  NULL  
)
GO
ALTER TABLE [dbo].[LocaleTrait] WITH CHECK ADD CONSTRAINT [UOM_LocaleTrait_FK1] FOREIGN KEY (
[uomID]
)
REFERENCES [dbo].[UOM] (
[uomID]
)
GO
ALTER TABLE [dbo].[LocaleTrait] WITH CHECK ADD CONSTRAINT [Trait_LocaleTrait_FK1] FOREIGN KEY (
[traitID]
)
REFERENCES [dbo].[Trait] (
[traitID]
)
GO
ALTER TABLE [dbo].[LocaleTrait] WITH CHECK ADD CONSTRAINT [Locale_LocaleTrait_FK1] FOREIGN KEY (
[localeID]
)
REFERENCES [dbo].[Locale] (
[localeID]
)
GO
ALTER TABLE [dbo].[LocaleTrait] ADD CONSTRAINT [LocaleTrait_PK] PRIMARY KEY CLUSTERED (
[traitID]
, [localeID]
)