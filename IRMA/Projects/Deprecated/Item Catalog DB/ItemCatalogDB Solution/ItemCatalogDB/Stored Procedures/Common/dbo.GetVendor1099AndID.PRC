SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetVendor1099AndID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetVendor1099AndID]
GO


CREATE PROCEDURE dbo.GetVendor1099AndID 
AS 

SELECT Vendor1099_ID, Vendor1099_Desc 
FROM Vendor1099 

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

