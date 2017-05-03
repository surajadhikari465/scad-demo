set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_IsDistributionCenter]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_IsDistributionCenter]

GO

CREATE FUNCTION [dbo].[fn_IsDistributionCenter]
(
    @StoreNo int
)
RETURNS FLOAT
AS
BEGIN
    DECLARE @DistributionCenter bit	

    SELECT @DistributionCenter = Distribution_Center FROM Store WHERE Store_No = @StoreNo

	IF @DistributionCenter IS NULL
		SELECT @DistributionCenter = 0

        RETURN @DistributionCenter
END

GO