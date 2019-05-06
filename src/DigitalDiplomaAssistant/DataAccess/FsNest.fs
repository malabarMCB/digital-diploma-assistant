namespace DataAccess

module internal FsNest = 
    open System
    open Nest

    let createElasticClient (options: ElasticOptions) = 
        let node = Uri(options.Uri)
        let settings = new ConnectionSettings(node);
        if options.UserName.IsSome && options.Password.IsSome then
            settings.BasicAuthentication(options.UserName.Value,options.Password.Value) |> ignore
        ElasticClient(settings);

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

