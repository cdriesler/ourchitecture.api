﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Rhino.Geometry;
using Newtonsoft.Json;
using Ourchitecture.Api.Protocols.Motley;

namespace Ourchitecture.Api.Protocols.Motley
{
    public enum PraxisStatus { Online, Offline, Depreciated }

    public class MotleyModule : NancyModule
    {
        private Dictionary<string, Func<string, string>> MotleyRoutes { get; } = new Dictionary<string, Func<string, string>>()
        {
            { "vendor", RunVendor },
        };

        private Dictionary<string, PraxisStatus> MotleyStatus { get; } = new Dictionary<string, PraxisStatus>()
        {
            { "vendor", PraxisStatus.Online },
            { "swerve", PraxisStatus.Depreciated },
            { "impact", PraxisStatus.Offline },
        };

        public MotleyModule()
        {
            Get["/motley"] = _ => MotleyManifest();
            Get["/motley/{dialect}"] = parameters => MotleyStatus[parameters.dialect];
            Post["/motley/{dialect}"] = parameters => MotleyRoutes[parameters.dialect](Request.Body.ToString());
        }

        private string MotleyManifest()
        {
            var crv = new LineCurve(Point3d.Origin, new Point3d(1, 1, 1));
            return JsonConvert.SerializeObject(crv);
            
            //return JsonConvert.SerializeObject(MotleyRoutes.Keys.Select(x => x));
        }

        private static string RunVendor(string request)
        {
            return VendorSchema.Solve(new VendorRequest(request)).ToSvg(Vendor.VendorDrawing.DefaultPlan);
        }
    }
}