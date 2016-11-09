CREATE TYPE [app].[PLUMapImportType] AS TABLE (
    [brandname]          NVARCHAR (100) NULL,
    [productdescription] NVARCHAR (100) NULL,
    [nationalPLU]        NVARCHAR (11)  NOT NULL,
    [flPLU]              NVARCHAR (11)  NULL,
    [maPLU]              NVARCHAR (11)  NULL,
    [mwPLU]              NVARCHAR (11)  NULL,
    [naPLU]              NVARCHAR (11)  NULL,
    [ncPLU]              NVARCHAR (11)  NULL,
    [nePLU]              NVARCHAR (11)  NULL,
    [pnPLU]              NVARCHAR (11)  NULL,
    [rmPLU]              NVARCHAR (11)  NULL,
    [soPLU]              NVARCHAR (11)  NULL,
    [spPLU]              NVARCHAR (11)  NULL,
    [swPLU]              NVARCHAR (11)  NULL,
    [ukPLU]              NVARCHAR (11)  NULL);

