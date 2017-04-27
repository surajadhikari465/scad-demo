IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Administration_Scale_DeleteStoreScaleConfig]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Administration_Scale_DeleteStoreScaleConfig]
GO

set ANSI_NULLS OFF
set QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Administration_Scale_DeleteStoreScaleConfig]
@Store_No int
AS
-- Delete the entry in the StoreScaleConfig table, which is used for the
-- Scale Push process.
BEGIN
   delete from StoreScaleConfig  
   where Store_No = @Store_No
END
GO

