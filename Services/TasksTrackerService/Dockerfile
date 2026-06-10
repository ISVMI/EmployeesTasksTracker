FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

COPY ["Common/Shared/Shared.csproj", "Common/Shared/"]

COPY ["Services/TasksTrackerService/EmployeesTasksTracker.TasksTrackerService.Application/EmployeesTasksTracker.TasksTrackerService.Application.csproj", "Services/TasksTrackerService/EmployeesTasksTracker.TasksTrackerService.Application/"]
COPY ["Services/TasksTrackerService/EmployeesTasksTracker.TasksTrackerService.Infrastructure/EmployeesTasksTracker.TasksTrackerService.Infrastructure.csproj", "Services/TasksTrackerService/EmployeesTasksTracker.TasksTrackerService.Infrastructure/"]
COPY ["Services/TasksTrackerService/EmployeesTasksTracker.TasksTrackerService.Core/EmployeesTasksTracker.TasksTrackerService.Core.csproj", "Services/TasksTrackerService/EmployeesTasksTracker.TasksTrackerService.Core/"]
COPY ["Services/TasksTrackerService/EmployeesTasksTracker.TasksTrackerService.Api/EmployeesTasksTracker.TasksTrackerService.Api.csproj", "Services/TasksTrackerService/EmployeesTasksTracker.TasksTrackerService.Api/"]

RUN dotnet restore "Services/TasksTrackerService/EmployeesTasksTracker.TasksTrackerService.Api/EmployeesTasksTracker.TasksTrackerService.Api.csproj"

COPY . ./

RUN dotnet publish "Services/TasksTrackerService/EmployeesTasksTracker.TasksTrackerService.Api/EmployeesTasksTracker.TasksTrackerService.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/sdk:9.
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EmployeesTasksTracker.TasksTrackerService.Api.dll"]