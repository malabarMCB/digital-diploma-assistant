namespace DataAccess.Query

module DashboardQuery =
    type TaskStatus =
        | ToDO
        | InProgress
        | Done
        
     type Task = {
         Name: string
         Asignee: string
         Group: string
         Status: TaskStatus
     }
    
    let getItems: Task list =
        let task1 = {
            Name = "Task 1"
            Asignee = "John Smith"
            Group = "BS-51"
            Status = InProgress
        }
        
        let task2 = {
            Name = "Task 2"
            Asignee = "Jane Doe"
            Group = "BS-52"
            Status = Done
        }
        
        [task1; task2]
        
