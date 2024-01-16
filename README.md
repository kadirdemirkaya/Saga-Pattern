# Saga Pattern

This project was done by using two different types (EventChoreography, Orchestration) in accordance with the saga pattern rules.

## EventChoreography

###### Event choreography is a model that describes the interaction of individual services in a system. Services use events to communicate with each other and coordinate operations.

- Projects inside
    - BasketAPI
    - OrderAPI
    - PaymentAPI
    - StockAPI
    - SharedLIBRARY

## Orchestration

###### Orchestration is a model that centrally manages and coordinates operations in a system. A master service or orchestrator directs other services and performs operations in a specified order and according to rules

- Projects inside
    - BasketAPI
    - OrderAPI
    - PaymentAPI
    - SharedLIBRARY
    - StateMachineWorkerService
    - StockAPI

## Run on Your Computer

Clone the project

```bash
  git clone https://github.com/kadirdemirkaya/SagaPattern.git
```

Make your own changes to the .json file

```bash
  appsettings.json
  appsettings.Development.json
```

Go to the project directory of the API projects and create a migration

```bash
  cd BasketAPI
  cd OrderAPI
  ...
```

Then enter the directory of all api projects and stand up the server

```bash
  dotnet run
```

## Features

- Basket is created in BasketAPI
- OrderAPI receives cart data and, depending on the situation, cancels or sends data for stock control
- If the stock is sufficient, data will be sent again for payment or canceled
- If the payment is made successfully in PaymentPI, the data is confirmed in the database.
- The common library contains the data that all API services will use.
- MSSQL was used for all these operations
- The same events are handled in two different ways (EventChoreography and Orchestration)
