namespace DataAccess.Task

open System
open Nest
open DataAccess
open Domain
open Domain.Task

module Queries = 
    let private setElasticTaskId (hit: IHit<ElasticTask>): ElasticTask = 
        {hit.Source with Id = hit.Id}

    let getDashboardTask (elasticOptions: ElasticOptions): DashboardTask seq =
        elasticOptions 
        |> FsNest.createElasticClient
        |> FsNest.query<ElasticTask> "dda-task" (fun sd ->  QueryContainer(MatchAllQuery()))
        |> FsNest.hits
        |> Seq.map (setElasticTaskId >> fun task -> 
            {
                Id = task.Id
                Type = task.Type
                Student = task.Student.FirstName + " " + task.Student.LastName
                Assignee = task.Assignee.FirstName + " " + task.Assignee.LastName
                Supervisor = task.Supervisor.FirstName + " " + task.Supervisor.LastName
                Group = task.Group
                Deadline = task.Deadline
                Status = DashboardTaskStatus.fromString task.Status
            }
        )
   
    let getTaskById (getAvaliableStatuses: GetAvaliableStatuses) (elasticOptions: ElasticOptions) (id: string): Task option = 
        elasticOptions
        |> FsNest.createElasticClient
        |> FsNest.query<ElasticTask> "dda-task" (fun sd -> 
            QueryContainer(IdsQuery(Values = [|Id id|])))
        |> FsNest.hits
        |> Seq.tryHead
        |> Option.map (setElasticTaskId >> fun task ->
            let status = TaskStatusExtended.fromString task.Status
            {
                Id = task.Id
                Type = task.Type
                Student = task.Student
                Assignee = task.Assignee
                Supervisor = task.Supervisor
                Group = task.Group
                Deadline = task.Deadline
                Status = status
                Description = task.Description
                Comments = task.Comments
                AvaliableStatuses = getAvaliableStatuses (TaskType.fromString task.Type) status
            }  
        )

    let getTaskDescription (elasticOptions: ElasticOptions) (taskType: TaskType) = 
        elasticOptions |> FsNest.createElasticClient
        |> FsNest.query<ElasticTask> "dda-task" (fun sd -> 
            sd.Size(Nullable(1)) |> ignore
            QueryContainer(TermQuery(Field = Field("type"), Value = TaskType.toString taskType)))
        |> FsNest.hits
        |> Seq.head
        |> fun hit -> 
            {
                Type = hit.Source.Type |> TaskType.fromString
                Description = hit.Source.Description
            }
