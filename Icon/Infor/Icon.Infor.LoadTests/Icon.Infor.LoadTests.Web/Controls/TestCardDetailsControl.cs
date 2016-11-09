namespace Icon.Infor.LoadTests.Web.Controls
{
    using System;
    using System.Web.Mvc;

    public class TestCardDetails : IDisposable
    {
        private readonly ViewContext viewContext;
        public TestCardDetails(ViewContext viewContext)
        {
            this.viewContext = viewContext;
        }

        public void Dispose()
        {
            viewContext.Writer.Write("</div>");
        }
    }
}