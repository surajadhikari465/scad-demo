CREATE TABLE [dbo].[tlog_tender] (
    [store_no]           CHAR (5)        NULL,
    [operator_no]        CHAR (9)        NULL,
    [trans_date]         CHAR (8)        NULL,
    [trans_time]         CHAR (6)        NULL,
    [terminal_no]        CHAR (5)        NULL,
    [transaction_no]     CHAR (10)       NULL,
    [trans_seq_no]       CHAR (5)        NULL,
    [trans_type_code]    CHAR (1)        NULL,
    [trans_void_indic]   CHAR (1)        NULL,
    [payment_amt]        DECIMAL (20, 2) NULL,
    [tender_type]        CHAR (2)        NULL,
    [foreign_tender_amt] DECIMAL (20, 2) NULL,
    [approval]           CHAR (9)        NULL,
    [eft_cas]            CHAR (1)        NULL,
    [chng_tender]        CHAR (1)        NULL,
    [void_tender]        CHAR (1)        NULL,
    [offline_tender]     CHAR (1)        NULL,
    [foodstamp_tender]   CHAR (1)        NULL,
    [card_no]            CHAR (26)       NULL,
    [ref_no]             CHAR (10)       NULL,
    [cas_ref_no]         CHAR (10)       NULL,
    [primary_acct]       CHAR (6)        NULL,
    [state_mnemonic]     CHAR (3)        NULL,
    [itemsize_subttl]    DECIMAL (20, 2) NULL,
    [domestic_amt]       DECIMAL (20, 2) NULL,
    [sales_entry_id]     CHAR (10)       NULL
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[tlog_tender] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[tlog_tender] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[tlog_tender] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tlog_tender] TO [IRMAReportsRole]
    AS [dbo];

