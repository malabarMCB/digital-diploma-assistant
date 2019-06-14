namespace Domain

module Metodist =
    type TaskType = 
    | PracticePresentation = 1
    | PracticeDairy = 2
    | PracticeReport = 3
    | TopicAcceptanceDocument = 4
    | PracticeReview = 5
    | PreDefencePresentation = 6
    | DefencePresentation = 7
    | SupervisorReview = 8
    | InjectionAct = 9
    | GuaranteeMail = 10
    | Diploma = 11

    type MetodistTaskDescription = {
        Type: TaskType
        Description: Description
    }

    module TaskType = 

        let toString (taskType: TaskType) = 
            match taskType with
            | TaskType.PracticePresentation -> "Презентація захисту практики"
            | TaskType.PracticeDairy -> "Щоденник практики"
            | TaskType.PracticeReport -> "Звіт з практики"
            | TaskType.TopicAcceptanceDocument -> "Бланк заяви про обрання теми"
            | TaskType.PracticeReview -> "Відгук з практики"
            | TaskType.PreDefencePresentation -> "Презентація предзахисту"
            | TaskType.DefencePresentation -> "Презентація до захисту"
            | TaskType.SupervisorReview -> "Відгук наукового керівника"
            | TaskType.InjectionAct -> "Акти впровадженя"
            | TaskType.GuaranteeMail -> "Гарантійний лист"
            | TaskType.Diploma -> "Дипломна робота"

        let create (taskType: string) = 
            match taskType with
            | "Презентація захисту практики" -> TaskType.PracticePresentation
            | "Щоденник практики" -> TaskType.PracticeDairy
            | "Звіт з практики" -> TaskType.PracticeReport
            | "Бланк заяви про обрання теми" -> TaskType.TopicAcceptanceDocument
            | "Відгук з практики" -> TaskType.PracticeReview
            | "Презентація предзахисту" -> TaskType.PreDefencePresentation
            | "Презентація до захисту" -> TaskType.DefencePresentation
            | "Відгук наукового керівника" -> TaskType.SupervisorReview
            | "Акти впровадженя" -> TaskType.InjectionAct
            | "Гарантійний лист" -> TaskType.GuaranteeMail
            | "Дипломна робота" -> TaskType.Diploma