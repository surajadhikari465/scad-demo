CREATE TABLE [dbo].[tlog_cmcard] (
    [store_no]                   CHAR (5)     NULL,
    [operator_no]                CHAR (9)     NULL,
    [trans_date]                 DATETIME     NULL,
    [time]                       ROWVERSION   NULL,
    [terminal_no]                NUMERIC (18) NULL,
    [transaction_no]             NUMERIC (18) NULL,
    [transaction_seq_no]         NCHAR (10)   NULL,
    [transaction_type_code]      CHAR (1)     NULL,
    [transaction_void_indicator] CHAR (1)     NULL,
    [key_input_type]             NUMERIC (18) NULL,
    [key_data_input]             CHAR (26)    NULL,
    [membership_id]              CHAR (26)    NULL,
    [household_id]               CHAR (26)    NULL,
    [member_level]               NUMERIC (18) NULL,
    [flags]                      NUMERIC (18) NULL
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[tlog_cmcard] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[tlog_cmcard] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[tlog_cmcard] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tlog_cmcard] TO [IRMAReportsRole]
    AS [dbo];

