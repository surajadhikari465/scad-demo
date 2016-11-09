create table ewic.AuthorizedProductList
(
	AgencyId nvarchar(2),
	ScanCode nvarchar(15),
	ItemDescription nvarchar(50) null,
	UnitOfMeasure nvarchar(6) null,
	PackageSize decimal(5,2) null,
	BenefitQuantity decimal(5,2) null,
	BenefitUnitDescription nvarchar(6) null,
	ItemPrice decimal(6,2) null,
	PriceType nvarchar(2) null,
	InsertDate datetime2(7) not null constraint DF_InsertDate default sysdatetime(),
	ModifiedDate datetime2(7) null,
	constraint [PK_ewic.AuthorizedProductList] primary key clustered ([AgencyId],[ScanCode]),
	constraint [FK_ewic.AuthorizedProductList_Agency] foreign key ([AgencyId]) references [ewic].[Agency] ([AgencyId])
);