if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetStoreJurisdiction]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetStoreJurisdiction]
GO

/****** Object:  StoredProcedure [dbo].[GetStoreJurisdiction]    Script Date: 11/06/2007 16:53:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[GetStoreJurisdiction]
AS 

SELECT StoreJurisdictionID
      ,StoreJurisdictionDesc
FROM storejurisdiction

GO