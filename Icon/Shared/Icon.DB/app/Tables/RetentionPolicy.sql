CREATE TABLE [app].[RetentionPolicy](
	[RetentionPolicyId] [int] IDENTITY(1,1) NOT NULL,
	[Server] [nvarchar](50) NULL,
	[Database] [nvarchar](16) NULL,
	[Schema] [nvarchar](50) NULL,
	[Table] [nvarchar](64) NULL,
	[DaysToKeep] [int] NULL,
 CONSTRAINT [PK_RetentionPolicy] PRIMARY KEY CLUSTERED 
(
	[RetentionPolicyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
