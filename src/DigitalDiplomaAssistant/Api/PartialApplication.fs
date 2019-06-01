namespace Api

[<AutoOpen>]
module PartialApplication = 
    open DataAccess
    open Common

    let private options = {
        Uri = "Elastic:Uri" |> getEnvVariable
        UserName = "Elastic:UserName" |> getEnvVariableOption
        Password = "Elastic:Password" |> getEnvVariableOption
    }
    
    let getDashboardTasks () = options |> DataAccess.Dashboard.Queries.getTasks |> Domain.Dashboard.Flows.getTasks
    let authenticate = options |> DataAccess.Authentication.Queries.getUser |> Authentication.Authentication.authenticate
