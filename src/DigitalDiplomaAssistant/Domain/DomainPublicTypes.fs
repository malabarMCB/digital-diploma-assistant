namespace Domain

open System;

type Person = {
    Id: string
    FirstName: string
    LastName: string
}

type Student = Person
type Supervisor = Person
type Assignee = Person

type Attachment = {
    Name: string
    FilePath: string
    UploadDate: DateTime
}

type Description = {
    Text: string
    Attachments: Attachment list
}

type Comment = {
    Author: Person
    Text: string
    PostDate: DateTime
    Attachments: Attachment list
}

type TaskStatus = 
| StudentToDo = 1
| StudentInProgress = 2
| SupervisorToDo = 3
| SupervisorInProgress = 4
| NormControllerToDo = 5
| NormControllerInProgress = 6
| UnicheckValidatorToDo = 7
| UnicheckValidatorInProgress = 8
| ReadyForMetodist = 9

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

type Task = {
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
}

