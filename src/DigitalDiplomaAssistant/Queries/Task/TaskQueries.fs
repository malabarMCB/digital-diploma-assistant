namespace Queries.Task

[<AutoOpen>]
module Queries = 
    open Nest;
    open DataAccess;
    open Queries.Task
    open Domain.Task
    open Domain.TaskPublicTypes

    let private setElasticTaskId (hit: IHit<ElasticTask>): ElasticTask = 
        {hit.Source with Id = hit.Id}

    let private toTask (getAvaliableStatuses: GetAvaliableStatuses) (task: ElasticTask): Domain.TaskPublicTypes.Task = 
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
            AvaliableStatuses = getAvaliableStatuses task.Type status
        }
    
    let getTaskById (getAvaliableStatuses: GetAvaliableStatuses) (elasticOptions: ElasticOptions) (id: string): Domain.TaskPublicTypes.Task option = 
        elasticOptions
        |> FsNest.createElasticClient
        |> FsNest.query<ElasticTask> "dda-task" (fun sd -> 
            QueryContainer(IdsQuery(Values = [|Id id|])))
        |> FsNest.hits<ElasticTask>
        |> Seq.tryHead
        |> Option.map (setElasticTaskId >> toTask getAvaliableStatuses)
