namespace DataAccess.Task

open System.Collections.Generic
open DataAccess
open Domain
open Nest

module Commands = 
    let updateTaskStatus: ElasticOptions -> UpdateTaskStatusInDb = 
        fun elasticOptions id status assignee -> 
            elasticOptions
            |> FsNest.createElasticClient
            |> FsNest.update "dda-task" id (fun request -> 
                let script = InlineScript("ctx._source.status = params.status; ctx._source.assignee = params.assignee")
                script.Lang <- "painless"
                script.Params <- Dictionary<_,_>()
                script.Params.Add("status", status.ToString())
                script.Params.Add("assignee", assignee)
                request.Script <- script
            )
            |> ignore

    let addComment: ElasticOptions -> AddCommentToDb = 
        fun elasticOptions id comment -> 
            elasticOptions
            |> FsNest.createElasticClient
            |> FsNest.update "dda-task" id (fun request -> 
                let script = InlineScript("ctx._source.comments.add(params.comment)")
                script.Lang <- "painless"
                script.Params <- Dictionary<_,_>()
                script.Params.Add("comment", comment)
                request.Script <- script
            )
            |> ignore

    let deleteAttachmentDescription: ElasticOptions ->  DeleteDescriptionAttachmentFromDb =
        fun elasticOptions attachment taskType -> 
            elasticOptions
            |> FsNest.createElasticClient
            |> FsNest.updateByQuery "dda-task" (fun request -> 
                let query = QueryContainer(TermQuery(Field = Field("taskType"), Value = TaskType.toString taskType))
                let script = InlineScript("ctx._source.description.attachments.remove(ctx._source.description.attachments.indexOf(params.attachment))")
                script.Lang <- "painless"
                script.Params <- Dictionary<_,_>()
                let param = {|Name = attachment.Name; FilePath = attachment.FilePath; UploadDate = attachment.UploadDate.ToString "yyyy-MM-dd"|}
                script.Params.Add("attachment", param)
                request.Script <- script
                request.Query <- query
            )
            |> ignore
    let updateTaskDesctiption: ElasticOptions -> TaskType -> Description -> unit = 
        fun elasticOptions taskType description ->
        elasticOptions
        |> FsNest.createElasticClient
        |> FsNest.updateByQuery "dda-task" (fun request -> 
            let query = QueryContainer(TermQuery(Field = Field("taskType"), Value = TaskType.toString taskType))
            let script = InlineScript("ctx._source.description = params.description")
            script.Lang <- "painless"
            script.Params <- Dictionary<_,_>()
            let attachments = description.Attachments |> List.map (fun (attachment: Domain.Attachment) -> 
                {|Name = attachment.Name; FilePath = attachment.FilePath; UploadDate = attachment.UploadDate.ToString "yyyy-MM-dd"|}
            )
            let param = {|Text = description.Text; Attachments = attachments|}
            script.Params.Add("description", param)
            request.Query <- query
            request.Script <- script
        )
        |> ignore
