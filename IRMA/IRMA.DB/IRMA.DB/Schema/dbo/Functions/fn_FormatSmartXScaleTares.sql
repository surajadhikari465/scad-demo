CREATE FUNCTION [dbo].[fn_FormatSmartXScaleTares]
    (@Zone1     DECIMAL(4,3) = 0,
     @Zone2     DECIMAL(4,3) = 0,
     @Zone3     DECIMAL(4,3) = 0,
     @Zone4     DECIMAL(4,3) = 0,
     @Zone5     DECIMAL(4,3) = 0,
     @Zone6     DECIMAL(4,3) = 0,
     @Zone7     DECIMAL(4,3) = 0,
     @Zone8     DECIMAL(4,3) = 0,
     @Zone9     DECIMAL(4,3) = 0,
     @Zone10    DECIMAL(4,3) = 0,
     @ForceTare BIT,
     @ScaleUOM  VARCHAR(25),
     @ScaleFW   VARCHAR(25))
RETURNS VARCHAR(100)
AS
BEGIN
    DECLARE @Result     VARCHAR(100)
    DECLARE @ScaleFW_VC VARCHAR(6)

    SET @ScaleFW_VC = '0'
    
    IF RTRIM(@ScaleUOM) = 'FIXED WEIGHT'
        IF ((@ScaleFW IS NOT NULL) AND (@ScaleFW <> ''))
            IF ISNUMERIC(@ScaleFW) = 1
                SET @ScaleFW_VC = CAST(@ScaleFW AS VARCHAR)
        
    IF @ForceTare = 1
        IF RTRIM(@ScaleUOM) = 'FIXED WEIGHT'
            SET @Result = '-' + @ScaleFW_VC + ',-0.00,-0.00,-0.00,-0.00,-0.00,-0.00,-0.00,-0.00,-0.00'
        ELSE
            SET @Result = '-0.00,-0.00,-0.00,-0.00,-0.00,-0.00,-0.00,-0.00,-0.00,-0.00'
    ELSE
        BEGIN
    
            IF @Zone1  <> 9.999 SET @Zone1  = @Zone1  * 10
            IF @Zone2  <> 9.999 SET @Zone2  = @Zone2  * 10
            IF @Zone3  <> 9.999 SET @Zone3  = @Zone3  * 10
            IF @Zone4  <> 9.999 SET @Zone4  = @Zone4  * 10
            IF @Zone5  <> 9.999 SET @Zone5  = @Zone5  * 10
            IF @Zone6  <> 9.999 SET @Zone6  = @Zone6  * 10
            IF @Zone7  <> 9.999 SET @Zone7  = @Zone7  * 10
            IF @Zone8  <> 9.999 SET @Zone8  = @Zone8  * 10
            IF @Zone9  <> 9.999 SET @Zone9  = @Zone9  * 10
            IF @Zone10 <> 9.999 SET @Zone10 = @Zone10 * 10

            IF RTRIM(@ScaleUOM) = 'FIXED WEIGHT'
                SET @Result = @ScaleFW_VC + ','
            ELSE
                SET @Result = CONVERT(VARCHAR, RTRIM(CAST(@Zone1  AS INT))) + '.' + RTRIM(SUBSTRING(CONVERT(VARCHAR,@Zone1),  3,2)) + ','
                
            SET @Result = @Result + CONVERT(VARCHAR, RTRIM(CAST(@Zone2  AS INT))) + '.' + RTRIM(SUBSTRING(CONVERT(VARCHAR,@Zone2),  3,2)) + ','
            SET @Result = @Result + CONVERT(VARCHAR, RTRIM(CAST(@Zone3  AS INT))) + '.' + RTRIM(SUBSTRING(CONVERT(VARCHAR,@Zone3),  3,2)) + ','
            SET @Result = @Result + CONVERT(VARCHAR, RTRIM(CAST(@Zone4  AS INT))) + '.' + RTRIM(SUBSTRING(CONVERT(VARCHAR,@Zone4),  3,2)) + ','
            SET @Result = @Result + CONVERT(VARCHAR, RTRIM(CAST(@Zone5  AS INT))) + '.' + RTRIM(SUBSTRING(CONVERT(VARCHAR,@Zone5),  3,2)) + ','
            SET @Result = @Result + CONVERT(VARCHAR, RTRIM(CAST(@Zone6  AS INT))) + '.' + RTRIM(SUBSTRING(CONVERT(VARCHAR,@Zone6),  3,2)) + ','
            SET @Result = @Result + CONVERT(VARCHAR, RTRIM(CAST(@Zone7  AS INT))) + '.' + RTRIM(SUBSTRING(CONVERT(VARCHAR,@Zone7),  3,2)) + ','
            SET @Result = @Result + CONVERT(VARCHAR, RTRIM(CAST(@Zone8  AS INT))) + '.' + RTRIM(SUBSTRING(CONVERT(VARCHAR,@Zone8),  3,2)) + ','
            SET @Result = @Result + CONVERT(VARCHAR, RTRIM(CAST(@Zone9  AS INT))) + '.' + RTRIM(SUBSTRING(CONVERT(VARCHAR,@Zone9),  3,2)) + ','
            SET @Result = @Result + CONVERT(VARCHAR, RTRIM(CAST(@Zone10 AS INT))) + '.' + RTRIM(SUBSTRING(CONVERT(VARCHAR,@Zone10), 3,2))
        END

    RETURN @Result
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_FormatSmartXScaleTares] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_FormatSmartXScaleTares] TO [IRMAClientRole]
    AS [dbo];

