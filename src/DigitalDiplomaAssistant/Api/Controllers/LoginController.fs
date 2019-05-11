namespace Api.Controllers

open Microsoft.AspNetCore.Mvc
open Api
open Microsoft.AspNetCore.Http
open System.Security.Claims
open Microsoft.AspNetCore.Authentication;
open Microsoft.AspNetCore.Authentication.Cookies;

[<Route("[controller]")>]
type LoginController (httpContextAccessor: IHttpContextAccessor) = 
    inherit Controller()
        
    let setAuthCookie (user: Authentication.User) = 
        let claims: Claim list = [
            Claim(ClaimTypes.Sid, user.Id);
            Claim(ClaimTypes.Name, user.FirstName)
            Claim(ClaimTypes.Surname, user.LastName)
            Claim(ClaimTypes.Role, user.Role.ToString())
        ]
        let id = ClaimsIdentity(claims, ClaimsIdentity.DefaultNameClaimType);
        httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id)) |> Async.AwaitTask

    [<HttpGet>]
    member this.Index() =
        this.View()

    [<HttpPost>]
    member this.Post(login: string, password: string) =
        match authenticate(login, password) with
        | Ok user -> user |> setAuthCookie |> ignore; this.Redirect("dashboard") :> IActionResult
        | Error _ -> this.View("index") :> IActionResult


