CREATE TABLE [dbo].[tlog_discnt] (
    [store_no]         CHAR (5)        NULL,
    [operator_no]      CHAR (9)        NULL,
    [trans_date]       CHAR (8)        NULL,
    [trans_time]       CHAR (6)        NULL,
    [terminal_no]      CHAR (5)        NULL,
    [transaction_no]   CHAR (10)       NULL,
    [trans_seq_no]     CHAR (5)        NULL,
    [trans_type_code]  CHAR (1)        NULL,
    [trans_void_indic] CHAR (1)        NULL,
    [trans_group]      CHAR (5)        NULL,
    [disc_void_flag]   CHAR (1)        NULL,
    [disc_rate]        DECIMAL (20, 2) NULL,
    [disc_amt]         DECIMAL (20, 2) NULL,
    [reason]           CHAR (5)        NULL,
    [emp_no]           CHAR (11)       NULL
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[tlog_discnt] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[tlog_discnt] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[tlog_discnt] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tlog_discnt] TO [IRMAReportsRole]
    AS [dbo];

