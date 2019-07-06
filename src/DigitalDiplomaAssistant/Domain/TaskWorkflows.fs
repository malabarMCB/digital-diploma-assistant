namespace Domain

open System
open System.IO

module TaskWorkflows =   
    //results
    type GetTaskWithAvaliableStatusesResult = Result<TaskWithAvaliableStatuses, string>
    type GetOptionalTaskWithAvaliableStatusesResult = Result<TaskWithAvaliableStatuses option, string>

    type CreateTaskWithAvaliableStatuses = Task -> GetTaskWithAvaliableStatusesResult
    type UpdateTaskByNewStatus = Task -> TaskStatus -> Task 
    type SaveCommentFileToStorage = string -> string -> Stream -> string
    type GetFileStream = string -> FileStream

    //database
    type GetTaskById = string -> Task option
    type UpdateTaskStatusInDb = string -> TaskStatus -> Person -> unit
    type SaveCommentToDb = string -> Comment -> unit
    type DeleteDescriptionAttachmentFromDb = Attachment -> TaskType -> unit

    //interfaces
    type GetTaskWithAvaliableStatusesWorkflow = string -> GetOptionalTaskWithAvaliableStatusesResult
    type ChangeStatusWorkflow = ChangeTaskStatusCommand -> GetTaskWithAvaliableStatusesResult
    type AddCommentWorkflow = AddCommentCommand -> GetOptionalTaskWithAvaliableStatusesResult
    type GetAttachmentFileStreamWorkflow = string -> string -> FileStream
    type GetDashboardTasksWorkflow = unit -> seq<DashboardTask>
    type GetMetodistTaskDescriptionWorkflow = TaskType -> MetodistTaskDescription
    type DeleteDescriptionAttachmentWorkflow = Attachment -> TaskType -> MetodistTaskDescription

    let getTaskWithAvaliableStatuses: GetTaskById -> CreateTaskWithAvaliableStatuses -> GetTaskWithAvaliableStatusesWorkflow =
        fun getTaskById createTaskWithAvaliableStatuses id -> 
            match getTaskById id with
            | Some task ->
                task
                |> createTaskWithAvaliableStatuses
                |> Result.map(fun x -> Some x)
            | None -> Ok None

    let changeStatus: GetTaskById -> UpdateTaskByNewStatus -> UpdateTaskStatusInDb -> CreateTaskWithAvaliableStatuses ->  ChangeStatusWorkflow = 
        fun getTaskById updateTaskByNewStatus updateTaskStatusInDb createTaskWithAvaliableStatuses command ->
            match getTaskById command.TaskId with
            | Some task -> 
                let task = updateTaskByNewStatus task command.TaskStatus
                updateTaskStatusInDb task.Id task.Status task.Assignee |> ignore
                task |> createTaskWithAvaliableStatuses
            | None -> Error "Incorrect task id"

    let addComment: SaveCommentFileToStorage -> SaveCommentToDb -> GetTaskWithAvaliableStatusesWorkflow -> AddCommentWorkflow = 
        fun saveCommentFileToStorage saveCommentToDb getTaskWithAvaliableStatuses command -> 
            let filePath = saveCommentFileToStorage command.TaskId command.AttachmentName command.AttachmentStream
            let comment = {
                Author = command.CommentAuthor
                Text = command.CommentText
                PostDate = DateTime.UtcNow
                Attachments = [
                    {
                        Name = command.AttachmentName
                        FilePath = filePath
                        UploadDate = DateTime.UtcNow
                    }
                ]
            }
            saveCommentToDb command.TaskId comment |> ignore
            command.TaskId |> getTaskWithAvaliableStatuses

    let getAttachmentFileStream: GetFileStream -> GetAttachmentFileStreamWorkflow = 
        fun getFileStream taskId filePath -> 
            @"\" + taskId + @"\" + filePath
            |> getFileStream

    let deleteDescriptionAttachment: DeleteDescriptionAttachmentFromDb -> GetMetodistTaskDescriptionWorkflow -> DeleteDescriptionAttachmentWorkflow =
        fun deleteDescriptionAttachmentFormDb getMetodistTaskDescription attachment taskType ->
            taskType |> deleteDescriptionAttachmentFormDb attachment |> ignore
            taskType |> getMetodistTaskDescription


