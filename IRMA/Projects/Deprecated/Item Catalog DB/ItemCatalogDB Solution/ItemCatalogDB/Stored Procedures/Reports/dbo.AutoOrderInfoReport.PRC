SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AutoOrderInfoReport]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AutoOrderInfoReport]
GO


CREATE PROCEDURE dbo.AutoOrderInfoReport
@Item_Key int
AS

SELECT Item_Key 
FROM Item 
WHERE Item_Key = @Item_Key

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

