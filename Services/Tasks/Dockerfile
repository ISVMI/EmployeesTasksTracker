FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["Common/Shared/Shared.csproj", "Common/Shared/"]

COPY ["Services/Tasks/EmployeesTasksTracker.TasksService.Application/EmployeesTasksTracker.TasksService.Application.csproj", "Services/Tasks/EmployeesTasksTracker.TasksService.Application/"]
COPY ["Services/Tasks/EmployeesTasksTracker.TasksService.Infrastructure/EmployeesTasksTracker.TasksService.Infrastructure.csproj", "Services/Tasks/EmployeesTasksTracker.TasksService.Infrastructure/"]
COPY ["Services/Tasks/EmployeesTasksTracker.TasksService.Core/EmployeesTasksTracker.TasksService.Core.csproj", "Services/Tasks/EmployeesTasksTracker.TasksService.Core/"]
COPY ["Services/Tasks/EmployeesTasksTracker.TasksService.Api/EmployeesTasksTracker.TasksService.Api.csproj", "Services/Tasks/EmployeesTasksTracker.TasksService.Api/"]

RUN dotnet restore "Services/Tasks/EmployeesTasksTracker.TasksService.Api/EmployeesTasksTracker.TasksService.Api.csproj"

COPY . ./

RUN dotnet publish "Services/Tasks/EmployeesTasksTracker.TasksService.Api/EmployeesTasksTracker.TasksService.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EmployeesTasksTracker.TasksService.Api.dll"]