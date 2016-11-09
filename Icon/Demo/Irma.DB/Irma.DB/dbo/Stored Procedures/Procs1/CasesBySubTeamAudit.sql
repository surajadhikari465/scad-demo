﻿CREATE PROCEDURE dbo.CasesBySubTeamAudit
@Store_No int,
@SubTeam_No int
AS

SELECT Identifier, Item_Description, Item.Package_Desc1, 
       SUM(ItemsIn / Item.package_Desc1) AS CasesIn, 
       SUM(ItemsOut / Item.package_Desc1) AS CasesOut
FROM ItemIdentifier (NOLOCK) INNER JOIN (
       Item (NOLOCK) INNER JOIN (
         SELECT Item_Key, SubTeam_No,
                SUM(CASE WHEN Adjustment_ID = 5 THEN Quantity ELSE 0 END) AS ItemsIn, 
                SUM(CASE WHEN Adjustment_ID = 6 THEN Quantity ELSE 0 END) AS ItemsOut
         FROM ItemHistory (NOLOCK) 
         WHERE Adjustment_ID IN (5,6) AND Store_No = @Store_No AND SubTeam_No = @SubTeam_No AND
               DateStamp > DateAdd(d,-90,GetDate())
         GROUP BY Item_Key, SubTeam_No
       ) T1 ON (Item.Item_Key = T1.Item_Key)
     ) ON (ItemIdentifier.Item_Key = Item.Item_Key AND ItemIdentifier.Default_Identifier = 1)
WHERE Item.Package_Desc1 <> 0 
GROUP BY Item.Item_Key, Identifier, Item_Description, Item.Package_Desc1
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CasesBySubTeamAudit] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CasesBySubTeamAudit] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CasesBySubTeamAudit] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CasesBySubTeamAudit] TO [IRMAReportsRole]
    AS [dbo];

