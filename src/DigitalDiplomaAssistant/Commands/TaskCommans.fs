namespace Commands

open Domain.TaskPublicTypes

module TaskCommands =
    open Domain
    open DataAccess

    type  StatusParams = {
        Status: string
        Assignee: Person
    }
    let updateTaskStatus (elasticOptions: ElasticOptions) (id: string) (status: TaskStatusExtended) (assignee: Person) = 
        elasticOptions |> FsNest.createElasticClient
        |> FsNest.update "dda-task" id {
            Lang = "painless"
            Inline = "ctx._source.status = params.status; ctx._source.assignee = params.assignee"
            Params = {
                Status = status.ToString()
                Assignee = assignee
            }
         } |> ignore
        
    type CommentParams = {
        Comment: Comment
    }
    let addComment (elasticOptions: ElasticOptions) (id: string) (comment: Comment) = 
        elasticOptions |> FsNest.createElasticClient
        |> FsNest.update "dda-task" id {
            Lang = "painless"
            Inline = "ctx._source.comments.add(params.comment)"
            Params= {
                Comment = comment
            }
        } |> ignore