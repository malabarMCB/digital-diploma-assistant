DELETE dda-task
PUT dda-task
PUT dda-task/_doc/_mapping
{
    "properties": {
        "type": {
            "type": "keyword"
        },
        "student": {
            "type": "nested",
            "properties": {
                "id": {
                    "type": "keyword"
                },
                "firstName": {
                    "type": "text",
                    "analyzer": "ukrainian",
                    "fields": {
                        "keyword": {
                            "type": "keyword",
                            "ignore_above": 256
                        }
                    }
                },
                "lastName": {
                    "type": "text",
                    "analyzer": "ukrainian",
                    "fields": {
                        "keyword": {
                            "type": "keyword",
                            "ignore_above": 256
                        }
                    }
                }
            }
        },
        "assignee": {
            "type": "nested",
            "properties": {
                "id": {
                    "type": "keyword"
                },
                "firstName": {
                    "type": "text",
                    "analyzer": "ukrainian",
                    "fields": {
                        "keyword": {
                            "type": "keyword",
                            "ignore_above": 256
                        }
                    }
                },
                "lastName": {
                    "type": "text",
                    "analyzer": "ukrainian",
                    "fields": {
                        "keyword": {
                            "type": "keyword",
                            "ignore_above": 256
                        }
                    }
                }
            }
        },
        "supervisor": {
            "type": "nested",
            "properties": {
                "id": {
                    "type": "keyword"
                },
                "firstName": {
                    "type": "text",
                    "analyzer": "ukrainian",
                    "fields": {
                        "keyword": {
                            "type": "keyword",
                            "ignore_above": 256
                        }
                    }
                },
                "lastName": {
                    "type": "text",
                    "analyzer": "ukrainian",
                    "fields": {
                        "keyword": {
                            "type": "keyword",
                            "ignore_above": 256
                        }
                    }
                }
            }
        },
        "group": {
            "type": "keyword"
        },
        "status": {
            "type": "keyword"
        },
        "deadline": {
            "type": "date"
        },
        "comments": {
            "type": "nested",
            "properties": {
                "author": {
                    "type": "nested",
                    "properties": {
                        "id": {
                            "type": "keyword"
                        },
                        "firstName": {
                            "type": "text",
                            "analyzer": "ukrainian",
                            "fields": {
                                "keyword": {
                                    "type": "keyword",
                                    "ignore_above": 256
                                }
                            }
                        },
                        "lastName": {
                            "type": "text",
                            "analyzer": "ukrainian",
                            "fields": {
                                "keyword": {
                                    "type": "keyword",
                                    "ignore_above": 256
                                }
                            }
                        }
                    }
                },
                "text": {
                    "type": "text",
                    "analyzer": "ukrainian",
                    "fields": {
                        "keyword": {
                            "type": "keyword",
                            "ignore_above": 256
                        }
                    }
                },
                "postDate": {
                    "type": "date"
                },
                "attachments": {
                    "type": "nested",
                    "properties": {
                        "name": {
                            "type": "keyword"
                        },
                        "filePath": {
                            "type": "keyword"
                        },
                        "uploadDate": {
                            "type": "date"
                        }
                    }
                }
            }
        },
        "description": {
            "properties": {
                "text": {
                    "type": "text"
                },
                "attachments": {
                    "properties": {
                        "name": {
                            "type": "keyword"
                        },
                        "filePath": {
                            "type": "keyword"
                        },
                        "uploadDate": {
                            "type": "date"
                        }
                    }
                }
            }
        }
    }
}

POST _bulk
{ "index" : { "_index" : "dda-task", "_type" : "_doc" } }
{"type":"Щоденник практики","student":{"id":"aa","firstName":"Антон","lastName":"Солярик"},"assignee":{"id":"aa","firstName":"Антон","lastName":"Солярик"},"supervisor":{"id":"bbb","firstName":"Альона","lastName":"Яковенко"},"group":"БС-51","status":"StudentInProgress","deadline":"2019-03-19","comments":[{"author":{"id":1,"firstName":"Антон","lastName":"Солярик"},"text":"very awesome comment","postDate":"2019-05-12","attachments":[{"name":"my diploma.docx","filePath":"C:\\dda-storage","uploadDate":"2019-05-12"}]},{"author":{"id":1,"firstName":"Антон","lastName":"Солярик"},"text":"another awesome comment","postDate":"2019-05-15","attachments":[{"name":"my diploma(2).docx","filePath":"C:\\dda-storage","uploadDate":"2019-05-15"}]}],"description":{"text":"perfect explanations","attachments":[{"name":"sample1.docx","filePath":"C:\\dda-storage","uploadDate":"2019-01-10"},{"name":"sample2.docx","filePath":"C:\\dda-storage","uploadDate":"2019-05-15"}]}}
{ "index" : { "_index" : "dda-task", "_type" : "_doc" } }
{"type": "Звіт з практики", "student": {"id":"aa", "firstName":"Антон", "lastName": "Солярик"}, "assignee": {"id":"aa", "firstName":"Антон", "lastName": "Солярик"}, "supervisor": {"id":"bbb", "firstName":"Альона", "lastName": "Яковенко"}, "group": "БС-51", "status": "StudentToDo", "deadline": "2019-03-19"}
{ "index" : { "_index" : "dda-task", "_type" : "_doc" } }
{"type": "Відгук керівника", "student": {"id":"aa", "firstName":"Антон", "lastName": "Солярик"}, "assignee": {"id":"aa", "firstName":"Антон", "lastName": "Солярик"}, "supervisor": {"id":"bbb", "firstName":"Альона", "lastName": "Яковенко"}, "group": "БС-51", "status": "StudentToDo", "deadline": "2019-03-19"}


DELETE dda-user
PUT dda-user

PUT dda-user/_doc/_mapping
{
  "properties": {
    "login": {
      "type": "keyword"
    },
    "password": {
      "type": "keyword"
    },
    "firstName": {
      "type": "keyword"
    },
    "lastName": {
      "type": "keyword"
    },
    "role":{
      "type": "keyword"
    }
  }
}

POST _bulk
{"index": {"_index":"dda-user", "_type":"_doc","_id": "1"}}
{"login":"login", "password": "123", "firstName": "Антон", "lastName":"Солярик", "role":"student"}
{"index": {"_index":"dda-user", "_type":"_doc","_id": "2"}}
{"login":"login1", "password": "123", "firstName": "НормконтроллерfirstName", "lastName":"НормконтролерlastName", "role":"normController"}
{"index": {"_index":"dda-user", "_type":"_doc","_id": "3"}}
{"login":"login2", "password": "123", "firstName": "ПлагиатfirstName", "lastName":"ПлагиатlastName", "role":"unicheckValidator"}
{"index": {"_index":"dda-user", "_type":"_doc","_id": "4"}}
{"login":"login3", "password": "123", "firstName": "МетодистfirstName", "lastName":"МетодистlastName", "role":"metodist"}