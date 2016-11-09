namespace Icon.Web.Api.DataLayer
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class IconContext : DbContext
    {
        public IconContext()
            : base("name=IconContext")
        {
        }
    }
}