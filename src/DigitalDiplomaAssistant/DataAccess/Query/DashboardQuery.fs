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
        Deadline: DateTime
        ScienceMaster: string
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
            Student = hit.Source.Student.FirstName + " " + hit.Source.Student.LastName
            Assignee = hit.Source.Assignee.FirstName + " " + hit.Source.Assignee.LastName
            ScienceMaster = hit.Source.ScienceMaster.FirstName + " " + hit.Source.ScienceMaster.LastName
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
        |> FsNest.query<ElasticTypes.Task> "dda-task" (fun sd ->  QueryContainer(MatchAllQuery()))
        |> FsNest.hits<ElasticTypes.Task>
        |> Seq.map toDashboardTask

    let getFiletedTask (filter: TaskQueryFilter): Task list =
        []

        
