namespace Domain

module TaskWithAvaliableStatuses = 
    let fromTask (task:Task) avaliableStatuses =
    {
        Id = task.Id
        Type = task.Type
        Student = task.Student
        Assignee = task.Assignee
        Supervisor = task.Supervisor
        Group = task.Group
        Status = task.Status
        Deadline = task.Deadline
        Description = task.Description
        Comments = task.Comments
        AvaliableStatuses = avaliableStatuses
    }

