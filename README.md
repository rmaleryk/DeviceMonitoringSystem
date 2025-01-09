# Device monitoring service

The solution was created as a pet-project to cover the following topics/technologies:
- Event-Driven Architecture
- Domain-Driven Design
- CQRS
- Event Sourcing
- MassTransit (incl. Saga, Transactional Outbox)
- RabbitMQ
- State machine (Stateless library)

The main goal of the solution is to allow various microservices in the system to receive device data (e.g. boiler temperature) available through an internal HTTP API, using an event-driven approach with the RabbitMQ message broker.

### Solutions
- `DMS.Monitor` - core microservice for managing device states;
- `DMS.Client` - RabbitMQ consumer that requests and receives device data;
- `DMS.FakeDevices` - fake devices HTTP API.

### Getting started
1. Start `DMS.Monitor`, `DMS.Client` and `DMS.FakeDevices` services.
2. `DMS.Client` will send requests every 5 seconds.
3. Also, REST API is available for `DMS.Monitor` and `DMS.Client` services.
