namespace Tests

open DataAccess.Query
open System
open NUnit.Framework
open Nest

type Data = {
        Name: string
        Student: string
        Assignee: string
        Group: string
        Status: string
        Type: string
}

[<TestClass>]
type TestClass () =
    [<Test>]
    member this.ShouldGetDocsFromElastic () =
        let node = new Uri("http://localhost:9200")
        let settings = new ConnectionSettings(node);
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
