namespace Api.Controllers

open Microsoft.AspNetCore.Mvc
open Api
open Api.Models

[<Route("[controller]")>]
type LoginController () =
    inherit Controller()

    [<HttpGet>]
    member this.Index() =
        this.View()

    [<HttpPost>]
    [<ValidateAntiForgeryToken>]
    member this.Post([<FromForm>]login: Login.LoginRequest) = 
        this.View("index")


