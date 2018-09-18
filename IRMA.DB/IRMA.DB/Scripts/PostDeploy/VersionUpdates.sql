print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Start: [UpdateVersion.sql]'
go

RAISERROR ('Updating Version table to 10.5.0', 10,1) with nowait
UPDATE [Version]
SET [Version] = '10.5.0'
where applicationname in ('DATABASE', 'SYSTEM')
GO
