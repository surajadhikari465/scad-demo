SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CheckForDuplicateIdentifier]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CheckForDuplicateIdentifier]
GO


CREATE PROCEDURE dbo.CheckForDuplicateIdentifier 
    @Identifier varchar(13) 
AS 
BEGIN
    SET NOCOUNT ON

    SELECT COUNT(*) AS IdentifierCount
    FROM ItemIdentifier
    WHERE Identifier = @Identifier AND Deleted_Identifier = 0

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



