IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Administration_Scale_InsertStoreScaleConfig]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Administration_Scale_InsertStoreScaleConfig]
GO

set ANSI_NULLS OFF
set QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Administration_Scale_InsertStoreScaleConfig]
@Store_No int, 
@ScaleFileWriterKey int

AS
-- Insert a new configuration record into the StoreScaleConfig table for the
-- POS Push process.
BEGIN
   INSERT INTO StoreScaleConfig (Store_No, ScaleFileWriterKey) 
   VALUES (@Store_No, @ScaleFileWriterKey)
END
GO
