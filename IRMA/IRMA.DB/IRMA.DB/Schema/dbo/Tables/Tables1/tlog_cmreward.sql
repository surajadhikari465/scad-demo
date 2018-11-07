CREATE TABLE [dbo].[tlog_cmreward] (
    [store_no]                     CHAR (5)        NULL,
    [operator_no]                  CHAR (9)        NULL,
    [trans_date]                   DATETIME        NULL,
    [time]                         ROWVERSION      NULL,
    [terminal_no]                  NUMERIC (18)    NULL,
    [transaction_no]               NUMERIC (18)    NULL,
    [transaction_seq_no]           NUMERIC (18)    NULL,
    [transaction_type_code]        CHAR (1)        NULL,
    [transaction_void_indicator]   CHAR (1)        NULL,
    [reward_type]                  NUMERIC (18)    NULL,
    [reward_category]              NUMERIC (18)    NULL,
    [reward_id]                    NUMERIC (18)    NULL,
    [associated_entry_id]          NUMERIC (18)    NULL,
    [item_code]                    NUMERIC (18)    NULL,
    [promotion_code]               NUMERIC (18)    NULL,
    [condition_reward_code]        NUMERIC (18)    NULL,
    [condition_reward_subcode]     NUMERIC (18)    NULL,
    [store_created_reward_flag]    CHAR (1)        NULL,
    [store_mod_reward_flag]        CHAR (1)        NULL,
    [membership_req_reward_flag]   CHAR (1)        NULL,
    [vendor_sponsored_reward_flag] CHAR (1)        NULL,
    [reward_detail_data_flag]      CHAR (1)        NULL,
    [reward_summary_data_flag]     CHAR (1)        NULL,
    [reward_level]                 NUMERIC (18)    NULL,
    [department]                   NUMERIC (18)    NULL,
    [itemizers]                    NUMERIC (18)    NULL,
    [ext_reward_amount]            NUMERIC (18)    NULL,
    [base_reward_amount]           NUMERIC (18)    NULL,
    [used_qty]                     NUMERIC (18)    NULL,
    [used_weight]                  NUMERIC (18)    NULL,
    [used_dollars]                 NUMERIC (18, 2) NULL,
    [custom_offer_id]              NUMERIC (18)    NULL
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[tlog_cmreward] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[tlog_cmreward] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[tlog_cmreward] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tlog_cmreward] TO [IRMAReportsRole]
    AS [dbo];

