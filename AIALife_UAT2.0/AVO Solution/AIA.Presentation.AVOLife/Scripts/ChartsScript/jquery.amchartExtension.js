$.fn.AVOCharts = function (options) {
    var settings = $.extend({
        // These are the defaults.


        Data: ""


    }, options);
    array = settings.Data;
    DivId = this[0].id;

    var ParentDiv = DivId.replace("_Chart", "");
    var TotalNoOfButtons = 0;
    TotalNoOfButtons += $("#" + ParentDiv + " .back").length;
    TotalNoOfButtons += $("#" + ParentDiv + " .Chart").length;
    TotalNoOfButtons += $("#" + ParentDiv + " .filter-white").length;
    var LabelWidth = $("#" + ParentDiv).width() - (TotalNoOfButtons * 42);
    if ($("#" + ParentDiv + "_AmChartTitle").length == 0) {
        $("#" + ParentDiv).prepend("<div class='AmChartTitle'  style='width:" + LabelWidth + "px;' id='" + ParentDiv + "_AmChartTitle'> " + array.ChartName + "</div>");
    }
    $("#" + ParentDiv + "_AmChartTitle").html(array.ChartName);

    if (array.chart != null && (array.chart.length == 0 || array.chart.length == undefined)) {
    $(this).append("<label class='DataNotAvailable' > Data not available </label>");
        return;
    }
    else {
        
        $(this).show();
        setChartStatus(DivId, array);
        var chart = "chart";
        var chartData = "";
        var GraphX1ValueField = "";
        var startDuration = "";
        var GraphY1Type = "";
        var GraphY1Title = "";
        var GraphY1ValueField = "";
        var GraphY2Type = "";
        var GraphY2Title = "";
        var GraphY2ValueField = "";
        var GraphY3Title = "";
        var GraphY3ValueField = "";
        if (array.chart != null && array.chart != undefined) {
            chartData = array.chart;

        }
        if (array.pieChart != null && array.pieChart != undefined) {
            chartData = array.pieChart;
        }
        
        if (array.GraphX1ValueField != null) {
            GraphX1ValueField = array.GraphX1ValueField;
        }
        if (array.startDuration != null) {
            startDuration = array.startDuration;
        }

        if (array.GraphType != null) {
            if (array.GraphType == "ColumnandLineChart") {
                GraphY1Type = "column";
                GraphY2Type = "line"
            }
            else if (array.GraphType == "3DBarChart") {
                GraphY1Type = "column";
            }
        }
        if (array.GraphY1Title != null) {
            GraphY1Title = array.GraphY1Title;
        }
        if (array.GraphY1ValueField != null) {
            GraphY1ValueField = array.GraphY1ValueField;
        }

        if (array.GraphY2Title != null) {
            GraphY2Title = array.GraphY2Title;
        }
        if (array.GraphY2ValueField != null) {
            GraphY2ValueField = array.GraphY2ValueField;
        }

        if (array.GraphY3Title != null) {
            GraphY3Title = array.GraphY3Title;
        }
        if (array.GraphY3ValueField != null) {
            GraphY3ValueField = array.GraphY3ValueField;
        }
        
        if (array.GraphType != null && array.GraphType == "ColumnandLineChart") {
            chart = new AmCharts.AmSerialChart();
            chart.dataProvider = chartData;
            AddTitle(chart, array.ChartName);
            chart.pathToImages = "../amcharts/images/";

            chart.categoryField = GraphX1ValueField; //"year";
            chart.startDuration = startDuration;//1;
            chart.depth3D = 20;
            chart.angle = 30;
            // AXES
            // category
            var categoryAxis = chart.categoryAxis;
            categoryAxis.gridPosition = "start";

            // value
            var valueAxis = new AmCharts.ValueAxis();
            valueAxis.axisAlpha = 0;            
            chart.addValueAxis(valueAxis);

            // GRAPHS
            // column graph
            var graph1 = new AmCharts.AmGraph();
            graph1.type = GraphY1Type;  //"column";
            graph1.title = GraphY1Title;

            //"Income";
            // graph1.lineColor = "#a668d5";
          //  var color = Colors.random();
            //  graph1.lineColor = color.rgb;
            var jsonArr = [];

            for (var i = 0; i <= chartData.length - 1; i++) {
                // var color = Colors.random();
                var color;
                if (i == 0) {
                    color="#00BOFO";
                }
                else if(i==1)
                {
                    color="#EC0092";
                }
                else if(i==2)
                {
                    color="#50B848";
                }
                else if (i == 3) {
                    color="#F15A22";
                }
                else if(i==4)
                {
                    color="#AB218E";
                }
                else if(i==5)
                {
                    color="#696A6D";
                }
                else if (i == 6) {
                    color="#ff6347";
                }
                else if (i == 7) {
                    color="#ee82ee";
                }
                else if (i == 8) {
                    color="#ffa500";
                }
                else if (i == 9) {
                    color="#3cb371";
                }
                else if(i==10)
                {
                    color = "#d2691e";
                }

                jsonArr.push(
                    color.rgb
                );
            }
            graph1.lineColor = jsonArr;
            graph1.valueField = GraphY1ValueField;//"income";
            graph1.lineAlpha = 1;
            graph1.fillAlphas = 1;
            graph1.showHandOnHover = true;
            graph1.dashLengthField = "dashLengthColumn";
            graph1.alphaField = "alpha";
            graph1.balloonText = "<span style='font-size:13px;'>[[title]] in [[category]]:<b>[[value]]</b> [[additional]]</span>";
            chart.addGraph(graph1);

            // line
            var graph2 = new AmCharts.AmGraph();
            graph2.type = GraphY2Type;//"line";
            graph2.title = GraphY2Title;//"Expenses";
            // graph2.lineColor = "#fcd202";
           // var color = Colors.random();
            // graph2.lineColor = color.rgb;
            graph2.lineColor = "#9932cc";
            graph2.valueField = GraphY2ValueField; //"expenses";
            graph2.lineThickness = 3;
            graph2.showHandOnHover = true;
            graph2.bullet = "round";
            graph2.bulletBorderThickness = 3;
            graph2.bulletBorderColor = "#fcd202";
            graph2.bulletBorderAlpha = 1;
            graph2.bulletColor = "#ffffff";
            graph2.dashLengthField = "dashLengthLine";
            graph2.balloonText = "<span style='font-size:13px;'>[[title]] in [[category]]:<b>[[value]]</b> [[additional]]</span>";
            chart.addGraph(graph2);

            // LEGEND                
            var legend = new AmCharts.AmLegend();
            legend.useGraphSettings = true;
            chart.addLegend(legend);
        }
        else if (array.GraphType != null && array.GraphType == "Pie3DChart" || array.GraphType != null && array.GraphType == "3Ddonutchart") {

            // PIE CHART
            //            $("#" + DivId).css("width", "230px");

            chart = new AmCharts.AmPieChart();
            chart.dataProvider = chartData;
            chart.titleField = GraphX1ValueField;//"country";
            chart.valueField = GraphY1ValueField;// "value";            
            AddTitle(chart, array.ChartName);
            if (array.GraphType == "Pie3DChart") {
                //var legend = new AmCharts.AmLegend();
                //legend.useGraphSettings = true;
                //legend.position = "right";
                //legend.markerType = "circle";
                // chart.labelsEnabled = false;
                chart.outlineAlpha = 0.8;
                chart.outlineThickness = 2;
                chart.radius = 100;
                chart.groupPercent = 1;
                //chart.maxLabelWidth = 20;

                // chart.labelsEnabled = false;
                chart.autoMargins = false;
                chart.marginTop = 0;
                chart.marginBottom = 0;
                chart.marginLeft = 0;
                chart.marginRight = 0;
                chart.pullOutRadius = 0;
                //if (chartData.length <= 10) {
                //    var legend = new AmCharts.AmLegend();
                //    legend.borderAlpha = 0.2;
                //    legend.valueText = false;
                //chart.addLegend(legend);
                //}
                if (array.DrilDown == true) {
                    if (array.DrilDownCategory == "PolicyIssuanceProductGraph2") {

                        chart.addListener("clickSlice", function (e) {

                            PolicyIssuanceProductwise(e, DivId);

                        });



                    }
                    else if (array.DrilDownCategory == "PolicyIssuanceProductGraph3") {
                        chart.addListener("clickSlice", function (e) {

                            PolicyIssuanceProductwisePremium(e, DivId);

                        });

                    }

                    else {
                        chart.addListener("clickSlice", function (e) {
                            if (typeof (DrillDownClick) == "function") {
                                // do something
                                DrillDownClick(e);
                            }
                        });
                    }
                }

            }
            else if (array.GraphType == "3Ddonutchart") {
                chart.sequencedAnimation = true;
                chart.startEffect = "elastic";
                chart.innerRadius = "30%";
                chart.startDuration = 2;
                AddTitle(chart, array.ChartName);
                chart.labelRadius = 15;
            }
           
            if (array.DrilDown == true) {
                if (array.DrilDownCategory == "ProductPortfolio") {

                    chart.addListener("clickSlice", function (e) {                                              
                        ResetBusinessChart(e, 'Product Portfolio');
                    });
                }
                else if (array.DrilDownCategory == "policyStatus") {
                    chart.addListener("clickSlice", function (e) {
                        ResetBusinessChart(e, 'Policy Status');
                    });
                }
                else if (array.DrilDownCategory == "premiumPaymentTerm") {
                    chart.addListener("clickSlice", function (e) {
                        ResetBusinessChart(e, 'Premium Payment Term');
                    });
                }

            }
            var jsonArr = [];

            for (var i = 0; i <= chartData.length - 1; i++) {
                //var color = Colors.random();
                //jsonArr.push(
                //    color.rgb
                //);
                var color;
                if (i == 0) {
                    color = "#00BOFO";
                }
                else if (i == 1) {
                    color = "#EC0092";
                }
                else if (i == 2) {
                    color = "#50B848";
                }
                else if (i == 3) {
                    color = "#F15A22";
                }
                else if (i == 4) {
                    color = "#AB218E";
                }
                else if (i == 5) {
                    color = "#696A6D";
                }
                else if (i == 6) {
                    color = "#ff6347";
                }
                else if (i == 7) {
                    color = "#ee82ee";
                }
                else if (i == 8) {
                    color = "#ffa500";
                }
                else if (i == 9) {
                    color = "#3cb371";
                }
                else if (i == 10) {
                    color = "#d2691e";
                }
                jsonArr.push(
                    color
                );
            }
            chart.colors = jsonArr;
            chart.balloonText = "[[title]]<br><span style='font-size:14px'><b>[[value]]</b> ([[percents]]%)</span>";
            // this makes the chart 3D
            chart.depth3D = 15;
            chart.angle = 30;

        }
        else if (array.GraphType != null && array.GraphType == "Linechartwithreversedvalueaxis") {

            // SERIAL CHART

            chart = new AmCharts.AmSerialChart();
            chart.dataProvider = chartData;
            AddTitle(chart, array.ChartName);
            chart.categoryField = GraphX1ValueField//"year";
            chart.startDuration = 0.5;
            chart.balloon.color = "#000000";
            chart.depth3D = 20;
            chart.angle = 30;
            // AXES
            // category
            var categoryAxis = chart.categoryAxis;
            categoryAxis.fillAlpha = 1;
            //  categoryAxis.fillColor = "#FAFAFA";
           // var color = Colors.random();
           // categoryAxis.fillColor = color.rgb;
            categoryAxis.fillColor = "#00BOFO";
            categoryAxis.gridAlpha = 0;
            categoryAxis.axisAlpha = 0;
            categoryAxis.gridPosition = "start";
            categoryAxis.position = "top";

            // value
            var valueAxis = new AmCharts.ValueAxis();
            if (array.AxisTitle != "") {
                valueAxis.title = array.AxisTitle;//"Place taken";
            }
            valueAxis.dashLength = 5;
            valueAxis.axisAlpha = 0;
            if (array.MinimumValue != "") {
                valueAxis.minimum = array.MinimumValue;//1000;
            }
            if (array.MaximumValue != "") {
                valueAxis.maximum = array.MaximumValue;//10000;
            }
            valueAxis.integersOnly = true;
            valueAxis.gridCount = 10;
            valueAxis.reversed = true; // this line makes the value axis reversed
            chart.addValueAxis(valueAxis);

            // GRAPHS
            // Italy graph						            		
            //var graph = new AmCharts.AmGraph();
            //graph.title = "Italy";
            //graph.valueField = "italy";
            //graph.hidden = true; // this line makes the graph initially hidden           
            //graph.balloonText = "place taken by Italy in [[category]]: [[value]]";
            //graph.lineAlpha = 1;
            //graph.bullet = "round";
            //chart.addGraph(graph);

            //// Germany graph
            //var graph = new AmCharts.AmGraph();
            //graph.title = "Germany";
            //graph.valueField = "germany";
            //graph.balloonText = "place taken by Germany in [[category]]: [[value]]";
            //graph.bullet = "round";
            //chart.addGraph(graph);

            //// United Kingdom graph
            //var graph = new AmCharts.AmGraph();
            //graph.title = "United Kingdom";
            //graph.valueField = "uk";
            //graph.balloonText = "place taken by UK in [[category]]: [[value]]";
            //graph.bullet = "round";
            //chart.addGraph(graph);

            //----------------------------looping since the content is same for y axis---------------------------------------------------------------------------//
            if (array.NumberofYAxis != "") {

                for (var i = 0; i < array.NumberofYAxis; i++) {

                    var graph = new AmCharts.AmGraph();
                    if (i == 0) {
                        graph.title = GraphY1Title;
                        graph.valueField = GraphY1ValueField;
                        if (array.BallonText1 != "") {
                            graph.balloonText = array.BallonText1 + "[[category]]: [[value]]";
                        }
                    }
                    else if (i == 1) {
                        graph.title = GraphY2Title;
                        graph.valueField = GraphY2ValueField;
                        if (array.BallonText2 != "") {
                            graph.balloonText = array.BallonText2 + "[[category]]: [[value]]";
                        }
                    }
                    else if (i == 2) {
                        graph.title = GraphY3Title;
                        graph.valueField = GraphY3ValueField;
                        if (array.BallonText3 != "") {
                            graph.balloonText = array.BallonText3 + "[[category]]: [[value]]";
                        }
                    }

                    graph.bullet = "round";
                    graph.showHandOnHover = true;
                    chart.addGraph(graph);

                }

            }

            //-----------------------------------------------------------------------------------------------------------------------------//
            // CURSOR
            var chartCursor = new AmCharts.ChartCursor();
            chartCursor.cursorPosition = "mouse";
            chartCursor.zoomable = false;
            chartCursor.cursorAlpha = 0;
            chart.addChartCursor(chartCursor);

            // LEGEND
            var legend = new AmCharts.AmLegend();
            legend.useGraphSettings = true;
            chart.addLegend(legend);




        }

        else if (array.GraphType != null && array.GraphType == "AngularGaugeGraph" || array.GraphType != null && array.GraphType == "AngularGaugeGraph3Slice") {
            chart = new AmCharts.AmAngularGauge();

            AddTitle(chart, array.ChartName);
            if (array.AxisTitle != "") {
                chart.addTitle(array.AxisTitle);
            }
            // create axis
            axis = new AmCharts.GaugeAxis();
            if (array.MinimumValue != "") {
                axis.startValue = array.MinimumValue; //0;
            }
            if (array.MaximumValue != "") {
                axis.endValue = array.MaximumValue;//220;
            }
            //-------------------------setting values should be added later---------------------------------//
            // color bands
            //var band1 = new AmCharts.GaugeBand();
            //band1.startValue = 0;
            //band1.endValue = 90;
            //band1.color = "#00CC00";

            //var band2 = new AmCharts.GaugeBand();
            //band2.startValue = 90;
            //band2.endValue = 130;
            //band2.color = "#ffac29";

            //var band3 = new AmCharts.GaugeBand();
            //band3.startValue = 130;
            //band3.endValue = 220;
            //band3.color = "#ea3838";
            //band3.innerRadius = "95%";
            var DataArr = [];

            for (var i = 0; i < array.ListCount; i++) {

                //   DataArr.push(array.lstBandDetails[i]);
                var band = new AmCharts.GaugeBand();
                band.startValue = array.lstBandDetails[i].StartValue;//0;
                band.endValue = array.lstBandDetails[i].EndValue;//90;

                // var color = Colors.random();

                // band.color = color.rgb; //"#00CC00";//if we want to hard code


                if (array.GraphType == "AngularGaugeGraph3Slice") {
                    if (i == 0) {
                        band.color = "#ea3838";
                    }
                    else if (i == 1) {
                        band.color = "#F7FE2E";
                    }
                    else if (i == 2) {


                        band.color = "#01DF01";

                    }
                }
                else {
                    if (i == 0) {
                        band.color = "#ea3838";
                    }
                    else if (i == 1) {


                        band.color = "#00CC00";

                    }

                }

                band.innerRadius = "65%";
                band.outerRadius = "80%";
                DataArr.push(band);
            }
            axis.bands = DataArr;//[band1, band2, band3];
            //-------------------------------------------------------------------------------------------------------------//
            // bottom text
            axis.bottomTextYOffset = -20;
            if (array.BottomText != "") {

                axis.setBottomText(array.BottomText);
            }
            chart.addAxis(axis);

            // gauge arrow
            arrow = new AmCharts.GaugeArrow();
            chart.addArrow(arrow);


            arrow.setValue(array.SetValue);
            array.SetValue = array.SetValue + "%";
            axis.setBottomText(array.SetValue);

        }


        else if (array.GraphType != null && array.GraphType == "AngularGauge") {
            chart = new AmCharts.AmAngularGauge();
            AddTitle(chart, array.ChartName);
            if (array.AxisTitle != "") {
                chart.addTitle(array.AxisTitle);
            }
            // create axis
            axis = new AmCharts.GaugeAxis();
            if (array.MinimumValue != "") {
                axis.startValue = array.MinimumValue; //0;
            }
            if (array.MaximumValue != "") {
                axis.endValue = array.MaximumValue;//220;
            }
            //-------------------------setting values should be added later---------------------------------//
            // color bands
            var band1 = new AmCharts.GaugeBand();
            band1.startValue = 0;
            band1.endValue = 90;
            band1.color = "#00CC00";

            var band2 = new AmCharts.GaugeBand();
            band2.startValue = 90;
            band2.endValue = 130;
            band2.color = "#ffac29";

            var band3 = new AmCharts.GaugeBand();
            band3.startValue = 130;
            band3.endValue = 220;
            band3.color = "#ea3838";
            band3.innerRadius = "95%";

            axis.bands = [band1, band2, band3];
            //-------------------------------------------------------------------------------------------------------------//
            // bottom text
            axis.bottomTextYOffset = -20;
            if (array.BottomText != "") {
                // axis.setBottomText("0 km/h");
                axis.setBottomText(array.BottomText);
            }
            chart.addAxis(axis);

            // gauge arrow
            arrow = new AmCharts.GaugeArrow();
            chart.addArrow(arrow);


            arrow.setValue("120");
            axis.setBottomText("120");


            //chart4.write("chartdivGauge");
            // change value every 2 seconds

            //        setInterval(randomValue, 2000);

            //        function randomValue() {
            //    var value = Math.round(Math.random() * 200);
            //    arrow.setValue(value);
            //            // axis.setBottomText(value + " km/h");
            //    axis.setBottomText(value);
            //}




        }
            //-----------------------3D-BarChart-------------------------------------------//
        else if (array.GraphType != null && array.GraphType == "3DBarChart") {

            var Colour = "";
            if (array.ColourValueField != null) {
                Colour = array.ColourValueField;
            }
            chart = new AmCharts.AmSerialChart();
            AddTitle(chart, array.ChartName);
            // chart.colors = ["#FF0F00", "#FF6600", "#FF9E01", "#FCD202", "#F8FF01", "#B0DE09", "#04D215", "#0D8ECF", "#0D52D1", "#2A0CD0", "#8A0CCF", "#CD0D74", "#754DEB", "#DDDDDD", "#999999", "#333333", "#000000", "#57032A", "#CA9726", "#990000", "#4B0C25"]
            // AXES
            chart.dataProvider = chartData;
            // chart.categoryField = "country";
            chart.categoryField = GraphX1ValueField;
            // the following two lines makes chart 3D
            chart.depth3D = 20;
            chart.angle = 30;
            if (chart.dataProvider.length <= 4) {
                chart.columnWidth = 50 / (500 / chart.dataProvider.length);//--------to set the width of the column----------------------------------//
            }
            // chart.colors = "#B0DE09 #000000 #990000 #0D8ECF".split(' ');
            var jsonArr = [];

            for (var i = 0; i <= chartData.length - 1; i++) {
                //var color = Colors.random();
                //jsonArr.push(
                //    color.rgb
                //);
                var color;
                if (i == 0) {
                    color = "#00BOFO";
                }
                else if (i == 1) {
                    color = "#EC0092";
                }
                else if (i == 2) {
                    color = "#50B848";
                }
                else if (i == 3) {
                    color = "#F15A22";
                }
                else if (i == 4) {
                    color = "#AB218E";
                }
                else if (i == 5) {
                    color = "#696A6D";
                }
                else if (i == 6) {
                    color = "#ff6347";
                }
                else if (i == 7) {
                    color = "#ee82ee";
                }
                else if (i == 8) {
                    color = "#ffa500";
                }
                else if (i == 9) {
                    color = "#3cb371";
                }
                else if (i == 10) {
                    color = "#d2691e";
                }
                jsonArr.push(color);
                              
            }
            chart.colors = jsonArr;
            if (array.RotatedAxis == true) {
                chart.rotate = true;
            }
            // category
            var categoryAxis = chart.categoryAxis;
            categoryAxis.autoWrap = true;

            if (array.GraphX1Title != null) {
                categoryAxis.title = array.GraphX1Title;

            }
            // categoryAxis.title = "RIO";

            categoryAxis.dashLength = 5;
            categoryAxis.gridPosition = "start";

            if ($("#" + ParentDiv).width() <= $(window).width()*(3/4) && chart.dataProvider.length > 7) {
                categoryAxis.labelRotation = 45;
            }

            // value
            var valueAxis = new AmCharts.ValueAxis();
            // valueAxis.title = "Visitors";
            //if (array.AxisTitle != "") {
            //    valueAxis.title = array.AxisTitle;
            //}
            valueAxis.title = array.GraphY1AxisTitle;
            valueAxis.dashLength = 5;
            valueAxis.titleBold = false;
            chart.addValueAxis(valueAxis);

            // GRAPH            
            var graph = new AmCharts.AmGraph();
            // graph.valueField = "visits";
            graph.valueField = GraphY1ValueField;
            // graph.colorField = "color";

            // graph.colorField = Colour;//"color";
            graph.balloonText = "<span style='font-size:14px'>[[category]]: <b>[[value]]</b></span>";
            // graph.type = "column";
            graph.type = GraphY1Type;
            graph.lineAlpha = 0;
            graph.fillAlphas = 1;
            graph.showHandOnHover = true;
            chart.addGraph(graph);


            //var jsonArr = [];

            //for (var i = 0; i <= chartData.length - 1; i++) {
            //    var color = Colors.random();
            //    jsonArr.push(
            //        color.rgb
            //    );
            //}
            //chart.colors = jsonArr;

            //// CURSOR
            //var chartCursor = new AmCharts.ChartCursor();
            //chartCursor.cursorAlpha = 0;
            //chartCursor.zoomable = false;
            //chartCursor.categoryBalloonEnabled = false;
            //chart.addChartCursor(chartCursor);
        }

            //--------------------------100% stacked column chart--(single)----------------------------------------------------------------------------------//
        else if (array.GraphType != null && array.GraphType == "100PercentStackedColumnChart" || array.GraphType != null && array.GraphType == "StackedColumnChart" || array.GraphType != null && array.GraphType == "MultiStackedColumnChartWithLine") {

            //debugger
            // SERIAL CHART
            chart = new AmCharts.AmSerialChart();
            AddTitle(chart, array.ChartName);
            chart.dataProvider = chartData;
            chart.categoryField = GraphX1ValueField;
            chart.plotAreaBorderAlpha = 0.2;
            //if (array.RotatedAxis == true) {
            //    chart.rotate = true;  
            //}
            chart.depth3D = 20;
            chart.angle = 30;

            // SCROLLBAR

            if (chart.dataProvider.length <= 4) {
                chart.columnWidth = 50 / (500 / chart.dataProvider.length);//--------to set the width of the column----------------------------------//
            }
            // AXES
            // category
            var categoryAxis = chart.categoryAxis;
            categoryAxis.gridAlpha = 0.1;
            categoryAxis.axisAlpha = 0;
            categoryAxis.gridPosition = "start";
            categoryAxis.title = array.GraphX1AxisField;
            categoryAxis.autoWrap = true;
            if ($("#" + ParentDiv).width() <= $(window).width() * (3 / 4) && chart.dataProvider.length > 7) {
                categoryAxis.labelRotation = 45;
            }
            // value
            var valueAxis = new AmCharts.ValueAxis();
            if (array.GraphType == "100PercentStackedColumnChart") {
                valueAxis.stackType = "100%";
            }
            else {
                valueAxis.stackType = "regular";
            }
            valueAxis.gridAlpha = 0.1;
            valueAxis.axisAlpha = 0;
            valueAxis.title = array.GraphY1AxisTitle;
            valueAxis.titleBold = false;
            valueAxis.minimum = 0;
            chart.addValueAxis(valueAxis);

            // GRAPHS
            //// first graph    
            //var graph = new AmCharts.AmGraph();
            //graph.title = "Europe";
            //graph.labelText = "[[value]]";
            //graph.valueField = "europe";
            //graph.type = "column";
            //graph.lineAlpha = 0;
            //graph.fillAlphas = 1;
            //graph.lineColor = "#C72C95";
            //chart.addGraph(graph);

            //// second graph              
            //graph = new AmCharts.AmGraph();
            //graph.title = "North America";
            //graph.labelText = "[[value]]";
            //graph.valueField = "namerica";
            //graph.type = "column";
            //graph.lineAlpha = 0;
            //graph.fillAlphas = 1;
            //graph.lineColor = "#D8E0BD";
            //chart.addGraph(graph);

            //// third graph                              
            //graph = new AmCharts.AmGraph();
            //graph.title = "Asia-Pacific";
            //graph.labelText = "[[value]]";
            //graph.valueField = "asia";
            //graph.type = "column";
            //graph.lineAlpha = 0;
            //graph.fillAlphas = 1;
            //graph.lineColor = "#B3DBD4";
            //chart.addGraph(graph);
            //------------------------------for graphs------------------------------------------------------//

            var YAxisLength = array.lstYAxis.length;

            if (YAxisLength > 0) {

                for (i = 0; i <= YAxisLength - 1; i++) {
                    var graph = new AmCharts.AmGraph();

                    if (array.lstYAxis[i] == "Y1") {
                        graph.title = array.GraphY1Title;
                        graph.labelText = "[[percents]]%";
                        graph.balloonText = "[[title]], [[category]]<br><span style='font-size:14px;'><b>[[value]]</b> ([[percents]]%)</span>";
                        graph.valueField = array.GraphY1ValueField;
                    }

                    if (array.lstYAxis[i] == "Y2") {
                        graph.title = array.GraphY2Title;
                        graph.labelText = "[[percents]]%";
                        graph.balloonText = "[[title]], [[category]]<br><span style='font-size:14px;'><b>[[value]]</b> ([[percents]]%)</span>";
                        graph.valueField = array.GraphY2ValueField;
                    }

                    if (array.lstYAxis[i] == "Y3") {
                        graph.title = array.GraphY3Title;
                        graph.labelText = "[[percents]]%";
                        graph.balloonText = "[[title]], [[category]]<br><span style='font-size:14px;'><b>[[value]]</b> ([[percents]]%)</span>";
                        graph.valueField = array.GraphY3ValueField;
                    }

                    if (array.lstYAxis[i] == "Y4") {
                        graph.title = array.GraphY4Title;
                        graph.labelText = "[[percents]]%";
                        graph.balloonText = "[[title]], [[category]]<br><span style='font-size:14px;'><b>[[value]]</b> ([[percents]]%)</span>";
                        graph.valueField = array.GraphY4ValueField;
                    }

                    if (array.lstYAxis[i] == "Y5") {
                        graph.title = array.GraphY5Title;
                        graph.labelText = "[[percents]]%";
                        graph.balloonText = "[[title]], [[category]]<br><span style='font-size:14px;'><b>[[value]]</b> ([[percents]]%)</span>";
                        graph.valueField = array.GraphY5ValueField;
                    }

                    graph.type = "column";
                    graph.showHandOnHover = true;
                    graph.lineAlpha = 0;
                    graph.fillAlphas = 1;
                    //  graph.lineColor = "#C72C95";


                   // var color = Colors.random();
                   // graph.lineColor = color.rgb;
                    var color;
                    if (i == 0) {
                        color = "#00BOFO";
                    }
                    else if (i == 1) {
                        color = "#EC0092";
                    }
                    else if (i == 2) {
                        color = "#50B848";
                    }
                    else if (i == 3) {
                        color = "#F15A22";
                    }
                    else if (i == 4) {
                        color = "#AB218E";
                    }
                    else if (i == 5) {
                        color = "#696A6D";
                    }
                    else if (i == 6) {
                        color = "#ff6347";
                    }
                    else if (i == 7) {
                        color = "#ee82ee";
                    }
                    else if (i == 8) {
                        color = "#ffa500";
                    }
                    else if (i == 9) {
                        color = "#3cb371";
                    }
                    else if (i == 10) {
                        color = "#d2691e";
                    }

                    graph.lineColor = color;
                    chart.addGraph(graph);







                }
            }

            //------------------------------------------------------------------------------------//



            //// value
            //var valueAxis2 = new AmCharts.ValueAxis();
            //valueAxis2.stackType = "regular";
            //valueAxis2.gridAlpha = 0.1;
            //valueAxis2.axisAlpha = 0;
            //valueAxis2.position = "right";
            //chart.addValueAxis(valueAxis2);


            //var graph2 = new AmCharts.AmGraph();
            //graph2.type = "line";
            //graph2.title = "test";

            //graph2.valueField = "lamerica"; //"Premium";
            //graph2.lineThickness = 3;
            //graph2.bullet = "round";
            //graph2.bulletBorderThickness = 3;
            //graph2.bulletBorderColor = "#fcd202";
            //graph2.bulletBorderAlpha = 1;
            //graph2.bulletColor = "#ffffff";
            //graph2.dashLengthField = "dashLengthLine";
            //graph2.valueAxis = valueAxis2;


            //----------------multi stacked column chart with line-----------------------------------------------------------------//

            if (array.GraphType == "MultiStackedColumnChartWithLine") {
                var valueAxis2 = new AmCharts.ValueAxis();
                //valueAxis2.stackType = "regular";
                // value
                var valueAxis2 = new AmCharts.ValueAxis();
                valueAxis2.stackType = "regular";
                valueAxis2.gridAlpha = 0.1;
                valueAxis2.axisAlpha = 0;
                valueAxis2.position = "right";
                valueAxis2.title = array.GraphY2AxisTitle;
                valueAxis2.titleBold = false;
                chart.addValueAxis(valueAxis2);
                var graph = new AmCharts.AmGraph();
                graph.type = "line";
                graph.title = array.GraphY6Title;
               // var RanClr = Colors.random()
               // graph.lineColor = RanClr.rgb;
                graph.lineColor = "#9932cc";
                graph.valueField = array.GraphY6ValueField;
                graph.lineThickness = 3;
                graph.bullet = "round";
                graph.bulletBorderThickness = 3;
                graph.showHandOnHover = true;
                graph.bulletBorderColor = "#fcd202";
                graph.bulletBorderAlpha = 1;
                graph.bulletColor = "#ffffff";
                graph.dashLengthField = "dashLengthLine";
                graph.valueAxis = valueAxis2;
                graph.balloonText = "[[title]], [[category]]<br><span style='font-size:14px;'><b>[[value]]</b></span>";
                chart.addGraph(graph);

                if (array.RotatedAxis == true) {
                    chart.rotate = true;
                }

                //var balloon = chart.balloon;
                //// set properties
                //balloon.adjustBorderColor = true;
                //balloon.color = "#000000";
                //balloon.cornerRadius = 5;
                //balloon.fillColor = "#FFFFFF";

                //balloon.offsetX = 0;
                //balloon.offsetY = 0;
                //balloon.fixedPosition = false;


            }

            //----------------------------------------------------------------------------------------//


            //  chart.addGraph(graph2);

            if (array.DrilDown == true) {
                if (array.DrilDownCategory == "PolicyIssuanceBusiness-Product") {

                    chart.addListener("clickGraphItem", function (e) {
                        
                        PolicyIssuanceProductBusinesswise(e, DivId);
                    });


                }
                else {
                    chart.addListener("clickGraphItem", function (e) {

                        DrillDownClick(e);

                    });
                }
            }

            // LEGEND                  
            var legend = new AmCharts.AmLegend();
            legend.borderAlpha = 0.2;
            legend.useGraphSettings = true;
            legend.valueWidth = 0;
            legend.horizontalGap = 10;
            chart.addLegend(legend);




        }

        else if (array.GraphType != null && array.GraphType == "MultiStackedColumnChart" || array.GraphType != null && array.GraphType == "3Dstackedcolumnchart" || array.GraphType != null && array.GraphType == "2BarChart2LineAxisMultiStackChart") {

            var Y1Colour = "";
            var Y2Colour = "";
            var Y3Colour = "";
            var Y4Colour = "";
            var Y5Colour = "";
            var Y6Colour = "";

            //debugger
            // SERIAL CHART
            chart = new AmCharts.AmSerialChart();
            AddTitle(chart, array.ChartName);
            chart.dataProvider = chartData;
            chart.categoryField = GraphX1ValueField;
            if (array.RotatedAxis == true) {
                chart.rotate = true;
            }
            chart.plotAreaBorderAlpha = 0.2;
            //var chartScrollbar = new AmCharts.ChartScrollbar();
            //chart.addChartScrollbar(chartScrollbar);
            if (array.GraphType == "2BarChart2LineAxisMultiStackChart") {


                // GRAPH
                var graph1 = new AmCharts.AmGraph();
                graph1.title = array.GraphY5Title;//"NoofPolicies";
                graph1.valueField = array.GraphY5ValueField;//"NoofPolicies";
                graph1.type = "column";

                // chartData
                // eval("x*y")
                // var Calc = Total / "[[]]";
                // count() / count() OVER (all([Axis.X]))
                // graph.balloonText = array.GraphY1Title + " " + "[[category]]:[[value]/100]" + ":";//"([[percents]]%)";
                graph1.balloonText = array.GraphY5Title + " " + "[[category]]:[[value]]";
                // chart.balloonText = "[[title]]<br><span style='font-size:14px'><b>[[value]]</b> ([[percents]]%)</span>";

                graph1.lineAlpha = 0;

               // var Y5Colour = Colors.random();
                // graph1.fillColors = Y5Colour.rgb;// "#bf1c25";
                graph1.fillColors = "#00BOFO";
                graph1.stackable = false;
                graph1.fillAlphas = 1;
                if (chart.dataProvider.length <= 4) {
                    chart.columnWidth = 50 / (500 / chart.dataProvider.length);//--------to set the width of the column----------------------------------//
                }
                chart.addGraph(graph1);
            }

            if (array.GraphType == "MultiStackedColumnChart" || array.GraphType == "2BarChart2LineAxisMultiStackChart") {
                chart.depth3D = 20;
                chart.angle = 30;
            }
            else if (array.GraphType == "3Dstackedcolumnchart") {
                chart.angle = 30;
                chart.depth3D = 40;
            }
            if (chart.dataProvider.length <= 4) {
                chart.columnWidth = 50 / (500 / chart.dataProvider.length);//--------to set the width of the column----------------------------------//
            }


            // add click listener
            if (array.DrilDown == true) {
                //chart.addListener("clickGraphItem", function (e) {

                //    handleClickOfChart(e, DivId);
                //});
                chart.addListener("clickGraphItem", function (e) {

                    DrillDownClick(e);

                });
                //----------click event of the graph-----------------------//

            }


            // AXES
            // category
            var categoryAxis = chart.categoryAxis;
            //categoryAxis.gridAlpha = 0.1;
            //categoryAxis.axisAlpha = 0;
            categoryAxis.gridPosition = "start";

            if (array.GraphType == "MultiStackedColumnChart") {
                categoryAxis.gridAlpha = 0.1;
                categoryAxis.axisAlpha = 0;
            }
            else if (array.GraphType == "3Dstackedcolumnchart") {
                categoryAxis.axisColor = "#DADADA";
                categoryAxis.axisAlpha = 1;
                categoryAxis.dashLength = 5;
            }
            categoryAxis.autoWrap = true;
            if ($("#" + ParentDiv).width() <= $(window).width() * (3 / 4) && chart.dataProvider.length > 7) {
                categoryAxis.labelRotation = 45;
            }
            // value
            var valueAxis = new AmCharts.ValueAxis();



            if (array.GraphType == "MultiStackedColumnChart" || array.GraphType == "2BarChart2LineAxisMultiStackChart") {

                valueAxis.stackType = "regular";
                valueAxis.gridAlpha = 0.1;
                valueAxis.axisAlpha = 0;
                valueAxis.title = array.GraphY1AxisTitle;
                valueAxis.titleBold = false;
            }
            else if (array.GraphType == "3Dstackedcolumnchart") {


                valueAxis.stackType = "3d"; // This line makes chart 3D stacked (columns are placed one behind another)
                valueAxis.gridAlpha = 0.2;
                valueAxis.axisColor = "#DADADA";
                valueAxis.axisAlpha = 1;
                valueAxis.dashLength = 5;

            }
            chart.addValueAxis(valueAxis);


            var YAxisLength = array.lstYAxis.length;
            // var div = Math.floor(YAxisLength / 2);
            var div = array.lstYAxis.length;
            if (div > 0) {

                for (i = 0; i <= div - 1; i++) {
                    var graph = new AmCharts.AmGraph();
                    var Identifierflag = false;
                    if (array.lstYAxis[i] == "Y1") {
                        graph.title = array.GraphY1Title;
                        graph.labelText = "[[percents]]%";
                        graph.balloonText = "[[title]], [[category]]<br><span style='font-size:14px;'><b>[[value]]</b> ([[percents]]%)</span>";
                        graph.valueField = array.GraphY1ValueField;

                       // Y1Colour = Colors.random();
                        // graph.lineColor = Y1Colour.rgb;
                        Y1Colour = "#EC0092";
                        graph.lineColor = "#EC0092";
                        Identifierflag = true;
                    }

                    if (array.lstYAxis[i] == "Y2") {
                        graph.title = array.GraphY2Title;
                        graph.labelText = "[[percents]]%";
                        graph.balloonText = "[[title]], [[category]]<br><span style='font-size:14px;'><b>[[value]]</b> ([[percents]]%)</span>";
                        graph.valueField = array.GraphY2ValueField;
                      //  Y2Colour = Colors.random();
                        // graph.lineColor = Y2Colour.rgb;
                        Y2Colour = "#50B848";
                        graph.lineColor = "#50B848";
                        Identifierflag = true;
                    }

                    if (array.lstYAxis[i] == "Y3") {
                        graph.title = array.GraphY3Title;
                        graph.labelText = "[[percents]]%";
                        graph.balloonText = "[[title]], [[category]]<br><span style='font-size:14px;'><b>[[value]]</b> ([[percents]]%)</span>";
                        graph.valueField = array.GraphY3ValueField;
                       // Y3Colour = Colors.random();
                        // graph.lineColor = Y3Colour.rgb;
                        Y3Colour = "#F15A22";
                        graph.lineColor = "#F15A22";
                        Identifierflag = true;
                    }

                    if (array.lstYAxis[i] == "Y4") {
                        graph.title = array.GraphY4Title;
                        graph.labelText = "[[percents]]%";
                        graph.balloonText = "[[title]], [[category]]<br><span style='font-size:14px;'><b>[[value]]</b> ([[percents]]%)</span>";
                        graph.valueField = array.GraphY4ValueField;
                       // Y4Colour = Colors.random();
                        // graph.lineColor = Y4Colour.rgb;
                        Y4Colour = "#AB218E";
                        graph.lineColor = "#AB218E";
                        Identifierflag = true;
                    }

                    if (array.lstYAxis[i] == "Y5") {
                        graph.title = array.GraphY5Title;
                        graph.labelText = "[[percents]]%";
                        graph.balloonText = "[[title]], [[category]]<br><span style='font-size:14px;'><b>[[value]]</b> ([[percents]]%)</span>";
                        graph.valueField = array.GraphY5ValueField;
                       // Y5Colour = Colors.random();
                        // graph.lineColor = Y5Colour.rgb;
                        Y5Colour = "#696A6D";
                        graph.lineColor = "#696A6D";
                        Identifierflag = true;
                    }
                    if (array.lstYAxis[i] == "Y6") {
                        graph.title = array.GraphY6Title;
                        graph.labelText = "[[percents]]%";
                        graph.balloonText = "[[title]], [[category]]<br><span style='font-size:14px;'><b>[[value]]</b> ([[percents]]%)</span>";
                        graph.valueField = array.GraphY6ValueField;
                      //  Y6Colour = Colors.random();
                        //  graph.lineColor = Y6Colour.rgb;
                        Y6Colour = "#ff6347";
                        graph.lineColor = "#ff6347";
                        Identifierflag = true;
                    }
                    if (Identifierflag == true) {
                        graph.type = "column";
                        graph.showHandOnHover = true;
                        graph.lineAlpha = 0;
                        graph.fillAlphas = 1;
                        //  graph.lineColor = "#C72C95";

                        chart.addGraph(graph);
                    }
                }





                if (array.NumAxis == 2) {
                    // value
                    var valueAxis2 = new AmCharts.ValueAxis();
                    valueAxis2.stackType = "regular";
                    valueAxis2.gridAlpha = 0.1;
                    valueAxis2.axisAlpha = 0;
                    valueAxis2.position = "right";

                    chart.addValueAxis(valueAxis2);


                    if (array.NumAxis == 2 && array.lstLineAxis != undefined && array.lstLineAxis.length > 0) {


                        for (i = 0; i <= div - 1; i++) {
                            var graph = new AmCharts.AmGraph();
                            var Identifierflag = false;
                            if (array.lstYAxis[i] == "Y5" && $.inArray("Y5", array.lstLineAxis) == -1) {
                                graph.title = array.GraphY5Title;
                                graph.labelText = "[[percents]]%";
                                graph.balloonText = "[[title]], [[category]]<br><span style='font-size:14px;'><b>[[value]]</b> ([[percents]]%)</span>";
                                graph.valueField = array.GraphY5ValueField;
                                if (array.StackSize != 3) {

                                    //graph.lineColor = Y1Colour.rgb;
                                    graph.lineColor = Y1Colour;
                                }
                                else {
                                   // var RanClr = Colors.random()
                                    // graph.lineColor = RanClr.rgb;
                                    graph.lineColor = "#e9967a";
                                    graph.stackable = false;
                                    valueAxis.position = "left";

                                }
                                Identifierflag = true;
                            }
                            else if (array.lstYAxis[i] == "Y5" && $.inArray("Y5", array.lstLineAxis) == 0) {

                                graph.type = "line";
                                graph.title = array.GraphY5Title;
                              //  var RanClr = Colors.random()
                               // graph.lineColor = RanClr.rgb;
                                graph.lineColor = "#8fbc8f";
                                graph.valueField = array.GraphY5ValueField;
                                graph.lineThickness = 3;
                                graph.bullet = "round";
                                graph.bulletBorderThickness = 3;
                                graph.bulletBorderColor = "#fcd202";
                                graph.bulletBorderAlpha = 1;
                                graph.bulletColor = "#ffffff";
                                graph.dashLengthField = "dashLengthLine";
                                graph.valueAxis = valueAxis2;
                                graph.balloonText = "[[title]], [[category]]<br><span style='font-size:14px;'><b>[[value]]</b></span>";
                                chart.addGraph(graph);

                            }
                            if (array.lstYAxis[i] == "Y6" && $.inArray("Y6", array.lstLineAxis) == -1) {
                                graph.title = array.GraphY6Title;
                                graph.labelText = "[[percents]]%";
                                graph.balloonText = "[[title]], [[category]]<br><span style='font-size:14px;'><b>[[value]]</b>([[percents]]%) </span>";
                                graph.valueField = array.GraphY6ValueField;
                                if (array.StackSize != 3) {

                                    // graph.lineColor = Y2Colour.rgb;
                                    graph.lineColor = Y2Colour;

                                }
                                else {
                                   // var RanClr = Colors.random()
                                    //graph.lineColor = RanClr.rgb;
                                    graph.lineColor = "#483d8b";
                                    valueAxis.position = "left";
                                    graph.stackable = false;

                                }
                                Identifierflag = true;
                            }

                            else if (array.lstYAxis[i] == "Y6" && $.inArray("Y6", array.lstLineAxis) == 0) {

                                graph.type = "line";
                                graph.title = array.GraphY6Title;
                              //  var RanClr = Colors.random()
                               // graph.lineColor = RanClr.rgb;
                                // graph.lineColor = "#fcd202";
                                graph.lineColor = "#483d8b";
                                graph.valueField = array.GraphY6ValueField;
                                graph.lineThickness = 3;
                                graph.bullet = "round";
                                graph.bulletBorderThickness = 3;
                                graph.bulletBorderColor = "#fcd202";
                                graph.bulletBorderAlpha = 1;
                                graph.bulletColor = "#ffffff";
                                graph.dashLengthField = "dashLengthLine";
                                graph.valueAxis = valueAxis2;
                                graph.balloonText = "[[title]], [[category]]<br><span style='font-size:14px;'><b>[[value]]</b></span>";
                                chart.addGraph(graph);

                            }

                            if (array.lstYAxis[i] == "Y7" && $.inArray("Y7", array.lstLineAxis) == -1) {
                                graph.title = array.GraphY7Title;
                                graph.labelText = "[[percents]]%";
                                graph.balloonText = "[[title]], [[category]]<br><span style='font-size:14px;'><b>[[value]]</b>([[percents]]%)</span>";
                                graph.valueField = array.GraphY7ValueField;
                                if (array.StackSize != 3) {
                                    // graph.lineColor = Y3Colour.rgb;
                                    graph.lineColor = Y3Colour;
                                }
                                else {
                                   // var RanClr = Colors.random()
                                    // graph.lineColor = RanClr.rgb;
                                    graph.lineColor = "#00ced1";
                                    valueAxis.position = "left";
                                    graph.stackable = false;

                                }
                                Identifierflag = true;
                            }
                            else if (array.lstYAxis[i] == "Y7" && $.inArray("Y7", array.lstLineAxis) == 0) {

                                graph.type = "line";
                                graph.title = array.GraphY7Title;
                                graph.lineColor = "#00ced1";
                              //  var RanClr = Colors.random()
                               // graph.lineColor = RanClr.rgb;
                                // graph.lineColor = "#fcd202";
                                graph.valueField = array.GraphY7ValueField;
                                graph.lineThickness = 3;
                                graph.bullet = "round";
                                graph.bulletBorderThickness = 3;
                                graph.bulletBorderColor = "#fcd202";
                                graph.bulletBorderAlpha = 1;
                                graph.bulletColor = "#ffffff";
                                graph.dashLengthField = "dashLengthLine";
                                graph.valueAxis = valueAxis2;
                                graph.balloonText = "[[title]], [[category]]<br><span style='font-size:14px;'><b>[[value]]</b> </span>";
                                chart.addGraph(graph);

                            }
                            if (array.lstYAxis[i] == "Y8" && $.inArray("Y8", array.lstLineAxis) == -1) {
                                graph.title = array.GraphY8Title;
                                graph.labelText = "[[percents]]%";
                                graph.balloonText = "[[title]], [[category]]<br><span style='font-size:14px;'><b>[[value]]</b>([[percents]]%) </span>";
                                graph.valueField = array.GraphY8ValueField;
                                if (array.StackSize != 3) {
                                   // graph.lineColor = Y4Colour.rgb;
                                    graph.lineColor = Y4Colour;
                                }
                                else {
                                   // var RanClr = Colors.random()
                                    // graph.lineColor = RanClr.rgb;
                                    graph.lineColor = "#1e90ff";
                                    valueAxis.position = "left";
                                    graph.stackable = false;

                                }
                                Identifierflag = true;
                            }

                            else if (array.lstYAxis[i] == "Y8" && $.inArray("Y8", array.lstLineAxis) == 0) {

                                graph.type = "line";
                                graph.title = array.GraphY8Title;
                              //  var RanClr = Colors.random()
                                //  graph.lineColor = RanClr.rgb;
                                graph.lineColor = "#1e90ff";
                                // graph.lineColor = "#fcd202";
                                graph.valueField = array.GraphY8ValueField;
                                graph.lineThickness = 3;
                                graph.bullet = "round";
                                graph.bulletBorderThickness = 3;
                                graph.bulletBorderColor = "#fcd202";
                                graph.bulletBorderAlpha = 1;
                                graph.bulletColor = "#ffffff";
                                graph.dashLengthField = "dashLengthLine";
                                graph.valueAxis = valueAxis2;
                                graph.balloonText = "[[title]], [[category]]<br><span style='font-size:14px;'><b>[[value]]</b></span>";
                                chart.addGraph(graph);

                            }
                            // k = k + 1;
                            if (Identifierflag == true) {
                                graph.valueAxis = valueAxis;
                                graph.type = "column";
                                graph.lineAlpha = 0;
                                graph.fillAlphas = 1;
                                graph.showHandOnHover = true;
                                //  graph.lineColor = "#C72C95";
                                //  graph.stackable = false;
                                //var color = Colors.random();
                                //graph.lineColor = color.rgb;
                                chart.addGraph(graph);
                            }
                        }







                    }

                    else {







                        for (i = 0; i <= div - 1; i++) {
                            var graph = new AmCharts.AmGraph();
                            var Identifierflag = false;
                            if (array.lstYAxis[i] == "Y5") {
                                graph.title = array.GraphY5Title;
                                graph.labelText = "[[percents]]%";
                                graph.balloonText = "[[title]], [[category]]<br><span style='font-size:14px;'><b>[[value]]</b> ([[percents]]%)</span>";
                                graph.valueField = array.GraphY5ValueField;
                                if (array.StackSize != 3) {
                                    if (array.NonComparisionAllCase == true) {

                                       // var RanClr = Colors.random()
                                        //  graph.lineColor = RanClr.rgb;
                                        graph.lineColor = "#e9967a";
                                    }
                                    else {
                                       // graph.lineColor = Y1Colour.rgb;
                                        graph.lineColor = Y1Colour;
                                    }
                                }
                                else {
                                   // var RanClr = Colors.random()
                                    //  graph.lineColor = RanClr.rgb;
                                    graph.lineColor = "#e9967a";

                                }
                                Identifierflag = true;
                            }

                            if (array.lstYAxis[i] == "Y6") {
                                graph.title = array.GraphY6Title;
                                graph.labelText = "[[percents]]%";
                                graph.balloonText = "[[title]], [[category]]<br><span style='font-size:14px;'><b>[[value]]</b> ([[percents]]%)</span>";
                                graph.valueField = array.GraphY6ValueField;
                                if (array.StackSize != 3) {
                                    if (array.NonComparisionAllCase == true) {

                                       // var RanClr = Colors.random()
                                        // graph.lineColor = RanClr.rgb;
                                        graph.lineColor = "#8fbc8f";
                                    }
                                    else {
                                        // graph.lineColor = Y2Colour.rgb;
                                        graph.lineColor = Y2Colour;
                                    }
                                }
                                else {
                                   // var RanClr = Colors.random()
                                    // graph.lineColor = RanClr.rgb;
                                    graph.lineColor = "#8fbc8f";

                                }
                                Identifierflag = true;
                            }

                            if (array.lstYAxis[i] == "Y7") {
                                graph.title = array.GraphY7Title;
                                graph.labelText = "[[percents]]%";
                                graph.balloonText = "[[title]], [[category]]<br><span style='font-size:14px;'><b>[[value]]</b> ([[percents]]%)</span>";
                                graph.valueField = array.GraphY7ValueField;
                                if (array.StackSize != 3) {
                                    if (array.NonComparisionAllCase == true) {

                                       // var RanClr = Colors.random()
                                        // graph.lineColor = RanClr.rgb;
                                        graph.lineColor ="#483d8b";
                                    }
                                    else {
                                        // graph.lineColor = Y3Colour.rgb;
                                        graph.lineColor =Y3Colour;
                                    }

                                }
                                else {
                                   // var RanClr = Colors.random()
                                    //graph.lineColor = RanClr.rgb;
                                    graph.lineColor = "#483d8b";

                                }
                                Identifierflag = true;
                            }

                            if (array.lstYAxis[i] == "Y8") {
                                graph.title = array.GraphY8Title;
                                graph.labelText = "[[percents]]%";
                                graph.balloonText = "[[title]], [[category]]<br><span style='font-size:14px;'><b>[[value]]</b> ([[percents]]%)</span>";
                                graph.valueField = array.GraphY8ValueField;
                                if (array.StackSize != 3) {

                                    if (array.NonComparisionAllCase == true) {

                                       // var RanClr = Colors.random()
                                        // graph.lineColor = RanClr.rgb;
                                        graph.lineColor = "#00ced1";
                                    }
                                    else {
                                        // graph.lineColor = Y4Colour.rgb;
                                        graph.lineColor = Y4Colour;
                                    }
                                }
                                else {
                                   // var RanClr = Colors.random()
                                    //  graph.lineColor = RanClr.rgb;
                                    graph.lineColor = "#00ced1";

                                }
                                Identifierflag = true;
                            }
                            // k = k + 1;
                            if (Identifierflag == true) {
                                graph.valueAxis = valueAxis2;
                                graph.type = "column";
                                graph.lineAlpha = 0;
                                graph.fillAlphas = 1;
                                graph.showHandOnHover = true;
                                //  graph.lineColor = "#C72C95";
                                //  graph.stackable = false;
                                //var color = Colors.random();
                                //graph.lineColor = color.rgb;
                                chart.addGraph(graph);
                            }
                        }
                    }

                }


            }


            //// LEGEND                  
            var legend = new AmCharts.AmLegend();
            legend.borderAlpha = 0.2;
            legend.useGraphSettings = true;
            legend.valueWidth = 0;
            legend.horizontalGap = 10;
            chart.addLegend(legend);

            //  chart.addLabel(110,10,'Total Cost of Loan : ',\"left\",12,\"#333333\",0,\"\",\"true\");
            // chart.addTitle("Net profit margin comparison against three peers and industry median", 12);

            //  chart.addLabel("Test");


        }

            //else if (array.GraphType != null && array.GraphType == "3Dstackedcolumnchart")
            //{
            //    chart = new AmCharts.AmSerialChart();
            //    chart.dataProvider = chartData;
            //    chart.categoryField = GraphX1ValueField;
            //    chart.fontSize = 14;
            //    chart.startDuration = 1;
            //    chart.plotAreaFillAlphas = 0.2;
            //    // the following two lines makes chart 3D
            //    chart.angle = 30;
            //    chart.depth3D = 40;

            //    // AXES
            //    // category
            //    var categoryAxis = chart.categoryAxis;
            //    categoryAxis.gridAlpha = 0.2;
            //    categoryAxis.gridPosition = "start";
            //    categoryAxis.axisColor = "#DADADA";
            //    categoryAxis.axisAlpha = 1;
            //    categoryAxis.dashLength = 5;

            //    // value
            //    var valueAxis = new AmCharts.ValueAxis();
            //    valueAxis.stackType = "3d"; // This line makes chart 3D stacked (columns are placed one behind another)
            //    valueAxis.gridAlpha = 0.2;
            //    valueAxis.axisColor = "#DADADA";
            //    valueAxis.axisAlpha = 1;
            //    valueAxis.dashLength = 5;
            //   // valueAxis.unit = "%";
            //    chart.addValueAxis(valueAxis);


            //    // GRAPHS       
            //        var div = array.lstYAxis.length;
            //        if (div > 0) {

            //            for (i = 0; i <= div - 1; i++) {
            //                var graph = new AmCharts.AmGraph();
            //                var Identifierflag = false;
            //                if (array.lstYAxis[i] == "Y1") {
            //                    graph.title = array.GraphY1Title;
            //                    graph.labelText = "[[percents]]%";
            //                    graph.balloonText = "[[title]], [[category]]<br><span style='font-size:14px;'><b>[[value]]</b> ([[percents]]%)</span>";
            //                    graph.valueField = array.GraphY1ValueField;
            //                    Y1Colour = Colors.random();
            //                    graph.lineColor = Y1Colour.rgb;
            //                    Identifierflag = true;
            //                }

            //                if (array.lstYAxis[i] == "Y2") {
            //                    graph.title = array.GraphY2Title;
            //                    graph.labelText = "[[percents]]%";
            //                    graph.balloonText = "[[title]], [[category]]<br><span style='font-size:14px;'><b>[[value]]</b> ([[percents]]%)</span>";
            //                    graph.valueField = array.GraphY2ValueField;
            //                    Y2Colour = Colors.random();
            //                    graph.lineColor = Y2Colour.rgb;
            //                    Identifierflag = true;
            //                }

            //                if (array.lstYAxis[i] == "Y3") {
            //                    graph.title = array.GraphY3Title;
            //                    graph.labelText = "[[percents]]%";
            //                    graph.balloonText = "[[title]], [[category]]<br><span style='font-size:14px;'><b>[[value]]</b> ([[percents]]%)</span>";
            //                    graph.valueField = array.GraphY3ValueField;
            //                    Y3Colour = Colors.random();
            //                    graph.lineColor = Y3Colour.rgb;
            //                    Identifierflag = true;
            //                }

            //                if (array.lstYAxis[i] == "Y4") {
            //                    graph.title = array.GraphY4Title;
            //                    graph.labelText = "[[percents]]%";
            //                    graph.balloonText = "[[title]], [[category]]<br><span style='font-size:14px;'><b>[[value]]</b> ([[percents]]%)</span>";
            //                    graph.valueField = array.GraphY4ValueField;
            //                    Y4Colour = Colors.random();
            //                    graph.lineColor = Y4Colour.rgb;
            //                    Identifierflag = true;
            //                }
            //                if (Identifierflag == true) {
            //                    graph.type = "column";
            //                    graph.lineAlpha = 0;
            //                    graph.fillAlphas = 1;
            //                    //  graph.lineColor = "#C72C95";

            //                    chart.addGraph(graph);
            //                }
            //            }
            //        }

            //    //// LEGEND                  
            //        var legend = new AmCharts.AmLegend();
            //        legend.borderAlpha = 0.2;
            //        legend.valueWidth = 0;
            //        legend.horizontalGap = 10;
            //        chart.addLegend(legend);

            //}


        else if (array.GraphType != null && array.GraphType == "LineChart") {
            chart = new AmCharts.AmSerialChart();
            AddTitle(chart, array.ChartName);
            chart.dataProvider = chartData;
            chart.categoryField = GraphX1ValueField;
            var graph = new AmCharts.AmGraph();
            graph.title = array.GraphY1Title;//"NoofPolicies";
            graph.valueField = array.GraphY1ValueField;//"NoofPolicies";
            graph.type = "line";
            graph.balloonText = array.GraphY1Title + " " + "[[category]]:[[value]]%";

            graph.lineColor = "#5fb503",
             graph.negativeLineColor = "#efcc26",
            //  graph.lineAlpha = 0;
            graph.lineThickness = 2;
            graph.bullet = "round";
            var categoryAxis = chart.categoryAxis;

            categoryAxis.title = array.GraphX1Title;
            categoryAxis.autoWrap = true;
            graph.showHandOnHover = true;
          //  var Y1Colour = Colors.random();
            // graph.fillColors = Y1Colour.rgb;// "#bf1c25";
            // graph.fillAlphas = 1;
            chart.addGraph(graph);

            var valueAxis = new AmCharts.ValueAxis();
            valueAxis.axisColor = "#DADADA";
            // valueAxis.title = "Income in millions, USD";
            valueAxis.gridAlpha = 0.1;
            valueAxis.title = array.GraphY1AxisTitle;
            valueAxis.titleBold = false;
            chart.addValueAxis(valueAxis);

            var legend = new AmCharts.AmLegend();
            legend.borderAlpha = 0.2;
            legend.useGraphSettings = true;
            legend.valueWidth = 0;
            legend.horizontalGap = 10;
            chart.addLegend(legend);

            // chart = new AmCharts.AmSerialChart();

            // chart.categoryField = GraphX1ValueField;
            // chart.dataProvider = chartData;
            // chart.startDuration= 1;
            //// chart.categoryField = "date";
            // chart.balloon.bulletSize = 5;
            // var categoryAxis = chart.categoryAxis;
            // categoryAxis.gridPosition = "start";
            //// categoryAxis.labelRotation = 90;
            // if (array.GraphX1Title != null) {
            //     categoryAxis.title = array.GraphX1Title;
            // }
            // categoryAxis.dashLength = 1;

            if (array.DrilDown == true) {
                chart.addListener("clickGraphItem", function (e) {

                    DrillDownClick(e);

                });

            }


        }

        else if (array.GraphType != null && array.GraphType == "3DBarChart2LineAxisChart" || array.GraphType != null && array.GraphType == "3DBarChart1LineAxisChart" || array.GraphType != null && array.GraphType == "3DBarChart2StepLineAxisChart" || array.GraphType != null && array.GraphType == "2BarChart2LineAxisChart" || array.GraphType != null && array.GraphType == "4BarChart" || array.GraphType != null && array.GraphType == "3BarChart" || array.GraphType != null && array.GraphType == "2BarChart") {

            // SERIAL CHART
            chart = new AmCharts.AmSerialChart();
            chart.dataProvider = chartData;
            chart.categoryField = GraphX1ValueField;
            AddTitle(chart, array.ChartName);

            // if (array.GraphType != "3DBarChart2StepLineAxisChart") {
            chart.depth3D = 20;
            chart.angle = 30;
            // }
            // AXES
            // Category
            if (array.DrilDown == true) {
                if (array.DrilDownCategory == "PolicyIssuanceGraph1") {
                    // chart.addListener("clickGraphItem", handleClickOfChart);//----------click event of the graph-----------------------//
                    // chart.addListener("clickGraphItem", PolicyIssuanceGraph1);

                    chart.addListener("clickGraphItem", function (e) {

                        PolicyIssuanceGraph1(e);

                    });


                }
                else {
                    chart.addListener("clickGraphItem", function (e) {

                        //  handleClickOfChart(e);
                        DrillDownClick(e);

                    });
                }
            }

            var categoryAxis = chart.categoryAxis;
            categoryAxis.gridPosition = "start";
            categoryAxis.axisColor = "#DADhDA";
            categoryAxis.fillAlpha = 1;
            categoryAxis.gridAlpha = 0;
            categoryAxis.fillColor = "#FAFAFA";
            categoryAxis.title = array.GraphX1AxisField;
            categoryAxis.autoWrap = true;
            if ($("#" + ParentDiv).width() <= $(window).width() * (3 / 4) && chart.dataProvider.length > 7) {
                categoryAxis.labelRotation = 45;
            }
            // value
            var valueAxis = new AmCharts.ValueAxis();
            valueAxis.axisColor = "#DADADA";
            // valueAxis.title = "Income in millions, USD";
            valueAxis.gridAlpha = 0.1;
            valueAxis.title = array.GraphY1AxisTitle;
            valueAxis.titleBold = false;
            chart.addValueAxis(valueAxis);

            // GRAPH
            var graph = new AmCharts.AmGraph();
            graph.title = array.GraphY1Title;//"NoofPolicies";
            graph.valueField = array.GraphY1ValueField;//"NoofPolicies";
            graph.type = "column";
            graph.showHandOnHover = true;
            
            graph.balloonText = array.GraphY1Title + " " + "[[category]]:[[value]]";
            // chart.balloonText = "[[title]]<br><span style='font-size:14px'><b>[[value]]</b> ([[percents]]%)</span>";

            graph.lineAlpha = 0;

           // var Y1Colour = Colors.random();
            // graph.fillColors = Y1Colour.rgb;// "#bf1c25";

            graph.fillColors = "#50B848";
            graph.fillAlphas = 1;
            if (chart.dataProvider.length <= 4) {
                chart.columnWidth = 50 / (500 / chart.dataProvider.length);//--------to set the width of the column----------------------------------//
            }
            chart.addGraph(graph);

            //-------other2 graphs-----------------------------------------------------//

            // value
            if (array.RotatedAxis == true) {
                chart.rotate = true;
            }
            if (array.GraphType == "3DBarChart2LineAxisChart" || array.GraphType == "2BarChart2LineAxisChart" || array.GraphType == "2BarChart2LineAxisMultiStackChart") {


                if (array.GraphType == "2BarChart2LineAxisChart") {

                    var graph = new AmCharts.AmGraph();
                    graph.title = array.GraphY4Title;//"NoofPolicies";
                    graph.valueField = array.GraphY4ValueField;//"NoofPolicies";
                    graph.type = "column";
                    graph.balloonText = array.GraphY4Title + " " + "[[category]]:[[value]]";
                    graph.lineAlpha = 0;
                   // var Colour = Colors.random();
                    // graph.fillColors = Colour.rgb;// "#bf1c25";
                    graph.fillColors = "#F15A22";
                    graph.fillAlphas = 1;
                    graph.showHandOnHover = true;
                    if (chart.dataProvider.length <= 4) {
                        chart.columnWidth = 50 / (500 / chart.dataProvider.length);//--------to set the width of the column----------------------------------//
                    }
                    chart.addGraph(graph);


                }
               
                var valueAxis2 = new AmCharts.ValueAxis();
                //  valueAxis2.stackType = "regular";
                valueAxis2.gridAlpha = 0.1;
                valueAxis2.axisAlpha = 0;
                valueAxis2.position = "right";
                // valueAxis2.title = array.GraphY1Title;
                valueAxis2.title = array.GraphY2AxisTitle;
                valueAxis2.titleBold = false;
                chart.addValueAxis(valueAxis2);

                // fourth graph  
              /*  var graph2 = new AmCharts.AmGraph();//Commented by samba not required lines graph
                graph2.type = "line";
                graph2.title = array.GraphY2Title;//"Premium";

               // var Y2Colour = Colors.random();


                // graph2.lineColor = Y2Colour.rgb; //"#fcd202";
                graph2.lineColor = "#AB218E";
                graph2.valueField = array.GraphY2ValueField; //"Premium";
                graph2.lineThickness = 3;
                graph2.bullet = "round";
                graph2.bulletBorderThickness = 3;
                graph2.bulletBorderColor = "#fcd202";
                graph2.bulletBorderAlpha = 1;
                graph2.showHandOnHover = true;
                graph2.bulletColor = "#ffffff";
                graph2.dashLengthField = "dashLengthLine";
                graph2.valueAxis = valueAxis2;
                graph2.balloonText = "<span style='font-size:13px;'>[[title]] in [[category]]:<b>[[value]]</b> [[additional]]</span>";
                chart.addGraph(graph2);*/

                if (array.GraphY3ValueField != null) {
                    var graph3 = new AmCharts.AmGraph();
                    graph3.type = "line";
                    graph3.title = array.GraphY3Title; //"Commission";
                   // var Y3Colour = Colors.random();
                    // graph3.lineColor = Y3Colour.rgb; //"#00FF00";
                    graph3.lineColor = "#696A6D";
                    graph3.valueField = array.GraphY3ValueField//"Commission";
                    graph3.lineThickness = 3;
                    graph3.bullet = "round";
                    graph3.bulletBorderThickness = 3;
                    graph3.bulletBorderColor = "red";
                    graph3.bulletBorderAlpha = 1;
                    graph3.bulletColor = "#ffffff";
                    graph3.showHandOnHover = true;
                    graph3.dashLengthField = "dashLengthLine";
                    graph3.valueAxis = valueAxis2;
                    graph3.balloonText = "<span style='font-size:13px;'>[[title]] in [[category]]:<b>[[value]]</b> [[additional]]</span>";
                    chart.addGraph(graph3);
                }
            }
            else if (array.GraphType == "3DBarChart2StepLineAxisChart") {

                var graph1 = new AmCharts.AmGraph();
                graph1.valueField = array.GraphY2ValueField;// "visits1";
                graph1.title = array.GraphY2Title;
                graph1.balloonText = "[[category]]: [[value]]";
                graph1.type = "column";
                graph1.lineAlpha = 0;
                graph1.fillAlphas = 0.8;
                graph1.showHandOnHover = true;
              //  var Y2Colour = Colors.random();
                // graph.fillColors = Y2Colour.rgb//"#a52a2a";
                graph1.fillColors = "#F15A22";
                chart.addGraph(graph1);


            }


            else if (array.GraphType == "3DBarChart1LineAxisChart") {
                // fourth graph  
                var graph2 = new AmCharts.AmGraph();
                graph2.type = "line";
                graph2.title = array.GraphY2Title;//"Premium";
               // var Y2Colour = Colors.random();


                //  graph2.lineColor = Y2Colour.rgb; //"#fcd202";
                graph2.lineColor = "#F15A22";
                graph2.valueField = array.GraphY2ValueField; //"Premium";
                graph2.lineThickness = 3;
                graph2.bullet = "round";
                graph2.bulletBorderThickness = 3;
                graph2.bulletBorderColor = "#fcd202";
                graph2.bulletBorderAlpha = 1;
                graph2.bulletColor = "#ffffff";
                graph2.dashLengthField = "dashLengthLine";
                graph2.valueAxis = valueAxis2;
                graph2.showHandOnHover = true;
                graph2.balloonText = "<span style='font-size:13px;'>[[title]] in [[category]]:<b>[[value]]</b> [[additional]]</span>";
                chart.addGraph(graph2);
            }

            else if (array.GraphType == "2BarChart" || array.GraphType == "3BarChart" || array.GraphType == "4BarChart") {
                var graph2 = new AmCharts.AmGraph();
                graph2.title = array.GraphY2Title;
                graph2.valueField = array.GraphY2ValueField;
                graph2.type = "column";
                graph2.balloonText = array.GraphY2Title + " " + "[[category]]:[[value]]";
                graph2.lineAlpha = 0;
                graph2.showHandOnHover = true;
              //  var Colour = Colors.random();
                // graph2.fillColors = Colour.rgb;// "#bf1c25";
                graph2.fillColors = "#F15A22";
                graph2.fillAlphas = 1;
                if (chart.dataProvider.length <= 4) {
                    chart.columnWidth = 50 / (500 / chart.dataProvider.length);//--------to set the width of the column----------------------------------//
                }
                chart.addGraph(graph2);
                if (array.GraphType == "3BarChart" || array.GraphType == "4BarChart") {
                    var graph3 = new AmCharts.AmGraph();
                    graph3.title = array.GraphY3Title;//"NoofPolicies";
                    graph3.valueField = array.GraphY3ValueField;//"NoofPolicies";
                    graph3.type = "column";
                    graph3.balloonText = array.GraphY3Title + " " + "[[category]]:[[value]]";
                    graph3.lineAlpha = 0;
                    graph3.showHandOnHover = true;
                   // var Colour = Colors.random();
                    // graph3.fillColors = Colour.rgb;// "#bf1c25";
                    graph3.fillColors = "#AB218E";
                    graph3.fillAlphas = 1;
                    if (chart.dataProvider.length <= 4) {
                        chart.columnWidth = 50 / (500 / chart.dataProvider.length);//--------to set the width of the column----------------------------------//
                    }
                    chart.addGraph(graph3);
                }
                if (array.GraphType == "4BarChart") {

                    var graph4 = new AmCharts.AmGraph();
                    graph4.title = array.GraphY4Title;//"NoofPolicies";
                    graph4.valueField = array.GraphY4ValueField;//"NoofPolicies";
                    graph4.type = "column";
                    graph4.balloonText = array.GraphY4Title + " " + "[[category]]:[[value]]";
                    graph4.lineAlpha = 0;
                   // var Colour = Colors.random();
                    // graph4.fillColors = Colour.rgb;// "#bf1c25";
                    graph4.fillColors = "#696A6D";
                    graph4.fillAlphas = 1;
                    graph4.showHandOnHover = true;
                    if (chart.dataProvider.length <= 4) {
                        chart.columnWidth = 50 / (500 / chart.dataProvider.length);//--------to set the width of the column----------------------------------//
                    }
                    chart.addGraph(graph4);
                }
            }
            //// LEGEND                  
            var legend = new AmCharts.AmLegend();
            legend.useGraphSettings = true;
            legend.borderAlpha = 0.2;
            legend.valueWidth = 0;
            legend.horizontalGap = 10;

            if (array.LegendPosition == "right") {
                legend.position = array.LegendPosition;

            }
            chart.addLegend(legend);
        }



        // WRITE
        if (DivId != "") {
            chart.write(DivId);

        }
        else {
            return;
        }
    }
}
function AddTitle(chart, Name) {
    //if (Name != undefined && Name != null) {
    //    chart.addTitle(Name, 12, "black");
    //}
}



