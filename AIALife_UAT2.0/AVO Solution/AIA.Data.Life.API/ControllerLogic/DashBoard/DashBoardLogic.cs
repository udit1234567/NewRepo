using AIA.Life.Repository.AIAEntity;
using AIA.Life.Models.DashBoard;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIA.Data.Life.API.ControllerLogic.DashBoard
{
    public class DashBoardLogic
    {

        AVOAIALifeEntities Context = new AVOAIALifeEntities();
        public GraphDetails GenerateDashboardData( GraphDetails objGraphDetails)
        {
            objGraphDetails.UserId = Context.tblUserDetails.Where(a => a.LoginID == objGraphDetails.UserName).Select(a => a.UserID).FirstOrDefault() ?? new Guid();
            GetGraphDetails(ref objGraphDetails);
            GetDataProvider(ref objGraphDetails);
            GetGraphs(ref objGraphDetails);
            return objGraphDetails;
        }
        private void GetGraphDetails(ref GraphDetails objGraphDetails)
        {
            
            using (AVOAIALifeEntities entity = new AVOAIALifeEntities())
            {
                int reportID = objGraphDetails.ReportID;
                var graphDetails = entity.tblDashboardReports.Where(a => a.ReportID == reportID).FirstOrDefault();
                objGraphDetails.ReportName = graphDetails.ReportTitle;
                objGraphDetails.Theme = graphDetails.Theme;
                objGraphDetails.Type = graphDetails.Type;
                //objGraphDetails.CategoryField = graphDetails.YAxisTitle;
                objGraphDetails.StartDuration = Convert.ToInt32(graphDetails.StartDuration);
                objGraphDetails.PlotAreaFillAlphas = graphDetails.PlotAreaFillAlphas ?? 0;
                objGraphDetails.GraphHeight = graphDetails.Height;
                objGraphDetails.GraphWidth = graphDetails.Width;
                objGraphDetails.IsGrowth = graphDetails.IsGrowth ?? false;
                objGraphDetails.Rotate = graphDetails.Rotate ?? false;
                objGraphDetails.valueAxes.position = graphDetails.VAPosition;
                objGraphDetails.valueAxes.labelsEnabled = graphDetails.VALabelsEnabled ?? false;
                objGraphDetails.valueAxes.title = graphDetails.ValueAxisTitle;
                objGraphDetails.valueAxes.unit = graphDetails.ValueAxisUnit;
                objGraphDetails.lstvalueAxes.Add(objGraphDetails.valueAxes);
                objGraphDetails.strvalueAxes = JsonConvert.SerializeObject(objGraphDetails.lstvalueAxes);
                objGraphDetails.categoryAxis.gridPosition = "start";
                objGraphDetails.categoryAxis.position = "left";
                objGraphDetails.categoryAxis.axisAlpha = 0;
                objGraphDetails.categoryAxis.tickLength = 0;
                objGraphDetails.categoryAxis.labelRotation = graphDetails.LabelRotation ?? 0;
                objGraphDetails.strcategoryAxis = JsonConvert.SerializeObject(objGraphDetails.categoryAxis);
                objGraphDetails.IsDrillDown = graphDetails.IsDrillDown ?? false;
                objGraphDetails.Depth3D = graphDetails.depth3D != null ? graphDetails.depth3D : 0;
                objGraphDetails.Angle = graphDetails.angle != null ? graphDetails.angle : 0;

            }
        }
        private void GetDataProvider(ref GraphDetails objGraphDetails)
        {
            using (AVOAIALifeEntities entity = new AVOAIALifeEntities())
            {
                int reportID = objGraphDetails.ReportID;
                Guid userId = objGraphDetails.UserId;
                var data = entity.tblDashboardSubReports.Where(a => a.ReportID == reportID && a.UserId == userId).ToList();
                if ( objGraphDetails.IsDrillDown)
                {
                    data = entity.tblDashboardSubReports.Where(a => a.ReportID == reportID && a.ParentId == null && a.UserId == userId
                    ).ToList();
                    foreach (var item in data)
                    {

                        Dictionary<string, string> datap = new Dictionary<string, string>();
                        objGraphDetails.CategoryField = item.YAxisTitle;
                        datap.Add(item.YAxisTitle, item.YAxisValue1);
                        datap.Add(item.XAxisTitle1, item.XAxisValue1);
                        if (!string.IsNullOrEmpty(item.ColorCodeTitle1))
                        {
                            datap.Add(item.ColorCodeTitle1, item.ColorCode1);
                        }
                        if (!string.IsNullOrEmpty(item.XAxisTitle2))
                        {
                            datap.Add(item.XAxisTitle2, item.XAxisValue2);
                        }
                        if (!string.IsNullOrEmpty(item.ColorCodeTitle2))
                        {
                            datap.Add(item.ColorCodeTitle2, item.ColorCode2);
                        }
                        if (!string.IsNullOrEmpty(item.XAxisTitle3))
                        {
                            datap.Add(item.XAxisTitle3, item.XAxisValue3);
                        }
                        datap.Add("url", "#");
                        datap.Add("description", "click to drill-down");
                        var Childata = entity.tblDashboardSubReports.Where(a => a.ParentId == item.SubReportID).ToList();
                        if (Childata != null && Childata.Count() > 0)
                        {
                            List<Dictionary<string, string>> ChilDataProvider = new List<Dictionary<string, string>>();
                            foreach (var childitem in Childata)
                            {
                                Dictionary<string, string> Child = new Dictionary<string, string>();
                                Child.Add(childitem.YAxisTitle, childitem.YAxisValue1);
                                Child.Add(childitem.XAxisTitle1, childitem.XAxisValue1);
                                if (!string.IsNullOrEmpty(childitem.ColorCodeTitle1))
                                {
                                    Child.Add(childitem.ColorCodeTitle1, childitem.ColorCode1);
                                }
                                if (!string.IsNullOrEmpty(childitem.XAxisTitle2))
                                {
                                    Child.Add(childitem.XAxisTitle2, childitem.XAxisValue2);
                                }
                                if (!string.IsNullOrEmpty(childitem.ColorCodeTitle2))
                                {
                                    Child.Add(childitem.ColorCodeTitle2, childitem.ColorCode2);
                                }
                                if (!string.IsNullOrEmpty(childitem.XAxisTitle3))
                                {
                                    Child.Add(childitem.XAxisTitle3, childitem.XAxisValue3);
                                }
                                ChilDataProvider.Add(Child);
                            }

                            datap.Add("data", JsonConvert.SerializeObject(ChilDataProvider));
                        }


                        objGraphDetails.DataProvider.Add(datap);
                    }
                }
               
                else
                {
                    foreach (var item in data)
                    {
                        Dictionary<string, string> datap = new Dictionary<string, string>();
                        objGraphDetails.CategoryField = item.YAxisTitle;
                        datap.Add(item.YAxisTitle, item.YAxisValue1);
                        datap.Add(item.XAxisTitle1, item.XAxisValue1);
                        if (!string.IsNullOrEmpty(item.ColorCodeTitle1))
                        {
                            datap.Add(item.ColorCodeTitle1, item.ColorCode1);
                        }
                        if (!string.IsNullOrEmpty(item.XAxisTitle2))
                        {
                            datap.Add(item.XAxisTitle2, item.XAxisValue2);
                        }
                        if (!string.IsNullOrEmpty(item.ColorCodeTitle2))
                        {
                            datap.Add(item.ColorCodeTitle2, item.ColorCode2);
                        }
                        if (!string.IsNullOrEmpty(item.XAxisTitle3))
                        {
                            datap.Add(item.XAxisTitle3, item.XAxisValue3);
                        }
                        objGraphDetails.DataProvider.Add(datap);
                    }
                }
                //if (data.Count > 1 && objGraphDetails.IsGrowth)
                //{
                //    decimal prev = Convert.ToDecimal(data[0].XAxisValue1);
                //    decimal present = Convert.ToDecimal(data[1].XAxisValue1);
                //    growth = GetGrowthRate(prev, present);
                //    if (objGraphDetails.ReportID == 2)
                //        objGraphDetails.Growth = "CI " + growth + "%";
                //    else
                //        objGraphDetails.Growth = "Growth " + growth + "%";
                //    if (!string.IsNullOrEmpty(data[0].XAxisTitle2))
                //    {
                //        decimal prevCII = Convert.ToDecimal(data[0].XAxisValue2);
                //        decimal presentCII = Convert.ToDecimal(data[1].XAxisValue2);
                //        string growthII = GetGrowthRate(prevCII, presentCII);
                //        if (objGraphDetails.ReportID == 2)
                //            objGraphDetails.Growth += "  CII " + growthII + "%";
                //        else
                //            objGraphDetails.Growth += "  Growth " + growthII + "%";
                //    }
                //}

            }

            objGraphDetails.strDataProvider = JsonConvert.SerializeObject(objGraphDetails.DataProvider);
        }
        private void GetGraphs(ref GraphDetails objGraphDetails)
        {
            using (AVOAIALifeEntities entity = new AVOAIALifeEntities())
            {
                int reportID = objGraphDetails.ReportID;
                if (objGraphDetails.IsDrillDown)
                {

                    objGraphDetails.lstGraphs = entity.tblDashboardReportGraphs.Where(a => a.ReportID == reportID).Select(a => new Graphs
                    {
                        balloonText = a.BallonText,
                        fillAlphas = a.FillAlphas ?? 0,
                        title = a.Title,
                        type = a.Type,
                        labelText = a.LabelText,
                        valueField = a.ValueField,

                        lineAlpha = a.LineAplhas ?? 0,
                        fixedColumnWidth = a.FixedColumnWidth ?? 0,

                        //fillColorsField = a.FillColorsField,
                        //bulletColor = a.BulletColor,
                        //bullet = a.Bullet,

                        bulletBorderAlpha = a.BulletBorderAlpha ?? 0,

                        bulletSize = a.BulletSize ?? 0,
                        useLineColorForBulletBorder = a.UseLineColorForBulletBorder ?? false,
                        lineThickness = a.LineThickness ?? 0,

                      dashLengthField= "dashLengthColumn",
                      urlField= "url",
                      alphaField= "alpha"


                    }).ToList();
                }
                else
                {

                    objGraphDetails.lstGraphs = entity.tblDashboardReportGraphs.Where(a => a.ReportID == reportID).Select(a => new Graphs
                    {
                        balloonText = a.BallonText,
                        fillAlphas = a.FillAlphas ?? 0,
                        title = a.Title,
                        type = a.Type,
                        labelText = a.LabelText,
                        valueField = a.ValueField,

                        lineAlpha = a.LineAplhas ?? 0,
                        fixedColumnWidth = a.FixedColumnWidth ?? 0,

                        fillColorsField = a.FillColorsField,
                        bulletColor = a.BulletColor,
                        bullet = a.Bullet,

                        bulletBorderAlpha = a.BulletBorderAlpha ?? 0,

                        bulletSize = a.BulletSize ?? 0,
                        useLineColorForBulletBorder = a.UseLineColorForBulletBorder ?? false,
                        lineThickness = a.LineThickness ?? 0,


                    }).ToList();
                }
                objGraphDetails.graphs = entity.tblDashboardReportGraphs.Where(a => a.ReportID == reportID).Select(a => new Graphs
                {
                    balloonText = a.BallonText,
                    fillAlphas = a.FillAlphas ?? 0,
                    title = a.Title,
                    type = a.Type,
                    labelText = a.LabelText,
                    valueField = a.ValueField,

                    lineAlpha = a.LineAplhas ?? 0,
                    fixedColumnWidth = a.FixedColumnWidth ?? 0,

                    fillColorsField = a.FillColorsField,
                    bulletColor = a.BulletColor,
                    bullet = a.Bullet,

                    bulletBorderAlpha = a.BulletBorderAlpha ?? 0,

                    bulletSize = a.BulletSize ?? 0,
                    useLineColorForBulletBorder = a.UseLineColorForBulletBorder ?? false,
                    lineThickness = a.LineThickness ?? 0,


                }).FirstOrDefault();
            }
            objGraphDetails.strlstGraphs = JsonConvert.SerializeObject(objGraphDetails.lstGraphs);
        }
        
    }
}