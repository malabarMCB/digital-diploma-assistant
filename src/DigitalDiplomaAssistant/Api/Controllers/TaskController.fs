namespace Api.Controllers

open Microsoft.AspNetCore.Mvc
open Api
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Authentication.Cookies;
open Microsoft.AspNetCore.Http
open Domain

[<Route("[controller]")>]
[<Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)>]
type TaskController(httpContextAccessor: IHttpContextAccessor) = 
    inherit Controller()

    [<HttpGet("{id}")>]
    member this.Index(id: string) = 
        match getTaskWithAvaliableStatusesWorkflow id with
        | Ok ok -> 
            match ok with
            | Some task -> this.View(task) :> IActionResult
            | None -> this.NotFound() :> IActionResult
        | Error _ -> this.UnprocessableEntity() :> IActionResult


    [<HttpPost("{id}/status")>]
    member this.ChangeTaskStatus(id: string, [<FromForm>]status: TaskStatus ) =     
        let command = {
            TaskId = id
            TaskStatus = status
        }
        match changeStatusWorkflow command with
        | Ok ok -> this.View("Index", ok) :> IActionResult
        | Error _ -> this.UnprocessableEntity() :> IActionResult


    [<HttpPost("{id}/comments")>]
    member this.AddComment(id: string, [<FromForm>]text: string, [<FromForm>]file: IFormFile) = 
        let command = {
            TaskId = id
            CommentText = text
            AttachmentName = file.FileName
            AttachmentStream = file.OpenReadStream()
            CommentAuthor = {
                Id = httpContextAccessor |> FsHttpContextAccessor.getUserId |> Option.get
                FirstName = httpContextAccessor |> FsHttpContextAccessor.getUserFirstName |> Option.get
                LastName = httpContextAccessor |> FsHttpContextAccessor.getUserLastName |> Option.get
            }
        }
        match addCommentWorkflow command with
        | Ok ok -> 
            match ok with
            | Some task -> this.View("Index", task) :> IActionResult
            | None -> this.NotFound() :> IActionResult
        | Error _ -> this.UnprocessableEntity() :> IActionResult


    [<HttpGet("{id}/file/{filePath}")>]
    member this.DownloadFile(id: string, filePath: string) = 
        let stream = getAttachmentFileStreamWorkflow id filePath
        this.File(stream, "application/octet-stream")
