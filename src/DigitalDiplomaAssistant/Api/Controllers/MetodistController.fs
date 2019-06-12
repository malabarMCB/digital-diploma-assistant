namespace Api.Controllers

open Microsoft.AspNetCore.Mvc
open Api
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Authentication.Cookies;
open Domain.TaskPublicTypes
open Microsoft.AspNetCore.Http
open System.Linq
open System.Security.Claims
open Domain.PublicTypes
open System.IO

[<Route("[controller]")>]
[<Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)>]
type MetodistController () = 
    inherit Controller()

