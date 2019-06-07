namespace Api.Controllers

open Microsoft.AspNetCore.Mvc
open Api
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Authentication.Cookies;
open Domain.TaskPublicTypes

[<Route("[controller]")>]
//[<Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)>]
type TaskController() = 
    inherit Controller()

    [<HttpGet>]
    member this.Index() = 
        let task = "Zf24MWsB23tLy1EGf-Z0" |> getTaskById |> Option.get
        this.View(task)

    [<HttpPost("{id}/status")>]
    member this.ChangeTaskStatus(id: string, [<FromForm>]status: TaskStatusExtended ) = 
        this.RedirectToAction("Index")
        
