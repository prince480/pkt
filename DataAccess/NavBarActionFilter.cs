using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PKT.DataAccess;
using PKT.Models;

public class NavBarActionFilter : IActionFilter
{
//    private readonly YourDbContext _dbContext;
    private readonly DBContext _DBContext;

    public NavBarActionFilter(DBContext dbContext)
    {
        _DBContext = dbContext;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Result is ViewResult viewResult)
        {
            var navBar = _DBContext.Get_UI_Elements_List("PKT_NavBar", "", "", "", "", "", "", "")
                .Cast<NavBar>()
                .ToList();

            viewResult.ViewData["NavBar"] = navBar;
        }
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        // This method is not used in this example
    }
}
