namespace Domain

open System
open System.IO
open Authentication

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

[<AutoOpen>]
module TaskQueryPublicTypes = 
    type DashboardTaskStatus =
        | ToDo
        | InProgress
        | Done
    
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
    
    type MetodistTaskDescription = {
        Type: TaskType
        Description: Description
    }

[<AutoOpen>]
module TaskWorkflowCommands = 
    type ChangeTaskStatusCommand = {
        TaskId: string
        TaskStatus: TaskStatus
    }

    type AddCommentCommand = {
        TaskId: string
        CommentText: string
        AttachmentStream: Stream
        AttachmentName: string
        CommentAuthor: Person
    }

[<AutoOpen>]
module Results = 
    type GetTaskWithAvaliableStatusesResult = Result<TaskWithAvaliableStatuses, string>
    type GetOptionalTaskWithAvaliableStatusesResult = Result<TaskWithAvaliableStatuses option, string>

[<AutoOpen>]
module WorkflowInterfaces = 
    type GetTaskWithAvaliableStatusesWorkflow = string -> GetOptionalTaskWithAvaliableStatusesResult
    type ChangeStatusWorkflow = ChangeTaskStatusCommand -> GetTaskWithAvaliableStatusesResult
    type AddCommentWorkflow = AddCommentCommand -> GetOptionalTaskWithAvaliableStatusesResult
    type GetAttachmentFileStreamWorkflow = string -> string -> FileStream
    type GetDashboardTasksWorkflow = unit -> seq<DashboardTask>
    type GetMetodistTaskDescriptionWorkflow = TaskType -> MetodistTaskDescription
    type DeleteDescriptionAttachmentWorkflow = Attachment -> TaskType -> MetodistTaskDescription

[<AutoOpen>]
module DataAccessInterfaces = 
    type GetTaskByIdFromDb = string -> Task option
    type UpdateTaskStatusInDb = string -> TaskStatus -> Person -> unit
    type AddCommentToDb = string -> Comment -> unit
    type DeleteDescriptionAttachmentFromDb = Attachment -> TaskType -> unit
    type GetPersonByRole = UserRole -> Person

[<AutoOpen>]
module InternalInterfaces = 
    type CreateTaskWithAvaliableStatuses = Task -> GetTaskWithAvaliableStatusesResult
    type UpdateTaskByNewStatus = Task -> TaskStatus -> Task 
    type SaveCommentFileToStorage = string -> string -> Stream -> string
    type GetFileStream = string -> FileStream