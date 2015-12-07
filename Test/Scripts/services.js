'use strict';

angular.module('app.services', [])

    .factory("Item", function ($http, apiConfig) {
        var itemFactory = {};
        var baseUrl = apiConfig.baseUrl + "/item";

        itemFactory.all = function () {
            return $http.get(baseUrl);
        };

        itemFactory.one = function (id) {
            return $http.get(baseUrl + "/" + id);
        };

        itemFactory.update = function (id, item) {
            $http.defaults.headers.put["Content-Type"] = "application/xml";
            return $http.put(baseUrl + "/" + id, item);
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

    .factory("Order", function ($http, apiConfig) {
        var orderFactory = {};
        var baseUrl = apiConfig.baseUrl + "/order";

        orderFactory.all = function () {
            return $http.get(baseUrl + "/getall");
        };

        orderFactory.deleteItemFromOrder = function (itemId, orderUniqueId) {
            return $http.delete(baseUrl + "/deleteOrderItem/" + itemId + "/" + orderUniqueId);
        };

        orderFactory.addItemToOrder = function (itemId, orderUniqueId, qty) {
            return $http.post(baseUrl + "/addOrderItem/" + itemId + "/" + orderUniqueId + "/" + qty);
        };

        orderFactory.one = function (id) {
            return $http.get(baseUrl + "/" + id);
        };

        orderFactory.update = function (id, order) {
            $http.defaults.headers.put["Content-Type"] = "application/xml";
            return $http.put(baseUrl + "/" + id, order)
        }

        orderFactory.create = function (order) {
            $http.defaults.headers.post["Content-Type"] = "application/xml";

            return $http.post(baseUrl, order)
        };

        orderFactory.getLines = function (id) {
            return $http.get(baseUrl + "/getlineitems/" + id);
        };

        orderFactory.cancel = function (id) {
            return $http.delete(baseUrl + "/" + id);
        };

        orderFactory.capturePayment = function (id) {
            return $http.get(baseUrl + "/acceptpayment/" + id);
        }

        return orderFactory;
    })