namespace Domain

module Task = 
    open Domain.TaskPublicTypes

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
            | TaskStatusExtended.NormControllerInProgress -> [TaskStatusExtended.UnicheckvalidatorToDo]
            | TaskStatusExtended.UnicheckvalidatorToDo -> [TaskStatusExtended.ReadyForMetodist]
            | TaskStatusExtended.ReadyForMetodist-> []

