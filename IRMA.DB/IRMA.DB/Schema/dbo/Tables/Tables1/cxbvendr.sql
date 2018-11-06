CREATE TABLE [dbo].[cxbvendr] (
    [vendno]      CHAR (8)       NULL,
    [vendname]    CHAR (24)      NULL,
    [address1]    CHAR (20)      NULL,
    [address2]    CHAR (20)      NULL,
    [city]        CHAR (20)      NULL,
    [state]       CHAR (2)       NULL,
    [zipcode]     CHAR (9)       NULL,
    [phone]       CHAR (10)      NULL,
    [contact]     CHAR (20)      NULL,
    [vendterms]   NUMERIC (6, 3) NULL,
    [netterms]    CHAR (2)       NULL,
    [discount]    NUMERIC (6, 3) NULL,
    [category]    CHAR (6)       NULL,
    [noitems]     SMALLINT       NULL,
    [commid]      CHAR (10)      NULL,
    [dunsid]      CHAR (9)       NULL,
    [ppdisc]      CHAR (2)       NULL,
    [cost_method] INT            NULL
);

