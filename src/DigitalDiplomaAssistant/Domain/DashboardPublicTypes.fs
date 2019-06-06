namespace Domain

module DashboardPublicTypes = 
    open System

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

