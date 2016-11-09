CREATE TABLE [dbo].[tlog_item] (
    [store_no]             CHAR (5)        NULL,
    [operator_no]          CHAR (9)        NULL,
    [trans_date]           CHAR (8)        NULL,
    [trans_time]           CHAR (6)        NULL,
    [terminal_no]          CHAR (5)        NULL,
    [transaction_no]       CHAR (10)       NULL,
    [trans_seq_no]         CHAR (5)        NULL,
    [trans_type_code]      CHAR (1)        NULL,
    [trans_void_indic]     CHAR (1)        NULL,
    [item_code]            CHAR (12)       NULL,
    [dept]                 CHAR (5)        NULL,
    [extended_price]       DECIMAL (20, 2) NULL,
    [base_price]           DECIMAL (20, 2) NULL,
    [deal_price]           DECIMAL (20, 2) NULL,
    [deal_qty]             CHAR (5)        NULL,
    [price_method]         CHAR (2)        NULL,
    [weight]               DECIMAL (10, 3) NULL,
    [qty]                  NUMERIC (18)    NULL,
    [min_order_qty]        CHAR (5)        NULL,
    [alt_price]            DECIMAL (20, 2) NULL,
    [alt_qty]              CHAR (5)        NULL,
    [item_type]            CHAR (2)        NULL,
    [scale]                CHAR (1)        NULL,
    [price_enter]          CHAR (1)        NULL,
    [price_req]            CHAR (1)        NULL,
    [log_item_exception]   CHAR (1)        NULL,
    [item_code_from_alias] CHAR (1)        NULL,
    [no_movement]          CHAR (1)        NULL,
    [emp_disc]             CHAR (1)        NULL,
    [keyed_price]          CHAR (1)        NULL,
    [non_disc]             CHAR (1)        NULL,
    [wgt_qty]              CHAR (1)        NULL,
    [item_void]            CHAR (1)        NULL,
    [refund_entered]       CHAR (1)        NULL,
    [log_exc_condition]    CHAR (1)        NULL,
    [proh_multi_coupon]    CHAR (1)        NULL,
    [auto_mrkdwn]          CHAR (1)        NULL,
    [void_all_item]        CHAR (1)        NULL,
    [err_correct_void]     CHAR (1)        NULL,
    [nsc2_priced]          CHAR (1)        NULL,
    [sale_item]            CHAR (1)        NULL,
    [manual_wgt]           CHAR (1)        NULL,
    [tax1]                 CHAR (1)        NULL,
    [tax2]                 CHAR (1)        NULL,
    [tax3]                 CHAR (1)        NULL,
    [tax4]                 CHAR (1)        NULL,
    [foodstamp]            CHAR (1)        NULL,
    [exchange]             CHAR (1)        NULL,
    [chargeable]           CHAR (1)        NULL,
    [ebt]                  CHAR (1)        NULL,
    [access_charge]        CHAR (1)        NULL,
    [item_6]               CHAR (1)        NULL,
    [item_7]               CHAR (1)        NULL,
    [item_8]               CHAR (1)        NULL,
    [item_9]               CHAR (1)        NULL,
    [fam_1]                CHAR (5)        NULL,
    [fam_2]                CHAR (5)        NULL,
    [fam_3]                CHAR (5)        NULL,
    [rpt_code]             CHAR (3)        NULL,
    [store_ord_code]       CHAR (10)       NULL,
    [access_code]          CHAR (2)        NULL,
    [mix_match_code]       CHAR (5)        NULL,
    [pace_setter_code]     CHAR (3)        NULL,
    [return_reason]        CHAR (3)        NULL,
    [disc_val]             DECIMAL (20, 2) NULL,
    [mrkdwn_amt]           DECIMAL (20, 2) NULL,
    [salable_media]        CHAR (3)        NULL,
    [nsc5_coupon_cnt]      CHAR (5)        NULL,
    [manuf_coupon_cnt]     CHAR (5)        NULL,
    [dept_manuf_cnt]       CHAR (5)        NULL,
    [store_coupon_cnt]     CHAR (5)        NULL,
    [dept_store_cnt]       CHAR (5)        NULL,
    [nsc5_err_type]        CHAR (2)        NULL,
    [sale_price]           DECIMAL (20, 2) NULL,
    [sale_deal]            CHAR (5)        NULL,
    [sale_entry_id]        CHAR (10)       NULL,
    [price_change_type]    CHAR (2)        NULL,
    [plu_condiment_list]   CHAR (5)        NULL,
    [condiment_list1]      CHAR (5)        NULL,
    [condiment_list2]      CHAR (5)        NULL,
    [max_pim_xprice]       CHAR (10)       NULL,
    [upc_128_nsc_code]     CHAR (4)        NULL,
    [upc_128_hhid]         CHAR (8)        NULL,
    [upc_128_expire_yr]    CHAR (2)        NULL,
    [upc_128_expire_mnth]  CHAR (2)        NULL,
    [upc_128_offer_code]   CHAR (5)        NULL
);




GO
GRANT DELETE
    ON OBJECT::[dbo].[tlog_item] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[tlog_item] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[tlog_item] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tlog_item] TO [IRMAReportsRole]
    AS [dbo];


GO
CREATE NONCLUSTERED INDEX [IX_tlog_item_Trans_Date]
    ON [dbo].[tlog_item]([trans_date] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_tlog_item_Store]
    ON [dbo].[tlog_item]([store_no] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_tlog_item_Item_Code]
    ON [dbo].[tlog_item]([item_code] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_tlog_item_Dept]
    ON [dbo].[tlog_item]([dept] ASC);

