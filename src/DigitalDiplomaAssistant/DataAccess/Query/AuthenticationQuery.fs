namespace DataAccess.Query

module AuthenticationQuery = 
    open Authentication
    open DataAccess.ElasticConfig
    open DataAccess
    open Nest
    open System

    type User = {
        Login: string
        FirstName: string
        LastName: string
        Role: string
    }

    let getUser (elasticOptions: ElasticOptions) login password: Authentication.User option = 
        elasticOptions
        |> FsNest.createElasticClient
        |> FsNest.query<User> "dda-user" (fun sd -> 
            sd.Size(Nullable(1)) |> ignore
            QueryContainer(BoolQuery(Must = [
                QueryContainer(TermQuery(Field = Field("login"), Value = login));
                QueryContainer(TermQuery(Field = Field("password"), Value = password))])))
        |> FsNest.hits<User> 
        |> Seq.tryHead
        |> Option.map (fun x -> 
            {
                Id = x.Id
                Login = x.Source.Login
                FirstName = x.Source.FirstName
                LastName = x.Source.LastName
                Role = match x.Source.Role with
                    | "student" -> Student
                    | "scienceMaster" -> ScienceMaster
                    | "metodist" -> Metodist
                    | "normController" -> NormController
                    | "unicheckManager" -> UnickeckManager
            })
            
