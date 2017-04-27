 if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fn_IsAvgCostAdjReasonActive]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[fn_IsAvgCostAdjReasonActive]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO



Create  FUNCTION dbo.fn_IsAvgCostAdjReasonActive
	(@ID int
)
RETURNS bit
AS

BEGIN  
	DECLARE @return bit
    
    SELECT @return = Active FROM AvgCostAdjReason WHERE ID = @ID
        
	RETURN @return
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
