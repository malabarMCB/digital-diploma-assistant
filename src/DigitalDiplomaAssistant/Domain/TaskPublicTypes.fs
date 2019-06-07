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
    | UnicheckvalidatorToDo = 7
    | UnicheckValidatorInProgress = 8
    | ReadyForMetodist = 9

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
