namespace Queries.Task

[<AutoOpen>]
module PublicTypes = 
    open System
    open Queries  

    type ElasticTask = {
        Id: string
        Type: string
        Student: Student
        Assignee: Assignee
        Supervisor: Supervisor
        Group: string
        Status: string
        Deadline: DateTime
        Description: Description
        Comments: Comment list
    }

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
