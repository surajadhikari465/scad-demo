SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateItemIdentifierDefault]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateItemIdentifierDefault]
GO


CREATE PROCEDURE dbo.UpdateItemIdentifierDefault 
@Item_Key int,
@Identifier_ID int
AS

UPDATE ItemIdentifier
SET Default_Identifier = (CASE WHEN Identifier_ID = @Identifier_ID THEN 1 ELSE 0 END)
WHERE Item_Key = @Item_Key
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


