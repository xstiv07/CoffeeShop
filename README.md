# CoffeeShop api and point of sale application

In order to persist data you need to create a database on localhost. The web.config in Restbucks API is looking for a database named CoffeeDb. The sql table creation script is in the root of DataFramework project. 

POS app is an angular application running on IIS, all the routing is done by angular. Images are persisted to AWS bucket. All the configs are in /Scripts/config.js  It runs on port 8080 (http://localhost:8080/). Calls are made directly to the API. Navigation is pretty straightforward, except in order to check out an ORDER details, you need to click on the green circle with an order number.

Customer status application runs on port 13184 (http://localhost:13184/). It is an MVC application that uses an API wrapper. There is a js script that sets a timer to query orders from the API every 40 sec, after that sets another timer to post ids of completed orders every 2 minutes, so completed orders are deleted automatically after that interval.

The API runs on port 2873 (http://localhost:2873/).

config.js file is not commited for security purposes.
github for the project is https://github.com/xstiv07 - CoffeeShop repo
