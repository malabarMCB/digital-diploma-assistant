namespace Api.Controllers

open Microsoft.AspNetCore.Mvc
open DataAccess.Query
open DataAccess

[<Route("[controller]")>]
type DashboardController () =
    inherit Controller()

    [<HttpGet>]
    member this.Index() =
        let items = DashboardQuery.getTasks
        this.View(items)
