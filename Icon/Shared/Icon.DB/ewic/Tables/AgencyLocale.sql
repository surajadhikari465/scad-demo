create table ewic.AgencyLocale
(
	AgencyId nvarchar(2),
	LocaleId int,
	constraint [PK_ewic.AgencyLocale] primary key clustered ([AgencyId], [LocaleId]),
	constraint [FK_ewic.AgencyLocale_Agency] foreign key ([AgencyId]) references [ewic].[Agency] ([AgencyId]),
	constraint [FK_ewic.AgencyLocale_Locale] foreign key ([LocaleId]) references [dbo].[Locale] ([localeID]),
	constraint [AK_ewic.AgencyLocale_LocaleId] unique nonclustered ([LocaleId])
);