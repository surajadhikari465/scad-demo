SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[IBMIncrementRecords]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[IBMIncrementRecords]
GO


CREATE PROCEDURE dbo.IBMIncrementRecords
@Store_No int,
@Records int
AS 

UPDATE Store 
SET BatchRecords = @Records
WHERE Store_No = @Store_No

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

