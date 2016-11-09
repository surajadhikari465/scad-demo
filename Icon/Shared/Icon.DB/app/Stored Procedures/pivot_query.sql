
create procedure app.[pivot_query]
   (
   @query         varchar(MAX),
   @row_fields    varchar(8000),
   @col_field     varchar(1000),
   @agg_func_list varchar(8000),
   @dest_table    varchar(1000) = null,
   @show_query    char(1) = null
   )
as
--**************************************************************************
-- Procedure: pivot_query()
--    Author: Ron Savage
--      Date: 05/20/2007
--
-- Description:
-- This procedure makes a pivot table out of the input arguments using the
-- spiffy new PIVOT feature in SQL Server 2005.  Only up to 147 columns
-- of pivoted data though .. more crashes it.
--
-- Syntax:
-- pivot_query '<query>', '<field list for each row>', '<pivot column>', '<aggregate expression list>', '[<results table>]', '[<show query>]'
--
--    '<query>'                     - Query defining the data to pivot
--    '<field list for each row>'   - List of fields to show for each row of data, to the left of the pivot (comma delimited)
--    '<pivot column>'              - The column that contains the column headers of the pivoted data
--    '<aggregate expression list>' - A list of function(field) expressions to calculate for the pivot.
--    '[<results table>]'           - An optional table to create for the results of the pivot, it will be dropped and re-created
--    '[<show query>]'              - An optional value to have the proc show the pivot query for debugging. Any char value makes it print
--
-- Change History:
-- Date        Init. Description
-- 03/13/2013  BBB      removed ##log_table calls from SP to prevent infrequent issue with usage of this FN
-- 10/09/2009  RS    Updated the internal temp table to be named '##tmp_' + @PROCID + @SPID so each
--                server process will get it's own temp table - multiple threads each get the same PROCID.
-- 10/08/2007  RS    Fixed sum() and count() cases for sql_variants.
-- 10/07/2007  RS    Altered to use the sql_variant type field in the inner temp tables to allow
--                multiple aggregate fields to be different types, and to convert the sql_variant
--                back to the original type on output.
-- 09/14/2007  RS    Increased parsing variable sizes to handle longer field name combinations.
-- 08/02/2007  RS    Copied from pivot_table and modified for passing in a query.
-- **************************************************************************
begin
   set nocount on;

   if ( @show_query is not null ) print char(13) + 'Starting ...';

   --**************************************************************************
   -- Declare some variables
   --**************************************************************************
   declare @pivot_sql  varchar(MAX);
   declare @sql        varchar(MAX);
   declare @piv_value  varchar(300);
   declare @piv_type   varchar(300);

   set @pivot_sql = '';
   set @sql       = '';

   --**************************************************************************
   -- Declare variables for parsing the agg expression list
   --**************************************************************************
   declare @sub_start  integer;
   declare @sub_len    integer;
   declare @del_loc    integer;
   declare @agg_exp    varchar(1000);
   declare @agg_func   varchar(300);
   declare @agg_lbl    varchar(300);
   declare @agg_tag    varchar(300);
   declare @agg_field  varchar(1000);
   declare @agg_fld    varchar(300);
   declare @tmp_table  varchar(300);
   declare @fld_index  integer;
   declare @tname      varchar(300);
   declare @tbl_select varchar(MAX);

   set @sub_start = 1;
   set @sub_len   = 0;
   set @fld_index = 1;

   --**************************************************************************
   -- Declare a process id specific temp table
   --**************************************************************************
   set @tmp_table = '##tmp_' + cast(@@procid as varchar(15))+ cast(@@spid as varchar(15));

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

      if ( @agg_fld = '*' ) set @agg_fld = '1';

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
                                @agg_tag + ' as cat, ' +
                                cast(@fld_index as varchar(3)) + ' as fld_num, ' +
                                'max(cast(' + char(13) +
                                'case cast(sql_variant_property(' + @agg_fld + ', ''BaseType'') as varchar(10))' + char(13) +
                                '   when ''decimal''  then cast(sql_variant_property(' + @agg_fld + ', ''BaseType'') as varchar(10)) + ''('' + cast(sql_variant_property(' + @agg_fld + ', ''Precision'') as varchar(10)) + '','' + cast(sql_variant_property(' + @agg_fld + ', ''Scale'') as varchar(10)) + '')''' + char(13) +
                                '   when ''numeric''  then cast(sql_variant_property(' + @agg_fld + ', ''BaseType'') as varchar(10)) + ''('' + cast(sql_variant_property(' + @agg_fld + ', ''Precision'') as varchar(10)) + '','' + cast(sql_variant_property(' + @agg_fld + ', ''Scale'') as varchar(10)) + '')''' + char(13) +
                                '   when ''char''     then cast(sql_variant_property(' + @agg_fld + ', ''BaseType'') as varchar(10)) + ''('' + cast(sql_variant_property(' + @agg_fld + ', ''MaxLength'') as varchar(10)) + '')''' + char(13) +
                                '   when ''nchar''    then cast(sql_variant_property(' + @agg_fld + ', ''BaseType'') as varchar(10)) + ''('' + cast(sql_variant_property(' + @agg_fld + ', ''MaxLength'') as varchar(10)) + '')''' + char(13) +
                                '   when ''varchar''  then cast(sql_variant_property(' + @agg_fld + ', ''BaseType'') as varchar(10)) + ''('' + cast(sql_variant_property(' + @agg_fld + ', ''MaxLength'') as varchar(10)) + '')''' + char(13) +
                                '   when ''nvarchar'' then cast(sql_variant_property(' + @agg_fld + ', ''BaseType'') as varchar(10)) + ''('' + cast(sql_variant_property(' + @agg_fld + ', ''MaxLength'') as varchar(10)) + '')''' + char(13) +
                                '   else                   cast(sql_variant_property(' + @agg_fld + ', ''BaseType'') as varchar(10))' + char(13) +
                                'end as varchar(100))) as fld_type,' + char(13) +
                                'cast(' + @agg_func + '(' + @agg_fld + ') as sql_variant) as value ' + char(13) + 'into ' + @tmp_table + char(13) +
                                ' from (' + @query + ') as qry ' + char(13) +
                                ' group by ' + @row_fields + ', ' + @col_field  +
                                ',' + @agg_tag;
      else
      --**************************************************************************
      -- Otherwise, define an insert-into-select SQL
      --**************************************************************************
         set @sql = 'insert into ' + @tmp_table + ' select ' + @row_fields + ', ' +
                                @col_field + ' as col_field, '+
                                @agg_tag + 'as cat, ' +
                                cast(@fld_index as varchar(3)) + ' as fld_num, ' +
                                'max(cast(' + char(13) +
                                'case cast(sql_variant_property(' + @agg_fld + ', ''BaseType'') as varchar(10))' + char(13) +
                                '   when ''decimal'' then cast(sql_variant_property(' + @agg_fld + ', ''BaseType'') as varchar(10)) + ''('' + cast(sql_variant_property(' + @agg_fld + ', ''Precision'') as varchar(10)) + '','' + cast(sql_variant_property(' + @agg_fld + ', ''Scale'') as varchar(10)) + '')''' + char(13) +
                                '   when ''numeric'' then cast(sql_variant_property(' + @agg_fld + ', ''BaseType'') as varchar(10)) + ''('' + cast(sql_variant_property(' + @agg_fld + ', ''Precision'') as varchar(10)) + '','' + cast(sql_variant_property(' + @agg_fld + ', ''Scale'') as varchar(10)) + '')''' + char(13) +
                                '   when ''char'' then cast(sql_variant_property(' + @agg_fld + ', ''BaseType'') as varchar(10)) + ''('' + cast(sql_variant_property(' + @agg_fld + ', ''MaxLength'') as varchar(10)) + '')''' + char(13) +
                                '   when ''nchar'' then cast(sql_variant_property(' + @agg_fld + ', ''BaseType'') as varchar(10)) + ''('' + cast(sql_variant_property(' + @agg_fld + ', ''MaxLength'') as varchar(10)) + '')''' + char(13) +
                                '   when ''varchar'' then cast(sql_variant_property(' + @agg_fld + ', ''BaseType'') as varchar(10)) + ''('' + cast(sql_variant_property(' + @agg_fld + ', ''MaxLength'') as varchar(10)) + '')''' + char(13) +
                                '   when ''nvarchar'' then cast(sql_variant_property(' + @agg_fld + ', ''BaseType'') as varchar(10)) + ''('' + cast(sql_variant_property(' + @agg_fld + ', ''MaxLength'') as varchar(10)) + '')''' + char(13) +
                                '   else cast(sql_variant_property(' + @agg_fld + ', ''BaseType'') as varchar(10))' + char(13) +
                                'end as varchar(100))) as fld_type,' + char(13) +
                                'cast(' + @agg_func + '(' + @agg_fld + ') as sql_variant) as value ' + char(13) +
                                ' from (' + @query + ') as qry ' + char(13) +
                                ' group by ' + @row_fields + ', ' + @col_field  +
                                ',' + @agg_tag;

      set @sub_start = @sub_start + @sub_len + 1;
      set @fld_index = @fld_index + 1;

      exec(@sql);
      end

   --**************************************************************************
   -- Update the input variables to the new temp table and fields
   --**************************************************************************
   set @tname        = @tmp_table;
   set @col_field    = 'cat';
   set @agg_field    = 'value';

   if ( lower(@agg_func) = 'count' ) set @agg_func = 'sum';

   if ( @show_query is not null ) print char(13) + 'Changed table and fields to refer to the temp:  [' + @tname + '] [' + @col_field + '] [' + @agg_field + '] ...';

   --**************************************************************************
   -- Start building the PIVOT SQL
   --**************************************************************************
   set @pivot_sql = @pivot_sql +  'select' + char(13);
   set @pivot_sql = @pivot_sql +  '   pvt.*' + char(13);

   --**************************************************************************
   -- Put all the input arguments in thier proper places in the query
   --**************************************************************************
      set @pivot_sql = @pivot_sql +  'from' + char(13);
   set @pivot_sql = @pivot_sql +  '   (select ' + @row_fields + ', ' + @col_field + ', isnull(' + @agg_field + ',0) as '+ @agg_field + ' from ' + @tname + ') as c' + char(13);
   set @pivot_sql = @pivot_sql +  '       PIVOT' + char(13);
   set @pivot_sql = @pivot_sql +  '          (' + char(13);
