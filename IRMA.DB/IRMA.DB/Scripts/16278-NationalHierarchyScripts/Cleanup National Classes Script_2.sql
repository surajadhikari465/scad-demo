SET NOCOUNT ON

--Update National Classes with different Category IDs in order to sync classes from Icon
PRINT 'Update National Classes with different Category IDs in order to sync classes from Icon'

BEGIN
	DECLARE @tempNatClass TABLE (
		ClassID INT
		,NewNatCatID INT
		)
	DECLARE @tempNatClassResults TABLE (
		Action NVARCHAR(30)
		,OldNatCatID INT
		,ClassID INT
		,ClassName VARCHAR(65)
		,NatCatID INT
		,LastUpdateTimestamp DATETIME
		)

	INSERT INTO @tempNatClass (
		ClassID
		,NewNatCatID
		)
	VALUES (
		42937
		,33696
		)
		,(
		44572
		,33787
		)
		,(
		44537
		,32299
		)
		,(
		45542
		,34696
		)
		,(
		45543
		,34695
		)
		,(
		45544
		,34697
		)
		,(
		41811
		,34490
		)
		,(
		41812
		,34490
		)
		,(
		44681
		,34430
		)
		,(
		44682
		,34430
		)
		,(
		45546
		,34698
		)

	UPDATE nic
	SET NatCatID = t.NewNatCatID
	OUTPUT 'Changed Parent Category ID' AS Action
		,deleted.NatCatID AS OldNatCatID
		,inserted.*
	INTO @tempNatClassResults
	FROM NatItemClass nic
	JOIN @tempNatClass t ON nic.ClassID = t.ClassID

	SELECT *
	FROM @tempNatClassResults
END

--Update Names in IRMA to match Icon's values
PRINT 'Update Names in IRMA to match Icon''s values'

BEGIN
	DECLARE @tempRenameNatItemFamily TABLE (
		NatFamilyID INT
		,NewNatFamilyName NVARCHAR(255)
		)
	DECLARE @tempRenameNatItemCat TABLE (
		NatCatID INT
		,NewNatCatName NVARCHAR(255)
		)
	DECLARE @tempRenameNatItemClass TABLE (
		ClassID INT
		,NewClassName NVARCHAR(255)
		)
	DECLARE @tempRenameNatItemFamilyResults TABLE (
		Action NVARCHAR(30)
		,OldNatFamilyName NVARCHAR(65)
		,NatFamilyID INT
		,NatFamilyName VARCHAR(65)
		,NatSubTeam_No INT
		,SubTeam_No INT
		,LastUpdateTimestamp DATETIME
		)
	DECLARE @tempRenameNatItemCatResults TABLE (
		Action NVARCHAR(30)
		,OldNatCatName NVARCHAR(65)
		,NatCatID INT
		,NatCatName VARCHAR(65)
		,NatFamilyID INT
		,LastUpdateTimestamp DATETIME
		)
	DECLARE @tempRenameNatItemClassResults TABLE (
		Action NVARCHAR(30)
		,OldClassName NVARCHAR(65)
		,ClassID INT
		,ClassName VARCHAR(65)
		,NatCatID INT
		,LastUpdateTimestamp DATETIME
		)

	INSERT INTO @tempRenameNatItemFamily (
		NatFamilyID
		,NewNatFamilyName
		)
	VALUES (
		20513
		,'Lifestyle - EyeWare'
		)
		,(
		20588
		,'Lifestyle - Pet Toys and Accessories'
		)
		,(
		20371
		,'Packaging & Supplies - Soup Containers and Lids'
		)
		,(
		20374
		,'Packaging & Supplies - Tamper Evident Containers'
		)

	INSERT INTO @tempRenameNatItemCat (
		NatCatID
		,NewNatCatName
		)
	VALUES (
		34768
		,'Pet Toys and Accessories'
		)
		,(
		33951
		,'Soup Containers and Lids'
		)
		,(
		33954
		,'Tamper Evident Containers'
		)

	INSERT INTO @tempRenameNatItemClass (
		ClassID
		,NewClassName
		)
	VALUES (
		46229
		,'Pet Toys and Accessories'
		)
		,(
		43335
		,'Soup Containers and Lids'
		)
		,(
		43338
		,'Tamper Evident Containers'
		)

	UPDATE nif
	SET NatFamilyName = t.NewNatFamilyName
	OUTPUT 'Renamed NatFamilyName' AS Action
		,deleted.NatFamilyName AS OldNatFamilyName
		,inserted.*
	INTO @tempRenameNatItemFamilyResults
	FROM NatItemFamily nif
	JOIN @tempRenameNatItemFamily t ON nif.NatFamilyID = t.NatFamilyID

	UPDATE nic
	SET NatCatName = t.NewNatCatName
	OUTPUT 'Renamed NatCatName' AS Action
		,deleted.NatCatName AS OldNatCatName
		,inserted.*
	INTO @tempRenameNatItemCatResults
	FROM NatItemCat nic
	JOIN @tempRenameNatItemCat t ON nic.NatCatID = t.NatCatID

	UPDATE nicl
	SET ClassName = t.NewClassName
	OUTPUT 'Renamed Class Name' AS Action
		,deleted.ClassName AS OldClassName
		,inserted.*
	INTO @tempRenameNatItemClassResults
	FROM NatItemClass nicl
	JOIN @tempRenameNatItemClass t ON nicl.ClassID = t.ClassID

	SELECT *
	FROM @tempRenameNatItemFamilyResults

	SELECT *
	FROM @tempRenameNatItemCatResults

	SELECT *
	FROM @tempRenameNatItemClassResults
