namespace Api.Controllers

open Microsoft.AspNetCore.Mvc
open Api
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Authentication;
open Microsoft.AspNetCore.Authentication.Cookies;

[<Route("[controller]")>]
type LoginController (httpContextAccessor: IHttpContextAccessor) = 
    inherit Controller()

    [<HttpGet>]
    member this.Index() =
        this.View()

    [<HttpPost>]
    member this.Login(login: string, password: string) =
        match authenticate(login, password) with
        | Ok user -> 
            user |> FsHttpContextAccessor.setAuthCookie httpContextAccessor |> ignore
            this.Redirect("dashboard") :> IActionResult
        | Error _ -> this.View("index") :> IActionResult
        
    [<HttpGet("logout")>]
    member this.Logout() =
        httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme)
        |> Async.AwaitTask
        |> ignore
 
        this.RedirectToAction("Index") :> IActionResult



