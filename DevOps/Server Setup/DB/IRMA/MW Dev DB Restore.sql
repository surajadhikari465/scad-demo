/*
The following VMs were supposed to have an “e:\sql_data_02”  mounted volume:
odwd6803.wfm.pvt (mw)
odwd6807.wfm.pvt (xx)
odwd6805.wfm.pvt (so)

Divide DB files across:
e:\sql_data_01\files\
e:\sql_data_02\files\

Log files go here:
e:\sql_logs_01\files\
*/


USE [master]; ALTER DATABASE [ItemCatalog] SET restricted_user WITH rollback immediate

use master
RESTORE DATABASE [ItemCatalog]
FROM DISK = N'\\atx-nas\irmabackups\mw-prd3\mwp\itemcatalog\itemcatalog.Full.BAK' WITH FILE = 1,   
MOVE N'ItemCatalog_Data' TO N'e:\SQL_DATA_01\files\ItemCatalogData.mdf',  
MOVE N'ItemCatalog2' TO N'e:\SQL_DATA_01\files\ItemCatalog2.ndf',  
MOVE N'ItemCatalog3' TO N'e:\SQL_DATA_01\files\ItemCatalog3.ndf',  
MOVE N'ItemCatalog4' TO N'e:\SQL_DATA_02\files\ItemCatalog4.ndf',
MOVE N'Warehouse_Data' TO N'e:\SQL_DATA_01\files\WarehouseData.ndf',  
MOVE N'ItemCatalog_Log' TO N'e:\SQL_LOGS_01\files\ItemCatalogLog.ldf',  
--NOUNLOAD, REPLACE, NORECOVERY, STATS = 10 -- if diff restore to follow...
NOUNLOAD, REPLACE, STATS = 10

USE [master]; ALTER DATABASE [ItemCatalog] SET restricted_user WITH rollback immediate

/*
orig:
use master
RESTORE DATABASE [ItemCatalog]
FROM DISK = N'\\atx-nas\irmabackups\mw-prd3\mwp\itemcatalog\itemcatalog.Full.BAK' WITH FILE = 1,   
MOVE N'ItemCatalog_Data' TO N'e:\SQL_DATA_01\files\ItemCatalog.mdf',  
MOVE N'ItemCatalog2' TO N'e:\SQL_DATA_01\files\ItemCatalog_1.ndf',  
MOVE N'ItemCatalog3' TO N'e:\SQL_DATA_01\files\ItemCatalog_2.ndf',  
MOVE N'ItemCatalog4' TO N'e:\SQL_DATA_01\files\ItemCatalog_3.ndf',
MOVE N'Warehouse_Data' TO N'e:\SQL_DATA_01\files\ItemCatalog_4.ndf',  
MOVE N'ItemCatalog_Log' TO N'e:\SQL_LOGS_01\files\ItemCatalog_5.ldf',  
--NOUNLOAD, REPLACE, NORECOVERY, STATS = 10 -- if diff restore to follow...
NOUNLOAD, REPLACE, STATS = 10

*/


