namespace Domain

module TaskActions = 
    open System.IO
    open System
    open Authentication 

    type GetPersonByRole = UserRole -> Person

    let saveCommentFile (folderPath: string) (taskId: string) (fileName: string) (file: Stream) = 
        let directory = folderPath + @"\" + taskId
        if  Directory.Exists directory = false then
            Directory.CreateDirectory directory |> ignore
        let createdFile = File.Create (directory + @"\" + Guid.NewGuid().ToString() + Path.GetExtension fileName)
        file.CopyTo createdFile
        Path.GetFileName createdFile.Name

    let updateTaskByNewStatus (getPersonByRole: GetPersonByRole) (task: Task) (status: TaskStatus) = 
        match status with 
        | TaskStatus.StudentToDo | TaskStatus.StudentInProgress -> {task with Status = status; Assignee = task.Student}
        | TaskStatus.SupervisorToDo | TaskStatus.SupervisorInProgress -> {task with Status = status; Assignee = task.Supervisor}
        | TaskStatus.NormControllerToDo | TaskStatus.NormControllerInProgress -> {task with Status = status; Assignee = getPersonByRole UserRole.NormController}
        | TaskStatus.UnicheckValidatorToDo | TaskStatus.UnicheckValidatorInProgress -> {task with Status = status; Assignee = getPersonByRole UserRole.UnickeckValidator}
        | TaskStatus.ReadyForMetodist -> {task with Status = status; Assignee = getPersonByRole UserRole.Metodist}
    
    let createTaskWithAvaliableStatuses (task: Task) =
        let getAvaliableStatuses taskType taskStatus: Result<TaskStatus list, string> = 
            let errorMsg = "Such combination of task type and task status is impossible"
            match taskType with
            | TaskType.PracticePresentation | TaskType.PracticeDairy | TaskType.PracticeReport
            | TaskType.TopicAcceptanceDocument | TaskType.PracticeReview | TaskType.PreDefencePresentation
            | TaskType.DefencePresentation | TaskType.SupervisorReview | TaskType.InjectionAct
            | TaskType.GuaranteeMail -> 
                match taskStatus with
                | TaskStatus.StudentToDo -> Ok [TaskStatus.StudentInProgress]
                | TaskStatus.StudentInProgress -> Ok [TaskStatus.StudentToDo; TaskStatus.SupervisorToDo]
                | TaskStatus.SupervisorToDo -> Ok [TaskStatus.SupervisorInProgress]
                | TaskStatus.SupervisorInProgress -> Ok [TaskStatus.StudentToDo; TaskStatus.ReadyForMetodist]
                | TaskStatus.ReadyForMetodist -> Ok []
                | _ -> Error errorMsg
            | TaskType.Diploma -> 
                match taskStatus with
                | TaskStatus.StudentToDo -> Ok [TaskStatus.StudentInProgress]
                | TaskStatus.StudentInProgress -> Ok [TaskStatus.StudentToDo; TaskStatus.SupervisorToDo]
                | TaskStatus.SupervisorToDo -> Ok [TaskStatus.SupervisorInProgress]
                | TaskStatus.SupervisorInProgress -> Ok [TaskStatus.StudentToDo; TaskStatus.NormControllerToDo]
                | TaskStatus.NormControllerToDo -> Ok [TaskStatus.NormControllerInProgress]
                | TaskStatus.NormControllerInProgress -> Ok [TaskStatus.UnicheckValidatorToDo]
                | TaskStatus.UnicheckValidatorToDo -> Ok [TaskStatus.ReadyForMetodist]
                | TaskStatus.ReadyForMetodist-> Ok []
                | _ -> Error errorMsg
        getAvaliableStatuses task.Type task.Status
        |> Result.map (fun avaliableStatuses -> TaskWithAvaliableStatuses.fromTask task avaliableStatuses)
    

