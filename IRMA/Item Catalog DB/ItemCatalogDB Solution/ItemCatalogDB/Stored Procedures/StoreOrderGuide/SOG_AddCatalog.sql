 SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SOG_AddCatalog')
	BEGIN
		DROP Procedure [dbo].SOG_AddCatalog
	END
GO

CREATE PROCEDURE dbo.SOG_AddCatalog
	@CatalogID		int,
	@ManagedByID	int,
	@CatalogCode	varchar(20),
	@Description	varchar(max),
	@Published		bit,
	@SubTeam		int,
	@ExpectedDate	bit,
	@InsertUser		varchar(50),
	@Details		varchar(max),
	@Copy			bit
WITH RECOMPILE
AS

-- **************************************************************************
-- Procedure: SOG_AddCatalog()
--    Author: Billy Blackerby
--      Date: 3/24/2009
--
-- Description:
-- Utilized by StoreOrderGuide to create a catalog
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 03/24/2009	BBB				Creation
-- 05/13/2009	BBB				Added Details parameter
-- 03/03/2011	BBB		1559	Changed Published value to 0 on CopyCatalog
-- **************************************************************************	
BEGIN
    SET NOCOUNT ON
	--**************************************************************************
	--Procedure Variables
	--**************************************************************************
	DECLARE @NewCatalogID int

	--**************************************************************************
	--Main SQL
	--**************************************************************************
	IF @Copy = 0
		BEGIN
			INSERT INTO
				Catalog
			(
				ManagedByID,
				CatalogCode,
				Description,
				Published,
				SubTeam,
				ExpectedDate,
				InsertUser,
				Details
			)
			VALUES
			(
				@ManagedByID,
				@CatalogCode,
				@Description,
				@Published,
				@SubTeam,
				@ExpectedDate,
				@InsertUser,
				@Details
			)
		END
	ELSE
		BEGIN TRANSACTION
			--**************************************************************************
			--Copy Catalog
			--**************************************************************************
			BEGIN
				INSERT INTO
					Catalog
				(
					ManagedByID,
					CatalogCode,
					Description,
					Published,
					SubTeam,
					ExpectedDate,
					InsertUser,
					Details
				)
				(
					SELECT
						ManagedByID,
						CatalogCode,
						'Copy of Catalog: ' + CONVERT(varchar(6), @CatalogID),
						0,
						SubTeam,
						ExpectedDate,
						@InsertUser,
						Details
					FROM
						Catalog
					WHERE
						CatalogID = @CatalogID
				)
			
				SELECT @NewCatalogID = SCOPE_IDENTITY()
			END

			--**************************************************************************
			--Copy CatalogStore
			--**************************************************************************
			BEGIN
				INSERT INTO
					CatalogStore
				(
					CatalogID,
					StoreNo,
					InsertUser
				)
				(
					SELECT
						@NewCatalogID,
						StoreNo,
						@InsertUser
					FROM
						CatalogStore
					WHERE
						CatalogID = @CatalogID
				)
			END
			
			--**************************************************************************
			--Copy CatalogItem
			--**************************************************************************
			BEGIN
				INSERT INTO
					CatalogItem
				(
					CatalogID,
					ItemKey,
					ItemNotes,
					InsertUser
				)
				(
					SELECT
						@NewCatalogID,
						ItemKey,
						ItemNotes,
						@InsertUser
					FROM
						CatalogItem
					WHERE
						CatalogID = @CatalogID
				)
			END
			
        --**************************************************************************
        --Commit the transaction
        --**************************************************************************
		IF @@TRANCOUNT > 0
                COMMIT TRANSACTION
	
    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 