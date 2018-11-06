--Object: Table: dbo.UploadSessionHistory_Insert - Script Date: 11/6/2008 4:28:59 PM
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UploadSessionHistory_Insert]'))
drop trigger [dbo].[UploadSessionHistory_Insert]
GO

CREATE TRIGGER UploadSessionHistory_Insert
   ON  UploadSessionHistory
   AFTER Insert
AS 
BEGIN
	SET NOCOUNT ON;

	   insert into ValidationQueue (UploadSessionHistoryID, ProcessingFlag)
		select UploadSessionHistoryID, 0
		from inserted

END

GO
