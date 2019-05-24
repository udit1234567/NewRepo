
function testPurpose() {
}

function setChartStatus(id, data) {

    var divObject = $('#' + id);
    divObject.data("data-ChartName", data.ChartName);
    divObject.data("data-GraphType", data.GraphType);

    divObject.data("data-ChartDetails", data.chart);
   // divObject.data("data-Key", data);
}

function ResetBusinessChart(e, cname) {

    var ProductName = e.dataItem.dataContext.X1Value;   
    var functionName = "";
    if (cname == "Product Portfolio") {
        functionName = "createPieChart()";
    }
    else if (cname == "Premium Payment Term") {
        functionName = "createPPT()";
    }
    else {
        functionName = "createPolicyStatus()"
    }
    var Div = e.chart.div.id;
    var chartdata = [];
    var id = ("#" + Div);
    $.ajax({
        url: "../MyBusiness/DrilDownChartData",
        type: 'POST',
        dataType: 'json',
        data: { "ProductName": ProductName, "cname": cname },
        contentTyp: 'application/json;charset=utf-8',
        success: function (result) {
            debugger
            $(id).empty();
            var DrilDownDiv = Div + "_DrilDiv";
            var ddl = "<div id='" + DrilDownDiv + "' onClick='" + functionName + "' class='back' style='Float:right' />";
            $("#" + Div).append(ddl);
            var ChartId = Div + "_DrilChart";
            $("#" + Div).append("<div class='Chart' id='" + ChartId + "'/>");

            $("#" + ChartId).attr("data-ParentDiv");
            $("#" + ChartId).attr("data-DrilDownOn");
            if (result != undefined) {
                $("#" + ChartId).AVOCharts({
                    Data: result
                });
            }

        }
    });
}
function createChart(DivId, charData, catField, valField,title) {
    debugger
    var chart = new AmCharts.makeChart(DivId, {
        "type": "serial",        
        "titles": [{ "text":title , "size": 18, "color": "blue",font:"bold" }],
        "theme": "light",
        "dataProvider": charData,
        "categoryField": catField,
        "graphs": [{
            "valueField": valField,
            "ballonText": "<span style='font-size:13px'>[[title]] in [[category]]:<b>[[value]]</span></b>",
            "title": valField,
            "fillAlphas": "0.8",
            "fillAlpha": "1",
            "type": "column"
        }],
        "categoryAxis": {
            "gridPosition": "start"
        },
        "depth3D": 15,
        "angle": 30,
        "startDuration": 1,
        "export": {
            "enabled": true
        }
    });
}