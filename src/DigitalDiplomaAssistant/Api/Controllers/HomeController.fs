namespace Api.Controllers

open Microsoft.AspNetCore.Mvc

[<Route("[controller]")>]
type HomeController () =
    inherit Controller()

    [<HttpGet>]
    member this.Index() =
        this.View()
