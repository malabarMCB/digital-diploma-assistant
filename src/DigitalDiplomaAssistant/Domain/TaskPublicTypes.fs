namespace Domain

module TaskPublicTypes = 
    open System

    type TaskStatusExtended = 
    | StudentToDo
    | StudentInProgress
    | SupervisorToDo
    | SupervisorInProgress
    | NormControllerToDo
    | NormControllerInProgress
    | UnicheckvalidatorToDo
    | UnicheckValidatorInProgress
    | ReadyForMetodist

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
