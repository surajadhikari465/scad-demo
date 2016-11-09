create table ewic.Mapping
(
	AgencyId nvarchar(2),
	AplScanCode nvarchar(15),
	ScanCodeId int,
	constraint [PK_ewic.Mapping] primary key clustered ([AgencyId], [AplScanCode], [ScanCodeId]),
	constraint [FK_ewic.Mapping_Agency] foreign key ([AgencyId]) references [ewic].[Agency] ([AgencyId]),
	constraint [FK_ewic.Mapping_AuthorizedProductList] foreign key ([AgencyId],[AplScanCode]) references [ewic].[AuthorizedProductList] ([AgencyId],[ScanCode]),
	constraint [FK_ewic.Mapping_ScanCode] foreign key ([ScanCodeId]) references [dbo].[ScanCode] ([scanCodeID])
);