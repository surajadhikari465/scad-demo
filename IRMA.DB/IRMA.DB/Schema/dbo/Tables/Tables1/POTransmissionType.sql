CREATE TABLE [dbo].[POTransmissionType] (
    [POTransmissionTypeID] INT          IDENTITY (1, 1) NOT NULL,
    [Description]          VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_POTransmissionType] PRIMARY KEY CLUSTERED ([POTransmissionTypeID] ASC)
);

