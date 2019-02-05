IF NOT EXISTS (SELECT 1 FROM [dbo].[OrderExternalSource] WHERE [Description] = 'AMAZON')
BEGIN
INSERT INTO [dbo].[OrderExternalSource]
           ([Description])
     VALUES
           ('AMAZON')
END
