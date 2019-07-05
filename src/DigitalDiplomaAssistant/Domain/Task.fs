namespace Domain

module Task = 
    open System.IO
    open System

    type GetAvaliableStatuses = TaskType -> TaskStatus -> TaskStatus list

    let getAvaliableStatuses taskType taskStatus: TaskStatus list = 
        match taskType with
        | TaskType.PracticePresentation | TaskType.PracticeDairy | TaskType.PracticeReport
        | TaskType.TopicAcceptanceDocument | TaskType.PracticeReview | TaskType.PreDefencePresentation
        | TaskType.DefencePresentation | TaskType.SupervisorReview | TaskType.InjectionAct
        | TaskType.GuaranteeMail -> 
            match taskStatus with
            | TaskStatus.StudentToDo -> [TaskStatus.StudentInProgress]
            | TaskStatus.StudentInProgress -> [TaskStatus.StudentToDo; TaskStatus.SupervisorToDo]
            | TaskStatus.SupervisorToDo -> [TaskStatus.SupervisorInProgress]
            | TaskStatus.SupervisorInProgress -> [TaskStatus.StudentToDo; TaskStatus.ReadyForMetodist]
            | TaskStatus.ReadyForMetodist -> []
        | TaskType.Diploma -> 
            match taskStatus with
            | TaskStatus.StudentToDo -> [TaskStatus.StudentInProgress]
            | TaskStatus.StudentInProgress -> [TaskStatus.StudentToDo; TaskStatus.SupervisorToDo]
            | TaskStatus.SupervisorToDo -> [TaskStatus.SupervisorInProgress]
            | TaskStatus.SupervisorInProgress -> [TaskStatus.StudentToDo; TaskStatus.NormControllerToDo]
            | TaskStatus.NormControllerToDo -> [TaskStatus.NormControllerInProgress]
            | TaskStatus.NormControllerInProgress -> [TaskStatus.UnicheckValidatorToDo]
            | TaskStatus.UnicheckValidatorToDo -> [TaskStatus.ReadyForMetodist]
            | TaskStatus.ReadyForMetodist-> []

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

    let updateTaskStatus (getPersonByRole: string -> Person) (task: Task) (status: TaskStatus) = 
        match status with 
        | TaskStatus.StudentToDo | TaskStatus.StudentInProgress -> {task with Status = status; Assignee = task.Student}
        | TaskStatus.SupervisorToDo | TaskStatus.SupervisorInProgress -> {task with Status = status; Assignee = task.Supervisor}
        | TaskStatus.NormControllerToDo | TaskStatus.NormControllerInProgress -> {task with Status = status; Assignee = getPersonByRole "normController"}
        | TaskStatus.UnicheckValidatorToDo | TaskStatus.UnicheckValidatorInProgress -> {task with Status = status; Assignee = getPersonByRole "unicheckValidator"}
        | TaskStatus.ReadyForMetodist -> {task with Status = status; Assignee = getPersonByRole "metodist"}

