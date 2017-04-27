IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PromoPivotTable]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[PromoPivotTable]
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go




CREATE procedure [dbo].[PromoPivotTable]
   (
   @tname         varchar(1000) = '##PrePivotPreOrders',
   @row_fields    varchar(8000) = 'Identifier,  Item_Description, Package_Desc2, Unit_Name,Brand_Name, CompanyName, item_id, Package_Desc1, Price,  Sale_Price, Sale_Cost, Comment1, Comment2, Item_Key',
   @col_field     varchar(1000) = 'Store_No',
   @agg_func_list varchar(8000) = 'sum(OrderQty)',
   @where_clause  varchar(8000) = Null,
   @dest_table    varchar(2000)  = '##PivotPreOrders',
   @show_query    char(1) = null,
	@startdate    datetime,
	@enddate	datetime,
	@deptno		int
   )
as
--**************************************************************************
-- Procedure: PromoPivotTable()
--    Author: Ron Savage
--      Date: 05/20/2007
--
-- Description:
--
-- Change History:
-- Date			Init.	Description
-- 08/07/2009	BBB		Rewrote query to be more readable; added cross apply
--						to VCH to pull only current record;
-- **************************************************************************


if object_id('tempdb..##PrePivotPreOrders') is not null drop table ##PrePivotPreOrders;

--if exists (select * from dbo.sysobjects where  name = '##PrePivotPreOrders') drop table ##PrePivotPreOrders;
--if exists (select * from dbo.sysobjects where  name = '##PrePivotPreOrders') drop table ##PrePivotPreOrders;
SELECT
	ppo.Item_Key, 
	ib.Brand_Name, 
	i.Item_Description, 
	vch.Package_Desc1, 
	i.Package_Desc2, 
	iu.Unit_Name, 
	pbp.Sale_Price, 
	pbp.Sale_Cost, 
	pbp.Price, 
	pbp.Comment1, 
	pbp.Comment2, 
	ppo.Identifier, 
	v.CompanyName, 
	iv.Item_ID, 
	ppo.Store_No, 
	ppo.OrderQty
INTO 
	##PrePivotPreOrders
FROM
	PromoPreOrders						(nolock) ppo 
	INNER JOIN dbo.PriceBatchPromo		(nolock) pbp	ON	ppo.Item_Key			= pbp.Item_Key 
														AND ppo.PriceBatchPromoID	= pbp.PriceBatchPromoID
	INNER JOIN dbo.Item					(nolock) i		ON	ppo.Item_Key			= i.Item_Key
	INNER JOIN dbo.ItemBrand			(nolock) ib		ON	i.Brand_ID				= ib.Brand_ID 
	INNER JOIN dbo.Vendor				(nolock) v		ON	pbp.Vendor_Id			= v.Vendor_ID
	INNER JOIN dbo.ItemVendor			(nolock) iv		ON	iv.item_key				= i.item_key 
														AND iv.vendor_id			= v.vendor_ID
	INNER JOIN dbo.StoreItemVendor		(nolock) siv	ON	siv.store_no			= pbp.store_no
														AND siv.item_key			= i.item_key
	INNER JOIN dbo.ItemUnit				(nolock) iu		ON i.Package_Unit_ID		= iu.Unit_ID
	CROSS APPLY
				(
					SELECT TOP 1 
						[Package_Desc1]	=	vh2.Package_Desc1
					FROM 
						VendorCostHistory			(nolock) vh2
					WHERE
						vh2.StoreItemVendorID	=	siv.StoreItemVendorID
						AND vh2.StartDate		<=	GETDATE()
						AND vh2.EndDate			>=	GETDATE()
					ORDER BY 
						vh2.VendorCostHistoryID DESC
				) AS vch
WHERE  
	(pbp.Start_Date = @startdate) 
	AND (pbp.Sale_End_Date = @enddate) 
	AND ((pbp.Dept_No = @deptno) OR
                      (pbp.Dept_no IN
                          (SELECT     dept_no
                            FROM          SubTeam
                            WHERE      team_no = @deptno)))
