namespace Api

[<AutoOpen>]
module PartialApplication = 
    open DataAccess
    open Common
    open Domain
    open Authentication

    let private options = {
        Uri = "Elastic:Uri" |> getEnvVariable
        UserName = "Elastic:UserName" |> getEnvVariableOption
        Password = "Elastic:Password" |> getEnvVariableOption
    }
    let fileStoragePath = getEnvVariableWithDefaultValue "FileStoragePath" @"D:\dda-file-storage"
    
    let authenticate = options |> User.Queries.getUser |> Authentication.authenticate

    let getDashboardTasks () = options |> Task.Queries.getDashboardTask
    
    let getTaskById (id: string)= Task.Queries.getTaskById Task.getAvaliableStatuses options id

    let getPersonByRole = User.Queries.getPersonByRole options
    let changeTaskStatus = Task.Commands.updateTaskStatus options
    let changeTaskAssigneeToStatus = Task.updateTaskStatus getPersonByRole

    let saveCommentFile taskId fileName (file: System.IO.Stream): string = Task.saveCommentFile fileStoragePath taskId fileName file
    let createComment = Task.createComment
    let addComment = options |> Task.Commands.addComment

    let getMetodistTaskDescription = Task.Queries.getTaskDescription options
    let deleteDesctiptionAttachments = Task.Commands.deleteAttachmentDescription options
    
