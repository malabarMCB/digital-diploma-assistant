namespace DataAccess.User

open System
open Nest
open DataAccess
open Domain.PublicTypes
open Authentication

module Queries = 

    let getUser (elasticOptions: ElasticOptions) login password: Authentication.User option = 
        elasticOptions
        |> FsNest.createElasticClient
        |> FsNest.query<User.User> "dda-user" (fun sd -> 
            sd.Size(Nullable(1)) |> ignore
            QueryContainer(BoolQuery(Must = [
                QueryContainer(TermQuery(Field = Field("login"), Value = login));
                QueryContainer(TermQuery(Field = Field("password"), Value = password))])))
        |> FsNest.hits<User.User> 
        |> Seq.tryHead
        |> Option.map (fun hit -> 
            {
                Id = hit.Id
                Login = hit.Source.Login
                FirstName = hit.Source.FirstName
                LastName = hit.Source.LastName
                Role = UserRole.fromString hit.Source.Role
            }          
        )

    let getPersonByRole (elasticOptions: ElasticOptions) (role: string): Person = 
        elasticOptions |> FsNest.createElasticClient 
        |> FsNest.query<Person> "dda-user" (fun sd -> 
            sd.Size(Nullable(1)) |> ignore
            QueryContainer(TermQuery(Field = Field("role"), Value = role)))
        |> FsNest.hits<Person>
        |> Seq.head
        |> fun hit -> {hit.Source with Id = hit.Id}


