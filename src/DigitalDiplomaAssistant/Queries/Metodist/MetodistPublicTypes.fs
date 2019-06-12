namespace Queries.Metodist

module PublicTypes = 
    open Domain.PublicTypes

    type ElasticTaskDescription = {
        Type: string
        Description: Description
    }