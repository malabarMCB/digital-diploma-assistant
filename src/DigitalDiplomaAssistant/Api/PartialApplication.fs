namespace Api

[<AutoOpen>]
module PartialApplication = 
    open DataAccess
    open Common
    open DataAccess.Query

    let private options = {
        Uri = "Elastic:Uri" |> getEnvVariable
        UserName = "Elastic:UserName" |> getEnvVariableOption
        Password = "Elastic:Password" |> getEnvVariableOption
    }
    
    let getDashboardTasks = DashboardQuery.getTasks options
