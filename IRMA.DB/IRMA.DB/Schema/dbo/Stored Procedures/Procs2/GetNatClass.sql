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
ORDER BY NatItemClass.ClassName
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetNatClass] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetNatClass] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetNatClass] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetNatClass] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetNatClass] TO [IRMAExcelRole]
    AS [dbo];

