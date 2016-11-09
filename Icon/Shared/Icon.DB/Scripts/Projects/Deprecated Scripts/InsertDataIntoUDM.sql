------------------------------------------------------------------------- :trait group
INSERT INTO traitgroup
(
   traitgroupdesc
)
VALUES
(
   'Item Attributes'
)
go

INSERT INTO traitgroup
(
   traitgroupdesc
)
VALUES
(
   'Locale Attributes'
)
go

INSERT INTO traitgroup
(
   traitgroupdesc
)
VALUES
(
   'Price Attributes'
)
go

INSERT INTO traitgroup
(
   traitgroupdesc
)
VALUES
(
   'eCommerce Attributes'
)
go

------------------------------------------------------------------------- :trait
----- :item attributes (1)
INSERT INTO trait
(
   traitpattern,
   traitdesc,
   traitgroupcode
)
VALUES
(
   '/^[0-9]{1,14}$/',
   'ScanCode',
   1
)
--INSERT INTO trait -- LONG DESCRIPTION IS ALREADY ON ITEM TABLE
--(
--   traitpattern,
--   traitdesc,
--   traitgroupcode
--)
--VALUES
--(
--   '^[a-zA-Z0-9_]*$',
--   'Long Description',
--   1
--)
INSERT INTO trait
(
   traitpattern,
   traitdesc,
   traitgroupcode
)
VALUES
(
   '^[a-zA-Z0-9_]*$',
   'Short Description',
   1
)
INSERT INTO trait
(
   traitpattern,
   traitdesc,
   traitgroupcode
)
VALUES
(
   '^[0-9]*\.?[0-9]+$',
   'Package Unit',
   1
)
INSERT INTO trait
(
   traitpattern,
   traitdesc,
   traitgroupcode
)
VALUES
(
   '0|1',
   'Food Stamp Eligible',
   1
)
INSERT INTO trait
(
   traitpattern,
   traitdesc,
   traitgroupcode
)
VALUES
(
   '^[0-9]*\.?[0-9]+$',
   'POS Scale Tare',
   1
)
----- :locale attributes (2)
INSERT INTO trait
(
   traitpattern,
   traitdesc,
   traitgroupcode
)
VALUES
(
   '^[0-9]*\.?[0-9]+$',
   'Retail Size',
   2
)
INSERT INTO trait
(
   traitpattern,
   traitdesc,
   traitgroupcode
)
VALUES
(
   '^[0-9]*\.?[0-9]+$',
   'Retail UOM',
   2
)
INSERT INTO trait
(
   traitpattern,
   traitdesc,
   traitgroupcode
)
VALUES
(
   '0|1',
   'TM Discount Eligible',
   2
)
INSERT INTO trait
(
   traitpattern,
   traitdesc,
   traitgroupcode
)
VALUES
(
   '0|1',
   'Case Discount Eligible',
   2
)
INSERT INTO trait
(
   traitpattern,
   traitdesc,
   traitgroupcode
)
VALUES
(
   '0|1',
   'Prohibit Discount',
   2
)
INSERT INTO trait
(
   traitpattern,
   traitdesc,
   traitgroupcode
)
VALUES
(
   '^[0-9]*\.?[0-9]+$',
   'Age Restrict',
   2
)
INSERT INTO trait
(
   traitpattern,
   traitdesc,
   traitgroupcode
)
VALUES
(
   '0|1',
   'Sold by Weight',
   2
)
INSERT INTO trait
(
   traitpattern,
   traitdesc,
   traitgroupcode
)
VALUES
(
   '0|1',
   'Force Tare',
   2
)
INSERT INTO trait
(
   traitpattern,
   traitdesc,
   traitgroupcode
)
VALUES
(
   '0|1',
   'Quantity Required',
   2
)
INSERT INTO trait
(
   traitpattern,
   traitdesc,
   traitgroupcode
)
VALUES
(
   '0|1',
   'Price Required',
   2
)
INSERT INTO trait
(
   traitpattern,
   traitdesc,
   traitgroupcode
)
VALUES
(
   '^[0-9]*\.?[0-9]+$',
   'Quantity Prohibit',
   2
)
INSERT INTO trait
(
   traitpattern,
   traitdesc,
   traitgroupcode
)
VALUES
(
   '0|1',
   'Visual Verify',
   2
)
INSERT INTO trait
(
   traitpattern,
   traitdesc,
   traitgroupcode
)
VALUES
(
   '0|1',
   'Restrict Sale',
   2
)
INSERT INTO trait
(
   traitpattern,
   traitdesc,
   traitgroupcode
)
VALUES
(
   '0|1',
   'Delete',
   2
)
----- :price attributes (3)
INSERT INTO trait
(
   traitpattern,
   traitdesc,
   traitgroupcode
)
VALUES
(
   '^[0-9]*\.?[0-9]+$',
   'Locale',
   3
)
INSERT INTO trait
(
   traitpattern,
   traitdesc,
   traitgroupcode
)
VALUES
(
   '^[0-9]*\.?[0-9]+$',
   'Price',
   3
)
INSERT INTO trait
(
   traitpattern,
   traitdesc,
   traitgroupcode
)
VALUES
(
   '^[0-9]*\.?[0-9]+$',
   'Price Multiple',
   3
)
INSERT INTO trait
(
   traitpattern,
   traitdesc,
   traitgroupcode
)
VALUES
(
   '^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d$',
   'Price Start Date',
   3
)
INSERT INTO trait
(
   traitpattern,
   traitdesc,
   traitgroupcode
)
VALUES
(
   '^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d$',
   'Price End Date',
   3
)
----- :ecommerce attributes (4)
INSERT INTO trait
(
   traitpattern,
   traitdesc,
   traitgroupcode
)
VALUES
(
   '^[a-zA-Z0-9_]*$',
   'Short Romance',
   4
)
INSERT INTO trait
(
   traitpattern,
   traitdesc,
   traitgroupcode
)
VALUES
(
   '^[a-zA-Z0-9_]*$',
   'Long Romance',
   4
)
INSERT INTO trait
(
   traitpattern,
   traitdesc,
   traitgroupcode
)
VALUES
(
   '0|1',
   'Gluten Free',
   4
)
INSERT INTO trait
(
   traitpattern,
   traitdesc,
   traitgroupcode
)
VALUES
(
   '0|1',
   'Premium Body Care',
   4
)
INSERT INTO trait
(
   traitpattern,
   traitdesc,
   traitgroupcode
)
VALUES
(
   '0|1',
   'Exclusive',
   4
)
INSERT INTO trait
(
   traitpattern,
   traitdesc,
   traitgroupcode
)
VALUES
(
   '0|1',
   'Whole Trade',
   4
)
INSERT INTO trait
(
   traitpattern,
   traitdesc,
   traitgroupcode
)
VALUES
(
   '0|1',
   'Non GMO',
   4
)
INSERT INTO trait
(
   traitpattern,
   traitdesc,
   traitgroupcode
)
VALUES
(
   '0|1',
   'HSH',
   4
)
INSERT INTO trait
(
   traitpattern,
   traitdesc,
   traitgroupcode
)
VALUES
(
   '0|1',
   'E2',
   4
)
INSERT INTO trait
(
   traitpattern,
   traitdesc,
   traitgroupcode
)
VALUES
(
   '0|1',
   'Vegan',
   4
)
INSERT INTO trait
(
   traitpattern,
   traitdesc,
   traitgroupcode
)
VALUES
(
   '0|1',
   'Vegetarian',
   4
)
INSERT INTO trait
(
   traitpattern,
   traitdesc,
   traitgroupcode
)
VALUES
(
   '0|1',
   'Kosher',
   4
)
INSERT INTO trait
(
   traitpattern,
   traitdesc,
   traitgroupcode
)
VALUES
(
   'Baseline|Premium|Ultra-Premium',
   'ECO Scale Rating',
   4
)
INSERT INTO trait
(
   traitpattern,
   traitdesc,
   traitgroupcode
)
VALUES
(
   '0|1',
   'Organic',
   4
)
go

