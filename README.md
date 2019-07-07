# digital-diploma-assistant

Control, accounting and automation system  for diploma business processes designing written in F#, ASP.NET Core MVC and Elasticsearch

### About
This is proof of concept of application, written entirely in F#. The goal is to create MVC application using only F#.

### Summary
It is quite simple application (yes, somewhere there are some 'todo')

Here you can

* Overview dashboard tasks
* Change tasks statuses and assignee wil change automatically according their roles
* Add comments with attachments to tasks
* Change tasks descriptions with attachments from special admin page

### Tech
* .NET Core 2.1
* ASP.NET Core MVC
* Elasticsearch 6.x
* Docker
* [NEST](https://github.com/elastic/elasticsearch-net)

### Project overview
* elk folder contains all necessary files to setup: Elasticsearch docker file with ukrainian analyzer, .yaml file and file with indices schemas and test data 
* markup folder contains murkup views:)
* uml folder contains some domain analysis and UI concepts [UMLET](https://www.umlet.com/) files 

#### Solution 
* Api - ASP.NET Core MVC application, it also contains partial application module, which acts as composition root
* Authentication - simple module for authentication
* Common - project with some infrastructural helpers code
* DataAccess - data access layer implementation. Follows [CQS](https://martinfowler.com/bliki/CommandQuerySeparation.html) and contains F# wrapper for [NEST](https://github.com/elastic/elasticsearch-net) [which is moved to separate repository](https://github.com/malabarMCB/nest-fs-extensions)
* Domain - project with business logic. Contains workflows, domain types, functions with domain logic and interfaces for them. Also it contains interfaces that should be implemented in data access layer
* Notification module - module for sending notifications according to some actions. Now contains no logic and has big 'todo' status
* Playground - place for making experiments and isolated mess :)

### Useful links
* [NEST F# wrapper](https://github.com/malabarMCB/nest-fs-extensions)
* ["Domain moddeling made functional" book](https://pragprog.com/book/swdddf/domain-modeling-made-functional)
* ["Domain moddeling made functional" source code](https://github.com/swlaschin/DomainModelingMadeFunctional)
* [Another example of application written entirely in F#](https://github.com/atsapura/CardManagement)
