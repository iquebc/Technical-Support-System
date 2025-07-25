FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /Technical-Support-System/AuthService

COPY *.csproj .
RUN dotnet restore 

COPY . .
RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /Technical-Support-System/AuthService

COPY --from=build /out .

EXPOSE 80

CMD ["dotnet", "AuthService.Web.API.dll"]