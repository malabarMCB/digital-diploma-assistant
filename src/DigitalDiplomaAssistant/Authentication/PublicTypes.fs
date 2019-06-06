namespace Authentication

type UserRole = 
    | Student = 1
    | Supervisor = 2
    | Metodist = 3
    | NormController = 4
    | UnickeckValidator = 5

type User = {
    Id: string
    Login: string
    FirstName: string
    LastName: string
    Role: UserRole
}        
