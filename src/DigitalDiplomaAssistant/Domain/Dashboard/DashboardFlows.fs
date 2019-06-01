namespace Domain.Dashboard

[<AutoOpen>]
module Flows = 
    type GetDbTasks = DataAccess.Dashboard.PublicTypes.Task seq

    let private toTask (dbTask: DataAccess.Dashboard.PublicTypes.Task): Task = 
        {
            Id = dbTask.Id
            Type = dbTask.Type
            Student = dbTask.Student.FirstName + " " + dbTask.Student.LastName
            Assignee = dbTask.Assignee.FirstName + " " + dbTask.Assignee.LastName
            ScienceMaster = dbTask.ScienceMaster.FirstName + " " + dbTask.ScienceMaster.LastName
            Group = dbTask.Group
            Deadline = dbTask.Deadline
            Status = match dbTask.Status with
                | "InProgress" -> TaskStatus.InProgress
                | "ToDo" -> TaskStatus.ToDo
                | "Done" -> TaskStatus.Done
        }        

    let getTasks (getDbTasks: GetDbTasks): Task seq =   
        getDbTasks |> Seq.map toTask
