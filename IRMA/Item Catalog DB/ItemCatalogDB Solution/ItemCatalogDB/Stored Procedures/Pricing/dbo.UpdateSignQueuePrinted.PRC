SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateSignQueuePrinted]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateSignQueuePrinted]
GO


CREATE PROCEDURE dbo.UpdateSignQueuePrinted
    @ItemList varchar(8000),
    @ItemListSeparator char(1),
    @Store_No int,
    @Printed bit,
    @User_ID int,
    @Type tinyint = NULL
AS

UPDATE SignQueue 
SET Sign_Printed = @Printed,
    [User_ID] = CASE WHEN @Printed = 0 THEN @User_ID ELSE [User_ID] END,
    User_ID_Date = CASE WHEN @Printed = 0 THEN GETDATE() ELSE User_ID_Date END,
    LastQueuedType = CASE WHEN @Printed = 0 THEN @Type ELSE LastQueuedType END
FROM SignQueue
INNER JOIN 
    fn_Parse_List(@ItemList, @ItemListSeparator) IL
    ON IL.Key_Value = SignQueue.Item_Key 
WHERE Store_No = @Store_No

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


