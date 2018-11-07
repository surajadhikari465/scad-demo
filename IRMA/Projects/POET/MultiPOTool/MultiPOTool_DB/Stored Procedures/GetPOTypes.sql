if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetPOTypes]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetPOTypes]
GO

CREATE PROCEDURE dbo.GetPOTypes

AS
BEGIN
	select POTypeID, cast(POTypeID as char(1)) + ' - ' + POTypeDescription as POTypeDescription from POType
END

GO