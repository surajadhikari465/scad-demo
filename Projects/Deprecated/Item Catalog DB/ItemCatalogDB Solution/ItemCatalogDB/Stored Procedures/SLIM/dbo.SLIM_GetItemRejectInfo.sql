IF EXISTS (SELECT * FROM dbo.sysobjects WHERE ID = OBJECT_ID(N'[dbo].[SLIM_GetItemRejectInfo]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[SLIM_GetItemRejectInfo]
GO

CREATE Procedure dbo.SLIM_GetItemRejectInfo
	@RequestIdList varchar(MAX)
AS
	SELECT ii.Identifier, i.Item_Description
	FROM SLIM_InStoreSpecials iss
	JOIN Item i ON i.Item_Key = iss.Item_Key
	JOIN ItemIdentifier ii ON ii.Item_Key = iss.Item_Key
	WHERE ii.Default_Identifier = 1 AND
		  iss.RequestId IN (SELECT Key_Value FROM dbo.fn_Parse_List(@RequestIdList,'|'))
GO