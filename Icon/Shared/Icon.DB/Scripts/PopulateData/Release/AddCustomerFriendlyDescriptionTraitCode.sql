SET IDENTITY_INSERT [dbo].[Trait] ON
GO

DECLARE @TraitID int
DECLARE @TraitGroupID int

SET @TraitID = (SELECT MAX(TraitID) FROM [dbo].[Trait])
SET @TraitID = @TraitID+1

SET @TraitGroupID = (SELECT TraitGroupID FROM [dbo].[Trait] WHERE traitCode= 'PRD')

IF NOT EXISTS (SELECT 1 FROM [dbo].[Trait] WHERE traitCode= 'CFD')
INSERT [dbo].[Trait] (
						[traitID], 
						[traitCode], 
						[traitPattern], 
						[traitDesc], 
						[traitGroupID]) 
						VALUES 
						(
						  @TraitID, 
						  N'CFD',
						  N'^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,60}$',
						  N'Customer Friendly Description', 
						  @TraitGroupID
						 )
GO

SET IDENTITY_INSERT [dbo].[Trait] OFF
GO