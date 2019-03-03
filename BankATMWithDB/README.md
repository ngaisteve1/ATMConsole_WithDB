# ATMConsole_WithEntityFramework
Implemented Entity Framework to store program data instead of using list object

Note: The previous version with Entity Framework is available at https://github.com/ngaisteve1/ATMConsole_With_OOP. 

### Software Development Summary
- Technology: C#
- ORM Framework: Entity Framework
- Framework: .NET Framework 4.6.1
- Project Type: Console
- IDE: Visual Studio Community 2017
- Paradigm or pattern of programming: Object-Oriented Programming (OOP)
- Data: Data of this demo program (Bank Account and Transaction data) are stored using Entity Framework ORM in SQL Server database.
- NuGet: ConsoleTables (Version 2.2), CsConsoleFormat (Coming Soon)

### <img class="emoji" alt="atm" height="20" width="20" src="https://github.githubassets.com/images/icons/emoji/unicode/1f3e7.png"> <img class="emoji" alt="credit_card" height="20" width="20" src="https://github.githubassets.com/images/icons/emoji/unicode/1f4b3.png"> ATM Basic Features / Use Cases (Bank Customer):
- [x] Check account balance
- [x] Place deposit
- [x] Make withdraw
- [x] Check card number and pin against database
- [x] Make third-party-transfer (Transfer within the same bank but different account number)
- [x] View bank transactions

### <img class="emoji" alt="atm" height="20" width="20" src="https://github.githubassets.com/images/icons/emoji/unicode/1f3e7.png"> <img class="emoji" alt="credit_card" height="20" width="20" src="https://github.githubassets.com/images/icons/emoji/unicode/1f4b3.png"> ATM Basic Features / Use Cases (Bank Administrator):
- [x] Add bank account
- [x] Manage bank account

#### Business Rules:
- User is not allow to withdraw or transfer more than the balance amount. A minimum RM20 is needed to maintain the bank account.
- If user key in the wrong pin more than 3 times, the bank account will be locked.

#### Assumption:
- All bank account are the from the same bank

#### Enhancement (To Do):
- input validation methods to handle any data type, input length and input label. fluent validation.

### OOP principles and C# features implemented:
- class
- object
- Object and collection initializer
- encapsulation: private, internal and public
- LINQ to object (LINQ Query syntax)
- List
- static
- this
- field
- property
- const
- switch case
- string interpolation
- while loop
- enum
- region
- System.Globalization for local currency symbol and format
- ternary operator ?


### Video Demo
- [ATM Program Demo Video - Part 3](http://www.youtube.com/watch?v=bG93WtkpRto)


[If this content is helpful to you, consider to support and buy me a cup of coffee :) ](https://ko-fi.com/V7V2PN67)
