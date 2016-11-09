-- Create procedure to retrieve error information.
CREATE PROCEDURE [dbo].[TryCatch_GetErrorInfo]
	@WriteToLog bit = 1,
	@AdditionalInfo nvarchar(1000) = NULL
AS
BEGIN

/*
	Return information about an error when using a TRY...CATCH statement.

	Within the scope of a CATCH block, the following system functions can be used to obtain information about 
	the error that caused the CATCH block to be executed: 

	- ERROR_NUMBER() [int] returns the number of the error.
	- ERROR_SEVERITY() [int] returns the severity.
	- ERROR_STATE() [int] returns the error state number.
	- ERROR_PROCEDURE() [nvarchar(126)] returns the name of the stored procedure or trigger where the error occurred.
	- ERROR_LINE() [int] returns the line number inside the routine that caused the error.
	- ERROR_MESSAGE() [nvarchar(4000)] returns the complete text of the error message. The text includes the values 
								supplied for any substitutable parameters, such as lengths, object names, or times.

	Additional information to return:

	- APP_NAME() [nvarchar(128)] returns the application name for the current session if set by the application.
	- HOST_NAME() [nchar] returns the workstation name.
	- USER_NAME() [] returns the name of the impersonated user; the current user in the current context is assumed. 
	- CURRENT_USER [sysname] returns the name of the current security context
	- SYSTEM_USER [nchar] returns the SQL Server login identification name (when using SQL Server Authentication), or
			the Windows login identification name in the form: DOMAIN\user_login_name (when using Windows Authentication)
	- SESSION_USER [nchar] returns the user name of the current context in the current database.
	- CURRENT_TIMESTAMP [datetime] returns the current date and time; equivalent to the GETDATE() function. 

	- @AdditionalInfo [nvarchar(1000)] is user-specified text which can provide addition details about the error
			such as variable values when the error occurred

*/
	SET NOCOUNT ON

	DECLARE @ErrorLogID int

	------------------------------------------------------------------------
	-- save the error information to the db
	------------------------------------------------------------------------
	IF @WriteToLog = 1
	  BEGIN
		INSERT INTO [dbo].[TryCatch_ErrorLog]
				   ([ErrorDateTime]
				   ,[AppName]
				   ,[HostName]
				   ,[CurrentUser]
				   ,[SystemUser]
				   ,[ErrorNumber]
				   ,[ErrorSeverity]
				   ,[ErrorState]
				   ,[ErrorProcedure]
				   ,[ErrorLine]
				   ,[ErrorMessage]
				   ,[AdditionalInfo])
			 VALUES
				   (CURRENT_TIMESTAMP
				   ,APP_NAME()
				   ,HOST_NAME()
				   ,CURRENT_USER
				   ,SYSTEM_USER
				   ,ERROR_NUMBER()
				   ,ERROR_SEVERITY()
				   ,ERROR_STATE()
				   ,ERROR_PROCEDURE()
				   ,ERROR_LINE()
				   ,ERROR_MESSAGE()
				   ,@AdditionalInfo)

		SELECT @ErrorLogID = SCOPE_IDENTITY()
	  END

	------------------------------------------------------------------------
	-- return the error information
	------------------------------------------------------------------------
    SELECT 
		[ErrorLogID] = @ErrorLogID,
		[AppName] = APP_NAME(),
		[HostName] = HOST_NAME(),
		[UserName] = USER_NAME(),
		[CurrentUser] = CURRENT_USER,
		[SystemUser] = SYSTEM_USER,
		[SessionUser] = SESSION_USER,
		[ErrorDateTime] = CURRENT_TIMESTAMP,
        [ErrorNumber] = ERROR_NUMBER(),
        [ErrorSeverity] = ERROR_SEVERITY(),
        [ErrorState] = ERROR_STATE(),
        [ErrorProcedure] = ERROR_PROCEDURE(),
        [ErrorLine] = ERROR_LINE(),
        [ErrorMessage] = ERROR_MESSAGE(),
		[AdditionalInfo] = @AdditionalInfo;

	RETURN ISNULL(@ErrorLogID, 0)

	SET NOCOUNT OFF
END