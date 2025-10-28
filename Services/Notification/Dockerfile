FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["Common/Shared/Shared.csproj", "Common/Shared/"]

COPY ["Services/Notification/EmployeesTasksTracker.NotificationService/EmployeesTasksTracker.NotificationService.csproj", "Services/Notification/EmployeesTasksTracker.NotificationService/"]

RUN dotnet restore "Services/Notification/EmployeesTasksTracker.NotificationService/EmployeesTasksTracker.NotificationService.csproj"

COPY . ./

RUN dotnet publish "Services/Notification/EmployeesTasksTracker.NotificationService/EmployeesTasksTracker.NotificationService.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EmployeesTasksTracker.NotificationService.dll"]