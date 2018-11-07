CREATE FUNCTION fn_GetEnumSubTeamTypes ()
RETURNS @eTmpSubTeamTypes TABLE (IRMASubTeamType int, eSubTeamType Int, EnumName varchar(50)) 

AS

BEGIN
	----------------------------------------------------------------------------------------------------
	-- Create a tmp table with the enum subteam types (as defined by the application)
	----------------------------------------------------------------------------------------------------
	INSERT INTO @eTmpSubTeamTypes  VALUES(1, 2, 'Retail')	
	INSERT INTO @eTmpSubTeamTypes  VALUES(2, 4, 'Manufacturing')
	INSERT INTO @eTmpSubTeamTypes  VALUES(3, 8, 'RetailManufacturing')
	INSERT INTO @eTmpSubTeamTypes  VALUES(4, 16, 'Expense')
	INSERT INTO @eTmpSubTeamTypes  VALUES(5, 32, 'Packaging')
	INSERT INTO @eTmpSubTeamTypes  VALUES(6, 64, 'Supplies')
	INSERT INTO @eTmpSubTeamTypes  VALUES(7, 128, 'Front_End')

	RETURN
END
GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_GetEnumSubTeamTypes] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_GetEnumSubTeamTypes] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_GetEnumSubTeamTypes] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_GetEnumSubTeamTypes] TO [IRMAReportsRole]
    AS [dbo];

