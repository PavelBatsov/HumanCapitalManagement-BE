# HumanCapitalManagement-BE

The main idea of the Application is managing people records. 

It has been developed in .Net 8 and has implemented JWT Authentication and custom accounts management.
There are 7 pages in total - Home, Register, Login, Users, Managers, Employees and Tasks.

Initially, we want to register an account with specific role, which has different access and functionality, or use the predefined one.
The business logic is the following - Admin should create Manager, then the Manager should create an Employee and assign in to any Manager. The Employee needs to see his/her tasks.

There are three roles in total - Admin (predefined), Manager and Employee.
The roles access and functionality: 
  Admin - full privilege in the application.
  Manager - access to Manager (ReadOnly), Employee (ReadWrite) and Task page.
  Employee - access only to Task page.
