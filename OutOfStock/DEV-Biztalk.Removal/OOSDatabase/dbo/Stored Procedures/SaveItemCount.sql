
CREATE  PROCEDURE [dbo].[SaveItemCount]
    @StoreId INT ,
    @TeamName VARCHAR(100) ,
    @ItemCount INT
AS 
    BEGIN
	DECLARE @id TABLE ( skuid INT NOT NULL )
	DECLARE @cnt INT


	PRINT CAST(@StoreId AS VARCHAR(100))
	PRINT @teamname

	
	INSERT INTO @id	
    SELECT  idSKUCount
    FROM    dbo.SKUCount sku
            INNER JOIN store s ON sku.STORE_PS_BU = s.PS_BU
            INNER JOIN dbo.TEAM_Interim t ON sku.TEAM_ID = t.idTeam
    WHERE   s.id = @StoreId
            AND t.teamName = @TeamName


	SET @cnt = @@ROWCOUNT

	IF @cnt = 0
		BEGIN
			PRINT 'insert'
			INSERT INTO dbo.SKUCount ( STORE_PS_BU , TEAM_ID , numberOfSKUs )
            SELECT  PS_BU ,
                    t.idTeam ,
                    @ItemCount
            FROM    store s
                    INNER JOIN dbo.TEAM_Interim t ON t.teamName = @TeamName
            WHERE   s.id = @StoreId

		END
	ELSE IF @cnt = 1 
		BEGIN
			PRINT 'update'
            UPDATE  dbo.SKUCount
            SET     numberOfSKUs = @ItemCount
            FROM    @id data
            WHERE   dbo.SKUCount.idSKUCount = data.skuid
		END
	ELSE IF @cnt > 1
		BEGIN
			-- error
			RAISERROR('something bad happened', 10 , 1) WITH nowait
		END      

END