namespace Api

[<AutoOpen>]
module PartialApplication = 
    open DataAccess
    open Common
    open Queries
    open Domain

    let private options = {
        Uri = "Elastic:Uri" |> getEnvVariable
        UserName = "Elastic:UserName" |> getEnvVariableOption
        Password = "Elastic:Password" |> getEnvVariableOption
    }
    
    let getDashboardTasks () = options |> Dashboard.Queries.getTasks
    let authenticate = options |> Authentication.Queries.getUser |> Authentication.Authentication.authenticate
    let getTaskById (id: string) : TaskPublicTypes.Task option = Task.Queries.getTaskById Task.getAvaliableStatuses options id
    
