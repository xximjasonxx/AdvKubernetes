FROM mcr.microsoft.com/dotnet/sdk:6.0 as builder
WORKDIR /code

COPY . .
RUN rm -f appsettings*
RUN dotnet restore && \
    dotnet publish -c Release -o /output

FROM mcr.microsoft.com/dotnet/runtime:6.0 as runtime
WORKDIR /app

COPY --from=builder /output .
CMD ["dotnet", "StockPriceGenerator.dll"]