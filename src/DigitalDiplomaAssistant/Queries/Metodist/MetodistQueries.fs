namespace Queries.Metodist

module Queries = 
    open DataAccess
    open Domain.Metodist
    open Queries.Metodist.PublicTypes
    open System
    open Nest

    let getTaskDescription (elasticOptions: ElasticOptions) (taskType: TaskType) = 
        let toMetodistTaskDescription (taskDescription: IHit<ElasticTaskDescription>): MetodistTaskDescription = 
            {
                Type = taskDescription.Source.Type |> TaskType.create
                Description = taskDescription.Source.Description
            }
        elasticOptions |> FsNest.createElasticClient
        |> FsNest.query<ElasticTaskDescription> "dda-task" (fun sd -> 
            sd.Size(Nullable(1)) |> ignore
            QueryContainer(TermQuery(Field = Field("type"), Value = TaskType.toString taskType)))
        |> FsNest.hits<ElasticTaskDescription>
        |> Seq.head
        |> toMetodistTaskDescription
