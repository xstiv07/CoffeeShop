var completedOrders = [];
var counter = 0;
var orderRefreshRate = 30000; //30 sec
var postCompletedrefreshRate = 200000; //2 min

$(document).ready(function () {
    setInterval(function () {
        getPendingOrders();
    }, orderRefreshRate);
});

function getPendingOrders() {
    $.ajax({
        type: 'GET',
        url: "/Home/Index",
        beforeSend: function () {
            $(".timer-loader").show();
        },
        dataType: "html",
        success: function (data, textStatus, jqXHR) {
            $('#content').html(data);
            $(".timer-loader").hide();

            if (counter == 0) {
                postCompletedOrders(completedOrders); //initial post

                //timer
                setInterval(function () {
                    postCompletedOrders(completedOrders);
                }, postCompletedrefreshRate);

                counter += 1;
            };
        }
    });
};

function postCompletedOrders(complIds) {
    $.ajax({
        type: 'POST',
        url: "/Home/getRidOfCompleted",
        data: { ids: complIds },
        success: function () {
            completedOrders = []; //clear out the completed orders array
        }
    });
};