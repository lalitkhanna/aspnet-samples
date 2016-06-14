This sample demonstrates how a web application powered by DevExpress ASP.NET Office Controls (Spreadsheet and Rich Text Edit) can be designed for running in Microsoft Azure's scalable environment.


## What Problem Does This Sample Solve?

Due to the server nature of DevExpress Office Controls, each time a RichEdit or Spreadsheet opens a document, this document is loaded into a specific in-memory document cache, which is preserved on a web server. When an end-user is working with the document in the browser on the client side, RichEdit/Spreadsheet sends asynchronous requests to update the cached document on the server.  

In case of running a web application in the multi-server Azure infrastructure, a document request is processed by Azure Load Balancer and can be arbitrarily routed to another web server instance, which is different to the one that contains the cached document. As a result, the document cache in the other server memory will not contain such a requested document, the document state will be lost, and an error may occur.  

This sample project shows how this problem can be solved by implementing an interlayer routing web server role. This routing app works between the Load Balancer and the front-end web application and uses the code provided by a DevExpress Azure-specific solution.
 




## Sample Project Structure

This sample is created based on a standard Visual Studio Azure Cloud Service project template. The sample project contains the following. 

####Two web roles:

 * **WebRole.DocumentSite**  
    A ready-to-scale template of a customer simple front-end ASP.NET web application containing webpages with DevExpress ASP.NET Office controls (Spreadsheet and RichEdit).

 * **WebRole.Routing**  
    A specific (scalable) interlayer routing application that works between the Microsoft Azure Load Balancer and the front-end web application (WebRole.DocumentSite) and uses the Azure Service Bus relay service to communicate with the front-end web role instances. This routing app maintains a table that tracks office documents (Spreadsheet or RichEdit documents) being opened by end-users. Each table record holds two identifiers - an open document's unique DocumentID and the identifier of the corresponding front-end web role instance on which the document was opened for the first time. Based on this table, the routing app directs load balanced client requests for a document to the certain front-end web role instance: requests with the same document ID are routed to the same front-end web role instance. This provides a kind of the 'document affinity' (by analogy with a commonly known 'session affinity' term) functionality. The routing app relies on the code provided by the DevExpress Azure-specific solution (see below).  

####DevExpress Azure-specific solution  
Three class libraries that implement the mechanism to maintain document affinity for applications that require running in multi-server configurations.  

 * **DevExpress.Web.OfficeAzureCommunication**
 * **DevExpress.Web.OfficeAzureDocumentServer**
 * **DevExpress.Web.OfficeAzureRoutingServer**

####A data access layer project  
 * **DB.Documents.DAL**  
    Provides logic to connect to a sample SQL database. The database contains sample office documents (RTF and XLS files).



## How to Run This Sample

Getting this sample run in your Azure environment is quite simple. Configure the sample by substituting placeholders in project files with your Azure credentials, build the sample and publish it to Azure. Additionally, you will need to deploy the provided sample database to Azure.


### Prerequisites

To run this sample you will need:  

 * Visual Studio 2013 (or higher)
 * Azure SDK for Visual Studio (version 2.7 or higher)
 * SQL Server 2012 (or higher) with SQL Server Management Studio 2012 (or higher)
 * DevExpress ASP.NET Subscription (version 16.1 or higher)

 * An Azure subscription (a free trial is sufficient: https://azure.microsoft.com/en-us/free/)
 * Azure Service Bus namespace (see how to create one: https://azure.microsoft.com/en-us/documentation/articles/service-bus-dotnet-how-to-use-topics-subscriptions/#create-a-namespace)  
You need the following information from the Service Bus namespace configuration:
   * Service Bus namespace name
   * Shared access policy name
   * Shared access policy primary key

 * Azure SQL Database logical server (see how to create one: https://azure.microsoft.com/en-us/documentation/articles/sql-database-get-started/#create-an-azure-sql-database-logical-server)  
You need the following SQL Server credentials:
   * Server name
   * Server administrator login
   * Password




###Configure the sample to use your Azure Credentials

Download this sample project, open it in Visual Studio and try to build the solution. The required NuGet packages will be installed automatically. Check whether all references are correctly resolved. 

Modify the following files by providing your own credentials:

**Azure.WebRole.DocumentSite\Web.config**  
In the appSettings section, specify values for the ServiceBusNamespace, ServiceBusSharedAccessKeyName and ServiceBusSharedAccessKey properties. 

 * Service Bus namespace name:
```xml
  <add key="ServiceBusNamespace" value="{Put the Service Bus Namespace Name here}" />
```

 * Service Bus shared access policy name:
```xml
  <add key="ServiceBusSharedAccessKeyName" value="{Put the Service Bus Shared Access Policy Name here}" />
```

 * Service Bus shared access policy key:
```xml
  <add key="ServiceBusSharedAccessKey" value="{Put the Service Bus Shared Access Policy Key here}" />
```


**Azure.WebRole.Routing\Web.config**  
In the appSettings section, specify values for the ServiceBusNamespace, ServiceBusSharedAccessKeyName and ServiceBusSharedAccessKey properties.

 * Service Bus namespace name:
```xml
  <add key="ServiceBusNamespace" value="{Put the Service Bus Namespace Name here}" />
```

 * Service Bus shared access policy name:
```xml
  <add key="ServiceBusSharedAccessKeyName" value="{Put the Service Bus Shared Access Policy Name here}" />
```

 * Service Bus shared access policy key:
```xml
  <add key="ServiceBusSharedAccessKey" value="{Put the Service Bus Shared Access Policy Key here}" />
```


**Azure.CloudService.Documents\ServiceConfiguration.Cloud.cscfg**  
In the configuration section of the Azure.WebRole.DocumentSite role, specify the value of the DocumentsConntectionString property:
```xml
  <Setting name="DocumentsConnectionString" value="{Put your Azure SQL Database connection string here}" />
```
Set this property value to the connection string created for a sample database after it was deployed to the Azure SQL Database (see the following Deploy a Sample Database to Azure section for a typical deployment scenario).



###Deploy a Sample Database to Azure

This sample project provides you with a sample database file of the .mdf type. You can find it in the project's \Database folder.

You need to deploy this database to Azure. A possible scenario can be as follows:

1. Attach the database to your local SQL Server. Use SQL Server Management Studio to do that. (learn more here: https://msdn.microsoft.com/en-us/library/ms190209(v=sql.120).aspx)

2. Deploy the database to Azure using the Deploy the Database to Microsoft Azure Database wizard in SQL Server Management Studio (learn more here: https://azure.microsoft.com/en-us/documentation/articles/sql-database-cloud-migrate-compatible-using-ssms-migration-wizard/). At this step, you will need to enter your Azure SQL Server's credentials.

After deployment, you can determine the database connection string in your Azure portal. Put this string to the sample project's ServiceConfiguration.Cloud.cscfg configuration file and provide your user name and password.



###Publish the Solution to Azure

Rebuild the solution in Visual Studio and publish this entire cloud service application to Azure under your Azure account.
