namespace Commands

open Domain.TaskPublicTypes

module TaskCommands =
    open Domain
    open DataAccess

    type  StatusParams = {
        Status: string
    }
    let updateTaskStatus (elasticOptions: ElasticOptions) (id: string) (status: TaskStatusExtended) = 
        elasticOptions |> FsNest.createElasticClient
        |> FsNest.update "dda-task" id {
            Lang = "painless"
            Inline = "ctx._source.status = params.status"
            Params = {
                Status = status.ToString()
            }
         } |> ignore
        
