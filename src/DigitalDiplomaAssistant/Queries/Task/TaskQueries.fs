namespace Queries.Task

[<AutoOpen>]
module Queries = 
    open Nest;
    open DataAccess;
    open Queries

    let private setElasticTaskId (hit: IHit<ElasticTask>): ElasticTask = 
        {hit.Source with Id = hit.Id}

    let private toTask (task: ElasticTask): Task = 
        {
            Id = task.Id
            Type = task.Type
            Student = task.Student
            Assignee = task.Assignee
            Supervisor = task.Supervisor
            Group = task.Group
            Deadline = task.Deadline
            Status = match task.Status with
                | "InProgress" -> TaskStatus.InProgress
                | "ToDo" -> TaskStatus.ToDo
                | "Done" -> TaskStatus.Done
            Description = task.Description
            Comments = task.Comments
        }
    
    let getTaskById (elasticOptions: ElasticOptions) (id: string): Task option = 
        elasticOptions
        |> FsNest.createElasticClient
        |> FsNest.query<ElasticTask> "dda-task" (fun sd -> 
            QueryContainer(IdsQuery(Values = [|Id id|])))
        |> FsNest.hits<ElasticTask>
        |> Seq.tryHead
        |> Option.map (setElasticTaskId >> toTask)
