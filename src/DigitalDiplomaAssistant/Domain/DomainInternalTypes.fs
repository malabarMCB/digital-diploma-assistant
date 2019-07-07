namespace Domain

open System.IO

[<AutoOpen>]
module InternalInterfaces = 
    type CreateTaskWithAvaliableStatuses = Task -> GetTaskWithAvaliableStatusesResult
    type UpdateTaskByNewStatus = Task -> TaskStatus -> Task 
    type SaveCommentFileToStorage = string -> string -> Stream -> string
    type GetFileStream = string -> FileStream

