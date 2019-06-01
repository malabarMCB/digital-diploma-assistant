namespace DataAccess.Dashboard

[<AutoOpen>]
module PublicTypes = 
    open System;

    type Attachment = {
        Name: string
        FilePath: string
        UploadDate: DateTime
     }

     type Description = {
        Text: string
        Attachments: Attachment list
     }

     type Person = {
        Id: string
        FirstName: string
        LastName: string
     }

     type Student = Person
     type ScienceMaster = Person
     type Assignee = Person

     type Comment = {
        Author: Person
        Text: string
        PostDate: DateTime
        Attachments: Attachment list
     }

     type Task = {
        Id: string
        Type: string
        Student: Student
        Assignee: Assignee
        ScienceMaster: ScienceMaster
        Group: string
        Status: string
        Deadline: DateTime
        //Description: Description
        //Comments: Comment list
     }
