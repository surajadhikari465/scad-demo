CREATE TABLE [dbo].[Status] (
    [StatusID]          INT            IDENTITY (1, 1) NOT NULL,
    [StatusCode]        NVARCHAR (3)   NOT NULL,
    [StatusDescription] NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_Status] PRIMARY KEY CLUSTERED ([StatusID] ASC)
);




