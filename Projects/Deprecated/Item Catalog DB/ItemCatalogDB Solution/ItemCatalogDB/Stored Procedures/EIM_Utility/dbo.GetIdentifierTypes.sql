
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetIdentifierTypes]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[GetIdentifierTypes]
GO

CREATE PROCEDURE dbo.GetIdentifierTypes
AS 

BEGIN
    SET NOCOUNT ON
    
	SELECT 'B' As IdentifierType
	UNION
	SELECT 'O' As IdentifierType
	UNION
	SELECT 'P' As IdentifierType
	UNION
	SELECT 'S' As IdentifierType
	    
    SET NOCOUNT OFF
END
GO