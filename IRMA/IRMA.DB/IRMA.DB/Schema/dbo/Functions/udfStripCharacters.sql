
	CREATE  FUNCTION dbo.udfStripCharacters(@Input VARCHAR(255))    
	RETURNS VARCHAR(255) WITH SCHEMABINDING    
	AS    
	BEGIN    
		/*
			dbo.udfStripCharacters(varchar(255))
				strips all non-digit characters from the input string
				limited to 255 characters because it is used in computed columns that may be indexed.
				limits for indexes are 999 characters but the smaller the better.
				
				WITH SCHEMABDINING is required when used in a PERSISTED computed column.
		*/

		DECLARE @pos INT    
		SET @Pos = PATINDEX('%[^0-9]%',@Input)    
		WHILE @Pos > 0    
		BEGIN    
			SET @Input = STUFF(@Input,@pos,1,'')    
			SET @Pos = PATINDEX('%[^0-9]%',@Input)    
		END    
		RETURN @Input    
	END 

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[udfStripCharacters] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[udfStripCharacters] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[udfStripCharacters] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[udfStripCharacters] TO [IRMAReportsRole]
    AS [dbo];

