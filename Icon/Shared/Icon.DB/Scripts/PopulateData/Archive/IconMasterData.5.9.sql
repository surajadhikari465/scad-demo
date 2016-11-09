/*
All master pop-data updates for each release go here.

Do not check in separate files to the '.../Scripts/PopulateData/Release/' folder,
just add your updates directly to IconMasterData.sql or IconPopulateData.sql.
*/

go
update isa
	set [GrassFed] = case when oip.Description = 'Grass Fed' then 1 else 0 end
	,[PastureRaised] = case when oip.Description = 'Pasture Raised' then 1 else 0 end
	,[FreeRange] = case when oip.Description = 'Free Range' then 1 else 0 end
	,[DryAged] = case when oip.Description = 'Dry Aged' then 1 else 0 end
	,[AirChilled] = case when oip.Description = 'Air Chilled' then 1 else 0 end
	,[MadeinHouse] = case when oip.Description = 'Made in House' then 1 else 0 end
	from ItemSignAttribute isa
	join #OldItemProdCalimData oip on isa.ItemID = oip.ItemID

	drop table #OldItemProdCalimData

	IF NOT EXISTS(SELECT * FROM vim.StorePosType WHERE Name = 'CLOSED')
BEGIN
	SET IDENTITY_INSERT vim.StorePosType  ON
	insert into vim.StorePosType(StorePosTypeId, Name)
	values (7, 'CLOSED')
	SET IDENTITY_INSERT vim.StorePosType  OFF
END

GO

INSERT INTO [app].[App]
           ([AppName])
     VALUES
           ('Vim Locale Controller')
GO