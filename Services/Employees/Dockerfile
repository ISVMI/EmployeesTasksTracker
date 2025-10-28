FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["Common/Shared/Shared.csproj", "Common/Shared/"]

COPY ["Services/Employees/EmployeesTasksTracker.EmployeesService.Application/EmployeesTasksTracker.EmployeesService.Application.csproj", "Services/Employees/EmployeesTasksTracker.EmployeesService.Application/"]
COPY ["Services/Employees/EmployeesTasksTracker.EmployeesService.Infrastructure/EmployeesTasksTracker.EmployeesService.Infrastructure.csproj", "Services/Employees/EmployeesTasksTracker.EmployeesService.Infrastructure/"]
COPY ["Services/Employees/EmployeesTasksTracker.EmployeesService.Core/EmployeesTasksTracker.EmployeesService.Core.csproj", "Services/Employees/EmployeesTasksTracker.EmployeesService.Core/"]
COPY ["Services/Employees/EmployeesTasksTracker.EmployeesService.Api/EmployeesTasksTracker.EmployeesService.Api.csproj", "Services/Employees/EmployeesTasksTracker.EmployeesService.Api/"]

RUN dotnet restore "Services/Employees/EmployeesTasksTracker.EmployeesService.Api/EmployeesTasksTracker.EmployeesService.Api.csproj"

COPY . .

RUN dotnet publish "Services/Employees/EmployeesTasksTracker.EmployeesService.Api/EmployeesTasksTracker.EmployeesService.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EmployeesTasksTracker.EmployeesService.Api.dll"]