CREATE PROCEDURE [dbo].[MarkIconItemChangeQueueEntriesInProcess]
       -- Add the parameters for the stored procedure here
       @NumberOfRows int, 
       @JobInstance varchar(30),
	   @NumberOfRowsMarked int OUTPUT
AS
BEGIN
      declare @updatedRows int = 0;
	  declare @deletedRows int = 0;

      update dbo.IconItemChangeQueue
         set InProcessBy = null
       where [InProcessBy] = @JobInstance
   
	 ;with IconItemChangeQueueTemp as 
	 (
		select TOP (@NumberOfRows) InProcessBy 
		  from dbo.IconItemChangeQueue with (rowlock, readpast, updlock)
		 where [ProcessFailedDate] is null
           and [InProcessBy] is null
	 )
      UPDATE IconItemChangeQueueTemp
         SET InProcessBy = @JobInstance

		set @updatedRows = @@ROWCOUNT;

	   delete a
	     from dbo.IconItemChangeQueue a 
		 join dbo.IconItemChangeQueue b
		   on a.Item_Key = b.Item_Key
		  and a.Identifier = b.Identifier and b.ProcessFailedDate is null
		  and a.QID > b.QID
	    where a.InProcessBy = @JobInstance	  and a.ProcessFailedDate is null
		
		set @deletedRows = @@ROWCOUNT  

		set @NumberOfRowsMarked = @updatedRows - @deletedRows;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[MarkIconItemChangeQueueEntriesInProcess] TO [IConInterface]
    AS [dbo];

