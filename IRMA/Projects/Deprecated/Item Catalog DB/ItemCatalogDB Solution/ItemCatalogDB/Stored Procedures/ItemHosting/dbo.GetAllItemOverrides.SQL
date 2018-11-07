if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetAllItemOverrides]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetAllItemOverrides]
GO

/****** Object:  StoredProcedure [dbo].[GetAllItemOverrides]    Script Date: 11/07/2007 11:03:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[GetAllItemOverrides]
    @Item_Key int
AS 
BEGIN
SELECT
   i.item_Key,
   sj.StoreJurisdictionDesc,
   sj.StoreJurisdictionID,
   I.Package_desc1,
   '/' as 'Slash',
   I.Package_desc2,
   iu.Unit_Name
   
FROM
   Item AS I (NOLOCK)
   JOIN ItemUnit iu
      on ( iu.Unit_Id = i.Retail_Unit_Id )
   JOIN StoreJurisdiction sj (NOLOCK)
      on ( sj.StoreJurisdictionId = i.StoreJurisdictionId)

WHERE
   i.Item_Key = @Item_Key

UNION

SELECT
   i.item_Key,
   sj.StoreJurisdictionDesc,
   sj.StoreJurisdictionID,
   I.Package_desc1,
   '/' as 'Slash',
   I.Package_desc2,
   iu.Unit_Name
   
FROM
   ItemOverride AS I (NOLOCK)
   JOIN ItemUnit iu
      ON ( iu.Unit_Id = i.Retail_Unit_Id )
   JOIN StoreJurisdiction sj (NOLOCK)
      ON ( sj.StoreJurisdictionId = i.StoreJurisdictionId)
WHERE
   i.Item_Key = @Item_Key

END

GO