CREATE TABLE [dbo].[tlog_taxrec] (
    [store_no]         CHAR (5)        NULL,
    [operator_no]      CHAR (9)        NULL,
    [trans_date]       CHAR (8)        NULL,
    [trans_time]       CHAR (6)        NULL,
    [terminal_no]      CHAR (5)        NULL,
    [transaction_no]   CHAR (10)       NULL,
    [trans_seq_no]     CHAR (5)        NULL,
    [trans_type_code]  CHAR (1)        NULL,
    [trans_void_indic] CHAR (1)        NULL,
    [tax_amt1]         DECIMAL (20, 2) NULL,
    [tax_gross1]       DECIMAL (20, 2) NULL,
    [tax_amt2]         DECIMAL (20, 2) NULL,
    [tax_gross2]       DECIMAL (20, 2) NULL,
    [tax_amt3]         DECIMAL (20, 2) NULL,
    [tax_gross3]       DECIMAL (20, 2) NULL,
    [tax_amt4]         DECIMAL (20, 2) NULL,
    [tax_gross4]       DECIMAL (20, 2) NULL
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[tlog_taxrec] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[tlog_taxrec] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[tlog_taxrec] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tlog_taxrec] TO [IRMAReportsRole]
    AS [dbo];

