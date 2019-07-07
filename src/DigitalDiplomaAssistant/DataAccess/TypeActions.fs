namespace DataAccess

open Domain

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

    let fromString (taskType: string) = 
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

module DashboardTaskStatus = 
    let fromString status = 
        match status with
        | "StudentInProgress" | "SupervisorInProgress" | "NormControllerInProgress" | "UnicheckValidatorInProgress" -> DashboardTaskStatus.InProgress
        | "StudentToDo"| "SupervisorToDo" | "NormControllerToDo" | "UnicheckValidatorToDo" -> DashboardTaskStatus.ToDo
        | "ReadyForMetodist" -> DashboardTaskStatus.Done

module TaskStatus = 
    let fromString (status: string) = 
        match status with 
        | "StudentToDo" -> TaskStatus.StudentToDo
        | "StudentInProgress" -> TaskStatus.StudentInProgress
        | "SupervisorToDo" -> TaskStatus.SupervisorToDo
        | "SupervisorInProgress" -> TaskStatus.SupervisorInProgress
        | "NormControllerToDo" -> TaskStatus.NormControllerToDo
        | "NormControllerInProgress" -> TaskStatus.NormControllerInProgress
        | "UnicheckValidatorToDo" -> TaskStatus.UnicheckValidatorToDo
        | "UnicheckValidatorInProgress" -> TaskStatus.UnicheckValidatorInProgress
        | "ReadyForMetodist" -> TaskStatus.ReadyForMetodist