GROUP BY
	ppo.Item_Key, 
	ib.Brand_Name, 
	i.Item_Description, 
	vch.Package_Desc1, 
	i.Package_Desc2, 
	iu.Unit_Name, 
	pbp.Sale_Price, 
	pbp.Sale_Cost, 
	pbp.Price, 
    pbp.Comment1, 
	pbp.Comment2, 
	ppo.Identifier, 
	v.CompanyName, 
	iv.Item_ID, 
	ppo.Store_No, 
	ppo.OrderQty;


--**************************************************************************
-- Procedure: pivot_table()
--    Author: Ron Savage
--      Date: 05/20/2007
--
-- Description:
-- This procedure makes a pivot table out of the input arguments using the
-- spiffy new PIVOT feature in SQL Server 2005.  Only up to 147 columns
-- of pivoted data though .. more crashes it.
--
-- Syntax:
-- pivot_table '<table>', '<field list for each row>', '<pivot column>', '<aggregate expression list>', '[<where clause>]', '[<results table>]', '[<show query>]'
--
--    '<table>'                     - Table containing the data
--    '<field list for each row>'   - List of fields to show for each row of data, to the left of the pivot (comma delimited)
--    '<pivot column>'              - The column that contains the column headers of the pivoted data
--    '<aggregate expression list>' - A list of function(field) expressions to calculate for the pivot.
--    '[<where clause>]'            - An optional statement to put in the where section to restrict the data from the original table
--    '[<results table>]'           - An optional table to create for the results of the pivot, it will be dropped and re-created
--    '[<show query>]'              - An optional value to have the proc show the pivot query for debugging. Any char value makes it print
--
-- Change History:
-- Date        Init. Description
-- 06/01/2007  RS    Put try / catch blocks around 'drop table' statements to avoid having to access
--                   dbo.sysobjects - which was causing errors due to Roles without dbo access.
--                   Also changed the temp table to a global temp so it survives past the exec with '##'!.
-- 05/26/2007  RS    Converted 'count' to 'sum' after the summary table, to get correct count() values.
-- 05/24/2007  RS    Increased the size of the varchar variables for the SQL generation.
-- 05/22/2007  RS    Added user defined tags onto the aggregate expression list.
-- 05/21/2007  RS    Modified to use aggregate expression list, for multiple results.
-- 05/21/2007  RS    Added where clause option, and option to show the pivot query.
-- 05/20/2007  RS    Created.
-- **************************************************************************
begin
   set nocount on;

   if ( @show_query is not null ) print char(13) + 'Starting ...';

   --**************************************************************************
   -- Declare some variables
   --**************************************************************************
   declare @pivot_sql  varchar(8000);
   declare @sql        varchar(5000);
   declare @piv_value  varchar(500);

   set @pivot_sql = '';
   set @sql       = '';

   --**************************************************************************
   -- Build a default where clause if none was sent in
   --**************************************************************************
   if ( @where_clause is null )
      set @where_clause = ' 1=1';

   --**************************************************************************
   -- Declare variables for parsing the agg expression list
   --**************************************************************************
   declare @sub_start  integer;
   declare @sub_len    integer;
   declare @del_loc    integer;
   declare @agg_exp    varchar(1000);
   declare @agg_func   varchar(500);
   declare @agg_lbl    varchar(100);
   declare @agg_tag    varchar(100);
   declare @agg_field  varchar(1000);
   declare @agg_fld    varchar(200);
   declare @tmp_table  varchar(200);
   declare @fld_index  integer;

   set @sub_start = 1;
   set @sub_len   = 0;
   set @fld_index = 1;

   --**************************************************************************
   -- Declare a process id specific temp table
   --**************************************************************************
   set @tmp_table = '##tmp_' + cast(@@procid as varchar(15));

   --**************************************************************************
   -- Drop if the temp table exists
   --**************************************************************************
--    if exists (select * from dbo.sysobjects where  name = @tmp_table)
   BEGIN TRY
      exec('drop table ' + @tmp_table);
   END TRY
   BEGIN CATCH
