﻿CREATE FUNCTION dbo.fn_ValidateDescriptionCharacters 
	(@Desc as varchar(50),
	 @DescType as varchar)
RETURNS varchar(50)
AS
BEGIN  
DECLARE @return varchar(50)
DECLARE @DescPos int

SET @DescPos = 1

WHILE @DescPos <= DATALENGTH(@Desc)
   BEGIN  
	   IF (ASCII(SUBSTRING(@Desc, @DescPos, 1)) < 34 AND ASCII(SUBSTRING(@Desc, @DescPos, 1)) <> 32) OR
	       ASCII(SUBSTRING(@Desc, @DescPos, 1)) > 90 OR
	       ASCII(SUBSTRING(@Desc, @DescPos, 1)) = 59
		  	 BREAK
			
	   SET @DescPos = @DescPos + 1	
   END
   
   IF @DescPos <= DATALENGTH(@Desc) 
		IF @DescType = 'I'
			SELECT @return = 'Invalid character ''' + CHAR(ASCII(SUBSTRING(@Desc, @DescPos, 1))) + ''' in Item Description'
			
		ELSE IF @DescType = 'P'
			SELECT @return = 'Invalid character ''' + CHAR(ASCII(SUBSTRING(@Desc, @DescPos, 1))) + ''' in POS Description' 
			
		ELSE
			SELECT @return = 'Invalid character ''' + CHAR(ASCII(SUBSTRING(@Desc, @DescPos, 1))) + ''' in Sign Description'			
   ELSE 
		SELECT @return = ''                    
                    
                    
RETURN @return
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_ValidateDescriptionCharacters] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_ValidateDescriptionCharacters] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_ValidateDescriptionCharacters] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_ValidateDescriptionCharacters] TO [IRMAReportsRole]
    AS [dbo];

