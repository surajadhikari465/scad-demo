CREATE TABLE [dbo].[REGION] (
    [ID]                    INT          IDENTITY (1, 1) NOT NULL,
    [REGION_ABBR]           VARCHAR (5)  NOT NULL,
    [REGION_NAME]           VARCHAR (50) NOT NULL,
    [IS_VISIBLE]            VARCHAR (10) NOT NULL,
    [TimeOffsetFromCentral] SMALLINT     CONSTRAINT [Default_Region_TimeOffsetFromCentral] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_REGION] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'Description', @value = 'Positive or Negative smallint that represents the offset in hours for a region from Central Time. Daylight Savings does not affect this value.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'REGION', @level2type = N'COLUMN', @level2name = N'TimeOffsetFromCentral';

