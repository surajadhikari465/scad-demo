CREATE TYPE infor.ItemAddOrUpdateType AS TABLE(
	ItemId INT NOT NULL,
	ItemTypeId INT NOT NULL,
	ScanCode NVARCHAR(13) NOT NULL,
	ScanCodeTypeId INT NOT NULL,
	InforMessageId UNIQUEIDENTIFIER NOT NULL,
	SequenceId NUMERIC(22, 0) NULL,
  HospitalityItem bit not null default(0),
  KitchenItem bit not null default(0),
  KitchenDescription nvarchar(15),
  ImageURL nvarchar(255)
)
GO