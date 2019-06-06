namespace Queries.Dashboard

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
    }

    type Task = {
        Id: string
        Type: string
        Student: string
        Assignee: string
        Group: string
        Status: TaskStatus
        Deadline: DateTime
        Supervisor: string
    }

