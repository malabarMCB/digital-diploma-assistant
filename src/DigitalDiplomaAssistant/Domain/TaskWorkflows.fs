namespace Domain

open System

module TaskWorkflows =   
    let getTaskWithAvaliableStatuses: GetTaskByIdFromDb -> CreateTaskWithAvaliableStatuses -> GetTaskWithAvaliableStatusesWorkflow =
        fun getTaskById createTaskWithAvaliableStatuses id -> 
            match getTaskById id with
            | Some task ->
                task
                |> createTaskWithAvaliableStatuses
                |> Result.map(fun x -> Some x)
            | None -> Ok None

    let changeStatus: GetTaskByIdFromDb -> UpdateTaskByNewStatus -> UpdateTaskStatusInDb -> CreateTaskWithAvaliableStatuses ->  ChangeStatusWorkflow = 
        fun getTaskById updateTaskByNewStatus updateTaskStatusInDb createTaskWithAvaliableStatuses command ->
            match getTaskById command.TaskId with
            | Some task -> 
                let task = updateTaskByNewStatus task command.TaskStatus
                updateTaskStatusInDb task.Id task.Status task.Assignee |> ignore
                task |> createTaskWithAvaliableStatuses
            | None -> Error "Incorrect task id"

    let addComment: SaveCommentFileToStorage -> AddCommentToDb -> GetTaskWithAvaliableStatusesWorkflow -> AddCommentWorkflow = 
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


