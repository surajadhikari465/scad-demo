-- THIS FUNCTION LOOKS ONLY AT THE IDENTIFIER PASSED IN TO DETERMINE IF IT MEETS SCALE CRITERIA (For Type 2 Identifiers)
-- THIS FUNCTION LOOKS ONLY AT THE Scale_Identifier FLAG FOR THE IDENTIFIER PASSED TO DETERMINE IF IT MEETS SCALE CRITERIA (For NON Type 2 Identifiers)
-- IT DOES NOT LOOK AT OTHER FIELDS THAT DETERMINE WHAT MAKES A SCALE ITEM (IE: ItemIdentifier = 'O')

-- THIS FUNCTION IS USED BY DATA CONVERSION TO DETERMINE HOW TO SET OTHER SCALE CRITERIA;
-- THE APPLICATION SHOULD USE THE FUNCTION: fn_IsScaleItem
Create  FUNCTION dbo.fn_IsScaleIdentifier
	(@Identifier varchar(13)
)
RETURNS bit
AS

BEGIN  
	DECLARE @return bit
	DECLARE @Scale_Identifier bit
	
	IF len(@Identifier) = 11 
		--Scale item criteria for Type 2 identifier
    	IF SUBSTRING(@Identifier,1,1) = '2' 
            IF SUBSTRING(@Identifier,7,5) = '00000'                            
				select @return = 1				
            ELSE
                select @return = 0
        ELSE 
		  BEGIN
			--Get Scale_Identifier flag to check for Non Type 2 scale item
        	SELECT @Scale_Identifier = Scale_Identifier 
			FROM ItemIdentifier (NOLOCK) 
			WHERE Identifier = @Identifier AND Deleted_Identifier = 0
			
	    	--Scale item criteria for Non Type 2 identifier		
			IF @Scale_Identifier = 1 
				select @return = 1	
			ELSE 
				select @return = 0
		  END
    ELSE  
        select @return = 0
        
	RETURN @return
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsScaleIdentifier] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsScaleIdentifier] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsScaleIdentifier] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsScaleIdentifier] TO [DataMigration]
    AS [dbo];

