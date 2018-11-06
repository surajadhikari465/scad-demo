SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdatePOSChangesGL_Pushed]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdatePOSChangesGL_Pushed]
GO


CREATE PROCEDURE dbo.UpdatePOSChangesGL_Pushed
@Store_No int,
@Sales_Date datetime
AS

UPDATE POSChanges
SET GL_Pushed = 1, GL_InQueue = 0
WHERE GL_Pushed = 0 AND Store_No = @Store_No AND Sales_Date = @Sales_Date AND Aggregated = 1
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

