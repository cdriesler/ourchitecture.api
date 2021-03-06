﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rhino.Geometry;

namespace Ourchitecture.Api.Protocols.Motley.Impact
{
    public class MemorialRegion
    {
        public Curve BaseRegion { get; }
        public Curve QuadRegion { get; }

        public List<Curve> QuadSegments { get; }
        public Curve QuadSegmentA { get; }
        public Curve QuadSegmentB { get; }
        public Curve QuadSegmentC { get; }
        public Curve QuadSegmentD { get; }

        public MemorialRegion(Curve region)
        {
            BaseRegion = region;

            if (region.SpanCount == 4 && region.Degree == 1)
            {
                QuadRegion = region.DuplicateCurve();
            }
            else
            {
                region.DivideByCount(4, true, out var pts);

                QuadRegion = new Polyline(new List<Point3d>
                {
                    pts[0],
                    pts[1],
                    pts[2],
                    pts[3],
                    pts[0]
                }).ToNurbsCurve();
            }

            //Identify composition of quad region (i.e. relative positions of all segments)
            QuadSegments = Composition.GetCurveAsSegmentsInClockwiseOrder(QuadRegion);
            QuadSegmentA = QuadSegments[0];
            QuadSegmentB = QuadSegments[1];
            QuadSegmentC = QuadSegments[2];
            QuadSegmentD = QuadSegments[3];
        }

        public bool IntersectsWithQuadRegion(Curve crv)
        {
            return Rhino.Geometry.Intersect.Intersection.CurveCurve(crv, QuadRegion, 0.1, 0.1).Any(x => x.IsPoint);
        }

        public bool CrossesQuadRegion(Curve crv)
        {
            return Rhino.Geometry.Intersect.Intersection.CurveCurve(crv, QuadRegion, 0.1, 0.1).Where(x => x.IsPoint).Count() > 1;
        }

        public Curve FirstSegmentIntersection(Curve crv)
        {
            throw new NotImplementedException();
        }

        public Curve GetSplinterSegment(Curve crv)
        {
            var ccx = Rhino.Geometry.Intersect.Intersection.CurveCurve(crv, QuadRegion, 0.1, 0.1)
                .Where(x => x.IsPoint);

            if (!ccx.Any()) return null;

            var targetPt = ccx
                .OrderBy(x => crv.PointAtStart.DistanceTo(x.PointA))
                .First()
                .PointA;

            var targetSegment = new List<Curve>(QuadSegments)
                .OrderBy(x =>
                {
                    x.ClosestPoint(targetPt, out var t);
                    return targetPt.DistanceTo(x.PointAt(t));
                })
                .First();

            var targetIndex = (QuadSegments.IndexOf(targetSegment) + 1) % 4;

            return QuadSegments[targetIndex];
        }
        
    }
}