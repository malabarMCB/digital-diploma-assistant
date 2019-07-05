namespace Authentication

[<AutoOpen>]
module Authentication =
    type GetUser = string -> string -> User option

    type AuthenticationResult = Result<User, string>

    let authenticate (getUser: GetUser) (login, password): AuthenticationResult = 
        password |> getUser login 
        |> (fun userOption -> 
            match userOption with 
            | Some user -> Ok user
            | None -> Error "There is no user with such login and password")
