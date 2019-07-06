namespace Domain

open System;

type DashboardTaskStatus =
    | ToDo
    | InProgress
    | Done

    module DashboardTaskStatus = 
        let fromString status = 
            match status with
            | "StudentInProgress" | "SupervisorInProgress" | "NormControllerInProgress" | "UnicheckValidatorInProgress" -> DashboardTaskStatus.InProgress
            | "StudentToDo"| "SupervisorToDo" | "NormControllerToDo" | "UnicheckValidatorToDo" -> DashboardTaskStatus.ToDo
            | "ReadyForMetodist" -> DashboardTaskStatus.Done

type DashboardTask = {
    Id: string
    Type: string
    Student: string
    Assignee: string
    Group: string
    Status: DashboardTaskStatus
    Deadline: DateTime
    Supervisor: string
}

type TaskWithAvaliableStatuses = {
    Id: string
    Type: TaskType
    Student: Student
    Assignee: Assignee
    Supervisor: Supervisor
    Group: string
    Status: TaskStatus
    Deadline: DateTime
    Description: Description
    Comments: Comment list
    AvaliableStatuses: TaskStatus list
}

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

type MetodistTaskDescription = {
    Type: TaskType
    Description: Description
}

