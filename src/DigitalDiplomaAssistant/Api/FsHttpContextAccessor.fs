namespace Api

open Authentication
open Microsoft.AspNetCore.Http
open System.Linq
open System.Security.Claims
open Microsoft.AspNetCore.Authentication;
open Microsoft.AspNetCore.Authentication.Cookies;

module FsHttpContextAccessor = 
    let private getClaimValue claimType (httpContextAccessor: IHttpContextAccessor) = 
        match httpContextAccessor.HttpContext.User.Claims.SingleOrDefault(fun (claim: Claim) -> claim.Type = claimType) with
        | null -> None
        | claim -> Some claim.Value

    let setAuthCookie (httpContextAccessor: IHttpContextAccessor) user = 
        let claims: Claim list = [
            Claim(ClaimTypes.Sid, user.Id);
            Claim(ClaimTypes.Name, user.FirstName)
            Claim(ClaimTypes.Surname, user.LastName)
            Claim(ClaimTypes.Role, user.Role.ToString())
        ]
        let id = ClaimsIdentity(claims, ClaimsIdentity.DefaultNameClaimType);
        httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id)) |> Async.AwaitTask

    let getUserId httpContextAccessor = 
        httpContextAccessor 
        |> getClaimValue ClaimTypes.Sid

    let getUserFirstName httpContextAccessor = 
        httpContextAccessor
        |> getClaimValue ClaimTypes.Name

    let getUserLastName httpContextAccessor = 
        httpContextAccessor
        |> getClaimValue ClaimTypes.Surname
            

