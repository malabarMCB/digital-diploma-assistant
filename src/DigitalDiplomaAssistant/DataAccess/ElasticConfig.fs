namespace DataAccess

[<AutoOpen>]
module ElasticConfig = 
    type ElasticOptions = {
        Uri: string
        UserName: string option
        Password: string option
    }
    

