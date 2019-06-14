namespace Api

[<AutoOpen>]
module PartialApplication = 
    open DataAccess
    open Common
    open Queries
    open Domain
    open Commands


    let private options = {
        Uri = "Elastic:Uri" |> getEnvVariable
        UserName = "Elastic:UserName" |> getEnvVariableOption
        Password = "Elastic:Password" |> getEnvVariableOption
    }
    let fileStoragePath = @"D:\dda-file-storage"
    
    let getDashboardTasks () = options |> Dashboard.Queries.getTasks
    let authenticate = options |> Authentication.Queries.getUser |> Authentication.Authentication.authenticate
    let getTaskById (id: string)= Task.Queries.getTaskById Task.getAvaliableStatuses options id

    let getPersonByRole = Task.Queries.getPersonByRole options
    let changeTaskStatus = TaskCommands.updateTaskStatus options
    let changeTaskAssigneeToStatus = Task.updateTaskStatus getPersonByRole

    let saveCommentFile taskId fileName (file: System.IO.Stream): string = Task.saveCommentFile fileStoragePath taskId fileName file
    let createComment = Task.createComment
    let addComment = options |> TaskCommands.addComment

    let getMetodistTaskDescription = Metodist.Queries.getTaskDescription options
    let deleteDesctiptionAttachments = MetodistCommands.deleteAttachmentDescription options
    
