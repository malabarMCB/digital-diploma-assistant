namespace Queries

[<AutoOpen>]
module PublicTypes = 
    open System;

    type TaskStatus =
    | ToDo
    | InProgress
    | Done

    type Person = {
        Id: string
        FirstName: string
        LastName: string
    }

    type Student = Person
    type ScienceMaster = Person
    type Assignee = Person

    type Attachment = {
        Name: string
        FilePath: string
        UploadDate: DateTime
    }

    type Description = {
        Text: string
        Attachments: Attachment list
    }

    type Comment = {
        Author: Person
        Text: string
        PostDate: DateTime
        Attachments: Attachment list
    }

