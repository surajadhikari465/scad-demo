print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Start: [UpdateVersion.sql]'
go

RAISERROR ('Updating Version table to 10.7.1', 10,1) with nowait
UPDATE [Version]
SET [Version] = '10.7.1'
where applicationname in ('IRMA Client', 'DATABASE', 'SYSTEM')
GO
