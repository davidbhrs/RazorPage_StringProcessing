a = Number(document.getElementById("aNum").value);
e = Number(document.getElementById("eNum").value);
o = Number(document.getElementById("oNum").value);
u = Number(document.getElementById("uNum").value);

google.charts.load('current', { 'packages': ['corechart'] });
google.charts.setOnLoadCallback(drawChart);

function drawChart() {

    var data = google.visualization.arrayToDataTable([
        ['Vokal', 'Änderungen'],
        ['a', a],
        ['e', e],
        ['o', o],
        ['u', u]
    ]);

    var options = {
        title: 'Ersetzte Vokale',
        backgroundColor: { fill: 'transparent' },
        titleTextStyle: {
            color: 'white'
        },
        hAxis: {
            textStyle: {
                color: 'white'
            },
            titleTextStyle: {
                color: 'white'
            }
        },
        vAxis: {
            textStyle: {
                color: 'white'
            },
            titleTextStyle: {
                color: 'white'
            }
        },
        legend: {
            textStyle: {
                color: 'white'
            }
        }
    };

    var chart = new google.visualization.PieChart(document.getElementById('piechart'));

    chart.draw(data, options);
}