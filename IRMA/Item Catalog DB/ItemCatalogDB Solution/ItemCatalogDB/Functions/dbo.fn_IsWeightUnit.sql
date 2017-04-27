SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fn_IsWeightUnit]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[fn_IsWeightUnit]
GO

CREATE FUNCTION [dbo].[fn_IsWeightUnit]
(
    @UnitID int
)
RETURNS Bit
AS
BEGIN
    DECLARE @IsWeightUnit as Bit	
    DECLARE @IsPackageUnit as Bit

    SELECT @IsWeightUnit = Weight_Unit 
    FROM ItemUnit 
    WHERE Unit_ID = @UnitID

    SELECT @IsPackageUnit = IsPackageUnit 
    FROM ItemUnit 
    WHERE Unit_ID = @UnitID
    
    If @IsWeightUnit = 1 or @IsPackageUnit = 1 RETURN 1
    Return 0
END
GO