/*
Refresh 11/7/18:
10 percent processed.
20 percent processed.
30 percent processed.
40 percent processed.
50 percent processed.
60 percent processed.
70 percent processed.
80 percent processed.
90 percent processed.
100 percent processed.
Processed 18481448 pages for database 'ItemCatalog', file 'ItemCatalog_Data' on file 1.
Processed 25612584 pages for database 'ItemCatalog', file 'ItemCatalog2' on file 1.
Processed 27099288 pages for database 'ItemCatalog', file 'ItemCatalog3' on file 1.
Processed 29385448 pages for database 'ItemCatalog', file 'ItemCatalog4' on file 1.
Processed 112 pages for database 'ItemCatalog', file 'Warehouse_Data' on file 1.
Processed 56877 pages for database 'ItemCatalog', file 'ItemCatalog_Log' on file 1.
Converting database 'ItemCatalog' from version 655 to the current version 869.
Database 'ItemCatalog' running the upgrade step from version 655 to version 668.
Database 'ItemCatalog' running the upgrade step from version 668 to version 669.
Database 'ItemCatalog' running the upgrade step from version 669 to version 670.
Database 'ItemCatalog' running the upgrade step from version 670 to version 671.
Database 'ItemCatalog' running the upgrade step from version 671 to version 672.
Database 'ItemCatalog' running the upgrade step from version 672 to version 673.
Database 'ItemCatalog' running the upgrade step from version 673 to version 674.
Database 'ItemCatalog' running the upgrade step from version 674 to version 675.
Database 'ItemCatalog' running the upgrade step from version 675 to version 676.
Database 'ItemCatalog' running the upgrade step from version 676 to version 677.
Database 'ItemCatalog' running the upgrade step from version 677 to version 679.
Database 'ItemCatalog' running the upgrade step from version 679 to version 680.
Database 'ItemCatalog' running the upgrade step from version 680 to version 681.
Database 'ItemCatalog' running the upgrade step from version 681 to version 682.
Database 'ItemCatalog' running the upgrade step from version 682 to version 683.
Database 'ItemCatalog' running the upgrade step from version 683 to version 684.
Database 'ItemCatalog' running the upgrade step from version 684 to version 685.
Database 'ItemCatalog' running the upgrade step from version 685 to version 686.
Database 'ItemCatalog' running the upgrade step from version 686 to version 687.
Database 'ItemCatalog' running the upgrade step from version 687 to version 688.
Database 'ItemCatalog' running the upgrade step from version 688 to version 689.
Database 'ItemCatalog' running the upgrade step from version 689 to version 690.
Database 'ItemCatalog' running the upgrade step from version 690 to version 691.
Database 'ItemCatalog' running the upgrade step from version 691 to version 692.
Database 'ItemCatalog' running the upgrade step from version 692 to version 693.
Database 'ItemCatalog' running the upgrade step from version 693 to version 694.
Database 'ItemCatalog' running the upgrade step from version 694 to version 695.
Database 'ItemCatalog' running the upgrade step from version 695 to version 696.
Database 'ItemCatalog' running the upgrade step from version 696 to version 697.
Database 'ItemCatalog' running the upgrade step from version 697 to version 698.
Database 'ItemCatalog' running the upgrade step from version 698 to version 699.
Database 'ItemCatalog' running the upgrade step from version 699 to version 700.
Database 'ItemCatalog' running the upgrade step from version 700 to version 701.
Database 'ItemCatalog' running the upgrade step from version 701 to version 702.
Database 'ItemCatalog' running the upgrade step from version 702 to version 703.
Database 'ItemCatalog' running the upgrade step from version 703 to version 704.
Database 'ItemCatalog' running the upgrade step from version 704 to version 705.
Database 'ItemCatalog' running the upgrade step from version 705 to version 706.
Database 'ItemCatalog' running the upgrade step from version 706 to version 770.
Database 'ItemCatalog' running the upgrade step from version 770 to version 771.
Database 'ItemCatalog' running the upgrade step from version 771 to version 772.
Database 'ItemCatalog' running the upgrade step from version 772 to version 773.
Database 'ItemCatalog' running the upgrade step from version 773 to version 774.
Database 'ItemCatalog' running the upgrade step from version 774 to version 775.
Database 'ItemCatalog' running the upgrade step from version 775 to version 776.
Database 'ItemCatalog' running the upgrade step from version 776 to version 777.
Database 'ItemCatalog' running the upgrade step from version 777 to version 778.
Database 'ItemCatalog' running the upgrade step from version 778 to version 779.
Database 'ItemCatalog' running the upgrade step from version 779 to version 780.
Database 'ItemCatalog' running the upgrade step from version 780 to version 781.
Database 'ItemCatalog' running the upgrade step from version 781 to version 782.
Database 'ItemCatalog' running the upgrade step from version 782 to version 801.
Database 'ItemCatalog' running the upgrade step from version 801 to version 802.
Database 'ItemCatalog' running the upgrade step from version 802 to version 803.
Database 'ItemCatalog' running the upgrade step from version 803 to version 804.
Database 'ItemCatalog' running the upgrade step from version 804 to version 805.
Database 'ItemCatalog' running the upgrade step from version 805 to version 806.
Database 'ItemCatalog' running the upgrade step from version 806 to version 807.
Database 'ItemCatalog' running the upgrade step from version 807 to version 808.
Database 'ItemCatalog' running the upgrade step from version 808 to version 809.
Database 'ItemCatalog' running the upgrade step from version 809 to version 810.
Database 'ItemCatalog' running the upgrade step from version 810 to version 811.
Database 'ItemCatalog' running the upgrade step from version 811 to version 812.
Database 'ItemCatalog' running the upgrade step from version 812 to version 813.
Database 'ItemCatalog' running the upgrade step from version 813 to version 814.
Database 'ItemCatalog' running the upgrade step from version 814 to version 815.
Database 'ItemCatalog' running the upgrade step from version 815 to version 816.
Database 'ItemCatalog' running the upgrade step from version 816 to version 817.
Database 'ItemCatalog' running the upgrade step from version 817 to version 818.
Database 'ItemCatalog' running the upgrade step from version 818 to version 819.
Database 'ItemCatalog' running the upgrade step from version 819 to version 820.
Database 'ItemCatalog' running the upgrade step from version 820 to version 821.
Database 'ItemCatalog' running the upgrade step from version 821 to version 822.
Database 'ItemCatalog' running the upgrade step from version 822 to version 823.
Database 'ItemCatalog' running the upgrade step from version 823 to version 824.
Database 'ItemCatalog' running the upgrade step from version 824 to version 825.
Database 'ItemCatalog' running the upgrade step from version 825 to version 826.
Database 'ItemCatalog' running the upgrade step from version 826 to version 827.
Database 'ItemCatalog' running the upgrade step from version 827 to version 828.
Database 'ItemCatalog' running the upgrade step from version 828 to version 829.
Database 'ItemCatalog' running the upgrade step from version 829 to version 830.
Database 'ItemCatalog' running the upgrade step from version 830 to version 831.
Database 'ItemCatalog' running the upgrade step from version 831 to version 832.
Database 'ItemCatalog' running the upgrade step from version 832 to version 833.
Database 'ItemCatalog' running the upgrade step from version 833 to version 834.
Database 'ItemCatalog' running the upgrade step from version 834 to version 835.
Database 'ItemCatalog' running the upgrade step from version 835 to version 836.
Database 'ItemCatalog' running the upgrade step from version 836 to version 837.
Database 'ItemCatalog' running the upgrade step from version 837 to version 838.
Database 'ItemCatalog' running the upgrade step from version 838 to version 839.
Database 'ItemCatalog' running the upgrade step from version 839 to version 840.
Database 'ItemCatalog' running the upgrade step from version 840 to version 841.
Database 'ItemCatalog' running the upgrade step from version 841 to version 842.
Database 'ItemCatalog' running the upgrade step from version 842 to version 843.
Database 'ItemCatalog' running the upgrade step from version 843 to version 844.
Database 'ItemCatalog' running the upgrade step from version 844 to version 845.
Database 'ItemCatalog' running the upgrade step from version 845 to version 846.
Database 'ItemCatalog' running the upgrade step from version 846 to version 847.
Database 'ItemCatalog' running the upgrade step from version 847 to version 848.
Database 'ItemCatalog' running the upgrade step from version 848 to version 849.
Database 'ItemCatalog' running the upgrade step from version 849 to version 850.
Database 'ItemCatalog' running the upgrade step from version 850 to version 851.
Database 'ItemCatalog' running the upgrade step from version 851 to version 852.
Database 'ItemCatalog' running the upgrade step from version 852 to version 853.
Database 'ItemCatalog' running the upgrade step from version 853 to version 854.
Database 'ItemCatalog' running the upgrade step from version 854 to version 855.
Database 'ItemCatalog' running the upgrade step from version 855 to version 856.
Database 'ItemCatalog' running the upgrade step from version 856 to version 857.
Database 'ItemCatalog' running the upgrade step from version 857 to version 858.
Database 'ItemCatalog' running the upgrade step from version 858 to version 859.
Database 'ItemCatalog' running the upgrade step from version 859 to version 860.
Database 'ItemCatalog' running the upgrade step from version 860 to version 861.
Database 'ItemCatalog' running the upgrade step from version 861 to version 862.
Database 'ItemCatalog' running the upgrade step from version 862 to version 863.
Database 'ItemCatalog' running the upgrade step from version 863 to version 864.
Database 'ItemCatalog' running the upgrade step from version 864 to version 865.
Database 'ItemCatalog' running the upgrade step from version 865 to version 866.
Database 'ItemCatalog' running the upgrade step from version 866 to version 867.
Database 'ItemCatalog' running the upgrade step from version 867 to version 868.
Database 'ItemCatalog' running the upgrade step from version 868 to version 869.
RESTORE DATABASE successfully processed 100635757 pages in 3826.689 seconds (205.456 MB/sec).


*/