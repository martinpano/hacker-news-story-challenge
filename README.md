# Application overview 📄
This repository holds a source code for an application that fetches data from a given apie called Hacker News, caches it and displays it in
a list on the UI. 

## Azure website 
You can access the hacker news site on the following link hosted on Azure
https://hacker-news-api.azurewebsites.net/stories

## Technology stack ⚙️
For the application needs the following technologies are used:
* .NET 6 API - The backend is an API using the .NET 6 as framework
* Angular 15 - The UI is implemented using Angular as a frontend framework

## Application Structure 🛠️
The application is developed as a monolith architecture. For its creation the existing Angular + .NET template is used. The files are separated in multiple projects that presents the layers of the application.
* HackerNews.Stories.Web
    * This is the application layer. Here we have the API controllers and the ClientApp.
    * For the UI, Angular Material components is used as a component framework, in order to display the data in a table, as well as filter it if needed. 
* HackerNews.Stories.Services
    * This is the service layer, where the interface abstractions for the services as well as their implementations are located. Because of the trivial nature of the code challenge there are only two services each holding one method.
    * Also here we have the Utils implemented, holding only a constant values for the memory cache key and the urls needed
* HackerNews.Stories.Models
    * This is the layer that holds the models shared across service and application layer. This layer simply holds only one model that maps the data fetched from the api.
* HackerNews.Stories.Tests
    * There is also a test project using MSTest framework for covering the service methods with unit tests. A Moq library is used in order to mock the service methods.

**In order to lower the dependencies and to make the code as much decoupled as possible, dependency injection as a concept is used across the services and the controllers.**

