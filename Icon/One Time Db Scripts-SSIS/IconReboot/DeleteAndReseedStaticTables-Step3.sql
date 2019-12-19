DELETE FROM DataType
GO

DELETE FROM AttributeGroup
GO

DBCC CHECKIDENT ('[DataType]', RESEED, 0);
GO
DBCC CHECKIDENT ('[AttributeGroup]', RESEED, 0);
GO

SET IDENTITY_INSERT dbo.DataType  ON 

IF NOT EXISTS( Select 1 from  dbo.DataType where DataType = 'Text')
INSERT INTO dbo.DataType(DataTypeId, DataType)
VALUES(1, 'Text')

IF NOT EXISTS( Select 1 from  dbo.DataType where DataType = 'Number')
INSERT INTO dbo.DataType(DataTypeId,DataType)
VALUES(2,'Number')

IF NOT EXISTS( Select 1 from  dbo.DataType where DataType = 'Boolean')
INSERT INTO dbo.DataType(DataTypeId,DataType)
VALUES(3,'Boolean')

SET IDENTITY_INSERT dbo.DataType  OFF
 

insert into AttributeGroup(AttributeGroupName)
SELECT Distinct ATTRIBUTE_TYPE
 from IconRebootData

