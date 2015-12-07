'use strict';

angular.module('app.controllers', [])

    .controller('MainCtrl', ['$scope', '$rootScope', 'Item', 'Order', function ($scope, $rootScope, Item, Order) {
        if ($rootScope.items == null) {
            Item.all().success(function (data) {
                $rootScope.items = data;
            });
        };

        if ($rootScope.orders == null) {
            Order.all().success(function (data) {
                $rootScope.orders = data;
            });
        };
    }])

    .controller('InventoryCtrl', ['$scope', '$rootScope', 'Item', function ($scope, $rootScope, Item) {
        if ($rootScope.items == null) {
            $rootScope.processing = true;
            Item.all().success(function (data) {
                $rootScope.items = data;
                $rootScope.processing = false;
            });
        }
    }])

    .controller("InventoryAddCtrl", ["$scope", "$rootScope", "Item", "$state", 'Upload', "S3", "apiConfig", "$timeout", function ($scope, $rootScope, Item, $state, Upload, S3, apiConfig, $timeout) {

        $scope.milkValues = apiConfig.milkEnum;
        $scope.sizeValues = apiConfig.sizeEnum;

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
                            window.location = '/inventory';
                        };
                    });

                }, function (resp) {
                    alert('Error status: ' + resp.status);
                }, function (evt) {
                    $scope.progressPercentage = parseInt(100.0 * evt.loaded / evt.total) + "%";
                });
            } else {
                $scope.error = 'Invalid form';
                $scope.displayError = true;
                $timeout(function () { $scope.displayError = false }, 3000);
            };
        };
       
    }])

    // Path: /inventory/{inventoryId}
    .controller('InventoryDetailsCtrl', ['$scope', "$stateParams", "Item", "$rootScope", "$state", "apiConfig", "$timeout", function ($scope, $stateParams, Item, $rootScope, $state, apiConfig, $timeout) {

        $rootScope.processing = true;

        $scope.milkValues = apiConfig.milkEnum;
        $scope.sizeValues = apiConfig.sizeEnum;

        var itemId = $stateParams.inventoryId;

        $scope.backToItems = function () {
            window.location = '/inventory';
        };

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
                    $scope.success = "Updated";
                    $scope.displaySuccess = true;
                    $timeout(function () { $scope.displaySuccess = false }, 3000);

                    $rootScope.processing = false;
                });
            };
        };

        $scope.deleteItem = function () {
            $rootScope.processing = true;
            Item.delete($scope.item.Id).success(function (data) {
                if (data.isDeleted) {
                    $rootScope.processing = false;
                    $scope.backToItems();
                };
            });
        };
    }])

    .controller("OrderCtrl", ["$scope", "$rootScope", "Order", "$state", function ($scope, $rootScope, Order, $state) {

        if ($rootScope.orders == null) {
            $rootScope.processing = true;

            Order.all().success(function (data) {
                $rootScope.orders = data;
                $rootScope.processing = false;
            });
        };

        $scope.cancelOrder = function (id) {
            $rootScope.processing = true;
            Order.cancel(id).success(function () {
                $rootScope.processing = false;
                window.location = "/orders";
            })
        };

        $scope.capturePayment = function (id) {
            $rootScope.processing = true;

            Order.capturePayment(id).success(function (data) {
                $rootScope.processing = false;
                window.location = "/orders";
            });
        };
    }])

    .controller("OrderDetailsCtrl", ["$scope", "$rootScope", "Order", "$state", "$stateParams", "Item", function ($scope, $rootScope, Order, $state, $stateParams, Item) {
        $rootScope.processing = true;
        var orderId = $stateParams.orderUniqueId;

        $scope.orderItems = [];
        $scope.newItem = {};
        $scope.newItemQty = {};

        var templateObj = { name: 'orderItems.html', url: 'orderItems.html' };
        $scope.templates = [templateObj];

        $scope.backToOrders = function () {
            window.location = "/orders";
        };

        $scope.deleteFromOrder = function (itemId) {
            $rootScope.processing = true;
            Order.deleteItemFromOrder(itemId, orderId).success(function (data) {
                $rootScope.processing = false;
                window.location = "/order/" + orderId;
            });
        };

        $scope.addItemToOrder = function () {
            $rootScope.processing = true;

            Order.addItemToOrder($scope.newItem.item.Id, orderId, $scope.newItemQty.qty).success(function () {
                $rootScope.processing = false;
                window.location = "/order/" + orderId;
            })
        };

        Order.getLines(orderId).success(function (data) {

            angular.forEach(data, function (line) {
                $rootScope.processing = true;
                Item.one(line.ItemId).success(function (orderItem) {
                    $rootScope.processing = false;
                    orderItem.LineQty = line.LineQty;
                    $scope.orderItems.push(orderItem);
                });
            });

            $rootScope.processing = false;
        });
    }])

    .controller("OrderCustomizeCtrl", ["$scope", "$rootScope", "Order", "$state", "$stateParams", "apiConfig", "$timeout", function ($scope, $rootScope, Order, $state, $stateParams, apiConfig, $timeout) {
        $scope.processing = true;
        var id = $stateParams.orderUniqueId;

        $scope.orderLocation = apiConfig.orderLocationEnum;
        $scope.orderStatus = apiConfig.orderStatusEnum;

        $scope.backToOrders = function () {
            window.location = "/orders";
        }

        Order.one(id).success(function (data) {
            $scope.order = data;
            $scope.processing = false;
        });

        $scope.updateOrder = function (isValid) {
            if (isValid) {
                $rootScope.processing = true;

                var orderXML = coffeeShop.helpers.generateOrderNoItemsXML($scope.order);
                Order.update($scope.order.Id, orderXML).success(function (data) {
                    $rootScope.processing = false;
                    $scope.success = "Updated";
                    $scope.displaySuccess = true;
                    $timeout(function () { $scope.displaySuccess = false }, 3000);
                });
            } else {
                $scope.error = 'Invalid Form';
                $scope.displayError = true;
                $timeout(function () { $scope.displayError = false }, 3000);
            };
        };
    }])

    .controller("OrderAddCtrl", ["$scope", "$rootScope", "Order", "apiConfig", "Item", "$timeout", "$state", function ($scope, $rootScope, Order, apiConfig, Item, $timeout, $state) {
        $scope.Location = apiConfig.orderLocation;
        $scope.Status = apiConfig.orderStatus;

        var templateObj = { name: 'orderItems.html', url: 'orderItems.html' };

        if ($rootScope.items != null) {
            $scope.items = $rootScope.items;
        } else {
            $rootScope.processing = true;
            Item.all().success(function (data) {
                $rootScope.items = data;
                $scope.items = data;
                $rootScope.processing = false;
            });
        };

        $scope.templates = [templateObj];
        $scope.orderItems = [];
        $scope.total = 0;
        $scope.order = {};

        $scope.orderTotal = function (action, id) {
            if (action == "add") {
                
                $scope.total += $scope.orderItems[id].Price * $("#qty_" + id).val();
            } else {
                $scope.total -= $scope.orderItems[(id - 1)].Price * $("#qty_" + (id - 1)).val();
            };
        };

        $scope.deleteOrderItem = function (id) {
            $scope.orderTotal("del", id);
            $scope.orderItems.splice(id, 1);
            $scope.templates.splice(id, 1);

            $("#cm_" + (id - 1)).css({'display': 'none'})
            $("#sel_" + (id - 1)).removeAttr('disabled')
            $("#qty_" + (id - 1)).removeAttr('disabled');
        };

        $scope.orderLocation = apiConfig.orderLocationEnum;
        $scope.orderStatus = apiConfig.orderStatusEnum;

        $scope.addOrderItem = function (id, isValid) {
            if (isValid) {
                $scope.templates.push(templateObj);
                $scope.orderTotal("add", id);

                $scope.orderItems[id].Quantity = $('#qty_' + id).val();

                //set a disabled class
                $("#cm_" + id).css({ 'display': 'block' });
                $("#sel_" + id).prop('disabled', 'disabled');
                $("#qty_" + id).prop('disabled', 'disabled');
            }else {
                $scope.error = 'Invalid Form';
                $scope.displayError = true;
                $timeout(function () { $scope.displayError = false }, 3000);
            };
        };

        $scope.addOrder = function (isValid) {
            if (isValid && $scope.total > 0) {
                $rootScope.processing = true;

                var itemsXML = "";

                angular.forEach($scope.orderItems, function (item) {
                    delete item['$$hashKey'];
                    delete item['Lines'];
                    itemsXML += coffeeShop.helpers.generateOrderItemXML(item);
                });

                var orderItemsXML = coffeeShop.helpers.generateItemsXML(itemsXML);
                var orderXML = coffeeShop.helpers.generateOrderXML($scope.order, orderItemsXML);

                Order.create(orderXML).success(function () {
                    $rootScope.processing = false;
                    window.location = "/orders";
                });

            } else {
                $scope.error = 'Invalid Form';
                $scope.displayError = true;
                $timeout(function () { $scope.displayError = false }, 3000);
            }
        }
    }])