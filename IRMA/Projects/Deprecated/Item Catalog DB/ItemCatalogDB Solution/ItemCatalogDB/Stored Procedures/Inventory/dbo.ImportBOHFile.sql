
if exists (select * from dbo.sysobjects where id = object_id(N'dbo.ImportBOHFile') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure dbo.ImportBOHFile
GO

CREATE PROCEDURE dbo.ImportBOHFile

-- EXEC dbo.ImportBOHFile

AS
BEGIN
    BEGIN TRAN

    DECLARE @error_no int
    SET @error_no = 0

	-- ###########################################################################
	--  5/29/08 Robin Eudy (bug 6632)
	--  All functionality in this stored procedure is no longer needed.
	
	-- ###########################################################################
	
	-- Change To Case Count:
	-- Change count from units to cases (EXE 5.5 - 4.6 sent case count)
	--UPDATE dbo.Warehouse_Inventory
	--SET Tot_BOH = Tot_BOH / UnitShipCase
	    -- TEMPORARY BEGIN - ONLY UNTIL PROBLEMS ARE RESOLVED
	    -- PER LAWRENCE PRIEST 4/15/2008
	    
	    -- Commented out again 4/16/08 per Lawrence
	    --, Dist_Center = 17
	    
	    -- TEMPORARY END

    SELECT @error_no = @@ERROR

    IF @error_no = 0
	    COMMIT TRAN
    ELSE
    BEGIN
        IF @@TRANCOUNT <> 0
            ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('dbo.ImportBOHFile failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

--GRANT  EXECUTE  ON dbo.ImportBOHFile  TO public
--GRANT  EXECUTE  ON dbo.ImportBOHFile  TO IRMAClientRole

GO

