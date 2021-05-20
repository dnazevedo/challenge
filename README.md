# Challenge WebAPI using RabbitMQ


API that receives a request and publishes its body in a defined exchange and route in request HTTP headers, containing an SDK abstracting the publication in the created endpoint. Creates a consumer that writes the published messages to a text file using the native RabbitMQ.Client, without abstractions.
