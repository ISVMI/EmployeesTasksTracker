FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["Common/Shared/Shared.csproj", "Common/Shared/"]

COPY ["Services/Projects/EmployeesTasksTracker.ProjectsService.Application/EmployeesTasksTracker.ProjectsService.Application.csproj", "Services/Projects/EmployeesTasksTracker.ProjectsService.Application/"]
COPY ["Services/Projects/EmployeesTasksTracker.ProjectsService.Infrastructure/EmployeesTasksTracker.ProjectsService.Infrastructure.csproj", "Services/Projects/EmployeesTasksTracker.ProjectsService.Infrastructure/"]
COPY ["Services/Projects/EmployeesTasksTracker.ProjectsService.Core/EmployeesTasksTracker.ProjectsService.Core.csproj", "Services/Projects/EmployeesTasksTracker.ProjectsService.Core/"]
COPY ["Services/Projects/EmployeesTasksTracker.ProjectsService.Api/EmployeesTasksTracker.ProjectsService.Api.csproj", "Services/Projects/EmployeesTasksTracker.ProjectsService.Api/"]

RUN dotnet restore "Services/Projects/EmployeesTasksTracker.ProjectsService.Api/EmployeesTasksTracker.ProjectsService.Api.csproj"

COPY . ./

RUN dotnet publish "Services/Projects/EmployeesTasksTracker.ProjectsService.Api/EmployeesTasksTracker.ProjectsService.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EmployeesTasksTracker.ProjectsService.Api.dll"]