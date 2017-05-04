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
    <table border="0" cellpadding="0" cellspacing="0" valign="top" width="100%">
      <tr>
        <td width="23%">
          <img height="136" width="136" src="/images/WFM_logo.jpg" alt="Whole Foods Market"/>&#160;&#160;
        </td>
        <td width="77%">
          <h1>
            <xsl:if test="purchaseOrder/credit = 'true'">
              Credit
            </xsl:if>
            <xsl:if test="purchaseOrder/credit = 'false'">
              Product
            </xsl:if>
            Purchase Order
            <br/>
            <xsl:value-of select="purchaseOrder/transferToSubteamName"/>
          </h1>
          <br/>

          <table align="left" border="0" cellpadding="0" cellspacing="2" >
            <tr>
              <td align="left" nowrap="true">
                <h1>
                  Order Number:
                </h1>
              </td>
              <td align="left" nowrap="true" >
                <h1>
                  <xsl:value-of select="purchaseOrder/@id"/>
                </h1>
              </td>
            </tr>
            <tr>
              <td align="left" nowrap="true">
                <h1>
                  Expected Delivery Date:
                </h1>
              </td>
              <td align="left" nowrap="true" >
                <h1>
                  <xsl:value-of select="purchaseOrder/expectedDate"/>
                </h1>
              </td>
            </tr>
            <tr>
              <td align="left" nowrap="true">
                  Create By:
                </td>
              <td align="left" nowrap="true">
                <xsl:value-of select="purchaseOrder/createdByFullName"/>
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
          </table>
        </td>
      </tr>
    </table>
    <br />
  </xsl:template>

  <xsl:template name="form">
    <table border="1" cellpadding="0" cellspacing="0" width="100%">
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
              <th> Purchaser </th>
            </tr>
            <xsl:apply-templates select="purchaseOrder/buyer"/>
          </table>
        </td>
        <td align="left">
          <table align="left" cellpadding="0" cellspacing="2" border="0" width="55%">
            <tr align="left">
              <th> Ship To </th>
            </tr>
            <xsl:apply-templates select="purchaseOrder/shipto"/>
          </table>
        </td>
      </tr>
    </table>
    <br/>

    <xsl:if test="purchaseOrder/comment != ''">
      <table border="1" cellpadding="1" cellspacing="0" width="100%">
        <tr>
          <td nowrap="true">
            <b>
              Notes From <xsl:value-of select="purchaseOrder/createdByFullName"/>:
            </b>
          </td>
          <td class="notes">
            <xsl:value-of select="purchaseOrder/comment"/>
          </td>
        </tr>
      </table>
    </xsl:if>

    <xsl:apply-templates select="purchaseOrder/items"/>
  </xsl:template>
  <!-- ================================ address information ===================================== -->
  <xsl:template match="vendor|buyer|shipto">
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
      <tr>
        <td nowrap="true">&#160;</td>
      </tr>
    </xsl:if>
  </xsl:template>
  <!-- ================================= header information =================================-->
  <xsl:template name="orderInfo">
    <td valign="top">
      <table align="left" border="0" cellpadding="1" cellspacing="1" width="100%">
        <tr align="left">
          <td nowrap="true">
            Order # <b>
              <xsl:value-of select="purchaseOrder/@id"/>
            </b>
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
    <table border="1" cellpadding="0" cellspacing="0" width="100%">
      <tr align="center">
        <td align="center">
          <b>VIN</b>
        </td>
        <td align="center">
          <b>UPC</b>
        </td>
        <td align="center">
          <b>Brand</b>
        </td>
        <td align="center">
          <b>Description</b>
        </td>
        <td align="center">
          <b>Qty</b>
        </td>
        <td align="center">
          <b>Order UOM</b>
        </td>
        <td align="center">
          <b>Total Eaches</b>
        </td>
        <td align="center">
          <b>Case Pack</b>
        </td>
        <td align="center">
          <b>Order UOM Cost</b>
        </td>
        <td align="center">
          <b>Total Discount</b>
        </td>
        <td align="center">
          <b>Net Cost</b>
        </td>
        <td align="center">
          <b>Net Unit Cost</b>
        </td>
        <td align="center">
          <b>Total</b>
      </td>
      </tr>
      <xsl:apply-templates select="item"/>
      <tr bgcolor="black">
        <td colspan="13">&#160;</td>
      </tr>
      <tr>
        <td class="totals" align="left" nowrap="true">
          <b>Totals:</b>
        </td>
        <td>&#160;</td>
        <td>&#160;</td>
        <td>&#160;</td>
        <td align="right">
          <b>
            <xsl:if test="/purchaseOrder/credit = 'true'">
              <xsl:value-of select="format-number(-1*sum(item/quantity), '#,###.##')"/>
            </xsl:if>
            <xsl:if test="/purchaseOrder/credit = 'false'">
              <xsl:value-of select="format-number(sum(item/quantity), '#,###.##')"/>
            </xsl:if>
          </b>
        </td>
        <td>&#160;</td>
        <td align="right">
          <b>
            <xsl:if test="/purchaseOrder/credit = 'true'">
              <xsl:value-of select="format-number(-1*sum(item/eaches), '#,###.##')"/>
            </xsl:if>
            <xsl:if test="/purchaseOrder/credit = 'false'">
              <xsl:value-of select="format-number(sum(item/eaches), '#,###.##')"/>
            </xsl:if>
          </b>
        </td>
        <td>&#160;</td>
        <td>&#160;</td>
        <td>&#160;</td>
        <td>&#160;</td>
        <td>&#160;</td>
        <td align="right">
          <b>
            <xsl:if test="/purchaseOrder/credit = 'true'">
              <xsl:value-of select="format-number(-1*sum(item/netLineItemCost), '#,##0.00')"/>
            </xsl:if>
            <xsl:if test="/purchaseOrder/credit = 'false'">
              <xsl:value-of select="format-number(sum(item/netLineItemCost), '#,##0.00')"/>
            </xsl:if>
          </b>
        </td>
      </tr>
      <tr>
        <td colspan="13" align="right">
          <br/>
          <h1>
            Order Cost:&#160;
            <xsl:if test="/purchaseOrder/credit = 'true'">
              <xsl:value-of select="format-number(-1*sum(item/netLineItemCost), '#,##0.00')"/>
            </xsl:if>
            <xsl:if test="/purchaseOrder/credit = 'false'">
              <xsl:value-of select="format-number(sum(item/netLineItemCost), '#,##0.00')"/>
            </xsl:if>
          </h1>
        </td>
      </tr>
    </table>
  </xsl:template>

  <xsl:template match="item">
    <tr>
      <td align="left">
        <xsl:value-of select="@vendorPartNum"/>&#160;
      </td>
      <td align="right">
        <xsl:value-of select="WFMSKU"/>
      </td>
      <td style="font-size:10px; text-align:left">
        <xsl:value-of select="brandName"/>
      </td>
      <td style="font-size:10px; text-align:left">
        <xsl:value-of select="productName"/>&#160;<xsl:value-of select="productName/@originCOPInfo"/>
        <br/>
        <xsl:value-of select="itemCasePack"/>/<xsl:value-of select="packSize"/>&#160;<xsl:value-of select="itemUOMName"/>
      </td>
      <td align="right">
        <xsl:if test="/purchaseOrder/credit = 'true'">
          <xsl:value-of select="-1*quantity"/>
        </xsl:if>
        <xsl:if test="/purchaseOrder/credit = 'false'">
          <xsl:value-of select="quantity"/>
        </xsl:if>
      </td>
      <td align="left">
        <xsl:value-of select="vendorOrderUOMName"/>
      </td>
      <td align="right">
        <xsl:if test="/purchaseOrder/credit = 'true'">
          <xsl:value-of select="format-number(-1*eaches, '#,###')"/>
        </xsl:if>
        <xsl:if test="/purchaseOrder/credit = 'false'">
          <xsl:value-of select="format-number(eaches, '#,###')"/>
        </xsl:if>
      </td>
      <td align="right">
        <xsl:value-of select="casePack"/>
      </td>
      <td align="right">
        <xsl:value-of select='format-number(vendorOrderUOMCost, "0.00")'/>
      </td>
      <td align="right">
        <xsl:if test="/purchaseOrder/credit = 'true'">
          <xsl:value-of select='format-number(totalDiscount, "0.00")'/>
        </xsl:if>
        <xsl:if test="/purchaseOrder/credit = 'false'">
          <xsl:value-of select='format-number(-1*totalDiscount, "0.00")'/>
        </xsl:if>
      </td>
      <td align="right">
        <xsl:value-of select='format-number(netOrderUOMCost, "0.00")'/>
      </td>
      <td align="right">
        <xsl:value-of select='format-number(netUnitCost, "0.00")'/>
      </td>
      <td align="right">
        <xsl:if test="/purchaseOrder/credit = 'true'">
          <xsl:value-of select='format-number(-1*netLineItemCost, "#,##0.00")'/>
        </xsl:if>
        <xsl:if test="/purchaseOrder/credit = 'false'">
          <xsl:value-of select='format-number(netLineItemCost, "#,##0.00")'/>
        </xsl:if>
      </td>



    </tr>
  </xsl:template>
  <!-- ==========================  END ITEM PROCESSING  ===================================== -->

  <xsl:template name="footer">
    <br/>
    <p>
      <div>
        <xsl:if test="purchaseOrder/footerMsg != ''">
          <table border="1" cellpadding="0" cellspacing="0" width="100%">
            <tr align="center">
              <td>
                <xsl:value-of select="purchaseOrder/footerMsg"/>
              </td>
            </tr>
          </table>
        </xsl:if>
      </div>
    </p>
    <p>
      <div>
        *********** &gt; END OF WFM ORDER # <xsl:value-of select="purchaseOrder/@id"/> &lt; ***********
      </div>
    </p>
  </xsl:template>
</xsl:stylesheet>
