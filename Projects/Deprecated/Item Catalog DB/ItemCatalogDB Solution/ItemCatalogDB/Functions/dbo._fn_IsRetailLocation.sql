set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_IsRetailLocation]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_IsRetailLocation]

GO

CREATE FUNCTION [dbo].[fn_IsRetailLocation]
(
    @StoreNo int
)
RETURNS FLOAT
AS
BEGIN
    DECLARE @WFMStore bit	

    SELECT @WFMStore = WFM_Store FROM Store WHERE Store_No = @StoreNo

        RETURN @WFMStore
END

GO

