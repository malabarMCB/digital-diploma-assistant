namespace Authentication

type UserRole = 
    | Student = 1
    | ScienceMaster = 2
    | Metodist = 3
    | NormController = 4
    | UnickeckManager = 5

type User = {
    Id: string
    Login: string
    FirstName: string
    LastName: string
    Role: UserRole
}        
