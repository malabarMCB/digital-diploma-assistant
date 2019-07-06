namespace Api.Controllers

open Microsoft.AspNetCore.Mvc
open Api
open Microsoft.AspNetCore.Http

[<Route("[controller]")>]
type LoginController (httpContextAccessor: IHttpContextAccessor) = 
    inherit Controller()

    [<HttpGet>]
    member this.Index() =
        this.View()

    [<HttpPost>]
    member this.Post(login: string, password: string) =
        match authenticate(login, password) with
        | Ok user -> 
            user |> FsHttpContextAccessor.setAuthCookie httpContextAccessor |> ignore
            this.Redirect("dashboard") :> IActionResult
        | Error _ -> this.View("index") :> IActionResult


