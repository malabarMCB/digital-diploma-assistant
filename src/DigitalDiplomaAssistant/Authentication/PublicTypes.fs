namespace Authentication

type UserRole = 
    | Student
    | ScienceMaster
    | Metodist
    | NormController
    | UnickeckManager

type User = {
    Id: string
    Login: string
    FirstName: string
    LastName: string
    Role: UserRole
}
