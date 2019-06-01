namespace DataAccess.Authentication

[<AutoOpen>]
module Queries = 
    open DataAccess
    open Nest
    open System

    let private  setUserId (hit: IHit<User>): User = 
        {hit.Source with Id = hit.Id}
        

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
        |> Option.map setUserId

