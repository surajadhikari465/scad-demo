if not exists (select * from sysobjects where name='IrmaItemKeyStaging' and xtype='U')
BEGIN
CREATE TABLE dbo.IrmaItemKeyStaging(
item_key int not null,
inforItemId int not null
)

END