--       print 'Woo! Caught one!';
   END CATCH

   --**************************************************************************
   -- Loop through each aggregate expression and insert the results into
   -- the temp table with a category label to pivot on when done
   --**************************************************************************
   if ( @show_query is not null ) print char(13) + 'Looping and parsing [' + @agg_func_list + '] ...';
   while ( @sub_start < len(@agg_func_list) )
      begin
      set @del_loc   = charindex(',', @agg_func_list, @sub_start);

      if ( @del_loc > 0 )
         set @sub_len   = @del_loc - @sub_start;
      else
         set @sub_len   = len(@agg_func_list) - @sub_start + 1;

      --**************************************************************************
      -- Parse the expression, field and function and label
      --**************************************************************************
      set @agg_exp  = ltrim(rtrim(substring(@agg_func_list, @sub_start, @sub_len)));
      set @agg_fld  = ltrim(rtrim(substring(@agg_exp, charindex('(',@agg_exp) + 1, charindex(')',@agg_exp) - charindex('(',@agg_exp) - 1)));
      set @agg_func = ltrim(rtrim(substring(@agg_exp, 1, charindex('(',@agg_exp) - 1)));
      set @agg_lbl  = ltrim(rtrim(reverse(substring(reverse(@agg_exp), 1, charindex(' ',reverse(@agg_exp))))));

      set @agg_tag = case
                        when ( @fld_index = 1 and @agg_lbl = '' ) then 'cast(' + @col_field + ' as varchar(100))'
                        when ( @fld_index > 1 and @agg_lbl = '' ) then 'cast(' + @col_field + ' as varchar(100)) + ''_' + @agg_func + '_' + @agg_fld + ''''
                        else 'cast(' + @col_field + ' as varchar(100)) + ''_' + @agg_lbl + ''''
                     end

      if ( @show_query is not null ) print char(13) + 'Processing: [' + @agg_exp + '] into [' + @agg_func + '] [' + @agg_fld + '] ...';

      --**************************************************************************
      -- If it's the first first expression, define a select - into SQL
      --**************************************************************************
      if ( @sub_start = 1 )
         set @sql = 'select ' + @row_fields + ', ' +
                                @col_field + ' as col_field, ' +
                                @agg_tag + 'as cat, ' +
                                cast(@fld_index as varchar(3)) + ' as fld_num, ' +
                                @agg_func + '(' + @agg_fld + ') as value into ' + @tmp_table + ' from ' +
                                @tname +
                                ' where ' +  @where_clause +
                                ' group by ' + @row_fields + ', ' + @col_field +
                                ',' + @agg_tag;
      else
      --**************************************************************************
      -- Otherwise, define an insert-into-select SQL
      --**************************************************************************
         set @sql = 'insert into ' + @tmp_table + ' select ' + @row_fields + ', ' +
                                @col_field + ' as col_field, '+
                                @agg_tag + 'as cat, ' +
                                cast(@fld_index as varchar(3)) + ' as fld_num, ' +
                                @agg_func + '(' + @agg_fld + ') as value from ' +
                                @tname +
                                ' where ' +  @where_clause +
                                ' group by ' + @row_fields + ', ' + @col_field +
                                ',' + @agg_tag;

      set @sub_start = @sub_start + @sub_len + 1;
      set @fld_index = @fld_index + 1;

      if ( @show_query is not null )
      begin
         print char(13) + 'Temp sub SQL:';
         print @sql;
      end

      exec(@sql);
      end

   --**************************************************************************
   -- Update the input variables to the new temp table and fields
   --**************************************************************************
   set @tname        = @tmp_table;
   set @col_field    = 'cat';
   set @agg_field    = 'value';
   set @where_clause = ' 1=1';

   if ( lower(@agg_func) = 'count' ) set @agg_func = 'sum';

   if ( @show_query is not null ) print char(13) + 'Changed table and fields to refer to the temp:  [' + @tname + '] [' + @col_field + '] [' + @agg_field + '] ...';

   --**************************************************************************
   -- Start building the PIVOT SQL
   --**************************************************************************
   if ( @show_query is not null ) print char(13) + 'Starting to build pivot_sql ...';
   set @pivot_sql = @pivot_sql +  'select' + char(13);
   set @pivot_sql = @pivot_sql +  '   pvt.*' + char(13);

   --**************************************************************************
   -- If we have a destination table, add the "into" statements
   --**************************************************************************
   if ( @show_query is not null ) print char(13) + 'checking for destination table option ...';
   if ( @dest_table is not null )
      begin
      if ( @show_query is not null ) print char(13) + 'Dropping the dest table [' + @dest_table + '] if it exists ...';

      set @sql ='drop table ' + @dest_table;

      BEGIN TRY
         exec(@sql);
      END TRY
      BEGIN CATCH
      END CATCH

      set @pivot_sql = @pivot_sql +  'into' + char(13);
      set @pivot_sql = @pivot_sql +  '   ' + @dest_table + char(13);
      end

   --**************************************************************************
   -- Put all the input arguments in thier proper places in the query
   --**************************************************************************
   if ( @show_query is not null ) print char(13) + 'adding ''from'' clause to the query ...';
   set @pivot_sql = @pivot_sql +  'from' + char(13);
   set @pivot_sql = @pivot_sql +  '   (select ' + @row_fields + ', ' + @col_field + ', ' + @agg_field + ' from ' + @tname + ' where ' + @where_clause + ') as c' + char(13);
   set @pivot_sql = @pivot_sql +  '       PIVOT' + char(13);
   set @pivot_sql = @pivot_sql +  '          (' + char(13);
   set @pivot_sql = @pivot_sql +  '          ' + @agg_func + '(' + @agg_field + ')' + char(13);
   set @pivot_sql = @pivot_sql +  '          for ' + @col_field + ' in' + char(13);
   set @pivot_sql = @pivot_sql +  '             ( ';

   --**************************************************************************
   -- Get all the unique values of the Pivot column in the data table,
   -- these will become the pivoted columns.  For some reason, the PIVOT
   -- chokes if there are more than 147 result columns ... determined
   -- experimentally, so your mileage may vary.
   --**************************************************************************
   if ( @show_query is not null ) print char(13) + 'Creating the #pivot_values table ...';
   create table #pivot_values ( piv_order  integer, piv_fldsort   varchar(1000), piv_fld   varchar(1000) );

   set @sql ='insert into #pivot_values select distinct top 147 fld_num, col_field, ' + @col_field  + ' from ' + @tname + ' where ' + @where_clause + ' order by col_field, fld_num';

   if ( @show_query is not null ) print char(13) + 'Building #pivot_values table ...' + char(13) + @sql + char(13);
   exec(@sql);

   --**************************************************************************
   -- Loop through the values and add them to the PIVOT query SQL
   --**************************************************************************
   declare piv_cursor cursor for
      select piv_fld from #pivot_values order by piv_fldsort, piv_order;

   open piv_cursor;
   fetch next from piv_cursor into @piv_value;

   while @@FETCH_STATUS = 0
   begin
      set @pivot_sql = @pivot_sql +  '[' + rtrim(@piv_value) + ']';

      fetch next from piv_cursor into @piv_value;

      if @@FETCH_STATUS = 0
         set @pivot_sql = @pivot_sql +  ', ';
   end
   close piv_cursor;

   --**************************************************************************
   -- Finish off the PIVOT SQL
   --**************************************************************************
   set @pivot_sql = @pivot_sql +  ' )' + char(13);
   set @pivot_sql = @pivot_sql +  '          ) as pvt' + char(13);
   set @pivot_sql = @pivot_sql +  'order by' + char(13);
   set @pivot_sql = @pivot_sql +  '   ' + @row_fields + char(13);

   --**************************************************************************
   -- Print the SQL if specified
   --**************************************************************************
   if ( @show_query is not null )
   begin
      print char(13) + 'Pivot SQL:';
		print @pivot_sql;
      select @pivot_sql;
   end

   --**************************************************************************
   -- Run the PIVOT SQL
   --**************************************************************************
   exec(@pivot_sql);

   BEGIN TRY
      exec('drop table ' + @tmp_table);
   END TRY
   BEGIN CATCH
   END CATCH
   return;
end

--if exists (select * from dbo.sysobjects where  name = '##PrePivotPreOrders') drop table ##PrePivotPreOrders;

GO




