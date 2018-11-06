
CREATE VIEW [zz_deprecated_CixNatItemFamily] AS
SELECT
family		AS NatFamilyID,
fam_name	AS NatFamilyName,
null		AS NatSubTeam_No,
null		AS SubTeam_No
FROM
cmmfmlyr
