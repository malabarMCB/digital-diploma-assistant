namespace Domain

module TaskPublicTypes = 
    open System

    type TaskStatusExtended = 
    | StudentToDo = 1
    | StudentInProgress = 2
    | SupervisorToDo = 3
    | SupervisorInProgress = 4
    | NormControllerToDo = 5
    | NormControllerInProgress = 6
    | UnicheckValidatorToDo = 7
    | UnicheckValidatorInProgress = 8
    | ReadyForMetodist = 9

    module TaskStatusExtended = 
        let fromString (status: string) = 
            match status with 
            | "StudentToDo" -> TaskStatusExtended.StudentToDo
            | "StudentInProgress" -> TaskStatusExtended.StudentInProgress
            | "SupervisorToDo" -> TaskStatusExtended.SupervisorToDo
            | "SupervisorInProgress" -> TaskStatusExtended.SupervisorInProgress
            | "NormControllerToDo" -> TaskStatusExtended.NormControllerToDo
            | "NormControllerInProgress" -> TaskStatusExtended.NormControllerInProgress
            | "UnicheckValidatorToDo" -> TaskStatusExtended.UnicheckValidatorToDo
            | "UnicheckValidatorInProgress" -> TaskStatusExtended.UnicheckValidatorInProgress
            | "ReadyForMetodist" -> TaskStatusExtended.ReadyForMetodist

    type Task = {
        Id: string
        Type: string
        Student: Student
        Assignee: Assignee
        Supervisor: Supervisor
        Group: string
        Status: TaskStatusExtended
        Deadline: DateTime
        Description: Description
        Comments: Comment list
        AvaliableStatuses: TaskStatusExtended list
    }
