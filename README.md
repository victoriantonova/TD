# TD
Для запуска теста необходимо: 
1. Развернуть приложение для тестирование и БД MySQL
2. Указать имя пользователя и пароль для доступа к TestRail в файле configTestRail.xml. 
3. Указать имя пользователя и пароль для доступа к базе данных в файле configDB.xml.
4. Указать имя пользователя и пароль для доступа к тестируему приложению в файле configApp.xml.
5. Открыть cmd и выполнить следующие команды:
	1. "{pathToMSBuild.exe}" /t:Restore "{pathTo_Aquality.Selenium.Template.sln}"
	2. dotnet test {pathTo_Aquality.Selenium.Template.SpecFlow.dll}
Note: при выполнение команды 2 можно указать параметры запуска, например, set "browserName=chrome" && set "isRemote=false"