END

--Move Items from one National Class to another
PRINT 'Move Items from one National Class to another'

BEGIN
	DECLARE @tempItemResults TABLE (
		Action NVARCHAR(30)
		,OldClassID INT
		,Item_Key INT
		,ClassID INT
		)

	UPDATE Item
	SET ClassID = 44471
	OUTPUT 'Changed National Class ID' AS Action
		,deleted.ClassID AS OldClassID
		,inserted.Item_Key
		,inserted.ClassID
	INTO @tempItemResults
	WHERE ClassID = 40515

	UPDATE Item
	SET ClassID = 45062
	OUTPUT 'Changed National Class ID' AS Action
		,deleted.ClassID AS OldClassID
		,inserted.Item_Key
		,inserted.ClassID
	INTO @tempItemResults
	WHERE ClassID = 45061

	SELECT *
	FROM @tempItemResults
END

--Delete IRMA records that don't exist in Icon
PRINT 'Delete IRMA records that don''t exist in Icon'

BEGIN
	DECLARE @tempDeleteNatItemFamily TABLE (NatFamilyID INT)
	DECLARE @tempDeleteNatItemCat TABLE (NatCatID INT)
	DECLARE @tempDeleteNatItemClass TABLE (ClassID INT)
	DECLARE @tempDeleteNatItemFamilyResults TABLE (
		Action NVARCHAR(30)
		,NatFamilyID INT
		,NatFamilyName VARCHAR(65)
		,NatSubTeam_No INT
		,SubTeam_No INT
		,LastUpdateTimestamp DATETIME
		)
	DECLARE @tempDeleteNatItemCatResults TABLE (
		Action NVARCHAR(30)
		,NatCatID INT
		,NatCatName VARCHAR(65)
		,NatFamilyID INT
		,LastUpdateTimestamp DATETIME
		)
	DECLARE @tempDeleteNatItemClassResults TABLE (
		Action NVARCHAR(30)
		,ClassID INT
		,ClassName VARCHAR(65)
		,NatCatID INT
		,LastUpdateTimestamp DATETIME
		)

	INSERT INTO @tempDeleteNatItemFamily (NatFamilyID)
	VALUES (20587)
		,(20582)
		,(20584)
		,(20585)

	INSERT INTO @tempDeleteNatItemCat (NatCatID)
	VALUES (30008)
		,(30009)
		,(30010)
		,(30012)
		,(32888)
		,(34763)
		,(34757)
		,(34759)
		,(34761)
		,(34760)
		,(34432)
		,(34431)
		,(34377)
		,(34393)
		,(33697)

	INSERT INTO @tempDeleteNatItemClass (ClassID)
	VALUES (40515)
		,(45061)

	DELETE NatItemClass
	OUTPUT 'Deleted NatItemClass' AS Action
		,deleted.*
	INTO @tempDeleteNatItemClassResults
	WHERE ClassID IN (
			SELECT ClassID
			FROM @tempDeleteNatItemClass
			)

	DELETE NatItemCat
	OUTPUT 'Deleted NatItemCat' AS Action
		,deleted.*
	INTO @tempDeleteNatItemCatResults
	WHERE NatCatID IN (
			SELECT NatCatID
			FROM @tempDeleteNatItemCat
			)

	DELETE NatItemFamily
	OUTPUT 'Deleted NatItemFamily' AS Action
		,deleted.*
	INTO @tempDeleteNatItemFamilyResults
	WHERE NatFamilyID IN (
			SELECT NatFamilyID
			FROM @tempDeleteNatItemFamily
			)

	SELECT *
	FROM @tempDeleteNatItemClassResults

	SELECT *
	FROM @tempDeleteNatItemCatResults

	SELECT *
	FROM @tempDeleteNatItemFamilyResults
END