IF EXISTS (SELECT * FROM sysobjects WHERE  name = N'fn_LoadExternalCycleCountEOP')
	DROP FUNCTION fn_LoadExternalCycleCountEOP
GO

CREATE FUNCTION dbo.fn_LoadExternalCycleCountEOP 
(
	@Date datetime = NULL
)
RETURNS smalldatetime
AS
BEGIN
	DECLARE @EOP smalldatetime, @week tinyint
    
    SELECT @Date = ISNULL(@Date, GETDATE())

    SELECT @week = Week FROM [Date] (nolock) WHERE Date_Key = CONVERT(smalldatetime, CONVERT(varchar(255), @Date, 101))

    SELECT @EOP = dbo.fn_PeriodBeginDate(@Date)

    IF @week NOT IN (1,2)
        SELECT @EOP = DATEADD(day, 28, @EOP)

    SELECT @EOP = DATEADD(minute, -1, @EOP)
    
	RETURN @EOP

END
GO  