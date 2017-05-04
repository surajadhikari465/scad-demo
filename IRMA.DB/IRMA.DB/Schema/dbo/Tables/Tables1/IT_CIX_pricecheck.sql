CREATE TABLE [dbo].[IT_CIX_pricecheck] (
    [upcno]        CHAR (13) NULL,
    [foodstamp]    CHAR (1)  NULL,
    [tradestamp]   CHAR (1)  NULL,
    [itemdisc]     CHAR (1)  NULL,
    [emp_discount] CHAR (1)  NULL,
    [qtyreq]       CHAR (1)  NULL,
    [coupon]       CHAR (1)  NULL,
    [agecode]      CHAR (2)  NULL,
    [restricted]   CHAR (1)  NULL,
    [scale]        CHAR (1)  NULL,
    [tare]         SMALLINT  NULL,
    [pricereq]     CHAR (1)  NULL,
    [qtyprob]      CHAR (1)  NULL,
    [grplist]      SMALLINT  NULL
);

