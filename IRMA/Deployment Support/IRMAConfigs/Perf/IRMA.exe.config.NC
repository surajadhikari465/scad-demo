<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<configSections>
		<section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler,log4net"/>
	</configSections>
  <appSettings>
	  <!-- identifies this application and the environment it is connected to -->
		<add key="ApplicationGUID" value="B2545A74-4808-4ACA-B5B3-82ABC972A450"/>
		<add key="EnvironmentGUID" value="6A62316D-B6D2-4607-848A-047588CDA1A8"/>

		<!-- Flag set to true if the database connection strings are encrypted -->
		<add key="encryptedConnectionStrings" value="True"/>
  </appSettings>

	<connectionStrings>
		<!-- DATABASE CONNECTIONS: formatted for WholeFoods.Utility.DataAccess objects -->
		<add name="ItemCatalog" providerName="System.Data.SqlClient" connectionString="PogeDuK3rM8lZmIiKBu9dI/g/RZpGOU35jQKGjiuqLdZngNRvVWw9F1iOx/rTJiugUCIVILnJYoAHz3FWp7qTBQ08fZHuvpwm6+DnHfLIgtEqnpTkpKZACdYP7wW1ibvoup+xKaAFpe9X3Cf2d1asQ==" />
		<!-- DATABASE CONNECTIONS: formatted for WholeFoods.IRMA DAO objects -->
		<add name="ODBC" connectionString ="XYP0rQPBWTAn4nfL8gfEfPeVe5Hs/aZJxuVgfHjCYqdxAT0fV3ZQIUBaqeeWtltL7daeNnc2w45i7LRZalJLx0GyWcPgq5HRqaDF2rinXq2Y5rEjV+eBLCoAo62fcc0bm26kMEZFVyhbTUTSjSic1Q=="/>
	</connectionStrings>

	<!-- LOG4NET CONFIGURATION OPTIONS.  See http://logging.apache.org/log4net for more details -->
	<log4net>
		<!-- The logging is configured for the ROOT logger by default.  -->
		<!-- Individual loggers can be added to set more detailed settings.  -->
		<!-- Log levels are:  ALL, DEBUG, INFO, WARN, ERROR, FATAL, OFF -->
		<root>
			<level value="INFO" />
			<appender-ref ref="ConsoleAppender" />
			<appender-ref ref="RollingFileAppender" />
		</root>
		<appender name="ConsoleAppender" type="log4net.Appender.ConsoleAppender">
			<layout type="log4net.Layout.PatternLayout">
				<conversionPattern value="%date [%thread] %-5level %logger - %message%newline" />
			</layout>
		</appender>
		<appender name="RollingFileAppender" type="log4net.Appender.RollingFileAppender">
			<file value="C:\temp\IRMAClient.NC.Perf.log" />
			<appendToFile value="true" />
			<rollingStyle value="Composite" />
			<datePattern value="yyyyMMdd" />
			<maxSizeRollBackups value="10" />
			<maximumFileSize value="10MB" />
			<layout type="log4net.Layout.PatternLayout">
				<conversionPattern value="%date [%thread] %-5level %logger - %message%newline" />
			</layout>
		</appender>
	</log4net>

	<system.net>
		<mailSettings>
			<!-- use for production -->
			<smtp deliveryMethod="Network">
				<network host="smtp.wholefoods.com" />
			</smtp>
		</mailSettings>
	</system.net>

	<startup useLegacyV2RuntimeActivationPolicy="true">
	    <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.0" />
	</startup>
</configuration>
