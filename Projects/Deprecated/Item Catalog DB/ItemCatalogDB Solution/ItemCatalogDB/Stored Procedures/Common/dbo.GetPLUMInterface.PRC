SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetPLUMInterface]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetPLUMInterface]
GO


CREATE PROCEDURE dbo.GetPLUMInterface

AS

BEGIN
    SET NOCOUNT ON

    SELECT * FROM PLUMInterface
    
    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

