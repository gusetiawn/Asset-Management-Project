$(document).ready(function () {
    //Chart Gender
    $.ajax({
        url: "https://localhost:44395/api/users/gendermale"
    }).done((result) => {
        var i = 0;
        result.forEach(function (number) {
            i += 1;
        });
        console.log(i);
        $.ajax({
            url: "https://localhost:44395/api/users/genderfemale"
        }).done((success) => {
            var j = 0
            success.forEach(function (number) {
                j += 1
            });
            console.log(j);
            var chartGender = document.getElementById('chartGenderCompany').getContext('2d');
            var genderChart = new Chart(chartGender, {
                type: 'pie',
                data: {
                    labels: ['Male', 'Female'],
                    datasets: [{
                        data: [i, j],
                        backgroundColor: [
                            'rgb(255, 99, 132)',
                            'rgb(255, 205, 86)'
                        ]
                    }],
                    hoverOffset: 4
                },
                options: {
                    plugins: {
                        legend: {
                            display: false
                        }
                    }
                }
            });
        }).fail((notSuccess) => {
            console.log(notSuccess);
            alert("error");
        });
    }).fail((error) => {
        console.log(error);
        alert("error");
    });

    //Chart Department
    $.ajax({
        url: "https://localhost:44395/api/users/deptengineering"
    }).done((result) => {
        var i = 0; //number of dept engineering
        result.forEach(function (number) {
            i += 1;
        });
        console.log(i);
        $.ajax({
            url: "https://localhost:44395/api/users/depthr"
        }).done((success) => {
            var j = 0 //number of dept hr
            success.forEach(function (number) {
                j += 1
            });
            console.log(j);
            $.ajax({
                url: "https://localhost:44395/api/users/deptfinance"
            }).done((ok) => {
                var k = 0 //number of dept finance
                ok.forEach(function (number) {
                    k += 1
                });
                console.log(k);
                $.ajax({
                    url: "https://localhost:44395/api/users/deptadmin"
                }).done((good) => {
                    var l = 0 //number of dept admin
                    good.forEach(function (number) {
                        l += 1
                    });
                    console.log(l);
                    //fill below
                    var chartDepartment = document.getElementById('chartDepartmentCompany').getContext('2d');
                    var departmentChart = new Chart(chartDepartment, {
                        type: 'doughnut',
                        data: {
                            labels: ['Engineering', 'Human Resources', 'Finance', 'Administration'],
                            datasets: [{
                                data: [i, j, k, l],
                                backgroundColor: [
                                    'rgb(255, 99, 132)',
                                    'rgb(54, 162, 235)',
                                    'rgb(255, 205, 86)',
                                    'rgb(75, 192, 192)'
                                ]
                            }],
                            hoverOffset: 4
                        },
                        options: {
                            plugins: {
                                legend: {
                                    display: false
                                }
                            }
                        }
                    });
                }).fail((notGood) => {
                    console.log(notGood);
                    alert("error");
                });
            }).fail((notOk) => {
                console.log(notOk);
                alert("error");
            });
        }).fail((notSuccess) => {
            console.log(notSuccess);
            alert("error");
        });
    }).fail((error) => {
        console.log(error);
        alert("error");
    });

    //Chart All Request
    $.ajax({
        url: "https://localhost:44395/API/RequestItems/reqwaiting"
    }).done((result) => {
        var i = 0; //number of waiting request
        result.forEach(function (number) {
            i += 1;
        });
        console.log(i);
        waiting = `<h2 class="text-dark mb-1 font-weight-medium">${i}</h2>`
        $(".reqWaiting").html(waiting);
        $.ajax({
            url: "https://localhost:44395/API/RequestItems/reqapprove"
        }).done((success) => {
            var j = 0 //number of approved request
            success.forEach(function (number) {
                j += 1
            });
            console.log(j);
            approved = `<h2 class="text-dark mb-1 font-weight-medium">${j}</h2>`
            $(".reqApprove").html(approved);
            $.ajax({
                url: "https://localhost:44395/API/RequestItems/reqreject"
            }).done((ok) => {
                var k = 0 //number of rejected request
                ok.forEach(function (number) {
                    k += 1
                });
                console.log(k);
                rejected = `<h2 class="text-dark mb-1 font-weight-medium">${k}</h2>`
                $(".reqReject").html(rejected);
                $.ajax({
                    url: "https://localhost:44395/API/RequestItems/reqreturn"
                }).done((good) => {
                    var l = 0 //number of returned request
                    good.forEach(function (number) {
                        l += 1
                    });
                    console.log(l);
                    returned = `<h2 class="text-dark mb-1 font-weight-medium">${l}</h2>`
                    $(".reqReturn").html(returned);
                    //fill below
                    var chartAllReq = document.getElementById('chartRequest').getContext('2d');
                    var myChart = new Chart(chartAllReq, {
                        type: 'bar',
                        data: {
                            labels: ['Waiting for Approval', 'Approved Request', 'Rejected Request', 'Returned Request'],
                            datasets: [{
                                data: [i, j, k, l],
                                backgroundColor: [
                                    'rgba(255, 99, 132, 0.2)',
                                    //'rgba(54, 162, 235, 0.2)',
                                    //'rgba(255, 206, 86, 0.2)',
                                    'rgba(75, 192, 192, 0.2)',
                                    'rgba(153, 102, 255, 0.2)',
                                    'rgba(255, 159, 64, 0.2)'
                                ],
                                borderColor: [
                                    'rgba(255, 99, 132, 1)',
                                    //'rgba(54, 162, 235, 1)',
                                    //'rgba(255, 206, 86, 1)',
                                    'rgba(75, 192, 192, 1)',
                                    'rgba(153, 102, 255, 1)',
                                    'rgba(255, 159, 64, 1)'
                                ],
                                borderWidth: 1
                            }]
                        },
                        options: {
                            scales: {
                                x: {
                                    ticks: {
                                        display: false
                                    }
                                },
                                y: {
                                    beginAtZero: true
                                }
                            },
                            plugins: {
                                legend: {
                                    display: false
                                }
                            }
                        }
                    });
                }).fail((notGood) => {
                    console.log(notGood);
                    alert("error");
                });
            }).fail((notOk) => {
                console.log(notOk);
                alert("error");
            });
        }).fail((notSuccess) => {
            console.log(notSuccess);
            alert("error");
        });
    }).fail((error) => {
        console.log(error);
        alert("error");
    });

    //Chart All Request by Id
    $.ajax({
        url: "https://localhost:44389/RequestItem/GetWaiting"
    }).done((result) => {
        var objWait = JSON.parse(result);
        var i = 0; //number of waiting request
        objWait.forEach(function (number) {
            i += 1;
        });
        console.log(i);
        waiting = `<h2 class="text-dark mb-1 font-weight-medium">${i}</h2>`
        $(".requestWaiting").html(waiting);
        $.ajax({
            url: "https://localhost:44389/RequestItem/GetApprove"
        }).done((success) => {
            var objApprove = JSON.parse(success);
            var j = 0 //number of approved request
            objApprove.forEach(function (number) {
                j += 1
            });
            console.log(j);
            approved = `<h2 class="text-dark mb-1 font-weight-medium">${j}</h2>`
            $(".requestApprove").html(approved);
            $.ajax({
                url: "https://localhost:44389/RequestItem/GetReject"
            }).done((ok) => {
                var objReject = JSON.parse(ok);
                var k = 0 //number of rejected request
                objReject.forEach(function (number) {
                    k += 1
                });
                console.log(k);
                rejected = `<h2 class="text-dark mb-1 font-weight-medium">${k}</h2>`
                $(".requestReject").html(rejected);
                $.ajax({
                    url: "https://localhost:44389/RequestItem/GetReturn"
                }).done((good) => {
                    var objReturn = JSON.parse(good);
                    var l = 0 //number of returned request
                    objReturn.forEach(function (number) {
                        l += 1
                    });
                    console.log(l);
                    returned = `<h2 class="text-dark mb-1 font-weight-medium">${l}</h2>`
                    $(".requestReturn").html(returned);
                    //fill below
                    var ctx = document.getElementById('chartRequestEmployee').getContext('2d');
                    var myChart = new Chart(ctx, {
                        type: 'bar',
                        data: {
                            labels: ['Waiting for Approval', 'Approved Request', 'Rejected Request', 'Returned Request'],
                            datasets: [{
                                data: [i, j, k, l],
                                backgroundColor: [
                                    'rgba(255, 99, 132, 0.2)',
                                    //'rgba(54, 162, 235, 0.2)',
                                    //'rgba(255, 206, 86, 0.2)',
                                    'rgba(75, 192, 192, 0.2)',
                                    'rgba(153, 102, 255, 0.2)',
                                    'rgba(255, 159, 64, 0.2)'
                                ],
                                borderColor: [
                                    'rgba(255, 99, 132, 1)',
                                    //'rgba(54, 162, 235, 1)',
                                    //'rgba(255, 206, 86, 1)',
                                    'rgba(75, 192, 192, 1)',
                                    'rgba(153, 102, 255, 1)',
                                    'rgba(255, 159, 64, 1)'
                                ],
                                borderWidth: 1
                            }]
                        },
                        options: {
                            scales: {
                                x: {
                                    ticks: {
                                        display: false
                                    }
                                },
                                y: {
                                    beginAtZero: true
                                }
                            },
                            plugins: {
                                legend: {
                                    display: false
                                }
                            }
                        }
                    });
                }).fail((notGood) => {
                    console.log(notGood);
                    alert("error");
                });
            }).fail((notOk) => {
                console.log(notOk);
                alert("error");
            });
        }).fail((notSuccess) => {
            console.log(notSuccess);
            alert("error");
        });
    }).fail((error) => {
        console.log(error);
        alert("error");
    });
});