--    set @pivot_sql = @pivot_sql +  '          ' + @agg_func + '(' + @agg_field + ')' + char(13);
   set @pivot_sql = @pivot_sql +  '          max(' + @agg_field + ')' + char(13);
   set @pivot_sql = @pivot_sql +  '          for ' + @col_field + ' in' + char(13);
   set @pivot_sql = @pivot_sql +  '             ( ';

   --**************************************************************************
   -- Get all the unique values of the Pivot column in the data table,
   -- these will become the pivoted columns.  For some reason, the PIVOT
   -- chokes if there are more than 147 result columns ... determined
   -- experimentally, so your mileage may vary.
   --**************************************************************************
   if ( @show_query is not null ) print char(13) + 'Creating the #pivot_values table ...';
   create table #pivot_values ( piv_order  integer, fld_type varchar(100), piv_fldsort   varchar(100), piv_fld   varchar(100) );

   set @sql ='insert into #pivot_values select distinct top 147 fld_num, fld_type, col_field, ' + @col_field  + ' from ' + @tname + ' order by col_field, fld_num';

   exec(@sql);

   --**************************************************************************
   -- Loop through the values and add them to the PIVOT query SQL
   --**************************************************************************
   declare @piv_value_count integer;
   set @piv_value_count = 0;
   set @tbl_select = 'select ' + @row_fields + ',';

   declare piv_cursor cursor for
      select piv_fld, fld_type from #pivot_values order by piv_fldsort, piv_order;

   open piv_cursor;
   fetch next from piv_cursor into @piv_value,@piv_type;

   while @@FETCH_STATUS = 0
   begin
      set @pivot_sql  = @pivot_sql +  '[' + rtrim(isnull(@piv_value,'null')) + ']';
      set @tbl_select = @tbl_select + 'cast([' + rtrim(isnull(@piv_value,'null')) + '] as ' + rtrim(isnull(@piv_type,'null')) + ') as [' + rtrim(isnull(@piv_value,'null')) + ']';

      fetch next from piv_cursor into @piv_value,@piv_type;

      if @@FETCH_STATUS = 0
      begin
         set @pivot_sql  = @pivot_sql  +  ', ';
         set @tbl_select = @tbl_select +  ', ' + char(13);
      end

      set @piv_value_count = @piv_value_count + 1;
   end
   close piv_cursor;

   --**************************************************************************
   -- Finish off the PIVOT SQL
   --**************************************************************************
   set @pivot_sql = @pivot_sql +  ' )' + char(13);
   set @pivot_sql = @pivot_sql +  '          ) as pvt' + char(13);

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

         set @pivot_sql = @tbl_select + ' into ' + @dest_table + ' from (' + @pivot_sql + ') as piv ' + char(13);
      end
   else
      set @pivot_sql = @tbl_select + ' from (' + @pivot_sql + ') as piv ' + char(13);

   if ( @show_query is not null ) print char(13) + 'adding order by clause ...';
   set @pivot_sql = @pivot_sql +  'order by' + char(13);
   set @pivot_sql = @pivot_sql +  '   ' + @row_fields + char(13);

   --**************************************************************************
   -- Run the PIVOT SQL
   --**************************************************************************
   if ( @show_query is not null ) print char(13) + 'Running the Pivot SQL ...';
   if ( @piv_value_count > 0 )
      exec(@pivot_sql);
   else
      select 'There was no data to pivot.';

   BEGIN TRY
      exec('drop table ' + @tmp_table);
   END TRY
   BEGIN CATCH
   END CATCH
   return;
end