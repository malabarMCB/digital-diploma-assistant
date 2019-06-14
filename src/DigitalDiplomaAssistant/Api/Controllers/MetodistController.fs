namespace Api.Controllers

open Microsoft.AspNetCore.Mvc
open Api
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Authentication.Cookies;
open Domain.Metodist
open Domain.PublicTypes
open System
open Microsoft.AspNetCore.Http

[<Route("[controller]")>]
[<Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)>]
type MetodistController () = 
    inherit Controller()

    [<HttpGet>]
    member this.Index() = 
        let description = getMetodistTaskDescription TaskType.PracticeDairy
        this.View("Index", description)

    [<HttpPost("deleteFile")>]
    member this.DeleteAttachment([<FromForm>]name: string,[<FromForm>] file: string, [<FromForm>] uploadDate: DateTime, [<FromForm>] taskType: TaskType) = 
        let attachment = {
            Name = name
            FilePath = file
            UploadDate = uploadDate
        }
        deleteDesctiptionAttachments attachment taskType
        let description = getMetodistTaskDescription TaskType.PracticeDairy
        this.View("Index", description)

    [<HttpPost>]
    member this.update([<FromForm>] text: string, [<FromForm>]file: IFormFile) = 
        let description = getMetodistTaskDescription TaskType.PracticeDairy
        this.View("Index", description)
