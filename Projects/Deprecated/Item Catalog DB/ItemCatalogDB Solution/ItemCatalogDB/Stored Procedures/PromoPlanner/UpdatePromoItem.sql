IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdatePromoItem]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[UpdatePromoItem]
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

CREATE PROCEDURE dbo.[UpdatePromoItem] 
	@salPr decimal(8,4),
    @salCo decimal(8,4),
	@ID int,
    @comment varchar(50),
    @qty int
AS
BEGIN
	
	SET NOCOUNT ON;

update 
PriceBatchPromo  
set 
comment1 = @comment, 
Sale_Price=@salPr,
Sale_Cost=@salCo, 
projunits = @qty 
where 
PriceBatchPromoID = @ID
END

GO