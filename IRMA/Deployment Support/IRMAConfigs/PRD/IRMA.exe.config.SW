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
    <add name="ItemCatalog" providerName="System.Data.SqlClient" connectionString="PogeDuK3rM8lZmIiKBu9dJ6UgX4KtvvI72ISsCCxphCCImgJcUYH6cIyBOqitZ8wyUMDb4bnIYfNRY9huMSnA0zEGlvyJ6sR4zFNYb8srg94WO8RGSke1f9dG1P6hpM72VPtx1MbU+4A61vSD9NpYg==" />
    <!-- DATABASE CONNECTIONS: formatted for WholeFoods.IRMA DAO objects -->
		<add name="ODBC" connectionString ="XYP0rQPBWTAn4nfL8gfEfPeVe5Hs/aZJxuVgfHjCYqf+48PK0tcp5qs6ARlhmdaxDHlSXpcR6Yfxb+YzmMnWDQ5CKmQGfW+bTmuSxjVMBInwHBb1OzmdTxlp6IDV0DHYRbTZpj7npJwQ20OguWg6Ww=="/>
		<add name="ODBCCrystal" connectionString ="XYP0rQPBWTAn4nfL8gfEfPeVe5Hs/aZJxuVgfHjCYqf+48PK0tcp5qs6ARlhmdaxDHlSXpcR6Yfxb+YzmMnWDX9izwCfFLtP6bz6oKnDH93dVCjtVBRxW2jkvHjSPaNbq75cnghhOKsHD/t+AnN4Ww=="/>
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
      <file value="C:\projects\IRMA\logs\logfile.txt" />
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
