## How to Execute

### Prerequisites

- Create an environment variable:
  - Name: `TESE_BOOKS_FOLDER_PATH`
  - Value: `c:\path_to_repo\2021-asyncio-pipelines\books`
 
-  Haver recent versions of tools:
     - msbuild -version Microsoft (R) Build Engine version 17.1.0+ae57d105c for .NET Framework (create enviroment variable if needed to point to a visual studio MSBUILD bin folder)

     - Apache Maven 3.8.1 (05c21c65bdfed0f71a2f2ada8b84da59348c4c5d)
      Maven home: C:\Program Files\JetBrains\IntelliJ IDEA Community Edition 2021.3.2\plugins\maven\lib\maven3\bin\..
      Java version: 18.0.1, vendor: Oracle Corporation,

      - Node -v v16.14.0
 
      - Have Nuget available on terminal
   
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

 -  `nuget restore lookwords.sln`
  
 - `msbuild lookwords.sln /p:Configuration=Release /p:Platform="Any CPU"`

3. Run the application:

   `cd lookwords\bin\Release`
   
   `lookwords.exe`


### JavaScript

1. Navigate to the JavaScript project directory:

  `cd ...\2021-asyncio-pipelines\dev\lookword js`

3. Run the application:

  `node program.js`
   


