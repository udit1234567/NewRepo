using System.Web;
using System.Web.Optimization;

namespace AIA.Presentation.AVOLife
{
    public class BundleConfig
    {

        public static void RegisterBundles(BundleCollection bundles)
        {



            bundles.Add(new ScriptBundle("~/bundles/Scripts")
                .Include("~/Scripts/jquery-{version}.js")
                 .Include("~/Scripts/jquery-ui-{version}.js")
                .Include("~/Scripts/jquery.validate*")
                .Include("~/Scripts/modernizr-*")
                .Include("~/Scripts/bootstrap.js", "~/Scripts/respond.js")
                .Include("~/Scripts/jquery.unobtrusive*")
                .Include("~/Scripts/Common.js")
                .Include("~/Scripts/iNube.js")
                .Include("~/Content/Scripts/metronic.js", "~/Content/Scripts/layout.js", "~/Content/Scripts/css3-mediaqueries.js")
                .Include("~/Scripts/Knockout/ko_dataGrid.js")
                .Include("~/Scripts/Knockout/knockout-2.1.0.js")
                 .Include("~/Scripts/moment.js", "~/Scripts/bootstrap-datetimepicker.js")
                 .Include("~/Scripts/typeahead.js").Include("~/Scripts/bootstrap-dialog.js")
                 .Include("~/FG/Scripts/Html5shiv.js", "~/Scripts/Respond.js")
                       .Include("~/Scripts/ChartsScript/ChartScript.js")
                         .Include("~/Scripts/ChartsScript/jquery.amchartExtension.js")
                         .Include("~/Scripts/ChartsScript/jquery.color.js")
                         .Include("~/Scripts/ChartsScript/RandomColours.js")

                );
            bundles.Add(new StyleBundle("~/Content/TreeView").Include("~/Content/TreeView.css"));
            bundles.Add(new ScriptBundle("~/bundles/MvcAjaxScript").Include(
                    "~/Scripts/gridmvc.js")
                  .Include("~/Scripts/gridmvc-ext.js")
                  .Include("~/Scripts/ladda-bootstrap/ladda.js")
                  .Include("~/Scripts/gridmvc.js")
                  .Include("~/Scripts/ladda-bootstrap/spin.js"));

            bundles.Add(new ScriptBundle("~/bundles/GridMVC")
               .Include("~/Scripts/gridmvc.js")
               .Include("~/Scripts/gridmvc-ext.js")
               .Include("~/Scripts/gridmvc.lang.fr.js")
               .Include("~/Scripts/gridmvc.lang.ru.js")
               .Include("~/Scripts/gridmvc.customwidgets.js")
               .Include("~/Scripts/ladda-bootstrap/ladda.js")
               .Include("~/Scripts/ladda-bootstrap/spin.js")
               .Include("~/Scripts/URI.js"));
          

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));


