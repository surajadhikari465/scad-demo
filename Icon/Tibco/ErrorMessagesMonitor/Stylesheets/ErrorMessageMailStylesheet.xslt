<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:err="http://www.tibco.com/schemas/ErrorMessagesMonitor/Schemas/Schema.xsd">
	<xsl:template match="/">
		<html>
			<body>
				<p>The following <b><xsl:value-of select="/err:ErrorMessages/err:ErrorCode" /></b> errors occurred in the <b><xsl:value-of select="/err:ErrorMessages/err:Application" /></b>.</p>
				<table border="1">
					<tr>
						<th><b>MessageID</b></th>
						<th><b>ErrorDetails</b></th>
					</tr>
					<xsl:for-each select="/err:ErrorMessages/err:ErrorMessage">
						<tr>
							<td><xsl:value-of select="err:MessageID" /></td>
							<td><xsl:value-of select="err:ErrorDetails" /></td>
						</tr>
					</xsl:for-each>
				</table>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>