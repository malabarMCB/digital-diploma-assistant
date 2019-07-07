namespace Api

open DataAccess
open Common
open Domain
open Authentication

[<AutoOpen>]
module PartialApplication = 

    let private getElasticOptions() = 
        {
            Uri = "Elastic:Uri" |> EnvVariables.getEnvVariable
            UserName = "Elastic:UserName" |> EnvVariables.getEnvVariableOption
            Password = "Elastic:Password" |> EnvVariables.getEnvVariableOption
        }
    let private getFileStoragePath() = 
        EnvVariables.getEnvVariableWithDefaultValue "FileStoragePath" @"D:\dda-file-storage"

    let private getTaskById: GetTaskByIdFromDb = getElasticOptions() |> Task.Queries.getTaskById
    
    let authenticate = getElasticOptions() |> User.Queries.getUser |> Authentication.authenticate

    let getDashboardTasksWorkflow: GetDashboardTasksWorkflow =
        fun () ->
            getElasticOptions() |> Task.Queries.getDashboardTasks  

    let getTaskWithAvaliableStatusesWorkflow: GetTaskWithAvaliableStatusesWorkflow = 
        fun taskId -> 
           TaskWorkflows.getTaskWithAvaliableStatuses getTaskById TaskActions.createTaskWithAvaliableStatuses taskId

    let changeStatusWorkflow: ChangeStatusWorkflow = 
        fun command -> 
            let updateTaskByNewStatus: UpdateTaskByNewStatus = getElasticOptions() |> User.Queries.getPersonByRole |> TaskActions.updateTaskByNewStatus
            let updateTaskStatusInDb: UpdateTaskStatusInDb = getElasticOptions() |> Task.Commands.updateTaskStatus
            TaskWorkflows.changeStatus getTaskById updateTaskByNewStatus updateTaskStatusInDb TaskActions.createTaskWithAvaliableStatuses command

    let addCommentWorkflow: AddCommentWorkflow = 
        fun command ->
            let saveCommentFileToStorage: SaveCommentFileToStorage = getFileStoragePath() |>TaskActions.saveCommentFile 
            let saveCommentToDb: AddCommentToDb = getElasticOptions() |> Task.Commands.addComment 
            TaskWorkflows.addComment saveCommentFileToStorage saveCommentToDb getTaskWithAvaliableStatusesWorkflow command

    let getAttachmentFileStreamWorkflow: GetAttachmentFileStreamWorkflow =
        fun taskId filePath -> 
            TaskWorkflows.getAttachmentFileStream (getFileStoragePath() |> IO.getFileStream) taskId filePath

    let getMetodistTaskDescriptionWorkflow: GetMetodistTaskDescriptionWorkflow = 
        fun taskType ->
                taskType |> Task.Queries.getTaskDescription (getElasticOptions()) 

    let deleteDescriptionAttachmentWorkflow: DeleteDescriptionAttachmentWorkflow =
        fun attachment taskType -> 
            TaskWorkflows.deleteDescriptionAttachment (getElasticOptions() |> Task.Commands.deleteAttachmentDescription) getMetodistTaskDescriptionWorkflow attachment taskType
    
