using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;

namespace ProductManagement.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //Angular JS
            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                 "~/Scripts/angular.js"
            ));

            //Custom
            bundles.Add(new ScriptBundle("~/bundles/productManagement")
                .Include("~/ClientApp/array.extensions.js")
                .Include("~/ClientApp/contract.js")
                .Include("~/ClientApp/dataAccess.service.js")
                .Include("~/ClientApp/productItem.component.js")
                .Include("~/ClientApp/product.controller.js")
                .Include("~/ClientApp/product.app.js")
            );

            bundles.Add(new StyleBundle("~/Content/productstyle")
                .Include("~/Content/product.css")
                );
        }
    }
}