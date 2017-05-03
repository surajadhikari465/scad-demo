CREATE VIEW [dbo].[zz_deprecated_vw_IndexFragmentation]
AS
     SELECT
            [object_id],
            index_id,
            partition_number,
            avg_fragmentation_in_percent, 
            page_count
     FROM sys.dm_db_index_physical_stats (DB_ID(), NULL, NULL , NULL, N'LIMITED')
     WHERE index_id > 0                -- Ignore heaps
            AND page_count > 25        -- Ignore small tables

