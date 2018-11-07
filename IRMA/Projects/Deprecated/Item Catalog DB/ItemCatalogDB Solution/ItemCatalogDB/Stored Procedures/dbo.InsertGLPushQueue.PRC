SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertGLPushQueue]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertGLPushQueue]
GO


CREATE PROCEDURE dbo.InsertGLPushQueue
@Start_Date DateTime,
@End_Date DateTime,
@Modified_By int,
@Distributions bit,
@Transfers bit
AS

INSERT INTO GLPushQueue (Start_Date, End_Date, Modified_By, Distributions, Transfers)
VALUES (@Start_Date, @End_Date, @Modified_By, @Distributions, @Transfers)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


