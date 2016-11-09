CREATE PROCEDURE [app].[MassUpdateItemMovement]
	-- Add the parameters for the stored procedure here
	@ESBMessageIds app.ESBMessageIdType READONLY, 
	@FailedDate datetime2(7) = null
AS
BEGIN
	Begin Tran
		Begin Try
			If (@FailedDate is null)
				Begin 
					insert into app.ItemMovementTransactionHistory(TransactionSequenceNumber, BusinessUnitID, TransDate, RegisterNumber)
					select distinct TransactionSequenceNumber, BusinessUnitID, TransDate, RegisterNumber
					  from app.ItemMovement a
					  join @ESBMessageIds b on a.ESBMessageID = b.ESBMessageID
			
					delete from app.ItemMovement 
					  from app.ItemMovement a
				inner join @ESBMessageIds b
						on a.ESBMessageID = b.ESBMessageID
				End
			Else
				Begin
					update app.ItemMovement
					   set ProcessFailedDate = @FailedDate,
						   InProcessBy = null
					  from app.ItemMovement a
				inner join @ESBMessageIds b
						on a.ESBMessageID = b.ESBMessageID	
				End

			if @@trancount > 0
					commit transaction;
		end try
		begin catch
			if @@trancount > 0
					rollback transaction;

			throw;
			
		end catch
END