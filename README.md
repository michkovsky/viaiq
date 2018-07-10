# viaiq

Proposed solution consists of three project files.

## [Coding](./Coding)

This project contains C# part of implementation. It also act as a web-application which can be used to test solution.

It can be started at local machine via command
```bash
dotnet run
```

or with docker
```bash
docker build -t smi/viaiq-coding .
docker run -p 8080:80 -p 8081:443 smi/viaiq-coding
```

and test with swagger
http://localhost:5000/swagger
## [Coding.Tests](./Coding.Tests)

XUnit tests for C# code
```bash
dotnet test
```
## [Coding/ClientApp](./Coding/ClientApp)

Sub-project with javascript UI Do not start it separately.
