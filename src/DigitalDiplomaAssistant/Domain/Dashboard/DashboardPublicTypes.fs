namespace Domain.Dashboard

[<AutoOpen>]
module PublicTypes = 
    open System;

    type TaskStatus =
        | ToDo
        | InProgress
        | Done
        
     type Task = {
        Id: string
        Type: string
        Student: string
        Assignee: string
        Group: string
        Status: TaskStatus
        Deadline: DateTime
        ScienceMaster: string
     }
