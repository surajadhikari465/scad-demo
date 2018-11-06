/****** Object:  StoredProcedure [dbo].[Conversion_SoPacAddToItemTemp]    Script Date: 05/19/2006 16:34:23 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Conversion_SoPacAddToItemTemp]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Conversion_SoPacAddToItemTemp]
GO

/****** Object:  StoredProcedure [dbo].[Conversion_SoPacAddToItemTemp]    Script Date: 06/09/2006 15:51:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




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
