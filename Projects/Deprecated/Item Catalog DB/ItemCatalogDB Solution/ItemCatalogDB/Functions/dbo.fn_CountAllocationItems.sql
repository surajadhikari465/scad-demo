SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_CountAllocationItems]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[fn_CountAllocationItems]

GO

CREATE FUNCTION [dbo].[fn_CountAllocationItems]
()
RETURNS INT
AS
BEGIN
    DECLARE @ItemCount int

    SELECT @ItemCount = COUNT(*) FROM tmpOrdersAllocateItems

    RETURN @ItemCount
END

GO