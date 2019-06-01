namespace DataAccess.Dashboard

[<AutoOpen>]
module Queries = 
    open Nest;
    open DataAccess;
    open DataAccess.Dashboard;

    let private setTaskId (hit: IHit<Task>): Task = 
        {hit.Source with Id = hit.Source.Id}

    let getTasks (elasticOptions: ElasticOptions): Task seq =
    elasticOptions 
    |> FsNest.createElasticClient
    |> FsNest.query<Task> "dda-task" (fun sd ->  QueryContainer(MatchAllQuery()))
    |> FsNest.hits<Task>
    |> Seq.map setTaskId
