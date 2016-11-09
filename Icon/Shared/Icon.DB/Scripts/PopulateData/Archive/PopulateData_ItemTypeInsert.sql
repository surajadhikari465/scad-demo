-- Date:		8/4/2014
-- TFS 2012 #:	4188
-- Description:	This deployment script will populate the ItemType table with
--				the 'Return' and 'Coupon' types. 
--				ItemType will be utilized and updated based on the Non Merchandise Trait
--				that each item is associated to.

IF NOT EXISTS (SELECT * FROM ItemType it WHERE it.itemTypeDesc = 'Return')
BEGIN
	INSERT INTO ItemType (itemTypeCode, itemTypeDesc)
	VALUES ('RTN', 'Return')
END

IF NOT EXISTS (SELECT * FROM ItemType it WHERE it.itemTypeDesc = 'Coupon')
BEGIN
	INSERT INTO ItemType (itemTypeCode, itemTypeDesc)
	VALUES ('CPN', 'Coupon')
END