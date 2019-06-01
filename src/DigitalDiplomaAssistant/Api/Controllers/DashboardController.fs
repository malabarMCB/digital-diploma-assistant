namespace Api.Controllers

open Microsoft.AspNetCore.Mvc
open Api
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Authentication.Cookies;
open Microsoft.AspNetCore.Http

[<Route("[controller]")>]
[<Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)>]
type DashboardController (httpContextAccessor: IHttpContextAccessor) =
    inherit Controller()

    [<HttpGet>]
    member this.Index() =
        let user = httpContextAccessor.HttpContext.User
        let items = getDashboardTasks()
        this.View(items)
