namespace Api.Controllers

open Microsoft.AspNetCore.Mvc

[<Route("[controller]")>]
type LoginController () =
    inherit Controller()

    [<HttpGet>]
    member this.Index() =
        this.View()

    [<HttpPost>]
    member this.Post(login: string, password: string) = 
        this.View("index")


