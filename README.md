# Boiler monitoring service

The main purpose of the microservice is to allow other microservices across the system to get the current temperature inside the Boiler (accessible via internal HTTP API) using event-driven approach with RabbitMQ message broker. In addition, the temperature is converted from Fahrenheit to Celsius under the hood.

### Solutions
- `BoilerMonitor` - main microservice; 
- `SharedKernel` - shared message contracts;
- `BoilerApi` - fake internal Boiler HTTP API;
- `BoilerClient` - fake RabbitMQ API consumer.

### Getting started
1. Build the `SharedKernel` assembly to be accessible in `BoilerMonitor` and `BoilerClient` as a reference.
2. Start `BoilerApi`, `BoilerMonitor` and `BoilerClient` apps.
3. `BoilerClient` will send request each 5 seconds.
