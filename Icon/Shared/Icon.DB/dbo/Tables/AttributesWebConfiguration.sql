CREATE TABLE dbo.AttributesWebConfiguration
(
	AttributesWebConfigurationId INT IDENTITY(1, 1) NOT NULL,
	AttributeId INT NOT NULL,
	GridColumnWidth INT NOT NULL,
	CharacterSetRegexPattern NVARCHAR(255) NULL,
	IsReadOnly BIT NOT NULL CONSTRAINT DF_AttributesWebConfigurationId_ReadOnly DEFAULT 0,
	CONSTRAINT PK_AttributesWebConfiguration_AttributesWebConfigurationId PRIMARY KEY CLUSTERED (AttributesWebConfigurationId ASC),
	CONSTRAINT FK_AttributesWebConfiguration_AttributeId FOREIGN KEY (AttributeId) REFERENCES dbo.Attributes(AttributeId) ON DELETE CASCADE,
	CONSTRAINT UC_AttributesWebConfiguration_AttributeId UNIQUE (AttributeId),
)