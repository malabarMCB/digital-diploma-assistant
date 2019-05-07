namespace Api.Controllers

open Microsoft.AspNetCore.Mvc
open Api

[<Route("[controller]")>]
type DashboardController () =
    inherit Controller()

    [<HttpGet>]
    member this.Index() =
        let items = getDashboardTasks
        this.View(items)
