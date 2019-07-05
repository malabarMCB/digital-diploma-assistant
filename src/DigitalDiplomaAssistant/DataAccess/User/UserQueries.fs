namespace DataAccess.User

open System
open Nest
open DataAccess
open Domain.PublicTypes

module Queries = 

    let getUser (elasticOptions: ElasticOptions) login password: User option = 
        elasticOptions
        |> FsNest.createElasticClient
        |> FsNest.query<User> "dda-user" (fun sd -> 
            sd.Size(Nullable(1)) |> ignore
            QueryContainer(BoolQuery(Must = [
                QueryContainer(TermQuery(Field = Field("login"), Value = login));
                QueryContainer(TermQuery(Field = Field("password"), Value = password))])))
        |> FsNest.hits<User> 
        |> Seq.tryHead
        |> Option.map (fun hit -> {hit.Source with Id = hit.Id})

    let getPersonByRole (elasticOptions: ElasticOptions) (role: string): Person = 
        elasticOptions |> FsNest.createElasticClient 
        |> FsNest.query<Person> "dda-user" (fun sd -> 
            sd.Size(Nullable(1)) |> ignore
            QueryContainer(TermQuery(Field = Field("role"), Value = role)))
        |> FsNest.hits<Person>
        |> Seq.head
        |> fun hit -> {hit.Source with Id = hit.Id}


