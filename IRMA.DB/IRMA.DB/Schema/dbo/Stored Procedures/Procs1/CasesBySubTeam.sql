CREATE PROCEDURE dbo.CasesBySubTeam
    @Store_No int
AS

SELECT SubTeam_Name, SUM(ItemsIn / Item.package_Desc1) AS CasesIn, SUM(ItemsOut / Item.package_Desc1) AS CasesOut
FROM SubTeam (NOLOCK) INNER JOIN (
       Item (NOLOCK) INNER JOIN (
         SELECT Item_Key, SubTeam_No,
                SUM(CASE WHEN Adjustment_ID = 5 THEN Quantity ELSE 0 END) AS ItemsIn, 
                SUM(CASE WHEN Adjustment_ID = 6 THEN Quantity ELSE 0 END) AS ItemsOut
         FROM ItemHistory (NOLOCK)
         WHERE Adjustment_ID IN (5,6) AND Store_No = @Store_No AND 
               DateStamp > DateAdd(d,-90,GetDate())
         GROUP BY Item_Key, SubTeam_No
       ) T1 ON (Item.Item_Key = T1.Item_Key)
     ) ON (SubTeam.SubTeam_No = T1.SubTeam_No)
WHERE Item.Package_Desc1 <> 0
GROUP BY SubTeam_Name
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CasesBySubTeam] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CasesBySubTeam] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CasesBySubTeam] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CasesBySubTeam] TO [IRMAReportsRole]
    AS [dbo];