//var ctx = document.getElementById('chartRequestEmployee').getContext('2d');
//var myChart = new Chart(ctx, {
//    type: 'bar',
//    data: {
//        labels: ['Waiting for Approval', 'Approved Request', 'Rejected Request', 'Returned Request'],
//        datasets: [{
//            data: [12, 19, 3, 5],
//            backgroundColor: [
//                'rgba(255, 99, 132, 0.2)',
//                //'rgba(54, 162, 235, 0.2)',
//                //'rgba(255, 206, 86, 0.2)',
//                'rgba(75, 192, 192, 0.2)',
//                'rgba(153, 102, 255, 0.2)',
//                'rgba(255, 159, 64, 0.2)'
//            ],
//            borderColor: [
//                'rgba(255, 99, 132, 1)',
//                //'rgba(54, 162, 235, 1)',
//                //'rgba(255, 206, 86, 1)',
//                'rgba(75, 192, 192, 1)',
//                'rgba(153, 102, 255, 1)',
//                'rgba(255, 159, 64, 1)'
//            ],
//            borderWidth: 1
//        }]
//    },
//    options: {
//        scales: {
//            x: {
//                ticks: {
//                    display: false
//                } 
//            },
//            y: {
//                beginAtZero: true
//            }
//        },
//        plugins: {
//            legend: {
//                display: false
//            }
//        }
//    }
//});



//var chartDepartment = document.getElementById('chartDepartmentCompany').getContext('2d');
//var departmentChart = new Chart(chartDepartment, {
//    type: 'doughnut',
//    data: {
//        labels: ['Engineering', 'Human Resources', 'Finance', 'Administration'],
//        datasets: [{
//            data: [12, 19, 5, 3],
//            backgroundColor: [
//                'rgb(255, 99, 132)',
//                'rgb(54, 162, 235)',
//                'rgb(255, 205, 86)',
//                'rgb(75, 192, 192)'
//            ]
//        }],
//        hoverOffset: 4
//    },
//    options: {
//        plugins: {
//            legend: {
//                display: false
//            }
//        }
//    }
//});