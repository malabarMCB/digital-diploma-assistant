namespace Tests

open Common
open System
open NUnit.Framework
open Nest
open DataAccess
open DataAccess.Task
open Domain
open System.Collections
open System.Collections.Generic

type Data = {
        Name: string
        Student: string
        Assignee: string
        Group: string
        Status: string
        Type: string
}

type A () = class end
type B () = class end

type Script<'T> = {
    Lang: string
    Inline: string
    Params: 'T
}

type Update<'T> = {
    Script: Script<'T>
}

type CommentParam = {
    Comment: Comment
}

[<TestClass>]
type TestClass () =
    let elasticUri = "Elastic:Uri" |> EnvVariables.getEnvVariable
    let elasticUserName = "Elastic:UserName" |> EnvVariables.getEnvVariable
    let elasticPassword = "Elastic:Password" |> EnvVariables.getEnvVariable

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

        let search = client |> FsNest.query<ElasticTask> "dda-task" (fun sd -> 
            let query = QueryContainer(MatchAllQuery())
            query
            ) 
        search.Hits |> Seq.cast<IHit<ElasticTask>> |> Seq.iter( fun x -> x.Source |> printfn "%A")
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
        |> FsNest.query<ElasticTask> "dda-task" (fun sd -> 
            QueryContainer(MatchQuery(Field = Field("type"), Query = "практики" ))
        ) |> FsNest.hits
        |> Seq.iter (fun x -> x.Source |> printfn "%A")
        ()

    [<Test>]
    member __.AddCommentScenario () =
        let node = new Uri(elasticUri)
        let settings = new ConnectionSettings(node);
        settings.BasicAuthentication(elasticUserName,elasticPassword) |> ignore
        let client = ElasticClient(settings)
        
        //this not work
        //let indexName = IndexName.op_Implicit "dda-task"
        //let typeName = TypeName.op_Implicit "_doc"
        //let id = Id.op_Implicit ""
        //let updateRequest = new UpdateRequest<Queries.Task.PublicTypes.ElasticTask, Queries.PublicTypes.Comment>(indexName, typeName, id)
        //updateRequest.Script = new Scriptbas() :> IScript
        //()

        //this works
        //let postData = Elasticsearch.Net.PostData.op_Implicit "{\"script\":{\"lang\":\"painless\",\"inline\":\"ctx._source.comments.add(params.comment)\",\"params\":{\"comment\":{\"text\":\"yoo\"}}}}"

        //this works too
        let update = {
            Script = {
                Lang = "painless"
                Inline = "ctx._source.comments.add(params.comment)"
                Params = {
                    Comment = {
                        Author= {
                            Id = "test id"
                            FirstName = "ddd"
                            LastName = "bbb"
                        }
                        Text = "some text 4 comment"
                        PostDate = System.DateTime.UtcNow
                        Attachments = [
                        {
                            Name = "file1"
                            FilePath = "C:"
                            UploadDate = System.DateTime.UtcNow
                        }
                        ]
                    }
                }
            } 
        } 

        let postData = Elasticsearch.Net.PostData.Serializable update
        let result = client.LowLevel.Update<Elasticsearch.Net.DynamicResponse>("dda-task", "_doc", "GJZPLGsB2N0LygMFURhw", postData)
        ()

    [<Test>]
    member __.UpdateDocByNestScenario() = 
        let node = new Uri(elasticUri)
        let settings = new ConnectionSettings(node);
        settings.BasicAuthentication(elasticUserName,elasticPassword) |> ignore
        let client = ElasticClient(settings)

        let indexName = IndexName.op_Implicit "dda-task"
        let typeName = TypeName.op_Implicit "_doc"
        let id = Id.op_Implicit "hqqpw2sBkN32tI4ypDeK"

        let updateRequest = UpdateRequest(indexName, typeName, id)
        updateRequest.Doc <- {|Group = "BS-anonymous"|}

        let updateResponse = client.Update<_,_>(updateRequest :> IUpdateRequest<_,_>)
        updateResponse |> printfn "%A"

    [<Test>]
    member __.UpdateScriptByNestScenario() = 
        let node = new Uri(elasticUri)
        let settings = new ConnectionSettings(node);
        settings.BasicAuthentication(elasticUserName,elasticPassword) |> ignore
        settings.EnableDebugMode(fun x -> x.DebugInformation |> printfn "Debug info %A") |> ignore
        let client = ElasticClient(settings)

        let indexName = IndexName.op_Implicit "dda-task"
        let typeName = TypeName.op_Implicit "_doc"
        let id = Id.op_Implicit "hqqpw2sBkN32tI4ypDeK"

        let updateRequest = UpdateRequest(indexName, typeName, id)
        let script =  InlineScript("ctx._source.comments.add(params.comment)")
        script.Lang <- "painless"
        //let commentUpdate = {|Text = "Updated from F# 2 "|}
        //param.Add("comment", commentUpdate )
        script.Params <- Dictionary<string, obj>()
        script.Params.Add("comment", {|Text = "Updated from F# 2 "|})

        updateRequest.Script <- script

        let updateResponse = client.Update<_,_>(updateRequest :> IUpdateRequest<_,_>)
        updateResponse |> printfn "%A"

    [<Test>]
    member __.UpdateByQueryByNestScenario() = 
        let node = new Uri(elasticUri)
        let settings = new ConnectionSettings(node);
        settings.BasicAuthentication(elasticUserName,elasticPassword) |> ignore
        settings.EnableDebugMode(fun x -> x.DebugInformation |> printfn "Debug info %A") |> ignore
        let client = ElasticClient(settings)

        let indexName = Indices.op_Implicit(IndexName.op_Implicit "dda-task")
        let typeName = Types.op_Implicit(TypeName.op_Implicit "_doc")

        let updateRequest = UpdateByQueryRequest(indexName, typeName)
        let script =  InlineScript("ctx._source.comments.add(params.comment)")
        script.Lang <- "painless"
        let param = Dictionary<string,obj>()
        let commentUpdate = {|Text = "Updated by query from F#"|}
        param.Add("comment", commentUpdate )
        script.Params <- param

        updateRequest.Script <- script
        updateRequest.Query <- QueryContainer(TermQuery(Field = Field("type"), Value = "Щоденник практики"))

        let updateResponse = client.UpdateByQuery(updateRequest)
        updateResponse |> printfn "%A"

