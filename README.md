# ATMConsole_WithDB
This version stored data in the database using Entity Framework with extra requirement

#### Other Versions
- OOP (without database) version - https://github.com/ngaisteve1/ATMConsole_With_OOP. This version will have lesser features.
- Procedural version - https://github.com/ngaisteve1/ATMConsole_Without_OOP. This version will have lesser features.


### Software Development Summary
- Technology: C#
- ORM Framework: Entity Framework 6.2 (Code-First Approach)
- Framework: .NET Framework 4.6.1
- Total Projects: 3 (1 Class Library project and 2 Console Projects)
- Solution Structure Design: Repository General CRUD interface layer, Use Case interface layer
- IDE: Visual Studio Community 2017
- Paradigm or pattern of programming: Object-Oriented Programming (OOP)
- Data: Data of this demo program (Bank Account and Transaction data) are stored in SQL Server database using Entity Framework ORM.
- NuGet: ConsoleTables (Version 2.2), FluentValidation (Version 8), CsConsoleFormat (Coming Soon)

### <img class="emoji" alt="atm" height="20" width="20" src="https://github.githubassets.com/images/icons/emoji/unicode/1f3e7.png"> <img class="emoji" alt="credit_card" height="20" width="20" src="https://github.githubassets.com/images/icons/emoji/unicode/1f4b3.png"> ATM Basic Features / Use Cases (Bank Customer):
- [x] Check account balance
- [x] Place deposit
- [x] Make withdraw
- [x] Check card number and pin against bank account list object (Note: No database is used on purpose to demo the use of list object)
- [x] Make third-party-transfer (Transfer within the same bank but different account number)
- [x] View bank transactions
- [x] View account details
- [ ] Change ATM Card Pin

### <img class="emoji" alt="atm" height="20" width="20" src="https://github.githubassets.com/images/icons/emoji/unicode/1f3e7.png"> <img class="emoji" alt="credit_card" height="20" width="20" src="https://github.githubassets.com/images/icons/emoji/unicode/1f4b3.png"> ATM Basic Features / Use Cases (Bank Administrator):
- [x] Add 3 sample bank account (Seeding)
- [x] Add bank account
- [x] Search and Delete bank account

#### Business Rules:
- User is not allow to withdraw or transfer more than the balance amount. A minimum RM20 is needed to maintain the bank account.
- If user key in the wrong pin more than 3 times, the bank account will be locked.
- Cannot duplicate NRIC
- Starting account balance needs minimum RM20.00 (Saving Account) and RM500 (Current Account).
- Card PIN code needs to be 6 digit number.
- Account number is 7 digit (auto-generated for this demo)
- ATM Card Number is 9 digit (auto-generated for this demo)

#### Assumption:
- All bank account are the from the same bank

### Activity Diagram
![atm_activity_diagram](https://user-images.githubusercontent.com/21274590/53474610-8615c080-3aa8-11e9-99b2-b1cb32f7ec1c.png)


#### Enhancement (To Do):
- [ ] Try catch finally block for Entity Framework CRUD operation. Add Dispose method as GC for EF db context object to manage resources (application memory).
- [ ] Logging with Log4Net external library
- [ ] Repository layer in Repository project, so that Bank Customer project and Bank Administrator project can call general repository method for CRUD operation with method parameter. 
- [ ] Investigate the possible cause of program 'clash / rerun' automatically.

### OOP principles and C# features implemented:
- class (POCO class and utility class)
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
- [Sample Bank ATM C# Console App with Entity Framework](https://youtu.be/sQ0esm7QlpQ)

[If this content is helpful to you, consider to support and buy me a cup of coffee :) ](https://ko-fi.com/V7V2PN67)
