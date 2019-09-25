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
    <add name="ItemCatalog" providerName="System.Data.SqlClient" connectionString="rpI6ry/2wBLNsQ5tUwxHrH0elOYZFVKD74WTto8wEFY10SoLisumcbNSjx5AI/mr2GWyNMNAMByYL1bFyelRDXJlWr66WnQ3KtDRnLHJ7PLOMY2u6e6nlnPDri2Tpzg3ya7yHKGxln01ffnd/Y5lNg==" />
    <add name="ODBC" connectionString="XYP0rQPBWTAn4nfL8gfEfBIJqhLhN3NOKymdf+vZgtiFryRYg9x4vvBkqU17wsDFjPnhWsoS9za9tU0Q/fGWDtjErrrcjeDNXJVEOWZD3vK5M6HrBKx9w1xxIPWCCyHiioQORcWsOoSreKfEZGdyjg=="/>
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
			<file value="C:\temp\IRMAClient.SW.QA1.log" />
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
