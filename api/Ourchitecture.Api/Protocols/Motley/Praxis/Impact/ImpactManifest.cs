﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rhino.Geometry;


namespace Ourchitecture.Api.Protocols.Motley.Impact
{
    public class ImpactManifest
    {
        //Primitive input
        public Curve PlanarBounds { get; }
        public Curve PrimaryPathCurve { get; }
        public Curve PrimaryMarketCellCurve { get; }
        public List<Curve> PrimaryRuinRegions { get; }
        public int PrimaryMarketQuota { get; }

        //Intermediate input geometry
        public List<Curve> PathSegments { get; set; } = new List<Curve>();
        public List<Curve> MarketCellSegments { get; set; } = new List<Curve>();

        //Raw measurement values
        public List<double> PrimaryPathSegmentLengths { get; set; }
        public List<double> PrimaryPathSegmentSlopes { get; set; }
        public List<double> MarketCellSegmentLengths { get; set; }
        public List<double> MarketCellSegmentSlopes { get; set; }
        public double MarketCellWidth { get; set; }
        public double MarketCellDepth { get; set; }
        public double MarketCellBoundingBoxArea { get; set; }
        public double MarketCellArea { get; set; }
        public double PrecastArchWidth { get; set; }

        //Average values
        public double AverageFromPrimaryPathSegmentLengths { get; set; }
        public double AverageFromPrimaryPathSegmentSlopes { get; set; }
        public double AverageFromMarketCellSegmentLengths { get; set; }
        public double AverageFromMarketCellSegmentSlopes { get; set; }

        //Variance values
        /// <summary>
        /// Standard deviation along path from the line that connects its endpoints.
        /// </summary>
        public double VarianceFromPrimaryPathDeflection { get; set; }
        /// <summary>
        /// Standard deviation of each path segment's length.
        /// </summary>
        public double VarianceFromPrimaryPathSegmentLengths { get; set; }
        /// <summary>
        /// Standard deviation of each path segment's slope.
        /// </summary>
        public double VarianceFromPrimaryPathSegmentSlopes { get; set; }
        /// <summary>
        /// Standard deviation of each cell segment's length.
        /// </summary>
        public double VarianceFromPrimaryMarketCellSegmentLengths { get; set; }
        /// <summary>
        /// Standard deviation of each cell segment's slope.
        /// </summary>
        public double VarianceFromPrimaryMarketCellSegmentSlopes { get; set; }
        /// <summary>
        /// Difference between cell's world coordinates bounding box area and cell's area.
        /// </summary>
        public double VarianceFromPrimaryMarketCellArea { get; set; }

        //Noise values (variance values normalized between 0 and 1 based on praxis qualities)
        public double NoiseFromPrimaryPathDeflectionVariance { get; set; }
        public double NoiseFromPrimaryPathSegmentLengthVariance { get; set; }
        public double NoiseFromPrimaryPathSegmentSlopeVariance { get; set; }
        public double NoiseFromPrimaryMarketCellSegmentLengthVariance { get; set; }
        public double NoiseFromPrimaryMarketCellSegmentSlopeVariance { get; set; }
        public double NoiseFromPrimaryMarketCellAreaVariance { get; set; }

        //Intermediate geometric models
        public List<MemorialRegion> MemorialRegions { get; set; } = new List<MemorialRegion>();
        public List<PrimarySpine> PrimarySpines { get; set; } = new List<PrimarySpine>();
        public List<SecondarySpine> SecondarySpines { get; set; } = new List<SecondarySpine>();
        public List<TertiarySpine> TertiarySpine { get; set; } = new List<TertiarySpine>();

        //Intermediate population statistics
        public int MarketCellsRemaining { get; set; }
        public int MarketCellsPlaced { get; set; }

        //Output geometry
        public List<Curve> Outlines { get; set; } = new List<Curve>();

        public ImpactManifest(ImpactRequest req)
        {
            PlanarBounds = req.ParentRequest.Boundary;
            PrimaryPathCurve = req.ParentRequest.PrimaryPath;
            PrimaryMarketCellCurve = req.ParentRequest.Cell;
            PrimaryRuinRegions = req.RuinRegions;
            PrimaryMarketQuota = req.MarketQuota;
        }
    }
}