<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<configSections>
		<section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler,log4net"/>
	</configSections>
  <appSettings>
	  <!-- identifies this application and the environment it is connected to -->
	  <add key="ApplicationGUID" value="B15FB40B-D5F3-46D4-A0A4-64CC7B6B7DB5"/>
	  <add key="EnvironmentGUID" value="8A5FF21E-9ACE-403E-8BFB-7338C71151F5"/>
	  <!-- Flag set to true if the database connection strings are encrypted -->
	  <add key="encryptedConnectionStrings" value="true"/>
  </appSettings>

	<connectionStrings>
		<!-- DATABASE CONNECTIONS: formatted for WholeFoods.Utility.DataAccess objects -->
		<add name="ItemCatalog" providerName="System.Data.SqlClient" connectionString="PogeDuK3rM8lZmIiKBu9dDTBhgV1nz5YUvNIoIqUT1ZftFgbQLObDZCDf/9jPBLH2dEI9dxtOAg36ttFBZgHUAqbnW/ai91d9rEcA9Tu/PpJURldhU+elQ+0gbrhMTcMbtAMeEXe8MwWqttXVsdH3A==" />
		<!-- DATABASE CONNECTIONS: formatted for WholeFoods.IRMA DAO objects -->
		<add name="ODBC" connectionString ="XYP0rQPBWTAn4nfL8gfEfPeVe5Hs/aZJxuVgfHjCYqeGULtPaEWBKVRAjWTvRet9ZJ7G73dnylLL8K6cBNIgGgYnyWp2dJzC8wqL8T8J4HUe0mCogDW+AEXV/NZiupQ9Oxp2/7/z9IPZg+8paDXgSQ=="/>
		<add name="ODBCCrystal" connectionString ="XYP0rQPBWTAn4nfL8gfEfPeVe5Hs/aZJxuVgfHjCYqeGULtPaEWBKVRAjWTvRet9ZJ7G73dnylLL8K6cBNIgGs9BOz+vlYCBh24RvTtgNALdmNb+K6L7P+l5ZEfrhnuryZSwYEU5KyKGDSb8nuTQlA=="/>
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
			<file value="C:\IRMA\Production\logs\ClientLog.txt" />
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
