using Exercises.Attribute;
using System.Web;
using System.Web.Mvc;

namespace Exercises
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AccountAttribute() { IsChecked = true });
        }
    }
}
