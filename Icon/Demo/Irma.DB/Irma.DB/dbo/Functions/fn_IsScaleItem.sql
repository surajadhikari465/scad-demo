Create  FUNCTION dbo.fn_IsScaleItem
	(@Identifier varchar(13)
)
RETURNS bit
AS

BEGIN  
	DECLARE @return bit
	DECLARE @IdentifierType char(1)
	DECLARE @Scale_Identifier bit
    
    IF len(@Identifier) = 11 AND SUBSTRING(@Identifier,1,1) = '2' 
		--Scale item Criteria for Type 2 identifier 
            IF SUBSTRING(@Identifier,7,5) = '00000'
                BEGIN
                --IDENTIFIER MATCHES SCALE FORMAT;  NOW VALIDATE THAT ItemIdentifier.IdentifierType = 'O' (OTHER)         
                --20101129 - DStacey TFS 13744 - screen out deleted identifiers
                       
                SELECT @IdentifierType = IdentifierType FROM ItemIdentifier (nolock) WHERE Identifier = @Identifier AND Deleted_Identifier = 0
                
                IF @IdentifierType = 'O'                
					select @return = 1
				ELSE
					select @return = 0
				END
			ELSE
                select @return = 0
                
    ELSE  
        BEGIN		
			--Get Scale_Identifier flag and ItemIdentifier.IdentifierType to check for Non Type 2 scale item (TFS 6744 - allow for Non Type 2 Scale Identifiers)
        	SELECT @Scale_Identifier = Scale_Identifier
        		 , @IdentifierType = IdentifierType
			FROM ItemIdentifier (NOLOCK) 
			WHERE Identifier = @Identifier AND Deleted_Identifier = 0
			   
	    	--Scale item criteria for Non Type 2 identifier		
			IF @Scale_Identifier = 1 
				select @return = 1	
			ELSE 
				select @return = 0
		  END
        
	RETURN @return
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsScaleItem] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsScaleItem] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsScaleItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsScaleItem] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsScaleItem] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsScaleItem] TO [ExtractRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsScaleItem] TO [DataMigration]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsScaleItem] TO [WFM\IRMA.developers]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsScaleItem] TO [IConInterface]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsScaleItem] TO [spice_user]
    AS [dbo];

