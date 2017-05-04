CREATE TABLE [dbo].[dtproperties] (
    [id]       INT            IDENTITY (1, 1) NOT NULL,
    [objectid] INT            NULL,
    [property] VARCHAR (64)   NOT NULL,
    [value]    VARCHAR (255)  NULL,
    [lvalue]   IMAGE          NULL,
    [version]  INT            CONSTRAINT [DF__dtpropert__versi__39AE55D6] DEFAULT ((0)) NOT NULL,
    [uvalue]   NVARCHAR (255) NULL,
    CONSTRAINT [pk_dtproperties] PRIMARY KEY CLUSTERED ([id] ASC, [property] ASC)
);

