if exists (select * from dbo.sysobjects where id = object_id(N'dbo.EIM_JurisdictionValidation') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.EIM_JurisdictionValidation
GO

CREATE PROCEDURE dbo.EIM_JurisdictionValidation
    @Item_Key int,
    @IsDefaultJurisdiction bit,
    @StoreJurisdictionId int
    ,
    @ValidationCode int OUTPUT
AS

BEGIN

	-- This validation runs only for existing item sessions just before upload.
    SET NOCOUNT ON
    
    DECLARE @ExistingAlternateStoreJurisdictionId int
       
    Set @ValidationCode = 0

	IF @IsDefaultJurisdiction = 1
	BEGIN
		-- given other EIM validation we know the item is in the Item table
		-- so if we cannot find it for the given jurisdiction then
		-- we know the user is trying to change the default jurisdiction,
		-- which is not allowed
		SELECT Item_Key FROM dbo.Item (NOLOCK) WHERE Item_Key = @Item_Key AND StoreJurisdictionId = @StoreJurisdictionId

		IF @@ROWCOUNT = 0
		BEGIN
			SET @ValidationCode = 2
		END	
	END
		
    SET NOCOUNT OFF
END

GO