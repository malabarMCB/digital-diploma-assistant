namespace Queries.Authentication

[<AutoOpen>]
module PublicTypes = 
    type User = {
        Id: string
        Login: string
        FirstName: string
        LastName: string
        Role: string
    }

