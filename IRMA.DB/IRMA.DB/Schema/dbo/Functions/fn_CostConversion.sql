﻿CREATE FUNCTION dbo.fn_CostConversion (
    @Amount decimal(22,8),
    @FromUnit int,
    @ToUnit int,
    @PD1 decimal(9,4),
    @PD2 decimal(9,4), 
    @PDU int)
  
RETURNS decimal(22,8)
AS
   -- **************************************************************************
   -- Procedure: fn_CostConversion()
   --    Author: n/a
   --      Date: n/a
   --
   -- Description:
   -- Called by multiple SP's and multiple times within some SP's to convert cost
   -- from one UOM at @Amount to another UOM at @Amount
   --
   -- Modification History:
   -- Date        Init	TFS		Comment
   -- 01/13/2011  BBB	13836	added in additional check for KG for item setups that dont
   --							reach the initial branch changed in 13334
   -- 12/13/2010  BBB	13334	modified call to ConvertItemUnit that converts from KG to LB 
   --							so that cost per unit is calculated at the correct weight 
   --							value, and let other weight values convert as intended
   -- **************************************************************************
BEGIN
    DECLARE @Result decimal(38,28)

    DECLARE @IsFromWeight bit, @IsToWeight bit, @IsFromPackage bit, @IsToPackage bit, @IsPDUWeight bit
    
	-- Make sure we don't divide by zero
    IF ISNULL(@PD1, 0) = 0
		SET @PD1 = 1
		
	IF ISNULL(@PD2, 0) = 0
		SET @PD2 = 1
    
    SELECT @IsFromWeight = Weight_Unit, @IsFromPackage = IsPackageUnit
    FROM ItemUnit WITH (nolock)
    WHERE Unit_ID = @FromUnit

    SELECT @IsToWeight = Weight_Unit, @IsToPackage = IsPackageUnit
    FROM ItemUnit WITH (nolock)
    WHERE Unit_ID = @ToUnit
    
    SELECT @IsPDUWeight = Weight_Unit
    FROM ItemUnit (nolock)
    WHERE Unit_ID = @PDU

    SELECT @IsFromWeight = ISNULL(@IsFromWeight, 0), 
           @IsFromPackage = ISNULL(@IsFromPackage, 0), 
           @IsToWeight = ISNULL(@IsToWeight, 0), 
           @IsToPackage = ISNULL(@IsToPackage, 0),
           @IsPDUWeight = ISNULL(@IsPDUWeight, 0)
           
    IF (@FromUnit <> @ToUnit) AND (@IsFromPackage = 0 OR @IsToPackage = 0)
    BEGIN
        IF @IsFromWeight = 1
        BEGIN
            IF @IsToPackage = 1
            BEGIN
                SELECT @Result = @Amount * @PD1 * @PD2
                
                IF (@IsPDUWeight = 1) AND (@PDU <> @FromUnit)
					-- Make sure we don't convert KG to LB
					IF (SELECT ISNULL(EDISysCode, PlumUnitAbbr) FROM ItemUnit WHERE Unit_ID = @PDU) <> 'KG'
						SELECT @Result = dbo.fn_ConvertItemUnit(@Result, @PDU, @FromUnit)
            END
            ELSE
            BEGIN
                IF @IsToWeight = 1
                BEGIN 
                    SELECT @Result =  dbo.fn_ConvertItemUnit(@Amount, @ToUnit, @FromUnit)
                END
                ELSE
                BEGIN 
                    SELECT @Result = @Amount
                END  
            END
        END
        ELSE
        BEGIN
            IF @IsFromPackage = 0
            BEGIN
                IF @IsToPackage = 1
                    SELECT @Result = @Amount * @PD1
                ELSE
                    SELECT @Result = @Amount
            END
            ELSE --FromUnit is case
            BEGIN
                SELECT @Result = @Amount / (@PD1 * CASE WHEN @IsToWeight = 1 THEN @PD2 ELSE 1 END)
                
                IF @IsToWeight = 1 And @IsPDUWeight = 1 And (@ToUnit <> @PDU)
				BEGIN
					-- Make sure we don't convert KG to LB
					IF (SELECT ISNULL(EDISysCode, PlumUnitAbbr) FROM ItemUnit WHERE Unit_ID = @PDU) <> 'KG'
						SELECT @Result = dbo.fn_ConvertItemUnit(@Result, @ToUnit, @PDU)
				END
            END
        END

    END
    ELSE
        SELECT @Result = @Amount
   
    RETURN @Result
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_CostConversion] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_CostConversion] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_CostConversion] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_CostConversion] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_CostConversion] TO [IMHARole]
    AS [dbo];

