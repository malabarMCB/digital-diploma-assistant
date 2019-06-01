namespace Authentication

[<AutoOpen>]
module Authentication =
    type GetUser = string -> string -> DataAccess.Authentication.PublicTypes.User option

    type AuthenticationResult = Result<User, string>

    let private toUser (dbUser: DataAccess.Authentication.PublicTypes.User): User = 
        {
                Id = dbUser.Id
                Login = dbUser.Login
                FirstName = dbUser.FirstName
                LastName = dbUser.LastName
                Role = match dbUser.Role with
                    | "student" -> UserRole.Student
                    | "scienceMaster" -> UserRole.ScienceMaster
                    | "metodist" -> UserRole.Metodist
                    | "normController" -> UserRole.NormController
                    | "unicheckManager" -> UserRole.UnickeckManager
            }

    let authenticate (getUser: GetUser) (login, password): AuthenticationResult = 
        password |> getUser login 
        |> Option.map toUser
        |> (fun userOption -> 
            match userOption with 
            | Some user -> Ok user
            | None -> Error "There is no user with such login and password")
