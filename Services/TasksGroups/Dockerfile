FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["Common/Shared/Shared.csproj", "Common/Shared/"]

COPY ["Services/TasksGroups/EmployeesTasksTracker.TasksGroupsService.Application/EmployeesTasksTracker.TasksGroupsService.Application.csproj", "Services/TasksGroups/EmployeesTasksTracker.TasksGroupsService.Application/"]
COPY ["Services/TasksGroups/EmployeesTasksTracker.TasksGroupsService.Infrastructure/EmployeesTasksTracker.TasksGroupsService.Infrastructure.csproj", "Services/TasksGroups/EmployeesTasksTracker.TasksGroupsService.Infrastructure/"]
COPY ["Services/TasksGroups/EmployeesTasksTracker.TasksGroupsService.Core/EmployeesTasksTracker.TasksGroupsService.Core.csproj", "Services/TasksGroups/EmployeesTasksTracker.TasksGroupsService.Core/"]
COPY ["Services/TasksGroups/EmployeesTasksTracker.TasksGroupsService.Api/EmployeesTasksTracker.TasksGroupsService.Api.csproj", "Services/TasksGroups/EmployeesTasksTracker.TasksGroupsService.Api/"]

RUN dotnet restore "Services/TasksGroups/EmployeesTasksTracker.TasksGroupsService.Api/EmployeesTasksTracker.TasksGroupsService.Api.csproj"

COPY . ./

RUN dotnet publish "Services/TasksGroups/EmployeesTasksTracker.TasksGroupsService.Api/EmployeesTasksTracker.TasksGroupsService.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EmployeesTasksTracker.TasksGroupsService.Api.dll"]