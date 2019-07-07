namespace DataAccess

module FsNest = 
    open System
    open Nest

    let createElasticClient (options: ElasticOptions) = 
        let node = Uri(options.Uri)
        let settings = new ConnectionSettings(node);
        if options.UserName.IsSome && options.Password.IsSome then
            settings.BasicAuthentication(options.UserName.Value,options.Password.Value) |> ignore
        ElasticClient(settings);

    let hits (query: ISearchResponse<'T>) = 
        query.Hits |> Seq.cast<IHit<'T>>

    let query<'T when 'T: not struct>  indexName (f:(SearchDescriptor<'T> -> QueryContainer)) (elasticClient: ElasticClient) : ISearchResponse<'T> = 
        elasticClient.Search<'T>(fun (s: SearchDescriptor<'T>) ->       
            let index = Indices.Parse(indexName)
            let types = Types.Parse("_doc")
            let request = SearchRequest(index, types)
            request.Query <- f(s)
            request :> ISearchRequest
        )

    let update: string -> string -> (UpdateRequest<_,_> -> unit) -> ElasticClient -> IUpdateResponse<_> = 
        fun indexName id f elasticClient  -> 
            let indexName = IndexName.op_Implicit indexName
            let typeName = TypeName.op_Implicit "_doc"
            let id = Id.op_Implicit id
            let updateRequest = UpdateRequest(indexName, typeName, id)
            f(updateRequest) |> ignore
            elasticClient.Update<_,_>(updateRequest :> IUpdateRequest<_,_>)

    let updateByQuery: string -> (UpdateByQueryRequest -> unit) -> ElasticClient -> IUpdateByQueryResponse = 
        fun indexName f elasticClient -> 
            let indexName = indexName |> IndexName.op_Implicit |>Indices.op_Implicit
            let typeName = "_doc" |> TypeName.op_Implicit  |> Types.op_Implicit
            let updateRequest = UpdateByQueryRequest(indexName, typeName)
            f(updateRequest) |> ignore
            elasticClient.UpdateByQuery(updateRequest)
  
    //type Script<'T> = {
    //    Lang: string
    //    Inline: string
    //    Params: 'T
    //}

    //type Update<'T> = {
    //    Script: Script<'T>
    //}

    //type Query<'T> = {
    //    Query: 'T
    //}

    //type UpdateByQuery<'T, 'U> = {
    //    Query: 'T
    //    Script: Script<'U>
    //}

    //let update indexName id (script: Script<'T>) (elasticClient: ElasticClient)= 
    //    let update = {Script = script}
    //    let postData = Elasticsearch.Net.PostData.Serializable update
    //    let response = elasticClient.LowLevel.Update<Elasticsearch.Net.DynamicResponse>(indexName, "_doc", id, postData)
    //    elasticClient.Refresh (Indices.Parse indexName) |> ignore
    //    response

    //let updateByQuery (elasticClient: ElasticClient) indexName (query: Query<'T>) (script: Script<'U>) = 
    //    let update = {
    //        Query = query.Query
    //        Script = script
    //    }
    //    let postData = Elasticsearch.Net.PostData.Serializable update
    //    let response = elasticClient.LowLevel.UpdateByQuery<Elasticsearch.Net.DynamicResponse>(indexName, "_doc", postData)
    //    elasticClient.Refresh (Indices.Parse indexName) |> ignore
    //    response
