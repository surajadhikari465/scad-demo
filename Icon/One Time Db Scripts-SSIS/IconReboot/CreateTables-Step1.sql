IF OBJECT_ID('dbo.[IconRebootData]', 'U') IS NOT NULL 
  DROP TABLE dbo.[IconRebootData]; 

IF OBJECT_ID('dbo.[PickListData]', 'U') IS NOT NULL 
  DROP TABLE dbo.PickListData; 

IF OBJECT_ID('dbo.[AttributeCharacterSets]', 'U') IS NOT NULL 
  DROP TABLE dbo.AttributeCharacterSets; 
    
IF OBJECT_ID('dbo.[CharacterSets]', 'U') IS NOT NULL 
  DROP TABLE dbo.CharacterSets; 


IF OBJECT_ID('dbo.[Attributes]', 'U') IS NOT NULL 
  DROP TABLE dbo.Attributes; 

  
  IF OBJECT_ID('dbo.[DataType]', 'U') IS NOT NULL 
  DROP TABLE dbo.DataType; 

  IF OBJECT_ID('dbo.[AttributeGroup]', 'U') IS NOT NULL 
  DROP TABLE dbo.AttributeGroup; 

  IF OBJECT_ID('dbo.[IconRebootTraitCodesData]', 'U') IS NOT NULL 
  DROP TABLE dbo.[IconRebootTraitCodesData]; 

  

GO
CREATE TABLE [dbo].[IconRebootData](
	[NAME] [nvarchar](255) NULL,
	[INFOR_SYSTEM_ID] [nvarchar](255) NULL,
	[EXTERNAL_SYSTEM_ID] [nvarchar](255) NULL,
	[ATTRIBUTE_TYPE] [nvarchar](255) NULL,
	[DATA_TYPE] [nvarchar](255) NULL,
	[DESCRIPTION] [nvarchar](255) NULL,
	[DEFAULT_VALUE] [nvarchar](255) NULL,
	[RULE_TYPE] [nvarchar](255) NULL,
	[RULE_VALUE] [nvarchar](255) NULL,
	[PICKLIST_VALUES] [nvarchar](255) NULL,
	[CHARACTER_SETS] [nvarchar](255) NULL,
	[SPECIAL_CHARACTERS] [nvarchar](255) NULL,
	[REQUIRED_FOR_VALIDATION] [nvarchar](255) NULL,
	[DISPLAY_ORDER] [nvarchar](255) NULL,
	[CREATED_BY] [nvarchar](255) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [nvarchar](255) NULL,
	[MODIFIED_ON] [datetime] NULL,
	[INITIAL_VALUE] [nvarchar](255) NULL,
	[INCREMENT_BY] [nvarchar](255) NULL,
	[INITIAL_MAX] [nvarchar](255) NULL,
	[SELECT_LOWEST_VALUE] [nvarchar](255) NULL,
	[DISPLAY_TYPE] [nvarchar](255) NULL
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[IconRebootTraitCodesData](
[NAME] [nvarchar](255) NULL,
TraitCode [nvarchar](255) NULL
)

CREATE TABLE dbo.DataType

(
	DataTypeId INT IDENTITY (1, 1) NOT NULL,
	DataType NVARCHAR(25),
	CONSTRAINT [PK_DataType] PRIMARY KEY CLUSTERED ([DataTypeId] ASC)
)
CREATE TABLE dbo.AttributeGroup
(
    AttributeGroupId INT IDENTITY (1, 1) NOT NULL,
	AttributeGroupName NVARCHAR(25),
	CONSTRAINT [PK_AttributeGroup] PRIMARY KEY CLUSTERED ([AttributeGroupId] ASC)
)

CREATE TABLE dbo.Attributes
(
	AttributeId INT  IDENTITY (1, 1) NOT NULL,
	DisplayName NVARCHAR(255),
	AttributeName NVARCHAR(255), 
	AttributeGroupId INT NULL,
	HasUniqueValues  BIT NULL,
	Description NVARCHAR(255),
	DefaultValue NVARCHAR(255) NULL,
	RequiredForPublishing BIT NULL,
	SpecialCharactersAllowed NVARCHAR(255) NULL,
	TraitCode NVARCHAR(10),
	DataTypeId INT,
	DisplayOrder Int Null,
	InitialValue INT NULL,
	IncrementBy INT NULL,
	InitialMax INT NULL,
	DisplayType Varchar(255) Null,
	MaxLengthAllowed int null,
	NumberValidationRule NVARCHAR(500) NULL,
	PickList bit NOT NULL CONSTRAINT [Attributes_PickList_DF] DEFAULT (0),
	CONSTRAINT [FK_Attributes_DataType_DataTypeId] FOREIGN KEY ([DataTypeId]) REFERENCES [dbo].[DataType] ([DataTypeId])  ,
	CONSTRAINT [PK_Attributes] PRIMARY KEY CLUSTERED ([AttributeId] ASC),
	CONSTRAINT [FK_Attributes_AttributeType_AttributeGroupId] FOREIGN KEY ([AttributeGroupId]) REFERENCES [dbo].AttributeGroup (AttributeGroupId)  ,
)



CREATE TABLE dbo.PickListData

(
	PickListId INT IDENTITY (1, 1) NOT NULL,
	AttributeId INT,
	PickListValue NVARCHAR(50),
	CONSTRAINT [FK_PickListData_Attributes_AttributeId] FOREIGN KEY ([AttributeId]) REFERENCES [dbo].[Attributes] ([AttributeId]),  
    CONSTRAINT [PK_PickListData] PRIMARY KEY CLUSTERED (PickListId ASC)
)

CREATE TABLE dbo.CharacterSets
(
	CharacterSetId INT IDENTITY (1, 1) NOT NULL,
	Name NVARCHAR(50),
	RegEx NVARCHAR(200),
	CONSTRAINT [PK_CharacterSets] PRIMARY KEY CLUSTERED (CharacterSetId ASC)
)

CREATE TABLE dbo.AttributeCharacterSets

(  AttributeCharacterSetID INT  IDENTITY (1, 1) NOT NULL,
	AttributeId  INT  NOT NULL,
	CharacterSetId INT  NOT NULL,
	CONSTRAINT [PK_AttributeCharacterSets] PRIMARY KEY CLUSTERED (AttributeCharacterSetID ASC),
    CONSTRAINT [FK_AttributeCharacterSets_Attributes_AttributeId] FOREIGN KEY ([AttributeId]) REFERENCES [dbo].[Attributes] ([AttributeId]),
    CONSTRAINT [FK_AttributeCharacterSets_CharacterSets_CharacterSetId] FOREIGN KEY ([CharacterSetId]) REFERENCES [dbo].CharacterSets ([CharacterSetId])   
)







