CREATE TABLE [dbo].[tlog_cmreserve] (
    [store_no]                   CHAR (5)     NULL,
    [operator_no]                CHAR (9)     NULL,
    [trans_date]                 DATETIME     NULL,
    [time]                       ROWVERSION   NULL,
    [terminal_no]                NUMERIC (18) NULL,
    [transaction_no]             NUMERIC (18) NULL,
    [transaction_seq_no]         NUMERIC (18) NULL,
    [transaction_type_code]      CHAR (1)     NULL,
    [transaction_void_indicator] CHAR (1)     NULL,
    [reserve_1]                  NUMERIC (18) NULL,
    [reserve_2]                  NUMERIC (18) NULL,
    [reserve_3]                  NUMERIC (18) NULL,
    [reserve_4]                  NUMERIC (18) NULL,
    [reserve_5]                  NUMERIC (18) NULL,
    [reserve_6]                  NUMERIC (18) NULL,
    [reserve_7]                  NUMERIC (18) NULL,
    [reserve_8]                  NUMERIC (18) NULL,
    [reserve_9]                  NUMERIC (18) NULL,
    [reserve_10]                 NUMERIC (18) NULL,
    [reserve_11]                 NUMERIC (18) NULL,
    [reserve_12]                 NUMERIC (18) NULL,
    [reserve_13]                 NUMERIC (18) NULL,
    [reserve_14]                 NUMERIC (18) NULL,
    [reserve_15]                 NUMERIC (18) NULL,
    [reserve_16]                 NUMERIC (18) NULL,
    [reserve_17]                 NUMERIC (18) NULL,
    [reserve_18]                 NUMERIC (18) NULL,
    [reserve_19]                 NUMERIC (18) NULL,
    [reserve_20]                 NUMERIC (18) NULL
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[tlog_cmreserve] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[tlog_cmreserve] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[tlog_cmreserve] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tlog_cmreserve] TO [IRMAReportsRole]
    AS [dbo];

