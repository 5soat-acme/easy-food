FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/", "./src/"]
RUN dotnet restore "src/Presentation/EF.Api/EF.Api.csproj"

#RUN dotnet tool install --global dotnet-ef
#ENV PATH="${PATH}:/root/.dotnet/tools"
#COPY ["deploy/database/migrations.sh", "./migrations.sh"]
#RUN chmod +x ./migrations.sh
#RUN ./migrations.sh

WORKDIR "/src/src/Presentation/EF.Api"
RUN dotnet build "EF.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EF.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EF.Api.dll"]
