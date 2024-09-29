# System Integration Compulsory Assignment 1

## Helicopter view description:

The assignment aims to demonstrate 2 domain specific services, communicating with each other over a RabbitMQ messaging system

## Service description:
- A [UserService](UserService) implemented as an API with basic CRUD controls for users
- A [TweetService](TweetService) implemented as an API with basic CRUD controls for "Tweets"

## Additional functionality description
- A [MessageClient], heavily inspired from the E-Commerce example. To provide an easy to setup client for messaging between the 2 main services
- A [MonitorService]. Not so much a service, as it is a class library to give us some functionality to communicate with the logging service

## Functionality
The 2 services are not aware of each other, and work very well within their isolated respective code bases. The only obstruction for the Tweeting service to persist a tweet, is that someone wanted to ensure that the user is actually a registered user. Therefore it submits a fire-and-hope request to a RabbitMQ, and waits until it has a confirming response before flushing data to it's database
For every event in both services, a debug messages is logged (Using Serilog with Console and Seq sinks), and can be queried in the Seq instance launched in Docker