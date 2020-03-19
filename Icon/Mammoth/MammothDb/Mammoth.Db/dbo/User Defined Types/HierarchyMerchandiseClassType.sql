CREATE TYPE dbo.HierarchyMerchandiseClassType AS TABLE(
  SegmentHCID INT,
  FamilyHCID INT,
  ClassHCID INT,
  BrickHCID INT,
  SubBrickHCID INT
)
GO