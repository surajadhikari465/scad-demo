print 'this transaction is trapped'
print  @@TRANCOUNT 
BEGIN TRY  
	BEGIN TRANSACTION;
	    print @@TRANCOUNT 
   		Insert Into Users (Id, UserName) values(10,'bob');
		Insert Into Users (Id, UserName) values(10,'frank');
	print 'committing transaction' 
	COMMIT TRANSACTION;
END TRY  

BEGIN CATCH  
	IF @@TRANCOUNT > 0  
	BEGIN
		ROLLBACK TRANSACTION;
		print 'my transction was rolled back';
	END
END CATCH  