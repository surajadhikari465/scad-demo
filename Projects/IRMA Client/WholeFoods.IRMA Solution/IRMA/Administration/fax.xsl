<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:fo="http://www.w3.org/1999/XSL/Format" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template match="/">
		<html>
			<head>
				<title>Whole Foods Market Order</title>
				<BASE HREF="http://dvo.wholefoods.com" />
				<style type="text/css">
					div {font-family: verdana; font-size: 12 }
					h1 {font-family: verdana; font-size: 16; font-weight: bold }
					td {font-family: verdana; font-size: 12 }
					th {font-family: verdana; font-size: 12; text-decoration: underline; font-weight: normal }
					p {font-family: verdana; font-size: 18 }
					notes { font-family: verdana; font-size:12; font-weight: bold }
					acknote { font-family: verdana; font-size:14 }
				</style>
			</head>
			<body bgcolor="#ffffff" text="#000000">
				<xsl:call-template name="header"/>
				<xsl:call-template name="form"/>
				<xsl:call-template name="footer"/>
			</body>
		</html>
	</xsl:template>
  <!--========================   Header Information  ===========================================-->
	<xsl:template name="header">
		<br/>
		<table border="0" cellpadding="0" cellspacing="0" valign="top" width="75%">
			<tr>
				<td width="77%">
					<h1>
						Whole Foods Market Purchase Order # <xsl:value-of select="purchaseOrder/@id"/>
					</h1>
					
					<b><font color="red">If this order does not meet the minimum for free shipping, please contact the store buyer.</font></b>
					<br/>
					<br/>
					
					<table align="left" border="0" cellpadding="0" cellspacing="2" >
						<tr>
							<td align="left" nowrap="true">
								Order Number:
							</td>
							<td align="left" nowrap="true" >
								<xsl:value-of select="purchaseOrder/@id"/>
							</td>
						</tr>
						<tr>
							<td align="left" nowrap="true">
								Order Date:
							</td>
							<td align="left" nowrap="true" >
								<xsl:value-of select="purchaseOrder/@orderDate"/>
							</td>
						</tr>
						<tr>
							<td align="left" nowrap="true">
								Store No:
							</td>
							<td align="left" nowrap="true">
								<xsl:value-of select="purchaseOrder/customer"/>
							</td>
						</tr>
						<tr>
							<td align="left" nowrap="true">
								Account No:
							</td>
							<td align="left" nowrap="true">
								<xsl:value-of select="purchaseOrder/customer_ID"/>
							</td>
						</tr>
						<tr>
							<td align="left" nowrap="true">
								Subteam:
							</td>
							<td align="left" nowrap="true">
								<xsl:value-of select="purchaseOrder/subteamName"/>
							</td>
						</tr>
						<tr>
							<td align="left" nowrap="true">
								Buyer:
							</td>
							<td align="left" nowrap="true">
								<xsl:value-of select="purchaseOrder/buyer/name"/>
							</td>
						</tr>
					</table>
				</td>
				<td width="23%">
					<img height="136" width="136" src="/images/WFM_logo.jpg"/>&#160;&#160;
				</td>
			</tr>
		</table>
		<br />
	</xsl:template>
		
	<xsl:template name="form">
		<table border="0" cellpadding="0" cellspacing="0" width="50%">
			<tr align="left">
				<td align="left">
					<table align="left" cellpadding="0" cellspacing="2" border="0" width="55%">
						<tr align="left">
							<th> Vendor </th>
						</tr>
						<xsl:apply-templates select="purchaseOrder/vendor"/>
					</table>
				</td>
				<td align="left">
					<table align="left" cellpadding="0" cellspacing="2" border="0" width="55%">
						<tr align="left">
							<th> Ship To/Bill To </th>
						</tr>
						<xsl:apply-templates select="purchaseOrder/shipto"/>
					</table>
				</td>
			</tr>
		</table>
		<br/>
		
		<table>
			<tr>
				<th>
					Notes From <xsl:value-of select="purchaseOrder/shipto/name"/>:
				</th>
				<td class="notes">
					<xsl:value-of select="purchaseOrder/comment"/>
				</td>
			</tr>
		</table>
		
		<xsl:apply-templates select="purchaseOrder/items"/>
	</xsl:template>
	<!-- ================================ address information ===================================== -->
	<xsl:template match="vendor|shipto">
		<tr>
			<td nowrap="true">
				<xsl:value-of select="name"/>
			</td>
		</tr>
		<tr>
			<td nowrap="true">
				<xsl:value-of select="address"/>
			</td>
		</tr>
		<xsl:if test="address2 != ''">
			<tr>
				<td nowrap="true">
					<xsl:value-of select="address2"/>
				</td>
			</tr>
		</xsl:if>
		<tr>
			<td nowrap="true">
				<xsl:value-of select="city"/>,
				<xsl:value-of select="state "/>
				&#160;
				<xsl:value-of select="zip"/>
			</td>
		</tr>
		<tr>
			<td nowrap="true">
				Phone: &#160;<xsl:value-of select="phone"/>
			</td>
		</tr>
		<tr>
			<td nowrap="true">
				Fax: &#160;<xsl:value-of select="fax"/>
			</td>
		</tr>
		<xsl:if test="address2 = ''">
			<tr><td nowrap="true">&#160;</td></tr>
		</xsl:if>
	</xsl:template>
	<!-- ================================= header information =================================-->
	<xsl:template name="orderInfo">
		<td valign="top">
			<table align="left" border="0" cellpadding="1" cellspacing="1" width="100%">
				<tr align="left">
					<td nowrap="true">
						Order # <b><xsl:value-of select="purchaseOrder/@id"/></b>
						<br/> Order Date: <xsl:value-of select="purchaseOrder/@orderDate"/>
						<br/> Customer ID# <xsl:value-of select="purchaseOrder/customerID"/>
					</td>
				</tr>
			</table>
		</td>
	</xsl:template>
	<!-- ====================================  Notes ========================================= -->
	<xsl:template match="comment">
		<xsl:param name="buyer"/>
		<table>
			<tr>
				<th>
					Notes From <xsl:value-of select="$buyer"/>:
				</th>
				<td class="notes">
					<xsl:value-of select="purchaseOrder/comment"/>
				</td>
			</tr>
		</table>
	</xsl:template>
	<!-- =============================  BEGIN ITEM PROCESSING  ================================-->
	<xsl:template match="items">
		<br/>
		<!-- create the header title row and then loop through all the item elments -->
		<table border="0" cellpadding="2" cellspacing="2" width="90%">
			<tr align="left">
				<th align="left">Line</th>
				<th align="left">Item No.</th>
				<th align="left">Qty</th>
				<th align="left">Description</th>
        <th align="left">Packsize</th>
				<th align="left">Size</th>
				<th align="left">UPC</th>
			</tr>
			<xsl:apply-templates select="item"/>
			<tr>
				<td colspan="6"><hr/></td>
			</tr>
			<tr>
				<td></td>
				<td align="left" nowrap="true">Totals:</td>
				<td align="left">
					<xsl:value-of select="sum( item/quantity )"/>
				</td>
				<td></td>
				<td></td>
				<td></td>
			</tr>
		</table>
	</xsl:template>
	
	<xsl:template match="item">
		<tr>
			<td align="left">
				<xsl:number value="position()" />
			</td>
			<td align="left">
				<xsl:value-of select="@vendorPartNum"/>
			</td>
			<td align="left">
				<xsl:value-of select="quantity"/>&#160;&#160;<xsl:value-of select="quantity/@Unit"/>
			</td>
			<td align="left">
				<xsl:value-of select="productName"/>
			</td>
			<td align="left">
        <xsl:value-of select="casePack"/>
			</td>
      <td align="left">
        <xsl:value-of select="packSize"/>&#160;&#160;<xsl:value-of select="UOM"/>
      </td>
      <td align="left">
				<xsl:value-of select="WFMSKU"/>
			</td>
		</tr>
	</xsl:template>
	<!-- ==========================  END ITEM PROCESSING  ===================================== -->
	
	<xsl:template name="footer">
		<br/>
		<p>
			<div>
				*********** &gt; END OF WFM ORDER # <xsl:value-of select="purchaseOrder/@id"/> &lt; ***********
			</div>
		</p>
	</xsl:template>
</xsl:stylesheet>