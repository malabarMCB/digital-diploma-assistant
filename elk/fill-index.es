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
        "scienceMaster": {
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
        }
    }
}

POST _bulk
{ "index" : { "_index" : "dda-task", "_type" : "_doc" } }
{"type": "Щоденник практики", "student": {"id":"aa", "firstName":"Антон", "lastName": "Солярик"}, "assignee": {"id":"aa", "firstName":"Антон", "lastName": "Солярик"}, "scienceMaster": {"id":"bbb", "firstName":"Альона", "lastName": "Яковенко"}, "group": "БС-51", "status": "InProgress", "deadline": "2019-03-19"}
{ "index" : { "_index" : "dda-task", "_type" : "_doc" } }
{"type": "Звіт з практики", "student": {"id":"aa", "firstName":"Антон", "lastName": "Солярик"}, "assignee": {"id":"aa", "firstName":"Антон", "lastName": "Солярик"}, "scienceMaster": {"id":"bbb", "firstName":"Альона", "lastName": "Яковенко"}, "group": "БС-51", "status": "ToDo", "deadline": "2019-03-19"}
{ "index" : { "_index" : "dda-task", "_type" : "_doc" } }
{"type": "Відгук керівника", "student": {"id":"aa", "firstName":"Антон", "lastName": "Солярик"}, "assignee": {"id":"aa", "firstName":"Антон", "lastName": "Солярик"}, "scienceMaster": {"id":"bbb", "firstName":"Альона", "lastName": "Яковенко"}, "group": "БС-51", "status": "ToDo", "deadline": "2019-03-19"}


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