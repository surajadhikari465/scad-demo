/*
Return all avaialble PLUs for given PLU category
*/CREATE PROCEDURE [app].[GetAvailablePlusForCategory]
	@PluCategoryID int,
	@MaxPlus int = 0
AS
BEGIN
		
	DECLARE @beginRange bigint, @endRange bigint, @scanCode bigint;
	DECLARE @posPlu int, @scalePlu int, @pluType int;
	DECLARE @beginRangeString varchar(12), @endRangeString varchar(12);

	set @scalePlu = (select scanCodeTypeID from ScanCodeType where scanCodeTypeDesc = 'Scale PLU')
	set @posPlu = (select scanCodeTypeID from ScanCodeType where scanCodeTypeDesc = 'POS PLU')

	select @beginRange = pc.beginRange, @endRange = pc.endRange from app.PLUCategory pc where pc.PLUCategoryID = @PluCategoryID
	
	set @beginRangeString = cast(@beginRange as varchar)
	set @endRangeString = cast(@endRange as varchar)
	if(@MaxPlus = 0)
		set @MaxPlus = 99999
	
	if(len(@beginRangeString) > 6)
	begin
	 set @beginRange = cast(SUBSTRING(@beginRangeString, 1, 6) as bigint)
	 set @pluType = @scalePlu
	end
	if(len(@endRangeString) > 6)
	begin
	 set @endRange = cast(SUBSTRING(@endRangeString, 1, 6) as bigint)
	end
	
	
	SELECT top (@MaxPlus)
	0							as [irmaItemID],
	null						as [regioncode],
	(case @pluType when  @scalePlu 
			then cast(sequence as varchar) + '00000' 
		else cast(sequence as varchar)
		end)					as [identifier],
	cast(1 as bit)				as [defaultIdentifier],
	null						as [brandName],
	null						as [itemDescription],
	null						as [posDescription],
    1							as [packageUnit],
    null						as [retailSize],
    null						as [retailUom],
	null						as [deliverySystem],
    cast(0 as bit)				as [foodStamp],
    cast(0 as decimal)			as [posScaleTare],
    cast(0 as bit)				as [departmentSale],
    null						as [giftCard],
    null						as [taxClassID],
    null						as [merchandiseClassID],
    SYSDATETIME()				as [insertDate],
	null						as [irmaSubTeamName],
	null						as [nationalClassId],
	null						as [AnimalWelfareRatingId],
	null						as [Biodynamic],
	null						as [CheeseMilkTypeId],
	null						as [CheeseRaw],
	null						as [EcoScaleRatingId],
	null						as [GlutenFreeAgencyId],
	null						as [HealthyEatingRatingId],
	null						as [KosherAgencyId],
	null						as [Msc],
	null						as [NonGmoAgencyId],
	null						as [OrganicAgencyId],
	null						as [PremiumBodyCare],
	null						as [SeafoodFreshOrFrozenId],
	null						as [SeafoodCatchTypeId],
	null						as [VeganAgencyId],
	null						as [Vegetarian],
	null						as [WholeTrade],
	null						as [GrassFed],
	null						as [PastureRaised],
	null						as [FreeRange],
	null						as [DryAged],
	null						as [AirChilled],
	null						as [MadeinHouse],
	null						as [AlcoholByVolume]
	from
	(
		select sequence from app.PLUSequence where sequence >= @beginRange and sequence <=@endRange
		except 
	
		(select subScancode as sequence from (
							select distinct (case when sc.scanCodeTypeID= @scalePlu then SUBSTRING(scanCode, 1, 6) 
										else ScanCode
									end )as subScancode
							from ScanCode sc
							where sc.scanCodeTypeID in (@scalePlu, @posPlu) 
							and (	(	cast(scanCode as bigint) >= @beginRange
										and cast(scanCode as bigint) <= @endRange
										and len(scanCode) <= 6
										and scanCodeTypeID = @posPlu
									)
									or
									(	cast(SUBSTRING(scanCode, 1, 6) as bigint) >= @beginRange
										and cast(SUBSTRING(scanCode, 1, 6) as bigint) <= @endRange
										and len(scanCode) = 11 and scanCode like '2%'
										and scanCodeTypeID = @scalePlu
									)
								)) subList
		)
	) missingPlus
	order by sequence
	
END;