<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Default" %>
<!DOCTYPE html>
<!--[if lt IE 7]>      <html class="no-js lt-ie9 lt-ie8 lt-ie7"> <![endif]-->
<!--[if IE 7]>         <html class="no-js lt-ie9 lt-ie8"> <![endif]-->
<!--[if IE 8]>         <html class="no-js lt-ie9"> <![endif]-->
<!--[if gt IE 8]><!--> <html class="no-js"> <!--<![endif]-->
    <head>
        <meta charset="utf-8">
        <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
        <title>WFM PO Reports</title>
        <meta name="description" content="">
        <meta name="viewport" content="width=device-width">

        <!-- Place favicon.ico and apple-touch-icon.png in the root directory -->

        <link rel="stylesheet" href="css/normalize.css">
        <link href="css/south-street/jquery-ui-1.10.2.custom.min.css" rel="stylesheet" type="text/css" />
        <link href="css/custom-theme/jquery-ui-1.10.0.custom.css" rel="stylesheet" type="text/css" />
        <link rel="stylesheet" href="js/vendor/jqwidgets/styles/jqx.base.css">
        <link rel="stylesheet" href="css/Elderkin/Elderkin.css">
        <link rel="stylesheet" href="css/main.css">
        <script src="js/vendor/modernizr-2.6.2.min.js"></script>    
    </head>
    <body>
        <!--[if lt IE 7]>
        <link rel="stylesheet" type="text/css" href="css/custom-theme/jquery.ui.1.10.0.ie.css" />
            <p class="chromeframe">You are using an <strong>outdated</strong> browser. Please <a href="http://browsehappy.com/">upgrade your browser</a> or <a href="http://www.google.com/chromeframe/?redirect=true">activate Google Chrome Frame</a> to improve your experience.</p>
        <![endif]-->

        <!-- Add your site or application content here -->
        <div class="top_content">
            <!-- HEADER: LOGO -->
            <div class="page_header">
                
                <div id="logo_background"></div>
                <img id="logo" src="img/po_reports_logo_7.png" alt="PO Reports"/>
            </div>
             <div class="toolbar"></div>
        </div>
        <div class="message"><%=MESSAGE%></div>
        <div style="margin:3em auto; ">
            <div class="content" id="home" style="border-radius:10px; width:500px; height:auto; min-width:500px; min-height:260px;">
                
                <div class="wrapper" id="login_form">
                    <h2 class="title center">Please Login</h2>
                    <div style="margin-bottom:1em;">
                        <label style="display:block; margin-bottom:0.5em;">Username: </label>
                        <input type="text" data-bind="value:un, valueUpdate:'afterkeydown', executeOnEnter: login" style="width:100%; display:block;" />
                    </div>
                    <div style="margin-bottom:1em;">
                        <label style="display:block; margin-bottom:0.5em;">Password: </label>
                        <input type="password" data-bind="value:pw, valueUpdate:'afterkeydown', executeOnEnter: login" style="width:100%; display:block;" />
                    </div>
        
                    <button class="btn_submit ui-button-success" style="float:right; display:block;" disabled="disabled" data-bind="click: login, button: { disabled: !$root.isValid() }">Login</button>
                    <div style="clear:both;"></div>
                </div>
            </div>
        </div>

        <img src="img/wfm_logo_white.png" alt="" style="display:block; margin:1em auto;" />
        <div class="copyright">Copyright &copy; 2013 Whole Foods Market Inc.<span>To report website issues please email <a href="mailto:<%=SUPPORT_EMAIL %>"><%=SUPPORT_EMAIL %></a></span></div>
        <div style="text-align:center; font-size:8pt; color: #999;">Server: <%=SERVER_NAME %></div>

        <div data-bind="visible: isLoading" class="loader" style="display:none;">
        	<h2>Loading</h2>
        	<img src="img/wfm_loading_circle2.gif" alt="" style="display:block; margin:0 auto; text-align:center;"/>
        </div>
        
        <div data-bind="visible: isLoading()" class="mask" style="display:none;"></div>
        <script type="text/javascript" src="js/date.js"></script>  
        <script type="text/javascript" src="js/vendor/jquery.js"></script>  
        <script src="js/vendor/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
        <script type="text/javascript" src="js/vendor/JSON2.js"></script>
        <script type="text/javascript" src="js/vendor/knockout.js"></script>  
        <script src="js/login.js?v=1.0"></script>    
    </body>
</html>
