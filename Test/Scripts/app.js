'use strict';

var coffeeShop = coffeeShop || {};

coffeeShop.helpers = {
    generateItemXML: function (item) {

        var itemNamespaceBegin = "<Item \r\n    xmlns:xsi=\"http:\/\/www.w3.org\/2001\/XMLSchema-instance\" \r\n    xmlns:xsd=\"http:\/\/www.w3.org\/2001\/XMLSchema\">\r\n";
        var itemNamespaceEnd = "</Item>";
        var x2js = new X2JS();
        var itemXML = itemNamespaceBegin + x2js.json2xml_str(item) + itemNamespaceEnd;

        return itemXML;
    },
    generateOrderItemXML: function (item) {

        var itemNamespaceBegin = "<Item>";
        var itemNamespaceEnd = "</Item>";
        var x2js = new X2JS();
        var itemXML = itemNamespaceBegin + x2js.json2xml_str(item) + itemNamespaceEnd;

        return itemXML;
    },
    generateItemsXML: function (items) {

        var itemNamespaceBegin = "<Items>";
        var itemNamespaceEnd = "</Items>";
        var itemsXML = itemNamespaceBegin + items + itemNamespaceEnd;

        return itemsXML;
    },
    generateOrderXML: function (order, items) {

        var orderNamespaceBegin = "<Order \r\n    xmlns:xsi=\"http:\/\/www.w3.org\/2001\/XMLSchema-instance\" \r\n    xmlns:xsd=\"http:\/\/www.w3.org\/2001\/XMLSchema\">\r\n";
        var orderNamespaceEnd = "</Order>";
        var x2js = new X2JS();
        var orderXML = orderNamespaceBegin + x2js.json2xml_str(order) + items + orderNamespaceEnd;

        return orderXML;
    },
    generateOrderNoItemsXML: function (order) {

        var orderNamespaceBegin = "<Order \r\n    xmlns:xsi=\"http:\/\/www.w3.org\/2001\/XMLSchema-instance\" \r\n    xmlns:xsd=\"http:\/\/www.w3.org\/2001\/XMLSchema\">\r\n";
        var orderNamespaceEnd = "</Order>";
        var x2js = new X2JS();
        var orderXML = orderNamespaceBegin + x2js.json2xml_str(order) + orderNamespaceEnd;

        return orderXML;
    },
};

angular.module('app', ['ui.router', 'app.services', 'app.controllers', 'app.directives', 'ngFileUpload', "customConfiguration"])

    .config(['$stateProvider', '$urlRouterProvider', '$locationProvider', '$httpProvider', function ($stateProvider, $urlRouterProvider, $locationProvider, $httpProvider) {

        $httpProvider.defaults.useXDomain = true;
        delete $httpProvider.defaults.headers.common['X-Requested-With'];

        $stateProvider
            .state('inventory', {
                url: '/inventory',
                templateUrl: '/views/inventory/inventory',
                controller: 'InventoryCtrl'

            })
            .state('inventoryCustomize', {
                url: "/inventory/:inventoryId",
                templateUrl: '/views/inventory/inventorydetails',
                controller: "InventoryDetailsCtrl"
            })
            .state("inventoryAdd", {
                url: "/inventoryAdd",
                templateUrl: '/views/inventory/inventoryadd',
                controller: "InventoryAddCtrl"
            })
            .state("orders", {
                url: "/orders",
                templateUrl: "/views/orders/all",
                controller: "OrderCtrl"
            })
            .state("newOrder", {
                url: "/neworder",
                templateUrl: '/views/orders/neworder',
                controller: "OrderAddCtrl"
            })
            .state("orderDetails", {
                url: "/order/:orderUniqueId",
                templateUrl: '/views/orders/details',
                controller: "OrderDetailsCtrl"
            })
            .state("customizeOrder", {
                url: "/customizeOrder/:orderUniqueId",
                templateUrl: '/views/orders/customize',
                controller: "OrderCustomizeCtrl"
            })

        $urlRouterProvider.otherwise('/inventory');

        $locationProvider.html5Mode({
            enabled: true,
            requireBase: false
        });

    }])


    .run(['$templateCache', '$rootScope', '$state', '$stateParams', function ($templateCache, $rootScope, $state, $stateParams) {

        var view = angular.element('#ui-view');
        $templateCache.put(view.data('tmpl-url'), view.html());

        // Allows to retrieve UI Router state information from inside templates
        $rootScope.$state = $state;
        $rootScope.$stateParams = $stateParams;

        $rootScope.$on('$stateChangeSuccess', function (event, toState) {

            // Sets the layout name, which can be used to display different layouts (header, footer etc.)
            // based on which page the user is located
            $rootScope.layout = toState.layout;
        });
    }]);