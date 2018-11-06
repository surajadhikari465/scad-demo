SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetCurrentOnHand]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetCurrentOnHand]
GO


CREATE PROCEDURE dbo.GetCurrentOnHand 
    @Item_Key int,
    @Store_No int,
    @SubTeam_No int
AS 
SELECT Quantity,
       Weight
FROM   OnHand (nolock)
WHERE  Item_Key = @Item_Key AND 
       Store_No = @Store_No AND
       SubTeam_No = @SubTeam_No
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



