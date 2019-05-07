namespace Common

[<AutoOpen>]
module EnvVariables =
    open System

    let getEnvVariableOption name : string option= 
        match Environment.GetEnvironmentVariable (name, EnvironmentVariableTarget.User) with
        | null -> None
        | value -> Some value
    
    let getEnvVariable name: string = 
        match getEnvVariableOption name with
        | None -> name |> failwithf "There is no env variable with name: %s"
        | Some value -> value 