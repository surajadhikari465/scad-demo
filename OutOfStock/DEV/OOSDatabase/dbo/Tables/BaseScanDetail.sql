/*
When this was written, it was checked into Azure repo here:
https://dev.azure.com/wholefoods/Supply%20Chain%20Application%20Development/_git/SCAD?path=/OutOfStock/DEV/OOSDatabase/dbo/Tables/BaseScanDetail.sql&version=GBmaster

Main tech doc(s) here: https://dev.azure.com/wholefoods/Supply%20Chain%20Application%20Development/_git/SCAD?path=/OutOfStock/DEV/OOSDatabase/_documentation/
*/
create table BaseScanDetail (
	BaseScanDetailId bigint identity(1,1) not null,
	RegionAbbr varchar(5) not null,
	StoreAbbr varchar(10) not null,
	PS_BU varchar(10) not null,
	OffsetCorrectedScanDate datetime not null,
	UPC varchar(25) not null,
	InsertDate datetime not null CONSTRAINT DF_BaseScanDetail_InsertDate default getdate(),
CONSTRAINT [PK_BaseScanDetailId] PRIMARY KEY CLUSTERED (BaseScanDetailId asc),
CONSTRAINT [UQ_BaseScanDetail_AllFields] UNIQUE(RegionAbbr, StoreAbbr, PS_BU, OffsetCorrectedScanDate, UPC)
);

go
CREATE NONCLUSTERED INDEX [IX_OffsetCorrectedScanDate] ON [dbo].[BaseScanDetail]
(
	[OffsetCorrectedScanDate] ASC
);

