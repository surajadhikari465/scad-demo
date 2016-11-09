CREATE TABLE [dbo].[woodruff_kehe] (
    [upc#]        VARCHAR (8000) NULL,
    [description] VARCHAR (8000) NULL,
    [pack]        VARCHAR (8000) NULL,
    [Col004]      VARCHAR (8000) NULL,
    [Col005]      VARCHAR (8000) NULL,
    [Col006]      VARCHAR (8000) NULL,
    [vendor]      VARCHAR (8000) NULL,
    [Col008]      VARCHAR (8000) NULL,
    [qty]         VARCHAR (8000) NULL
);




GO
GRANT SELECT
    ON OBJECT::[dbo].[woodruff_kehe] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[woodruff_kehe] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[woodruff_kehe] TO [IRMAReportsRole]
    AS [dbo];

