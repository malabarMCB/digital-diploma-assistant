namespace Authentication

[<AutoOpen>]
module Authentication =
    type GetUser = string -> string -> User option

    let authenticate (getUser: GetUser) login password = 
        match getUser login password with
        | Some user -> Ok user
        | None -> Error (sprintf "There is no user with login= %s and password= %s" login password)
