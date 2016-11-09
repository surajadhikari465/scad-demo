DECLARE @nonMerchandiseTraitId int;
SET @nonMerchandiseTraitId = (SELECT traitID FROM Trait WHERE traitDesc = 'Non Merchandise');

UPDATE Trait
SET traitPattern = 'Bottle Deposit|CRV|Coupon|Bottle Return|CRV Credit|Legacy POS Only'
WHERE traitID = @nonMerchandiseTraitId

IF NOT EXISTS (SELECT * FROM ItemType t WHERE t.itemTypeDesc = 'Non Retail')
BEGIN
	INSERT INTO ItemType (itemTypeCode, itemTypeDesc)
	VALUES ('NRT', 'Non Retail')
END