CREATE TABLE [irma].[PriceChgType] (
    [Region]              NCHAR (2)    NOT NULL,
    [PriceChgTypeID]      TINYINT      NOT NULL,
    [PriceChgTypeDesc]    VARCHAR (20) NOT NULL,
    [Priority]            SMALLINT     NOT NULL,
    [On_Sale]             BIT          CONSTRAINT [DF_PriceChgType_On_Sale] DEFAULT ((1)) NOT NULL,
    [MSRP_Required]       BIT          CONSTRAINT [DF_PriceChgType_MSRP_Required] DEFAULT ((0)) NOT NULL,
    [LineDrive]           BIT          CONSTRAINT [DF_PriceChgType_LineDrive] DEFAULT ((0)) NOT NULL,
    [LastUpdateTimestamp] DATETIME     NULL,
    [Competitive]         BIT          DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_PriceChgType] PRIMARY KEY CLUSTERED ([Region] ASC, [PriceChgTypeID] ASC) WITH (FILLFACTOR = 100)
);

