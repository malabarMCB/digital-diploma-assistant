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

    let hits<'T when 'T: not struct> (query: ISearchResponse<'T>) = 
        query.Hits |> Seq.cast<IHit<'T>>

    let query<'T when 'T: not struct>  indexName (f:(SearchDescriptor<'T> -> QueryContainer)) (elasticClient: ElasticClient) : ISearchResponse<'T> = 
        elasticClient.Search<'T>(fun (s: SearchDescriptor<'T>) ->       
            let index = Indices.Parse(indexName)
            let types = Types.Parse("_doc")
            let request = SearchRequest(index, types)
            request.Query <- f(s)
            request :> ISearchRequest
        )

