CREATE PROCEDURE [dbo].[EIM_ValidateCostedByWeightUnit]
    @UomId int,
	@CheckWeightedUnit int,
    @ValidationCode int OUTPUT
AS

BEGIN

    SET NOCOUNT ON
           
    Set @ValidationCode = 1

	-- validates that the uom is a valid costed by weight UOM
	IF EXISTS
		(SELECT 1 FROM ItemUnit WHERE IsPackageUnit = 1 OR Weight_Unit = 1 AND Unit_ID = @UomID)
	BEGIN
		SET @ValidationCode = 0
	END

	-- if @CheckWeightedUnit = 1, then uom can be considered valid if weighted unit
	IF @CheckWeightedUnit = 1
	BEGIN
		IF EXISTS
		(SELECT 1 FROM ItemUnit WHERE Weight_Unit = 1 AND Unit_ID = @UomID)
		BEGIN
			SET @ValidationCode = 0
		END
	END
	
	
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_ValidateCostedByWeightUnit] TO [IRMAClientRole]
    AS [dbo];

