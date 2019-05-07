namespace Tests

open Common
open DataAccess.Query
open System
open NUnit.Framework
open Nest
open DataAccess.Types

type Data = {
        Name: string
        Student: string
        Assignee: string
        Group: string
        Status: string
        Type: string
}

module FsNest = 
    open Nest

    let query<'T when 'T: not struct>  indexName typeName (f:(unit -> QueryContainer)) (elasticClient: ElasticClient)= 
        elasticClient.Search<'T>(fun (s: SearchDescriptor<'T>) -> 
            let index = Indices.Parse(indexName)
            let types = Types.Parse(typeName)
            let request = SearchRequest(index, types)
            request.Query <- f()
            request :> ISearchRequest
        )

    let hits<'T when 'T: not struct> (query: ISearchResponse<'T>) = 
        query.Hits |> Seq.cast<IHit<'T>>


[<TestClass>]
type TestClass () =
    let elasticUri = "Elastic:Uri" |> getEnvVariable
    let elasticUserName = "Elastic:UserName" |> getEnvVariable
    let elasticPassword = "Elastic:Password" |> getEnvVariable

    [<Test>]
    member __.ShouldGetDocsFromElastic () =
        let node = new Uri(elasticUri)
        let settings = new ConnectionSettings(node);
        settings.BasicAuthentication(elasticUserName,elasticPassword) |> ignore
        settings.EnableDebugMode(fun x -> x.DebugInformation |> printfn "Debug info %A") |> ignore
        settings.DefaultIndex("dda") |> ignore
        let client = new ElasticClient(settings);
        
        //this works
        let search1 = client.Search<Data>(fun (s:SearchDescriptor<Data>)  ->
          new SearchRequest(
            Query = new QueryContainer(query = BoolQuery(Should = [
              new QueryContainer(query = new TermQuery(Field = new Field("filename"), Value = "a"));
              new QueryContainer(query = new TermQuery(Field = new Field("contents"), Value = "b"))]))
          ) :> ISearchRequest)
        
        //this works
        let search = client.Search<Data>(fun (s: SearchDescriptor<Data>) ->
            SearchRequest(
                                 Query = QueryContainer(MatchQuery(Field = Field("status"), Query ="in progress"))
                             ) :> ISearchRequest)
        
        //this works
        let search2 = client.Search<Data> (fun (s : SearchDescriptor<Data>) ->
            let indeces = Indices.Parse("dda")
            let types = Types.Parse("task")
            let request = SearchRequest(indeces, types)
            request.Query <- QueryContainer(MatchAllQuery()) 
            request :> ISearchRequest
            )
        
        search2.Documents |> printfn "%A"
        ()

    [<Test>]
    member __.ShouldReturnTasks () =
        let node = new Uri(elasticUri)
        let settings = new ConnectionSettings(node);

        //this compiles but does not work
        //settings.DefaultMappingFor<DashboardQuery.Task>(FsNest.config<DashboardQuery.Task> (fun x -> x.IdProperty("Id") :> IClrTypeMapping<DashboardQuery.Task>)) |> ignore

        settings.BasicAuthentication(elasticUserName, elasticPassword) |> ignore
        settings.EnableDebugMode(fun x -> x.DebugInformation |> printfn "Debug info %A") |> ignore
        settings.DefaultIndex("dda") |> ignore
        let client = ElasticClient(settings);

        let search = client |> FsNest.query<ElasticTypes.Task> "dda" "task" (fun () -> 
            let query = QueryContainer(MatchAllQuery())
            query
            ) 
        search.Hits |> Seq.cast<IHit<ElasticTypes.Task>> |> Seq.iter( fun x -> x.Source |> printfn "%A")
        search.Documents |> printfn "%A"
        ()

    [<Test>]
    member __.ShouldFindByKeyword () = 
        let node = new Uri(elasticUri)
        let settings = new ConnectionSettings(node);
        settings.BasicAuthentication(elasticUserName,elasticPassword) |> ignore
        settings.EnableDebugMode(fun x -> x.DebugInformation |> printfn "Debug info %A") |> ignore
        settings.DefaultIndex("dda") |> ignore
        let client = new ElasticClient(settings);

        client
        |> FsNest.query<ElasticTypes.Task> "dda" "task" (fun () -> 
            QueryContainer(MatchQuery(Field = Field("type"), Query = "практики" ))
        ) |> FsNest.hits<ElasticTypes.Task>
        |> Seq.iter (fun x -> x.Source |> printfn "%A")
        ()
