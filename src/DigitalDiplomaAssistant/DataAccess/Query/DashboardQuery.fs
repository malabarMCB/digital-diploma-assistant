namespace DataAccess.Query

module DashboardQuery =
    open System
    open DataAccess
    open DataAccess.Types
    open Nest

    type TaskStatus =
        | ToDo
        | InProgress
        | Done
        
     type Task = {
        Id: string
        Type: string
        Student: string
        Assignee: string
        Group: string
        Status: TaskStatus
        Deadline: string
     }

     type TaskQueryFilterSearchTarget = 
        | Type
        | Student
        | Assignee
        | Group

     type TaskQueryFilter = {
        SearchString: string
        SearchTarget: TaskQueryFilterSearchTarget
     }

    let private toDashboardTask (hit: IHit<ElasticTypes.Task>):Task = 
        {
            Id = hit.Id
            Type = hit.Source.Type
            Student = hit.Source.Student
            Assignee = hit.Source.Assignee
            Group = hit.Source.Group
            Deadline = hit.Source.Deadline
            Status = match hit.Source.Status with
                | "InProgress" -> TaskStatus.InProgress
                | "ToDo" -> TaskStatus.ToDo
                | "Done" -> TaskStatus.Done
        }

    
    let getTasks (elasticOptions: ElasticOptions): Task seq =
        elasticOptions 
        |> FsNest.createElasticClient
        |> FsNest.query<ElasticTypes.Task> "dda" "task" (fun () ->  QueryContainer(MatchAllQuery()))
        |> FsNest.hits<ElasticTypes.Task>
        |> Seq.map toDashboardTask

    let getFiletedTask (filter: TaskQueryFilter): Task list =
        []
        
