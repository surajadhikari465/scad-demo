/* Title : TFS 5898 - Trait Insert POS Dept, Team Number, Team Name
 * Description: db update to add POSDept, Team Number, Team Name
 * Author: Benjamin Loving
 * Date: 12/3/2014
 */

DECLARE @traitGroupID INT
SELECT @traitGroupID = (SELECT traitGroupID FROM dbo.traitgroup WHERE traitGroupDesc = 'Hierarchy Class Traits')

-- POS Department
IF NOT EXISTS (SELECT * FROM dbo.Trait WHERE traitCode = 'PDN' OR traitDesc = 'POS Department Number')
BEGIN
	INSERT INTO dbo.Trait (traitCode, traitPattern, traitDesc, traitGroupID)
	VALUES ('PDN', '^[0-9_]*$', 'POS Department Number', @traitGroupID);
	PRINT 'Trait ''POS Department Number'' inserted successfully';
END
ELSE
BEGIN
	PRINT 'Record already exists where traitCode = ''PDN'' or traitDesc = ''POS Department Number''';
END

-- Team Number
IF NOT EXISTS (SELECT * FROM dbo.Trait WHERE traitCode = 'TNO' OR traitDesc = 'Team Number')
BEGIN
	INSERT INTO dbo.Trait (traitCode, traitPattern, traitDesc, traitGroupID)
	VALUES ('NUM', '^[0-9_]*$', 'Team Number', @traitGroupID);
	PRINT 'Trait ''Team Number'' inserted successfully';
END
ELSE
BEGIN
	PRINT 'Record already exists where traitCode = ''NUM'' or traitDesc = ''Team Number''';
END

-- Team Name
IF NOT EXISTS (SELECT * FROM dbo.Trait WHERE traitCode = 'NAM' OR traitDesc = 'Team Name')
BEGIN
	INSERT INTO dbo.Trait (traitCode, traitPattern, traitDesc, traitGroupID)
	VALUES ('NAM', '^[a-zA-Z0-9_]*$', 'Team Name', @traitGroupID);
	PRINT 'Trait ''Team Name'' inserted successfully';
END
ELSE
BEGIN

	PRINT 'Record already exists where traitCode = ''NAM'' or traitDesc = ''Team Name''';
END
GO