SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'dbo.InsertPriceBatchDetailMissingOffSale') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.InsertPriceBatchDetailMissingOffSale
GO
