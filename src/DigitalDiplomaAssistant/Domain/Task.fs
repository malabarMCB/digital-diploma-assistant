namespace Domain

module Task = 
    open Domain.TaskPublicTypes
    open System.IO
    open System

    type GetAvaliableStatuses = string -> TaskStatusExtended -> TaskStatusExtended list

    let getAvaliableStatuses taskType taskStatus: TaskStatusExtended list = 
        match taskType with
        | "Презентація захисту практики" | "Щоденник практики" | "Звіт з практики" 
        | "Бланк заяви про обрання теми" | "Відгук керівника" | "Презентація предзахисту"
        | "Презентація до захисту" | "Відгук наукового керівника" | "Акти впровадженя"
        | "Гарантійний лист" -> 
            match taskStatus with
            | TaskStatusExtended.StudentToDo -> [TaskStatusExtended.StudentInProgress]
            | TaskStatusExtended.StudentInProgress -> [TaskStatusExtended.StudentToDo; TaskStatusExtended.SupervisorToDo]
            | TaskStatusExtended.SupervisorToDo -> [TaskStatusExtended.SupervisorInProgress]
            | TaskStatusExtended.SupervisorInProgress -> [TaskStatusExtended.StudentToDo; TaskStatusExtended.ReadyForMetodist]
            | TaskStatusExtended.ReadyForMetodist -> []
        | "Дипломна робота" -> 
            match taskStatus with
            | TaskStatusExtended.StudentToDo -> [TaskStatusExtended.StudentInProgress]
            | TaskStatusExtended.StudentInProgress -> [TaskStatusExtended.StudentToDo; TaskStatusExtended.SupervisorToDo]
            | TaskStatusExtended.SupervisorToDo -> [TaskStatusExtended.SupervisorInProgress]
            | TaskStatusExtended.SupervisorInProgress -> [TaskStatusExtended.StudentToDo; TaskStatusExtended.NormControllerToDo]
            | TaskStatusExtended.NormControllerToDo -> [TaskStatusExtended.NormControllerInProgress]
            | TaskStatusExtended.NormControllerInProgress -> [TaskStatusExtended.UnicheckValidatorToDo]
            | TaskStatusExtended.UnicheckValidatorToDo -> [TaskStatusExtended.ReadyForMetodist]
            | TaskStatusExtended.ReadyForMetodist-> []

    let saveCommentFile (folderPath: string) (taskId: string) (fileName: string) (file: Stream) = 
        let directory = folderPath + @"\" + taskId
        if  Directory.Exists directory = false then
            Directory.CreateDirectory directory |> ignore
        let createdFile = File.Create (directory + @"\" + Guid.NewGuid().ToString() + Path.GetExtension fileName)
        file.CopyTo createdFile
        Path.GetFileName createdFile.Name

    let createComment (author: Person) (text: string) (fileName: string) (filePath: string): Comment =
        {
            Author = author
            Text = text
            PostDate = DateTime.UtcNow
            Attachments = [
                {
                    Name = fileName
                    FilePath = filePath
                    UploadDate = DateTime.UtcNow
                }
            ]
        }

    let updateTaskStatus (getPersonByRole: string -> Person) (task: Task) (status: TaskStatusExtended) = 
        match status with 
        | TaskStatusExtended.StudentToDo | TaskStatusExtended.StudentInProgress -> {task with Status = status; Assignee = task.Student}
        | TaskStatusExtended.SupervisorToDo | TaskStatusExtended.SupervisorInProgress -> {task with Status = status; Assignee = task.Supervisor}
        | TaskStatusExtended.NormControllerToDo | TaskStatusExtended.NormControllerInProgress -> {task with Status = status; Assignee = getPersonByRole "normController"}
        | TaskStatusExtended.UnicheckValidatorToDo | TaskStatusExtended.UnicheckValidatorInProgress -> {task with Status = status; Assignee = getPersonByRole "unicheckValidator"}
        | TaskStatusExtended.ReadyForMetodist -> {task with Status = status; Assignee = getPersonByRole "metodist"}

