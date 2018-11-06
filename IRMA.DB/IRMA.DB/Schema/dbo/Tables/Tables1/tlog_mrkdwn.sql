CREATE TABLE [dbo].[tlog_mrkdwn] (
    [store_no]                CHAR (5)        NULL,
    [operator_no]             CHAR (9)        NULL,
    [trans_date]              CHAR (8)        NULL,
    [trans_time]              CHAR (6)        NULL,
    [terminal_no]             CHAR (5)        NULL,
    [transaction_no]          CHAR (10)       NULL,
    [trans_seq_no]            CHAR (5)        NULL,
    [trans_type_code]         CHAR (1)        NULL,
    [trans_void_indic]        CHAR (1)        NULL,
    [upc]                     CHAR (12)       NULL,
    [void_mrkdwn]             CHAR (1)        NULL,
    [auto_mrkdwn]             CHAR (1)        NULL,
    [auto_item_mrkdwn]        CHAR (1)        NULL,
    [auto_dept_mrkdwn]        CHAR (1)        NULL,
    [freq_buyer_mrkdwn]       CHAR (1)        NULL,
    [min_order_mrkdwn]        CHAR (1)        NULL,
    [min_order_adj]           CHAR (1)        NULL,
    [var_price_per_lb_mrkdwn] CHAR (1)        NULL,
    [var_price_per_lb_adj]    CHAR (1)        NULL,
    [dept]                    CHAR (5)        NULL,
    [price]                   DECIMAL (20, 2) NULL,
    [price_rate]              DECIMAL (20, 2) NULL,
    [mrkdwn_amt]              DECIMAL (20, 2) NULL
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[tlog_mrkdwn] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[tlog_mrkdwn] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[tlog_mrkdwn] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tlog_mrkdwn] TO [IRMAReportsRole]
    AS [dbo];

