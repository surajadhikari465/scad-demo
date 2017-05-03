SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetNatClass]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetNatClass]
GO


CREATE PROCEDURE dbo.GetNatClass
AS 

SELECT NatItemClass.ClassID,   NatFamilyName + ': ' + NatCatName + ': ' + NatItemClass.ClassName + ' - ' + Convert(char(5), ClassID) ClassName
FROM NatItemClass (nolock)
    INNER JOIN
        NatItemCat (nolock)
        ON NatItemCat.natCatID = NatItemClass.NatCatID
    INNER JOIN
        NatItemFamily (nolock)
        ON NatItemFamily.NatFamilyID = NatItemCat.NatFamilyID
    INNER JOIN
        SubTeam
        ON NatItemFamily.SubTeam_No = SubTeam.SubTeam_No
WHERE NatItemFamily.SubTeam_No is not null
ORDER BY NatItemClass.ClassName

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

