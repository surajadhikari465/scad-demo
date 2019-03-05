print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Start: [UpdateVersion.sql]'
go

RAISERROR ('Updating Version table to 10.9.0', 10,1) with nowait
UPDATE [Version]
SET [Version] = '10.9.0'
where applicationname in ('IRMA Client', 'DATABASE', 'SYSTEM')
GO
