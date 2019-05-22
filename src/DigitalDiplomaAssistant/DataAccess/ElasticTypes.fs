namespace DataAccess.Types

module ElasticTypes = 
     open System

     type Person = {
        Id: string
        FirstName: string
        LastName: string
     }

     type Student = Person
     type ScienceMaster = Person
     type Assignee = Person

     type Task = {
        Type: string
        Student: Student
        Assignee: Assignee
        ScienceMaster: ScienceMaster
        Group: string
        Status: string
        Deadline: DateTime
     }
