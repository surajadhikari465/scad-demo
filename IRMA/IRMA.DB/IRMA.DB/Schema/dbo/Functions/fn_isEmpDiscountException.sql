CREATE FUNCTION [dbo].[fn_isEmpDiscountException]

    (@Store_No  int,
     @Subteam_No int,
     @Discountable bit)

RETURNS bit
AS
BEGIN

      DECLARE @isNoDiscount BIT
      DECLARE @empDiscount BIT

      SET @empDiscount = @Discountable

      If @Discountable = 1

         BEGIN

         	SELECT @isNoDiscount = ISNULL((SELECT store_no
                FROM StoreSubteamDiscountException
		WHERE
			Store_No = @Store_No
	  	  AND
                        Subteam_No = @Subteam_No),0)

 		IF @isNoDiscount = 1

        	BEGIN

           	   SET @empDiscount = 0

        	END                    
       END       

    RETURN @empDiscount

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_isEmpDiscountException] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_isEmpDiscountException] TO [IRMAClientRole]
    AS [dbo];

