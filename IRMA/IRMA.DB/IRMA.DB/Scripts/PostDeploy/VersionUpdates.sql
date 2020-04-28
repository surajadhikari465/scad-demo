print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Start: [UpdateVersion.sql]'
go

RAISERROR ('Updating Version table to 12.1.0', 10,1) with nowait
UPDATE [Version]
SET [Version] = '12.1.0'
where applicationname in ('IRMA Client', 'DATABASE', 'SYSTEM')
GO
