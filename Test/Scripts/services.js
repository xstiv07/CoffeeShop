'use strict';

// Demonstrate how to register services
// In this case it is a simple value service.
angular.module('app.services', [])

    .factory("Item", function ($http) {
        var itemFactory = {};
        var baseUrl = "http://localhost:2873/api/item"

        itemFactory.all = function () {
            return $http.get(baseUrl);
        };

        return itemFactory;
    })