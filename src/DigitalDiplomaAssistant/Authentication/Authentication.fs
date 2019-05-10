namespace Authentication

[<AutoOpen>]
module Authentication =
    type GetUser = string -> string -> User option

    type AuthenticationResult = Result<User, string>

    let authenticate (getUser: GetUser) (login, password): AuthenticationResult = 
        match getUser login password with
        | Some user -> Ok user
        | None -> Error "There is no user with such login and password"
