BEGIN TRANSACTION;  

BEGIN TRY  
	-- to modify a udf, rename it, create a new type, change reference and drop renamed type
	EXEC sys.sp_rename 'dbo.IconUpdateItemType', 'oldIconUpdateItemType'

	-- Create type again with changes
	CREATE TYPE [dbo].[IconUpdateItemType] AS TABLE (
		[ItemId]                INT            NOT NULL,
		[ValidationDate]        NVARCHAR (255) NOT NULL,
		[ScanCode]              NVARCHAR (13)  NOT NULL,
		[ScanCodeType]          NVARCHAR (255) NOT NULL,
		[ProductDescription]    NVARCHAR (255) NOT NULL,
		[PosDescription]        NVARCHAR (255) NOT NULL,
		[PackageUnit]           NVARCHAR (255) NOT NULL,
		[FoodStampEligible]     NVARCHAR (255) NOT NULL,
		[Tare]                  NVARCHAR (255) NULL,
		[BrandId]               INT            NOT NULL,
		[BrandName]             NVARCHAR (35)  NOT NULL,
		[TaxClassName]          NVARCHAR (50)  NOT NULL,
		[NationalClassCode]     NVARCHAR (50)  NOT NULL,
		[SubTeamName]           NVARCHAR (255) NULL,
		[SubTeamNo]             INT            NOT NULL,
		[DeptNo]                INT            NOT NULL,
		[SubTeamNotAligned]     BIT            NOT NULL,
		[EventTypeId]           INT            NULL,
		[AnimalWelfareRating]   NVARCHAR (10)  NULL,
		[Biodynamic]            BIT            NULL,
		[CheeseMilkType]        NVARCHAR (40)  NULL,
		[CheeseRaw]             BIT            NULL,
		[EcoScaleRating]        NVARCHAR (30)  NULL,
		[GlutenFree]            BIT            NULL,
		[Kosher]                BIT            NULL,
		[HealthyEatingRating]	NVARCHAR (10)  NULL,
		[NonGmo]                BIT            NULL,
		[Organic]               BIT            NULL,
		[PremiumBodyCare]       BIT            NULL,
		[FreshOrFrozen]         NVARCHAR (30)  NULL,
		[SeafoodCatchType]      NVARCHAR (15)  NULL,
		[Vegan]                 BIT            NULL,
		[Vegetarian]            BIT            NULL,
		[WholeTrade]            BIT            NULL,
		[Msc]                   BIT            NULL,
		[GrassFed]              BIT            NULL,
		[PastureRaised]         BIT            NULL,
		[FreeRange]             BIT            NULL,
		[DryAged]               BIT            NULL,
		[AirChilled]            BIT            NULL,
		[MadeInHouse]           BIT            NULL,
		[HasItemSignAttributes] BIT            NULL,
		[RetailSize]            DECIMAL (9, 4) NULL,
		[RetailUom]             VARCHAR (5)    NULL,
		PRIMARY KEY CLUSTERED ([ItemId] ASC));



	GRANT EXECUTE
		ON TYPE::[dbo].[IconUpdateItemType] TO [IConInterface];

	DECLARE @Name NVARCHAR(776);

	-- update references
	DECLARE REF_CURSOR CURSOR FOR 
	SELECT referencing_schema_name + '.' + referencing_entity_name 
	FROM sys.dm_sql_referencing_entities('dbo.IconUpdateItemType', 'TYPE');

	OPEN REF_CURSOR;

	FETCH NEXT FROM REF_CURSOR INTO @Name; 
	WHILE (@@FETCH_STATUS = 0) 
	BEGIN 
		EXEC sys.sp_refreshsqlmodule @name = @Name; 
		FETCH NEXT FROM REF_CURSOR INTO @Name; 
	END;

	CLOSE REF_CURSOR; 
	DEALLOCATE REF_CURSOR;

	DROP TYPE dbo.oldIconUpdateItemType;

COMMIT TRANSACTION; 
END TRY

BEGIN CATCH  
ROLLBACK TRANSACTION 
END CATCH;  