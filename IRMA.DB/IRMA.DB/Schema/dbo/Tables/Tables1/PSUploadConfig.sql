CREATE TABLE [dbo].[PSUploadConfig] (
    [Region_Code]       VARCHAR (3) NOT NULL,
    [PS_OPRID]          VARCHAR (8) NULL,
    [UploadDelay_Sun]   INT         DEFAULT ((0)) NULL,
    [UploadDelay_Mon]   INT         DEFAULT ((0)) NULL,
    [UploadDelay_Tues]  INT         DEFAULT ((0)) NULL,
    [UploadDelay_Wed]   INT         DEFAULT ((0)) NULL,
    [UploadDelay_Thurs] INT         DEFAULT ((0)) NULL,
    [UploadDelay_Fri]   INT         DEFAULT ((0)) NULL,
    [UploadDelay_Sat]   INT         DEFAULT ((0)) NULL,
    CONSTRAINT [PK_PSUploadConfig_Region_Code] PRIMARY KEY CLUSTERED ([Region_Code] ASC)
);

