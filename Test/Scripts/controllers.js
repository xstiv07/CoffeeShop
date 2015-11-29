'use strict';

angular.module('app.controllers', ['ui.router'])

    // Path: /inventory
    .controller('InventoryCtrl', ['$scope', '$rootScope', 'Item', function ($scope, $rootScope, Item) {
        $rootScope.processing = true;
        Item.all().success(function (data) {
            $rootScope.items = data;
            $rootScope.processing = false;
        });
    }])

    // Path: /inventory/{inventoryId}
    .controller('InventoryDetailsCtrl', ['$scope', "$stateParams", "Item", "$rootScope", "$state", function ($scope, $stateParams, Item, $rootScope, $state) {

        $rootScope.processing = true;

        $scope.milkValues = ["None", "Lowfat", "Whole"];
        $scope.sizeValues = ["Small", "Medium", "Large"];

        var itemId = $stateParams.inventoryId;

        Item.one(itemId).success(function (data) {
            $scope.item = data;
            $rootScope.processing = false;
        });

        $scope.update = function (isValid) {
            if (isValid) {
                $rootScope.processing = true;

                var itemNamespaceBegin = "<Item \r\n    xmlns:xsi=\"http:\/\/www.w3.org\/2001\/XMLSchema-instance\" \r\n    xmlns:xsd=\"http:\/\/www.w3.org\/2001\/XMLSchema\">\r\n";
                var itemNamespaceEnd = "</Item>";

                var x2js = new X2JS();

                var itemXML = itemNamespaceBegin + x2js.json2xml_str($scope.item) + itemNamespaceEnd;

                Item.update($scope.item.Id, itemXML).success(function (data) {
                    $scope.item = data;
                    $rootScope.processing = false;
                });
            };
        };

        $scope.deleteItem = function () {
            $rootScope.processing = true;
            Item.delete($scope.item.Id).success(function (data) {
                if (data.isDeleted) {
                    $state.go('inventory');
                    $rootScope.processing = false;
                };
            });
        };
    }])