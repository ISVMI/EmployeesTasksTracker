FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["Common/Shared/Shared.csproj", "Common/Shared/"]

COPY ["Services/History/EmployeesTasksTracker.HistoryService.Application/EmployeesTasksTracker.HistoryService.Application.csproj", "Services/History/EmployeesTasksTracker.HistoryService.Application/"]
COPY ["Services/History/EmployeesTasksTracker.HistoryService.Infrastructure/EmployeesTasksTracker.HistoryService.Infrastructure.csproj", "Services/History/EmployeesTasksTracker.HistoryService.Infrastructure/"]
COPY ["Services/History/EmployeesTasksTracker.HistoryService.Core/EmployeesTasksTracker.HistoryService.Core.csproj", "Services/History/EmployeesTasksTracker.HistoryService.Core/"]
COPY ["Services/History/EmployeesTasksTracker.HistoryService.Api/EmployeesTasksTracker.HistoryService.Api.csproj", "Services/History/EmployeesTasksTracker.HistoryService.Api/"]

RUN dotnet restore "Services/History/EmployeesTasksTracker.HistoryService.Api/EmployeesTasksTracker.HistoryService.Api.csproj"

COPY . ./

RUN dotnet publish "Services/History/EmployeesTasksTracker.HistoryService.Api/EmployeesTasksTracker.HistoryService.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EmployeesTasksTracker.HistoryService.Api.dll"]