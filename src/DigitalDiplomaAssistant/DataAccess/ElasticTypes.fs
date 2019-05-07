namespace DataAccess.Types

module ElasticTypes = 
     open System

     type Task = {
        Type: string
        Student: string
        Assignee: string
        Group: string
        Status: string
        Deadline: DateTime
     }
