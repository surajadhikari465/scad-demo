if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EIM_HasNoPrimaryVendor]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[EIM_HasNoPrimaryVendor]
GO

CREATE PROCEDURE dbo.EIM_HasNoPrimaryVendor
    @Item_Key int,
    @StoreNoList varchar(8000),
    @StoreListSeparator char(1)
    ,
    @HasNoPrimaryVendor bit OUTPUT
AS
BEGIN
    SET NOCOUNT ON

	SET @HasNoPrimaryVendor = 0
	
	DECLARE @Store_No AS INT
	
	DECLARE Store_cursor CURSOR FOR
		SELECT CAST(Key_Value AS int) AS Store_No
		FROM dbo.fn_ParseStringList(@StoreNoList, @StoreListSeparator)
		
	OPEN Store_cursor
	
	FETCH NEXT FROM Store_cursor INTO @Store_No

	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF NOT EXISTS (SELECT *
			FROM StoreItemVendor (NOLOCK)
			WHERE Item_Key = @Item_Key
			AND PrimaryVendor = 1
			AND Store_No = @Store_No)
		BEGIN
			SET @HasNoPrimaryVendor = 1
		END
	
		FETCH NEXT FROM Store_cursor INTO @Store_No
	END
	
	CLOSE Store_cursor
	DEALLOCATE Store_cursor
	
    SET NOCOUNT OFF
END
GO   