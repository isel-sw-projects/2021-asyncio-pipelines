## How to Execute

### Prerequisites

- Create an environment variable:
  - Name: `TESE_BOOKS_FOLDER_PATH`
  - Value: `c:\path_to_repo\2021-asyncio-pipelines\books`

### Cloning the Repository

1. Open your terminal.
2. Navigate to the desired directory:

   `cd c:\path_to_repo\`

 4. Clone the repository:

   `git clone https://github.com/isel-sw-projects/2021-asyncio-pipelines.git`


### Java

1. Navigate to the Java project directory:

`cd 2021-asyncio-pipelines\dev\lookwords-master`

2. Build the project using Maven:

  `mvn clean install -f pom.xml -Dmaven.test.skip=true`

3. Run the application:

 `cd target`
 
 `java -jar lookwords-1.0-SNAPSHOT.jar AppTest`


### C#

1. Navigate to the C# project directory:

 `cd c:\path_to_repo\2021-asyncio-pipelines\dev\lookword c#\lookwords`

2. Build the project using MSBuild:

  `msbuild lookwords.sln /p:Configuration=Release /p:Platform="Any CPU"`

3. Run the application:

   `cd lookwords\bin\Release`
   `lookwords.exe`


### JavaScript

1. Navigate to the JavaScript project directory:

  `cd ...\2021-asyncio-pipelines\dev\lookword js`

3. Run the application:

  `node program.js`
   


