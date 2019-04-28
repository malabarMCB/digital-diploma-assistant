namespace Domain

module Dashboard =
    open System
    
    type ToDoTaskType =
        | ReadyForStudent
        | ReadyForScienceMaster
        | ReadyForMetodist
        | ReadyForUnicheck
        | ReadyForNormChecker
        
    type InProgressTaskType =
        | StudentInProgress
        | ScienceMasterInProgress
        | MetodistInProgress
        | UnicheckInProgress
        | NormCheckerInProgress
        
    type DoneTaskType =
        | Complete
    
    type StatusType =
        | ToDo of ToDoTaskType
        | InProgress of InProgressTaskType
        | Done of DoneTaskType

    type TaskInfo ={
       Asingee: string
       Student: string
       Group: string
       Deadline: DateTime
       Status: StatusType
    }
 
    type Task =
        | PractisePresentationDefence of TaskInfo
        | PractiseDairy of TaskInfo
    
    let test =
        let task = PractiseDairy ({
            Asingee = "John Doe"
            Student = "Jane Doe"
            Group = "BS-51"
            Deadline = DateTime.Now
            Status = InProgress NormCheckerInProgress
        })
        match task with
        | PractiseDairy t ->printf "It`s practise"
        | _ -> printf "It is not practise"
        ()
    
        
