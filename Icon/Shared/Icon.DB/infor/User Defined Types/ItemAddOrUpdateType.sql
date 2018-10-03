CREATE TYPE infor.ItemAddOrUpdateType AS TABLE(
	ItemId INT NOT NULL,
	ItemTypeId INT NOT NULL,
	ScanCode NVARCHAR(13) NOT NULL,
	ScanCodeTypeId INT NOT NULL,
	InforMessageId UNIQUEIDENTIFIER NOT NULL,
	SequenceId NUMERIC(22, 0) NULL,
  HospitalityItem bit null,
  KitchenItem bit null,
  KitchenDescription nvarchar(15) null,
  ImageURL nvarchar(255) null
)
GO