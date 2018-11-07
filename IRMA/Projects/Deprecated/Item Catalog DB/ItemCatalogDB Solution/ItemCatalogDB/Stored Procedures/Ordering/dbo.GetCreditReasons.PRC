SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetCreditReasons]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetCreditReasons]
GO


CREATE PROCEDURE dbo.GetCreditReasons
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT * 
    FROM  CreditReasons
    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



