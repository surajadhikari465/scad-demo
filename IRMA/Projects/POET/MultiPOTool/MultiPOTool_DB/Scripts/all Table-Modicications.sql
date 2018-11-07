-- =============================================
-- Script Template
-- =============================================
alter table poHeader add IRMAPONumber int null

alter table PONumber add Used bit null

  
  alter table Regions add CentralTimeZoneOffset int null

  ALTER TABLE [dbo].[PONumber] ADD  CONSTRAINT [DF_PONumber_Used]  DEFAULT ((0)) FOR [Used]

  alter table PoHeader add ReasonCode int null
  alter table PoItem add ReasonCode int null

GO

--drop helplinks
/****** Object:  Table [dbo].[HelpLinks]    Script Date: 06/02/2011 11:26:57 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HelpLinks]') AND type in (N'U'))
DROP TABLE [dbo].[HelpLinks]
GO

--create helplinks
CREATE TABLE [dbo].[HelpLinks](
	[HelpLinksID] [int] IDENTITY(1,1) NOT NULL,
	[LinkDescription] [varchar](max) NULL,
	[LinkURL] [varchar](max) NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedUserID] [int] NULL,
	[OrderOfAppearance] [int] NULL,
 CONSTRAINT [PK_HelpLinks] PRIMARY KEY CLUSTERED 
(
	[HelpLinksID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

--insert helplinks

