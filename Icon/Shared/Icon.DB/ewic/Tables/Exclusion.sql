create table ewic.Exclusion
(
	AgencyId nvarchar(2),
	ScanCodeId int,
	constraint [PK_ewic.Exclusion] primary key clustered ([AgencyId], [ScanCodeId]),
	constraint [FK_ewic.Exclusion_Agency] foreign key ([AgencyId]) references [ewic].[Agency] ([AgencyId]),
	constraint [FK_ewic.Exclusion_ScanCode] foreign key ([ScanCodeId]) references [dbo].[ScanCode] ([scanCodeID])
);