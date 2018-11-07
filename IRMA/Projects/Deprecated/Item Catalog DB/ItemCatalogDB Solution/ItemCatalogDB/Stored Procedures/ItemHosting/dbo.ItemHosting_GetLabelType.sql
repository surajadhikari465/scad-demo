SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ItemHosting_GetLabelType]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ItemHosting_GetLabelType]
GO

CREATE PROCEDURE dbo.ItemHosting_GetLabelType AS

BEGIN
    SET NOCOUNT ON

    SELECT LabelType_ID, LabelTypeDesc FROM LabelType ORDER BY LabelTypeDesc
    
    SET NOCOUNT OFF

END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
