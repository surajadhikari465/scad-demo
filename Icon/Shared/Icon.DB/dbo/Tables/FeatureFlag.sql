CREATE TABLE [dbo].[FeatureFlag] (
	 FeatureFlagId INT NOT NULL IDENTITY
	,FlagName NVARCHAR(255) NOT NULL
	,Enabled BIT NOT NULL
	,Description NVARCHAR(255) NOT NULL
	,CreatedDateUtc DATETIME CONSTRAINT [FeatureFlag_CreatedDateUtc_DF] DEFAULT (GETUTCDATE()) NOT NULL
	,LastModifiedDateUtc DATETIME NULL
	,LastModifiedBy NVARCHAR(100) NULL
	,CONSTRAINT [PK_FeatureFlag] PRIMARY KEY CLUSTERED (FeatureFlagId ASC),
	 CONSTRAINT UC_FeatureFlag_FlagName UNIQUE (FlagName)
	)
GO