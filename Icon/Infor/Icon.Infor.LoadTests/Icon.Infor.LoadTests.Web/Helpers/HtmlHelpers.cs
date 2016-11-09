namespace Icon.Infor.LoadTests.Web.Helpers
{
    using Controls;
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;

    public static class HtmlHelpers
    {
        public static IDisposable BuildTestCard(this HtmlHelper helper, string status, object htmlAttributes = null)
        {
            var div = new TagBuilder("div");
            div.AddCssClass("card card-block");
            if (htmlAttributes != null)
            {
                div.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            }
            if (status == "Running")
            {
                div.AddCssClass("test-running");
                //div.MergeAttribute("style", "border-left: 5px solid lightgreen");
            }
            else
            {
                div.AddCssClass("test-idle");
                //div.MergeAttribute("style", "border-left: 5px solid gray");
            }
            helper.ViewContext.Writer.Write(div.ToString(TagRenderMode.StartTag));

            return new TestCardDetails(helper.ViewContext);
        }
    }
}