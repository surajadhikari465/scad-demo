Create Function dbo.JDA_LimitMaxDate(@InDate datetime)
        returns datetime
Begin
	DECLARE @OutDate datetime
	DECLARE @DB2MaxDate datetime

	SET @OutDate = NULL
	SET @DB2MaxDate = '12/31/2039'

	IF @InDate IS NOT NULL
		SELECT @OutDate = CASE WHEN YEAR(@InDate) < 2040 THEN @InDate ELSE @DB2MaxDate END

	RETURN @OutDate;
	
End