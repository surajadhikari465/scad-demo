SELECT
   *
FROM   ( SELECT
            hierarchyclassname AS [family name],
            hierarchyclassid   AS [family id]
         FROM   hierarchyclass hc
         JOIN   hierarchy h ON hc.hierarchyid = h.hierarchyid
                           AND h.hierarchyname = 'National Hierarchy'
                           AND hc.parenthierarchyclassid IS NULL ) fam
JOIN   ( SELECT
            hierarchyclassname AS [cat name],
            hierarchyclassid   AS [cat id],
            parenthierarchyclassid
         FROM   hierarchyclass hc
         JOIN   hierarchy h ON hc.hierarchyid = h.hierarchyid
                           AND h.hierarchyname = 'National Hierarchy' ) cat ON fam.[family id] = cat.parenthierarchyclassid
JOIN   ( SELECT
            hierarchyclassname AS [cls name],
            hierarchyclassid   AS [cls id],
            parenthierarchyclassid
         FROM   hierarchyclass hc
         JOIN   hierarchy h ON hc.hierarchyid = h.hierarchyid
                           AND h.hierarchyname = 'National Hierarchy' ) cls ON cat.[cat id] = cls.parenthierarchyclassid
ORDER  BY
   fam.[family name] 
