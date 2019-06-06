namespace Queries.Dashboard

[<AutoOpen>]
module PublicTypes = 
    open System
    open Domain

    type ElasticTask = {
        Id: string
        Type: string
        Student: Student
        Assignee: Assignee
        Supervisor: Supervisor
        Group: string
        Status: string
        Deadline: DateTime
    }

