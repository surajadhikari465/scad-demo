CREATE   PROCEDURE [dbo].[Conversion_SoPacAddToItemTemp]  
@fromRow integer, @toRow integer  
AS

--begin tran
--get a the subset of the item_temp2 table

--insert item_temp into item_temp2 for recursive purposes



select * into #item_test from 
item_temp2 it where lkey between @fromRow and @toRow;


--now insert all of #item_test into item_temp
--clear item_temp first
truncate table item_temp

	insert into Item_Temp
    select * from #item_test

Print cast(getdate() as char(20)) +' Inserted into IRMA IRMA rows: ' + cast(@fromRow as nvarchar) + ' to row: ' + cast(@toRow as nvarchar)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Conversion_SoPacAddToItemTemp] TO [DataMigration]
    AS [dbo];