            bundles.Add(new ScriptBundle("~/bundles/AgentonBoardingScripts").Include("~/Scripts/Common.js")
               .Include("~/Scripts/AgentonBoarding/AgentonBoardingScripts.js")
               );
            bundles.Add(new ScriptBundle("~/bundles/Hierarchy").Include("~/Scripts/Common.js")
              .Include("~/Scripts/Hierarchy/Hierarchy.js")
              );
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                       "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/iNube").Include("~/Scripts/iNube.js"));

            bundles.Add(new ScriptBundle("~/bundles/UserManagement")
              .Include("~/Scripts/UserManagement/UserManagement.js"));
            bundles.Add(new StyleBundle("~/Content/AjaxGridCss").Include(               
                       "~/Content/ladda-bootstrap/ladda-themeless.css", "~/Content/Popup.css"));


            bundles.Add(new ScriptBundle("~/bundles/Dialog")
            .Include("~/Scripts/jquery.ui.dialog.js"));
            bundles.Add(new StyleBundle("~/Content/Dialogcss")
            .Include("~/Content/jquery-ui.css"));

            bundles.Add(new ScriptBundle("~/bundles/FGHome").Include("~/Scripts")
                 .Include("~/FG/Scripts/Home/fullcalendar.js").Include("~/FG/Scripts/Home/gcal.js")
                 );


            bundles.Add(new StyleBundle("~/Content/css").Include(
                   "~/Content/bootstrap.css",
                  "~/Content/site.css",
                  "~/Content/bootstrap-datetimepicker.css"));

            bundles.Add(new StyleBundle("~/Content/JSStyles")
                .Include("~/Content/Styles/Layout.css")
              .Include("~/Content/bootstrap.css", "~/Content/bootstrap-theme.css", "~/Content/Styles/font-awesome.css", "~/Content/Styles/simple-line-icons.css", "~/Content/Styles/bootstrap-dialog.css", "~/Content/Styles/bootstrap-dialog.min.css")
            .Include("~/Content/Styles/Components.css", "~/Content/Styles/Grey.css", "~/Content/amcharts_3.21.14/amcharts/plugins/export/export.css")
              .Include("~/Content/Styles/NewLayout.css", "~/Content/Styles/FG-Main.css", "~/Content/Styles/main-styles.css", "~/Content/iControlGrid.css", "~/Content/bootstrap-datetimepicker.css", "~/Content/jquery-ui.css", "~/Content/Gridmvc.css"));

            bundles.Add(new StyleBundle("~/Content/JSLayout").Include("~/Content/Layout.css"));

            bundles.Add(new StyleBundle("~/Content/FGStyles")
                        .Include("~/FG/Content/Layout.css")
                        .Include("~/Content/bootstrap/css/bootstrap-theme.css", "~/Content/bootstrap/css/font-awesome.css", "~/Content/bootstrap/css/simple-line-icons.css", "~/Content/bootstrap/css/bootstrap-dialog.css", "~/Content/bootstrap/css/bootstrap-dialog.css")
                        .Include("~/FG/Content/Components.css", "~/FG/Content/Grey.css")
                        .Include("~/FG/Content/NewLayout.css", "~/FG/Content/FG-Main.css", "~/FG/Content/main-styles.css")
                        .Include("~/Content/iControlGrid.css")
                );

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include("~/Content/bootstrap/bootstrap.css"));


            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                    "~/Content/themes/base/jquery.ui.core.css",
                    "~/Content/themes/base/jquery.ui.resizable.css",
                    "~/Content/themes/base/jquery.ui.selectable.css",
                    "~/Content/themes/base/jquery.ui.accordion.css",
                    "~/Content/themes/base/jquery.ui.autocomplete.css",
                    "~/Content/themes/base/jquery.ui.button.css",
                    "~/Content/themes/base/jquery.ui.dialog.css",
                    "~/Content/themes/base/jquery.ui.slider.css",
                    "~/Content/themes/base/jquery.ui.tabs.css",
                    "~/Content/themes/base/jquery.ui.datepicker.css",
                    "~/Content/themes/base/jquery.ui.progressbar.css",
                    "~/Content/themes/base/jquery.ui.theme.css",
                    "~/Content/themes/base/jquery-ui.css"));





            bundles.Add(new ScriptBundle("~/bundles/Common").Include("~/Scripts/Common.js"));

            bundles.Add(new ScriptBundle("~/bundles/amcharts")
                         .Include("~/Content/amcharts_3.21.14/amcharts/amcharts.js",
                                   "~/Content/amcharts_3.21.14/amcharts/funnel.js",
                                   "~/Content/amcharts_3.21.14/amcharts/gantt.js",
                                   "~/Content/amcharts_3.21.14/amcharts/gauge.js",
                                   "~/Content/amcharts_3.21.14/amcharts/pie.js",
                                   "~/Content/amcharts_3.21.14/amcharts/radar.js",
                                   "~/Content/amcharts_3.21.14/amcharts/serial.js",
                                   "~/Content/amcharts_3.21.14/amcharts/xy.js",
                                   "~/Content/amcharts_3.21.14/amcharts/plugins/export/export.js",
                                   "~/Content/amcharts_3.21.14/amcharts/themes/light.js",
                                   "~/Content/amcharts_3.21.14/amcharts/themes/black.js",
                                   "~/Content/amcharts_3.21.14/amcharts/themes/chalk.js",
                                   "~/Content/amcharts_3.21.14/amcharts/themes/dark.js",
                                   "~/Content/amcharts_3.21.14/amcharts/themes/patterns.js"));

        }
    }
}
