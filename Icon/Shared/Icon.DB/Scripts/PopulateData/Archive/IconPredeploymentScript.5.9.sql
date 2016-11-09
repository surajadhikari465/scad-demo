/*
All pre deploymnet op-data updates for each release go here.

Do not check in separate files to the '.../Scripts/PopulateData/Release/' folder,
just add your updates directly to IconPredeployment.sql
*/
	create table #OldItemProdCalimData (ItemID int, Description nvarchar(255));

	IF EXISTS  (SELECT * FROM sys.columns 
					WHERE Name = N'ProductionClaimsId' AND Object_ID = Object_ID(N'ItemSignAttribute')
			   )
	AND EXISTS (SELECT * FROM sys.tables
					WHERE name = 'ItemSignAttribute' AND object_id = Object_ID(N'ItemSignAttribute')
			    )			
		Begin
			
			insert  #OldItemProdCalimData 
			exec ('select ItemID,  Description
			from [dbo].[ItemSignAttribute] isa 
			join ProductionClaim pc on isa.ProductionClaimsId = pc.ProductionClaimId')
		end
		else
		begin
			print 'Table and column does not exists'
		end
	go

	--Set  ProductionClaimsId to null to prevent dataloss error message and let db script drop the column as we will stored data in temp table and populating it as post script
	IF EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'ProductionClaimsId' AND Object_ID = Object_ID(N'IRMAItem'))
			Begin
				exec ('Update app.IRMAItem set ProductionClaimsId = null');
			end

	--Set  ProductionClaimsId to null to prevent dataloss error message and let db script drop the column
	IF EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'ProductionClaimsId' AND Object_ID = Object_ID(N'ItemSignAttribute'))
			Begin
				exec ('update ItemSignAttribute set ProductionClaimsId = null');
			end
	
