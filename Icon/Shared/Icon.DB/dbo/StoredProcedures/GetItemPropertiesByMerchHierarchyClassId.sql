CREATE PROCEDURE [dbo].[GetItemPropertiesByMerchHierarchyClassId] @MerchHierarchyClassId INT
AS
BEGIN
	DECLARE @mfmtraitid INT = (
			SELECT traitID
			FROM Trait
			WHERE traitCode = 'MFM'
			)
		,@nmtraitid INT = (
			SELECT traitID
			FROM Trait
			WHERE traitCode = 'NM'
			)
		,@prhtraitid INT = (
			SELECT traitID
			FROM Trait
			WHERE traitCode = 'PRH'
			)

	SELECT mfm.traitvalue AS FinancialHierarcyClassId
		,nm.traitValue AS NonMerchandiseTraitValue
		,convert(BIT, prh.traitValue) AS ProhibitDiscount
	FROM HierarchyClass
	LEFT JOIN hierarchyclasstrait mfm ON HierarchyClass.hierarchyClassID = mfm.hierarchyClassID
		AND mfm.traitID = @mfmtraitid
	LEFT JOIN hierarchyclasstrait nm ON HierarchyClass.hierarchyClassID = nm.hierarchyClassID
		AND nm.traitID = @nmtraitid
	LEFT JOIN hierarchyclasstrait prh ON HierarchyClass.hierarchyParentClassID = prh.hierarchyClassID
		AND prh.traitID = @prhtraitid
	WHERE HierarchyClass.HIERARCHYID = 1
		AND HierarchyClass.hierarchyClassID = @MerchHierarchyClassId
END