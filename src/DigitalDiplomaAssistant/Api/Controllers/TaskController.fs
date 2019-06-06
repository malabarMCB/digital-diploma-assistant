namespace Api.Controllers

open Microsoft.AspNetCore.Mvc
open Api
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Authentication.Cookies;

[<Route("[controller]")>]
//[<Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)>]
type TaskController() = 
    inherit Controller()

    [<HttpGet>]
    member this.Index() = 
        let task = "GJZPLGsB2N0LygMFURhw" |> getTaskById |> Option.get
        this.View(task)
        


