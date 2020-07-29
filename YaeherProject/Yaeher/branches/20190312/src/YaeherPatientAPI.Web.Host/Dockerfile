FROM microsoft/aspnetcore:2.0.1

FROM microsoft/dotnet:2.1-aspnetcore-runtime
WORKDIR /app
COPY . .
EXPOSE 5001
ENTRYPOINT ["dotnet", "YaeherPatientAPI.Web.Host.dll"]
