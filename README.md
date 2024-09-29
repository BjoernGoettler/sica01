# System Integration Compulsory Assignment 1

## Helicopter view description:

The assignment aims to demonstrate 2 domain specific services, communicating with each other over a RabbitMQ messaging system

## Helpers for reviewing
There are a couple of configuration files for [Bruno](bruno_collection.json) and [Postman](postman_collection.json)

## Service description:
- A [UserService](UserService) implemented as an API with basic CRUD controls for users
- A [TweetService](TweetService) implemented as an API with basic CRUD controls for "Tweets"

## Additional functionality description
- A [MessageClient](MessageCLient), heavily inspired from the E-Commerce example. To provide an easy to setup client for messaging between the 2 main services
- A [MonitorService](Monitoring). Not so much a service, as it is a class library to give us some functionality to communicate with the logging service

## Functionality
The 2 services are not aware of each other, and work very well within their isolated respective code bases. The only obstruction for the Tweeting service to persist a tweet, is that someone wanted to ensure that the user is actually a registered user. Therefore it submits a fire-and-hope request to a RabbitMQ, and waits until it has a confirming response before flushing data to it's database
For every event in both services, a debug messages is logged (Using Serilog with Console and Seq sinks), and can be queried in the Seq instance launched in Docker

## Scaling considerations
The solution is split on the y-axis to begin with. Each of the two domains, Users and Tweets, live in their own logical universe and are happily unaware of their neighbours. In real life it would be very easy to split them on the x-axis as well, the only reason for not showing it here, is that I got some clashes on the ports I use for testing with Postman. I guess it could be mitigated by adding a web-proxy to the project, such as Nginx or similar

## Missed opportunities
At the time of writing, a few things are not implemented

- The user validation is not happening at all. Seq shows an attempt, RabbitMQ shows a connection, but I guess it is a misunderstanding when implementing the MessageClient
- The Tweet model is too simple. If inspected carefully, it is clear that one user can only have 1 tweet. There would have to be a new Tweet-ID added, in order to let users tweet more than 1 tweet (personally I actually enjoy thr thought of giving a person 1 tweet in life, and that's it!!!)
- The users are only validated, and nothing more. It would be cool to update the name they used for tweeting, when they updated their name in the UserService
- There is no error handling. Be carefull when testing, because both services will crash hard when doing something I didn't image someone would do to a service