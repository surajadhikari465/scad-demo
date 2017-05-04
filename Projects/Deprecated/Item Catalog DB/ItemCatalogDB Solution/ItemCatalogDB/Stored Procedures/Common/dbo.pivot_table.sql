if exists (select * from dbo.sysobjects where  name = 'pivot_table') drop procedure pivot_table;
go

create procedure dbo.pivot_table
   (
   @tname         varchar(100),
   @row_fields    varchar(8000),
   @col_field     varchar(100),
   @agg_func_list varchar(8000),
   @where_clause  varchar(8000) = null,
   @dest_table    varchar(200) = null,
   @show_query    char(1) = null
   )
as
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
   declare @sql        varchar(1000);
   declare @piv_value  varchar(50);

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
   declare @agg_func   varchar(50);
   declare @agg_lbl    varchar(100);
   declare @agg_tag    varchar(100);
   declare @agg_field  varchar(1000);
   declare @agg_fld    varchar(20);
   declare @tmp_table  varchar(20);
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
   create table #pivot_values ( piv_order  integer, piv_fldsort   varchar(100), piv_fld   varchar(100) );

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
go
