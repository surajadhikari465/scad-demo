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
    <!-- Server: idd-ma   Database: ItemCatalog_Test -->
    <!-- DATABASE CONNECTIONS: formatted for WholeFoods.Utility.DataAccess objects -->
    <add name="ItemCatalog" providerName="System.Data.SqlClient" connectionString="FktsCUEMiAzpkeqE0gu12Qw8Hd6peTSTOjlPAkr8PsAZelTAVsu7uT8K2zNIk6aPEIfs8VT7NHByaVzP3kZeeyUpx62jDJgIruSfdbjoD3W8uNlvs43f4y8GN7jQfPoT" />
    <!-- DATABASE CONNECTIONS: formatted for WholeFoods.IRMA DAO objects -->
		<add name="ODBC" connectionString ="XYP0rQPBWTAn4nfL8gfEfD3RfZvm7m3revACrFBp7c+TnWx7KXKzChMPxLDbmO5OHwXpQoNEE2MlDq4jovphhTNnFn6Ugl3l3NdI7eeDfi4bNnLqxWTeMP7UH8NUmU7M"/>
		<add name="ODBCCrystal" connectionString ="XYP0rQPBWTAn4nfL8gfEfD3RfZvm7m3revACrFBp7c+TnWx7KXKzChMPxLDbmO5OHwXpQoNEE2MlDq4jovphhdDf6CnoJ2d8wBngdm6g11ivhiQSbtAaKacq86VRy7AP"/>
  </connectionStrings>

  <!-- LOG4NET CONFIGURATION OPTIONS.  See http://logging.apache.org/log4net for more details -->
  <log4net>
    <!-- The logging is configured for the ROOT logger by default.  -->
    <!-- Individual loggers can be added to set more detailed settings.  -->
    <!-- Log levels are:  ALL, DEBUG, INFO, WARN, ERROR, FATAL, OFF -->
    <root>
      <level value="INFO" />
      <appender-ref ref="ConsoleAppender" />
      <appender-ref ref="RollingFile" />
    </root>
    <appender name="ConsoleAppender" type="log4net.Appender.ConsoleAppender">
      <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date [%thread] %-5level %logger - %message%newline" />
      </layout>
    </appender>
    <appender name="RollingFile" type="log4net.Appender.RollingFileAppender">
	<file value="./logs/logfile.txt" />
	<appendToFile value="true" />
	<rollingStyle value="Size" />
	<datePattern value="yyyyMMdd" />
	<maxSizeRollBackups value="10" />
	<maximumFileSize value="20MB" /> 
	<staticLogFileName value="true" />
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
