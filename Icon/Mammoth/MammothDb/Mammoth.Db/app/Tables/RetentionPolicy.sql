CREATE TABLE [app].[RetentionPolicy](

[RetentionPolicyId] [int] IDENTITY(1,1) NOT NULL,

[Database] [nvarchar](16) NOT NULL,

[Schema] [nvarchar](50) NOT NULL,

[Table] [nvarchar](64) NOT NULL,

[ReferenceColumn] [nvarchar](50) NOT NULL,

[DaysToKeep] [int] NOT NULL,

[TimeToStart] [int] NOT NULL,

[TimeToEnd] [int] NOT NULL,

[IncludedInDailyPurge] [bit] NOT NULL,

[DailyPurgeCompleted] [bit] NOT NULL,

[PurgeJobName] [nvarchar](50) NOT NULL,

[LastPurgedDateTime] [datetime] NULL,

 CONSTRAINT [PK_RetentionPolicy] PRIMARY KEY CLUSTERED 

(

[RetentionPolicyId] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]

) ON [PRIMARY]