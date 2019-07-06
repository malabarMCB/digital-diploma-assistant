namespace Domain

open System.IO

[<AutoOpen>]
module TaskWorkflowCommands = 

    type ChangeTaskStatusCommand = {
        TaskId: string
        TaskStatus: TaskStatus
    }

    type AddCommentCommand = {
        TaskId: string
        CommentText: string
        AttachmentStream: Stream
        AttachmentName: string
        CommentAuthor: Person
    }
