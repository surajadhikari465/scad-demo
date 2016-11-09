--NOTES 
--> This script must be run by a user with BULK INSERT permissions.
-- Update file path based on file location


BEGIN

	declare @hierarchyFile nvarchar(128) = '\\cewd6503\buildshare\National Hierarchy\IconCurrent Hierarchy.txt'
	DECLARE @file_exists int
	Declare @nationalClassID int, @classCodeTraitID int
	SELECT @file_exists = 0

	EXEC master.dbo.xp_fileexist
		@hierarchyFile,
		@file_exists OUTPUT

	IF @file_exists = 1
	BEGIN
		PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Item-coding input-file found.'
	END
	ELSE
	BEGIN
		PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + '[[Error -- Cannot Continue]] Item-coding file not found.  Please verify file and try again.'
		set noexec on
	END

	set @nationalClassID = (select hierarchyID from Hierarchy where hierarchyName = 'National Class')
	set @classCodeTraitID = (select traitID from Trait where traitCode = 'NCC')

	CREATE TABLE #tmpMainHierarchyAll
			(
				FamilyName			[varchar](255)			NOT NULL,
				FamilyID			int	NULL,
				CategoryName			[varchar](255)			NOT NULL,
				CategoryID			int	NULL,
				SubCategoryName			[varchar](255)			NOT NULL,
				SubCategoryID			int	NULL,
				ClassLineage			[varchar](255)			NOT NULL,
				ClassName			[varchar](255),
				ClassID			[varchar](255)			NOT NULL,
			
			
		) ON [PRIMARY]
		
		-- Import Hierarchy data.
		PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Importing hierarchy text file...'
		BULK INSERT #tmpMainHierarchyAll FROM '\\cewd6503\buildshare\National Hierarchy\IconCurrent Hierarchy.txt'
		WITH (FIRSTROW = 1, FIELDTERMINATOR = '|', rowterminator = '\n') 

		
		--Family

		insert into HierarchyClass (hierarchyClassName, hierarchyID, hierarchyLevel, hierarchyParentClassID)
		select distinct FamilyName, @nationalClassID, 1,  null from #tmpMainHierarchyAll

		--Category
		insert into HierarchyClass (hierarchyClassName, hierarchyID, hierarchyLevel, hierarchyParentClassID)
		select distinct cat.CategoryName, @nationalClassID, 2, hc.hierarchyClassID
		from 
		(
		select distinct FamilyName, CategoryName  from #tmpMainHierarchyAll
		) cat
		join HierarchyClass hc on hc.hierarchyClassName = cat.FamilyName and hc.hierarchyID = @nationalClassID

		--SubCategory

		insert into HierarchyClass (hierarchyClassName, hierarchyID, hierarchyLevel, hierarchyParentClassID)
		select distinct subcat.SubCategoryName, @nationalClassID, 3, cathc.hierarchyClassID
		from 
		(
		select distinct FamilyName, CategoryName, SubCategoryName  from #tmpMainHierarchyAll
		) subcat
		join HierarchyClass cathc on cathc.hierarchyClassName = subcat.CategoryName and cathc.hierarchyID = @nationalClassID
		join HierarchyClass fmhc on cathc.hierarchyParentClassID = fmhc.hierarchyClassID and fmhc.hierarchyClassName = subcat.FamilyName and fmhc.hierarchyID = @nationalClassID

		--Class
		--Cursor is needed as input file has duplicate calss names with different classIDs
		DECLARE class_cursor CURSOR
		FOR  (
				select distinct cls.ClassName, subcat.hierarchyClassID, ClassID
				from 
					(
						select distinct FamilyName, CategoryName, SubCategoryName, ClassName, ClassID  from #tmpMainHierarchyAll
					) cls
				join HierarchyClass subcat on subcat.hierarchyClassName = cls.SubCategoryName and subcat.hierarchyID = @nationalClassID
				join HierarchyClass cathc on subcat.hierarchyParentClassID = cathc.hierarchyClassID and  cathc.hierarchyClassName = cls.CategoryName and cathc.hierarchyID = @nationalClassID	
				join HierarchyClass fmhc on cathc.hierarchyParentClassID = fmhc.hierarchyClassID and fmhc.hierarchyClassName = cls.FamilyName AND fmhc.hierarchyID = @nationalClassID
			)
			declare @className  varchar(255), @parentID int, @classCode int, @classHierarchyID int
			CREATE TABLE #classIds  (id int);



		OPEN class_cursor
		FETCH NEXT FROM class_cursor 
		INTO @className, @parentID, @classCode

		WHILE @@FETCH_STATUS = 0
		BEGIN
				
				insert into HierarchyClass (hierarchyClassName, hierarchyID, hierarchyLevel, hierarchyParentClassID)	
				OUTPUT INSERTED.hierarchyClassID into #classIds
				select @className, @nationalClassID, 4, @parentID
			
				insert into HierarchyClassTrait (traitID, hierarchyClassID, uomID, traitValue)
				select top 1 @classCodeTraitID, id, null, @classCode
				from #classIds

				truncate table #classIds

			
				 FETCH NEXT FROM class_cursor 
				INTO @className, @parentID, @classCode
		END 
		CLOSE class_cursor;
		DEALLOCATE class_cursor;
		drop table #classIds
		drop table  #tmpMainHierarchyAll

	
	set noexec off
END