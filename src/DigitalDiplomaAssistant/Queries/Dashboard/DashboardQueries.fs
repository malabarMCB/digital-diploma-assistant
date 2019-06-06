namespace Queries.Dashboard

[<AutoOpen>]
module Queries = 
    open Nest;
    open DataAccess
    open Queries.Dashboard
    open Domain


    let private setElasticTaskId (hit: IHit<ElasticTask>): ElasticTask = 
        {hit.Source with Id = hit.Id}

    let private toTask (task: ElasticTask): DashboardPublicTypes.Task = 
        {
            Id = task.Id
            Type = task.Type
            Student = task.Student.FirstName + " " + task.Student.LastName
            Assignee = task.Assignee.FirstName + " " + task.Assignee.LastName
            Supervisor = task.Supervisor.FirstName + " " + task.Supervisor.LastName
            Group = task.Group
            Deadline = task.Deadline
            Status = match task.Status with
                | "InProgress" -> TaskStatus.InProgress
                | "ToDo" -> TaskStatus.ToDo
                | "Done" -> TaskStatus.Done
        } 

    let getTasks (elasticOptions: ElasticOptions): DashboardPublicTypes.Task seq =
    elasticOptions 
    |> FsNest.createElasticClient
    |> FsNest.query<ElasticTask> "dda-task" (fun sd ->  QueryContainer(MatchAllQuery()))
    |> FsNest.hits<ElasticTask>
    |> Seq.map (setElasticTaskId >> toTask)
    
