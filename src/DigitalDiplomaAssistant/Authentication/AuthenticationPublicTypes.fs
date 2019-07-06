namespace Authentication

type UserRole = 
    | Student = 1
    | Supervisor = 2
    | Metodist = 3
    | NormController = 4
    | UnickeckValidator = 5

module UserRole = 
    let fromString role = 
        match role with 
        | "student" -> UserRole.Student
        | "supervisor" -> UserRole.Supervisor
        | "metodist" -> UserRole.Metodist
        | "normController" -> UserRole.NormController
        | "unicheckValidator" -> UserRole.UnickeckValidator

    let toString role = 
        match role with 
        | Student -> "student"
        | Supervisor -> "supervisor"
        | Metodist -> "metodist"
        | NormController -> "normController"
        | UnickeckValidator -> "unicheckValidator"

type User = {
    Id: string
    Login: string
    FirstName: string
    LastName: string
    Role: UserRole
}        
