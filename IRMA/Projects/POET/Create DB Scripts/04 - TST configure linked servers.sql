
EXEC sp_addlinkedserver[idd-sw]
EXEC sp_addlinkedserver[idd-sp]
EXEC sp_addlinkedserver[idd-rm]
EXEC sp_addlinkedserver[idd-pn]
EXEC sp_addlinkedserver[idd-nc]
EXEC sp_addlinkedserver[idd-na]
EXEC sp_addlinkedserver[idd-ne]
EXEC sp_addlinkedserver[idd-mw]
EXEC sp_addlinkedserver[idd-ma]
EXEC sp_addlinkedserver[idd-fl]
GO


EXEC sp_addlinkedsrvlogin [idd-sw], false, 'MultiPOToolUser', 'IRMASchedJobs','W0lver1ne'
EXEC sp_addlinkedsrvlogin [idd-sp], false, 'MultiPOToolUser', 'IRMASchedJobs','H3r0e$H3r0e$'
EXEC sp_addlinkedsrvlogin [idd-rm], false, 'MultiPOToolUser', 'IRMASchedJobs','L@v3rn3'
EXEC sp_addlinkedsrvlogin [idd-pn], false, 'MultiPOToolUser', 'IRMASchedJobs','pm3Gamma'
EXEC sp_addlinkedsrvlogin [idd-nc], false, 'MultiPOToolUser', 'IRMASchedJobs','Pal0m1n0'
EXEC sp_addlinkedsrvlogin [idd-na], false, 'MultiPOToolUser', 'IRMASchedJobs','ShepH@rd.'
EXEC sp_addlinkedsrvlogin [idd-ne], false, 'MultiPOToolUser', 'IRMASchedJobs','ShepH@rd.'
EXEC sp_addlinkedsrvlogin [idd-mw], false, 'MultiPOToolUser', 'IRMASchedJobs','m1550ur1'
EXEC sp_addlinkedsrvlogin [idd-ma], false, 'MultiPOToolUser', 'IRMASchedJobs','Turk#yday'
EXEC sp_addlinkedsrvlogin [idd-fl], false, 'MultiPOToolUser', 'IRMASchedJobs','Sp1d#rm@n!'
GO
