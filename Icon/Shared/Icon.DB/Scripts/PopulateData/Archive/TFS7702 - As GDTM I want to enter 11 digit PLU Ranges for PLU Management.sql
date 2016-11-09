INSERT INTO app.PLUSequence
SELECT TOP (999999) sequence = CONVERT(INT, ROW_NUMBER() OVER (ORDER BY s1.[object_id]))
FROM sys.all_objects AS s1 CROSS JOIN sys.all_objects AS s2