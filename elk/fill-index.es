DELETE dda

PUT dda
PUT dda/task/_mapping
{
  "properties": {
    "type":{
      "type": "keyword"
    },
    "student":{
      "type": "text",
      "analyzer": "ukrainian",
      "fields" : {
        "keyword" : {
          "type" : "keyword",
          "ignore_above" : 256
        }
      }
    },
    "assignee": {
      "type": "text",
      "analyzer": "ukrainian",
      "fields" : {
        "keyword" : {
          "type" : "keyword",
          "ignore_above" : 256
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
      "type": "keyword"
    }
  }
}

POST _bulk
{ "index" : { "_index" : "dda", "_type" : "task" } }
{"type": "Щоденник практики", "student": "Антон Солярик", "assignee": "Антон Солярик", "group": "БС-51", "status": "InProgress", "deadline": "no"}