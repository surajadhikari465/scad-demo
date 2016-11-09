CREATE TABLE [dbo].[price_max] (
    [upcno]         CHAR (13)      NULL,
    [user_def1]     VARCHAR (50)   NULL,
    [foodstamp]     CHAR (1)       NULL,
    [tradestamp]    CHAR (1)       NULL,
    [itemdisc]      CHAR (1)       NULL,
    [emp_discount]  CHAR (1)       NULL,
    [qtyreq]        CHAR (1)       NULL,
    [coupon]        CHAR (1)       NULL,
    [agecode]       CHAR (2)       NULL,
    [restricted]    CHAR (1)       NULL,
    [scale]         CHAR (1)       NULL,
    [tare]          SMALLINT       NULL,
    [pricereq]      CHAR (1)       NULL,
    [qtyprob]       CHAR (1)       NULL,
    [grplist]       SMALLINT       NULL,
    [linkcode]      CHAR (13)      NULL,
    [tagtype]       INT            NULL,
    [bonus]         SMALLINT       NULL,
    [aisleloc]      CHAR (6)       NULL,
    [atdamtfl]      CHAR (1)       NULL,
    [LockAuth]      CHAR (4)       NULL,
    [LastDate]      SMALLDATETIME  NULL,
    [PriceReqPrice] NUMERIC (6, 2) NULL
);


GO
CREATE NONCLUSTERED INDEX [pm_u]
    ON [dbo].[price_max]([upcno] ASC);

