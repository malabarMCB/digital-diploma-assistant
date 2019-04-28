namespace Api.Controllers

open Microsoft.AspNetCore.Mvc
open DataAccess.Query

[<Route("[controller]")>]
type DashboardController () =
    inherit Controller()

    [<HttpGet>]
    member this.Index() =
        let items = DashboardQuery.getItems
        this.View(items)
