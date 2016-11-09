Icon.DB README

TD 03/31/2014:
there is 1 security-related file that is commented out, but is necessary for deployment:
Icon.DB\Security\iConUser.sql

they will be necessary once TFS Admin deploys to QA and PRD for the initial db creation



KM 2014/02/20

Two warnings are now suppressed for this project.  The team concluded that they did not indicate an actual problem and were only
creating visual noise in our Error List.

The two warnings are:
SQL71558 The object reference [dbo].[item] differs only by case from the object definition [dbo].[Item].
SQL70588 WITH CHECK | NOCHECK option for existing data check enforcement is ignored.


