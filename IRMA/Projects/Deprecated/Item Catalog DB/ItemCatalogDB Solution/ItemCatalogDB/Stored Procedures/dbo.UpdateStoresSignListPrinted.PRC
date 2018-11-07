SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateStoresSignListPrinted]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateStoresSignListPrinted]
GO


CREATE PROCEDURE dbo.UpdateStoresSignListPrinted
@StoreSignListID int
AS

UPDATE StoresSignList 
SET Sign_Printed = 1 
WHERE StoreSignListID = @StoreSignListID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


