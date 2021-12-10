$(document).ready(function () {
    var roles = [];
    var amount = [];
    myajax = $.ajax({
        type: "GET",
        url: "/Dashboard/GetRoles",
        success: function (data) {
            $.each(data.data, function (index, value) {
                roles.push(value.roleName);
                amount.push(value.amount);
            })
        }
    })

    $.when(myajax).done(function () {
        Chart.defaults.global.defaultFontFamily = 'Nunito', '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
        Chart.defaults.global.defaultFontColor = '#858796';


        var ctx = document.getElementById("myPieChart");
        var myPieChart = new Chart(ctx, {
            type: 'doughnut',
            data: {
                labels: roles,
                datasets: [{
                    data: amount,
                    backgroundColor: ['#4e73df', '#1cc88a', '#36b9cc'],
                    hoverBackgroundColor: ['#2e59d9', '#17a673', '#2c9faf'],
                    hoverBorderColor: "rgba(234, 236, 244, 1)",
                }],
            },
            options: {
                maintainAspectRatio: false,
                tooltips: {
                    backgroundColor: "rgb(255,255,255)",
                    bodyFontColor: "#858796",
                    borderColor: '#dddfeb',
                    borderWidth: 1,
                    xPadding: 15,
                    yPadding: 15,
                    displayColors: false,
                    caretPadding: 10,
                },
                legend: {
                    display: false
                },
                cutoutPercentage: 80,
            },
        });
    })
})