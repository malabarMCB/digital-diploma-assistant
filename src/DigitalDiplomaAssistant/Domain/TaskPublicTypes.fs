namespace Domain

module TaskPublicTypes = 
    open System

    type Task = {
        Id: string
        Type: string
        Student: Student
        Assignee: Assignee
        Supervisor: Supervisor
        Group: string
        Status: TaskStatus
        Deadline: DateTime
        Description: Description
        Comments: Comment list
    }
