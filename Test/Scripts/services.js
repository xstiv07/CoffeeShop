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

        itemFactory.one = function (id) {
            return $http.get(baseUrl + "/" + id);
        };

        itemFactory.update = function (id, item) {
            $http.defaults.headers.put["Content-Type"] = "application/xml";
            return $http.put(baseUrl + "/" + id, item)
        };

        itemFactory.delete = function (id) {
            return $http.delete(baseUrl + "/" + id);
        };

        itemFactory.create = function (item) {
            $http.defaults.headers.post["Content-Type"] = "application/xml";
            return $http.post(baseUrl, item);
        };

        return itemFactory;
    })