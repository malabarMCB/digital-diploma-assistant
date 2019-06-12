namespace Domain

module Metodist =
    type TaskType = 
    | PracticePresentation = 1
    | Practicedairy = 2
    | PracticeRepost = 3
    | TopicAcceptanceDocument = 4
    | SupervisorReview = 5
    | InjectionAct = 6
    | GuaranteeMail = 7
    | Diploma = 8

    module TaskType = 
        open TaskType
        let toString (taskType: TaskType) = 
            match taskType with
            | PracticePresentation -> 
