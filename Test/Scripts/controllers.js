'use strict';

angular.module('app.controllers', [])

    // Path: /inventory
    .controller('InventoryCtrl', ['$scope', '$rootScope', 'Item', function ($scope, $rootScope, Item) {
        $rootScope.processing = true;
        Item.all().success(function (data) {
            $rootScope.items = data;
            $rootScope.processing = false;
        });
    }])

    .controller("InventoryAddCtrl", ["$scope", "$rootScope", "Item", "$state", 'Upload', "S3", "$timeout", function ($scope, $rootScope, Item, $state, Upload, S3, $timeout) {

        $scope.milkValues = ["Lowfat", "Whole"];
        $scope.sizeValues = ["Small", "Medium", "Large"];

        $scope.item = {};

        $scope.setFile = function (element) {
            $scope.$apply(function ($scope) {
                $scope.currentDocument = element.files[0];
            });
        }

        $scope.add = function (isValid) {
            if (isValid) {
                var file = $scope.currentDocument;
                $rootScope.processing = true;
                $scope.progress = true;

                Upload.upload({
                    url: S3.url,
                    method: 'POST',
                    data: {
                        key: file.name,
                        AWSAccessKeyId: S3.AWSAccessKeyId,
                        acl: S3.acl,
                        policy: S3.policy,
                        signature: S3.signature,
                        "Content-Type": file.type != '' ? file.type : 'application/octet-stream',
                        filename: file.name,
                        file: file
                    }
                }).then(function (resp) {
                    $scope.progress = false;
                    $scope.item.ImageURL = file.name;

                    var itemXML = coffeeShop.helpers.generateItemXML($scope.item);
                    Item.create(itemXML).success(function (data) {
                        if (data != null) {
                            $rootScope.processing = false;
                            $state.go('inventory');
                        };
                    });

                }, function (resp) {
                    alert('Error status: ' + resp.status);
                }, function (evt) {
                    $scope.progressPercentage = parseInt(100.0 * evt.loaded / evt.total) + "%";
                });
            } else {
                console.log($scope.form)
                $scope.error = 'Invalid form';
                $scope.displayError = true;
                $timeout(function () { $scope.displayError = false }, 3000);
            };
        };
       
    }])

    // Path: /inventory/{inventoryId}
    .controller('InventoryDetailsCtrl', ['$scope', "$stateParams", "Item", "$rootScope", "$state", function ($scope, $stateParams, Item, $rootScope, $state) {

        $rootScope.processing = true;

        $scope.milkValues = ["Lowfat", "Whole"];
        $scope.sizeValues = ["Small", "Medium", "Large"];

        var itemId = $stateParams.inventoryId;

        Item.one(itemId).success(function (data) {
            $scope.item = data;
            $rootScope.processing = false;
        });

        $scope.update = function (isValid) {
            if (isValid) {
                $rootScope.processing = true;

                var itemXML = coffeeShop.helpers.generateItemXML($scope.item);

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