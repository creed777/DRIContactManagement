# Purpose 
The purpose of the DRI Management app is to manage the list of DRI (directly responsible individuals) for services deployed in Azure.

# Projects
The main project is the contact manager that allows view/update of the contact records in SQL Server. There is a second project for an update service.  This is a Azure timer function that runs daily.  It compares the service information from Kusto with the records in SQL Server and add/soft-deletes records based on changes.

# Technology
ASP.Net MVC application.  Teklerik will provide UI controls that are highly customizable and reduce development time.
C# is the preferred web development programming language and is easily supported.
Azure Active Directory will serve as authentication for the system. 
