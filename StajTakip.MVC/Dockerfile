FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
EXPOSE 80
COPY Core/*.csproj Core/
COPY StajTakip.Business/*.csproj StajTakip.Business/
COPY StajTakip.DataAccess/*.csproj StajTakip.DataAccess/
COPY StajTakip.Entities/*.csproj StajTakip.Entities/
COPY StajTakip.MVC/*.csproj StajTakip.MVC/
RUN dotnet restore StajTakip.MVC/*.csproj
COPY . .
RUN dotnet publish StajTakip.MVC/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "StajTakip.MVC.dll"]
