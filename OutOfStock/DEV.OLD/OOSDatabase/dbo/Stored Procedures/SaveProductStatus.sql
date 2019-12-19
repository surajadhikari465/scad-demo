
CREATE PROCEDURE [dbo].[SaveProductStatus]
    @Region VARCHAR(10) ,
    @UPC VARCHAR(13) ,
    @ExpirationDate DATETIME ,
    @ProductStatus VARCHAR(2000)
AS 
    BEGIN

		IF @ExpirationDate IS NULL
			SET @ExpirationDate = DATEADD(week,2,DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE())))      

        DECLARE @regionid INT
		-- left pad UPC to 13 digits.
        IF LEN(@upc) < 13 
            SET @upc = RIGHT('000000000000' + @upc, 13)

        SET @regionid = ( SELECT    id
                          FROM      region
                          WHERE     REGION_ABBR = @region
                        )

        IF @regionid IS NOT NULL 
            BEGIN
                IF EXISTS ( SELECT  1
                            FROM    dbo.ProductStatus ps
                            WHERE   upc = @upc
                                    AND Region = @Region ) 
                    BEGIN
						-- update
                        UPDATE  dbo.ProductStatus
                        SET     ProductStatus = @productStatus,
								ExpirationDate = @ExpirationDate,
								StartDate = GETDATE()
								
                        WHERE   upc = @upc
                                 AND Region = @Region;

                    END 
                ELSE 
                    BEGIN
						-- insert                  
                        INSERT  INTO dbo.ProductStatus
                                ( Region ,
                                  UPC ,
                                  ExpirationDate,
                                  ProductStatus
			                    )
                        VALUES  ( @region ,
                                  @upc ,
                                  @expirationdate ,
                                  @productstatus
			                    )
                    END 
            END
    END