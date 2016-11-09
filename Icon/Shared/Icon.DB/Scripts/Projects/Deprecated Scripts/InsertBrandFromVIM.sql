-- clean out the brands first:
DELETE itemhierarchyclass
WHERE  hierarchyclassid IN ( SELECT
                                hierarchyclassid
                             FROM   hierarchyclass
                             WHERE  hierarchyid = ( SELECT
                                                       hierarchyid
                                                    FROM   hierarchy
                                                    WHERE  hierarchyname LIKE 'brand' ) )
DELETE hierarchyclass
WHERE  hierarchyid = ( SELECT
                          hierarchyid
                       FROM   hierarchy
                       WHERE  hierarchyname LIKE 'brand' ) 

-- go get them from VIMData
INSERT INTO hierarchyclass
(
   hierarchyid,
   hierarchyclassname
)
SELECT
   (select hierarchyid from hierarchy where hierarchyname like 'brand'),
   vim.brand
FROM   VIMData vim
GROUP  BY
   vim.brand

-- check it
SELECT
   hc.*
FROM   hierarchyclass hc
JOIN   hierarchy h ON hc.hierarchyid = h.hierarchyid
                  AND h.hierarchyname like 'brand' 
