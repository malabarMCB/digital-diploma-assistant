namespace DataAccess.Task

open DataAccess
open Domain
open FsNest

module Commands = 
    type  StatusParams = {
        Status: string
        Assignee: Person
    }
    let updateTaskStatus: ElasticOptions -> UpdateTaskStatusInDb = 
        fun elasticOptions id status assignee -> 
            elasticOptions |> FsNest.createElasticClient
            |> FsNest.update "dda-task" id {
                Lang = "painless"
                Inline = "ctx._source.status = params.status; ctx._source.assignee = params.assignee"
                Params = {
                    Status = status.ToString()
                    Assignee = assignee
                }
             } |> ignore
    
    type CommentParams = {
        Comment: Comment
    }
    let addComment: ElasticOptions -> AddCommentToDb = 
        fun elasticOptions id comment -> 
            elasticOptions |> FsNest.createElasticClient
            |> FsNest.update "dda-task" id {
                Lang = "painless"
                Inline = "ctx._source.comments.add(params.comment)"
                Params= {
                    Comment = comment
                }
            } |> ignore

    //metodist
    type ElasticAttachment = {
        Name: string
        FilePath: string
        UploadDate: string
    }

    type AttachmentParams = {
        Attachment: ElasticAttachment
    }

    type QueryValue = {
      Value: string 
    }

    type TypeQuery = {
        Type: QueryValue
    }

    type TaskTypeTermQuery = {
        Term: TypeQuery
    }

    type ElasticDescription = {
        Text: string
        Attachments: ElasticAttachment list
    }

    type DescriptionParams = {
        Description: ElasticDescription
    }

    //let deleteAttachmentDescription (elasticOptions: ElasticOptions) (attachment: Attachment) (taskType: TaskType) =
    let deleteAttachmentDescription: ElasticOptions ->  DeleteDescriptionAttachmentFromDb =
        fun elasticOptions attachment taskType -> 
            let query = {
                Query = {
                    Term = {
                        Type = {
                          Value = TaskType.toString taskType
                        }             
                    }
                }
            }
            let script: FsNest.Script<AttachmentParams> = {
                Lang = "painless"
                Inline = "ctx._source.description.attachments.remove(ctx._source.description.attachments.indexOf(params.attachment))"
                Params= {
                    Attachment = {
                        Name = attachment.Name
                        FilePath = attachment.FilePath
                        UploadDate = attachment.UploadDate.ToString "yyyy-MM-dd"
                    }
                }
            }
            let client = elasticOptions |> FsNest.createElasticClient
            FsNest.updateByQuery client "dda-task" query script |> ignore

    let updateTaskDesctiption (elasticOptions: ElasticOptions) (taskType: TaskType) (description: Description) = 
        let toElasticAttachment (attachment: Attachment) =
            {
                Name = attachment.Name
                FilePath = attachment.FilePath
                UploadDate = attachment.UploadDate.ToString "yyyy-MM-dd"
            }
        let query = {
            Query = {
                Term = {
                    Type = {
                        Value = TaskType.toString taskType
                    }             
                }
            }
        }
        let script: FsNest.Script<DescriptionParams> = {
            Lang = "painless"
            Inline = "ctx._source.description = params.description"
            Params= {
                Description = {
                    Text = description.Text
                    Attachments = List.map toElasticAttachment description.Attachments
                }
            }
        }
        let client = elasticOptions |> FsNest.createElasticClient
        FsNest.updateByQuery client "dda-task" query script |> ignore