------------------------------------------------------------------------- :locale
INSERT INTO locale
(
   ownerorgpartyid,
   localename,
   localeopendate,
   localetypecode
)
VALUES
(
   1,
   'Chain',
   '09/20/1980',
   1
)
go

------------------------------------------------------------------------- :hierarchy
INSERT INTO hierarchy
(
   hierarchyname
)
VALUES
(
   'National Hierarchy'
)
INSERT INTO hierarchy
(
   hierarchyname
)
VALUES
(
   'Brand'
)
INSERT INTO hierarchy
(
   hierarchyname
)
VALUES
(
   'Tax'
)
INSERT INTO hierarchy
(
   hierarchyname
)
VALUES
(
   'Browsing'
)
go

------------------------------------------------------------------------- :hierarchyclass
INSERT INTO hierarchyclass
(
   hierarchyid,
   parenthierarchyclassid,
   hierarchyclassid,
   hierarchyclassname
)
SELECT
   1,
   NULL,
   fam.natfamilyid,
   fam.natfamilyname
FROM   [idt-sw].[ItemCatalog_Test].[dbo].natitemfamily fam

go

INSERT INTO hierarchyclass
(
   hierarchyid,
   parenthierarchyclassid,
   hierarchyclassid,
   hierarchyclassname
)
SELECT
   1,
   cat.natfamilyid,
   cat.natcatid,
   cat.natcatname
FROM   [idt-sw].[ItemCatalog_Test].[dbo].natitemcat cat

go

INSERT INTO hierarchyclass
(
   hierarchyid,
   parenthierarchyclassid,
   hierarchyclassid,
   hierarchyclassname
)
SELECT
   1,
   cls.natcatid,
   cls.classid,
   cls.classname
FROM   [idt-sw].[ItemCatalog_Test].[dbo].natitemclass cls

go 
