﻿CREATE TABLE [dbo].[cxsprc2d] (
    [priclnk]           INT             NULL,
    [upcno]             VARCHAR (13)    NULL,
    [pzone]             SMALLINT        NULL,
    [store]             SMALLINT        NULL,
    [spricend]          DATETIME        NULL,
    [fustopric]         VARCHAR (10)    NULL,
    [nanetotfl]         VARCHAR (1)     NULL,
    [frbuydisc]         VARCHAR (1)     NULL,
    [frshopmlt]         SMALLINT        NULL,
    [velcode]           SMALLINT        NULL,
    [splprflag]         VARCHAR (1)     NULL,
    [splprmult]         SMALLINT        NULL,
    [splprice]          DECIMAL (6, 2)  NULL,
    [scancomm]          INT             NULL,
    [limqprice]         VARCHAR (10)    NULL,
    [coupcode]          SMALLINT        NULL,
    [subdept]           SMALLINT        NULL,
    [altprflg]          VARCHAR (1)     NULL,
    [itypeflg]          SMALLINT        NULL,
    [bookpr]            VARCHAR (10)    NULL,
    [minckage]          VARCHAR (1)     NULL,
    [tax5]              VARCHAR (1)     NULL,
    [tax6]              VARCHAR (1)     NULL,
    [procpmlt]          VARCHAR (1)     NULL,
    [ckrprovrd]         VARCHAR (1)     NULL,
    [lnktodep]          VARCHAR (1)     NULL,
    [wicflag]           VARCHAR (1)     NULL,
    [grplist]           SMALLINT        NULL,
    [xgrplist]          VARCHAR (1)     NULL,
    [prohidisc]         VARCHAR (1)     NULL,
    [eleccoup]          VARCHAR (1)     NULL,
    [si9]               VARCHAR (1)     NULL,
    [si10]              VARCHAR (1)     NULL,
    [si11]              VARCHAR (1)     NULL,
    [si12]              VARCHAR (1)     NULL,
    [si13]              VARCHAR (1)     NULL,
    [si14]              VARCHAR (1)     NULL,
    [resint1]           INT             NULL,
    [resint2]           INT             NULL,
    [si15]              VARCHAR (1)     NULL,
    [si16]              VARCHAR (1)     NULL,
    [si17]              VARCHAR (1)     NULL,
    [si18]              VARCHAR (1)     NULL,
    [si19]              VARCHAR (1)     NULL,
    [si20]              VARCHAR (1)     NULL,
    [last_pr_chg_user]  VARCHAR (20)    NULL,
    [ad_track_id]       VARCHAR (8)     NULL,
    [item_sc_flag]      VARCHAR (1)     NULL,
    [tran_sc_flag]      VARCHAR (1)     NULL,
    [tagtype]           VARCHAR (3)     NULL,
    [tagqty]            SMALLINT        NULL,
    [sale_start_time]   DATETIME        NULL,
    [sale_end_time]     DATETIME        NULL,
    [auth_sale]         VARCHAR (1)     NULL,
    [ineligible_flag]   VARCHAR (1)     NULL,
    [pren_wic_flag]     VARCHAR (1)     NULL,
    [tran_disc_flag]    VARCHAR (1)     NULL,
    [no_coupon_flag]    VARCHAR (1)     NULL,
    [no_coup_mult_flag] VARCHAR (1)     NULL,
    [ad_level]          SMALLINT        NULL,
    [prsdisc5]          VARCHAR (1)     NULL,
    [freq_shop_status]  SMALLINT        NULL,
    [item_type]         SMALLINT        NULL,
    [weight]            DECIMAL (10, 3) NULL
);

