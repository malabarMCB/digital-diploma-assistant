namespace Api.Models

open System.ComponentModel.DataAnnotations;

module Login = 
    type LoginRequest = {
        [<Required>]
        Login: string

        [<Required>]
        Password: string
    }