namespace Api.Controllers

open Microsoft.AspNetCore.Mvc
open Api
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Authentication.Cookies;
open Domain.TaskPublicTypes
open Microsoft.AspNetCore.Http
open System.Linq
open System.Security.Claims
open Domain.PublicTypes
open System.IO

[<Route("[controller]")>]
[<Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)>]
type TaskController(httpContextAccessor: IHttpContextAccessor) = 
    inherit Controller()

    let getClaimValue claimType = 
         httpContextAccessor.HttpContext.User.Claims.Single(fun (claim: Claim) -> claim.Type = claimType).Value

    [<HttpGet>]
    member this.Index() = 
        let task = "Zf24MWsB23tLy1EGf-Z0" |> getTaskById |> Option.get
        this.View(task)

    [<HttpPost("{id}/status")>]
    member this.ChangeTaskStatus(id: string, [<FromForm>]status: TaskStatusExtended ) =     
        let task = id |> getTaskById |> Option.get
        let task = changeTaskAssigneeToStatus task status
        changeTaskStatus id status task.Assignee
        let task = id |> getTaskById |> Option.get
        this.View("Index", task)

    [<HttpPost("{id}/comments")>]
    member this.AddComment(id: string, [<FromForm>]text: string, [<FromForm>]file: IFormFile) = 
        let stream = file.OpenReadStream()
        let filePath = saveCommentFile id file.FileName stream
        let author: Person =  {
            Id = getClaimValue ClaimTypes.Sid
            FirstName = getClaimValue ClaimTypes.Name
            LastName = getClaimValue ClaimTypes.Surname
        }
        let comment = createComment author text file.FileName filePath
        addComment id comment
        let task = id |> getTaskById |> Option.get
        this.View("Index", task)

    [<HttpGet("{id}/file/{filePath}")>]
    member this.DownloadFile(id: string, filePath: string) = 
        let path = fileStoragePath + @"\" + id + @"\" + filePath
        let stream = File.OpenRead path
        this.File(stream, "application/octet-stream")
