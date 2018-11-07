IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ValidatedNationalClass]')
			 AND TYPE IN (N'U'))
DROP TABLE [dbo].ValidatedNationalClass

CREATE TABLE ValidatedNationalClass
(
	ValidatedNationalClassId int identity(1, 1),
	IrmaId int,
	IconId int,
	Level int,
	InsertDate datetime default GETDATE()
)
ALTER TABLE [dbo].[ValidatedNationalClass] ADD CONSTRAINT [ValidatedNationalClass_PK] PRIMARY KEY CLUSTERED ([ValidatedNationalClassId]) WITH (FILLFACTOR = 80)

-- add security script..icon user and irma user.
grant SELECT, DELETE, UPDATE, INSERT on dbo.ValidatedNationalClass to IRMAClientRole, IConInterface
grant SELECT, DELETE, UPDATE, INSERT on dbo.NatItemFamily to IRMAClientRole, IConInterface
grant SELECT, DELETE, UPDATE, INSERT on dbo.NatItemClass to IRMAClientRole, IConInterface
grant SELECT, DELETE, UPDATE, INSERT on dbo.NatItemCat to IRMAClientRole, IConInterface