HOW TO EXECUTE


create environment variable name: TESE_BOOKS_FOLDER_PATH with value "c:\\path_to_repo\2021-asyncio-pipelines\books"

cd c:\path_to_repo\

reopen terminal 

git clone https://github.com/isel-sw-projects/2021-asyncio-pipelines.git

JAVA

cd c:\path_to_repo\2021-asyncio-pipelines\dev\lookwords-master

mvn clean install -f pom.xml -Dmaven.test.skip=true

cd target 

java -jar lookwords-1.0-SNAPSHOT.jar AppTest


C#

cd c:\path_to_repo\ 2021-asyncio-pipelines\dev\lookword c#\lookwords

msbuild lookwords.sln /p:Configuration=Release /p:Platform="Any CPU"

cd lookwords\bin\Release\

lookwords.exe


JS

cd c:\path_to_repo\ 2021-asyncio-pipelines\dev\lookword c#\

node program


