namespace BlazorDiagrams.Core.Geometry;

/// <summary>
/// Helper class for geometric calculations
/// </summary>
public static class GeometryHelper
{
    /// <summary>
    /// Calculates the angle in radians between two points
    /// </summary>
    public static double AngleBetween(Point from, Point to)
    {
        return Math.Atan2(to.Y - from.Y, to.X - from.X);
    }
    
    /// <summary>
    /// Calculates the angle in degrees between two points
    /// </summary>
    public static double AngleBetweenDegrees(Point from, Point to)
    {
        return AngleBetween(from, to) * 180.0 / Math.PI;
    }
    
    /// <summary>
    /// Rotates a point around a center point by the given angle in radians
    /// </summary>
    public static Point RotatePoint(Point point, Point center, double angleRadians)
    {
        var cos = Math.Cos(angleRadians);
        var sin = Math.Sin(angleRadians);
        var dx = point.X - center.X;
        var dy = point.Y - center.Y;
        
        return new Point(
            center.X + dx * cos - dy * sin,
            center.Y + dx * sin + dy * cos
        );
    }
    
    /// <summary>
    /// Finds the closest point on a line segment to a given point
    /// </summary>
    public static Point ClosestPointOnSegment(Point point, Point lineStart, Point lineEnd)
    {
        var dx = lineEnd.X - lineStart.X;
        var dy = lineEnd.Y - lineStart.Y;
        
        if (dx == 0 && dy == 0)
            return lineStart;
        
        var t = ((point.X - lineStart.X) * dx + (point.Y - lineStart.Y) * dy) / (dx * dx + dy * dy);
        t = Math.Clamp(t, 0, 1);
        
        return new Point(lineStart.X + t * dx, lineStart.Y + t * dy);
    }
    
    /// <summary>
    /// Calculates the distance from a point to a line segment
    /// </summary>
    public static double DistanceToSegment(Point point, Point lineStart, Point lineEnd)
    {
        var closest = ClosestPointOnSegment(point, lineStart, lineEnd);
        return point.DistanceTo(closest);
    }
    
    /// <summary>
    /// Checks if two line segments intersect
    /// </summary>
    public static bool SegmentsIntersect(Point a1, Point a2, Point b1, Point b2)
    {
        var d = (a2.X - a1.X) * (b2.Y - b1.Y) - (a2.Y - a1.Y) * (b2.X - b1.X);
        
        if (Math.Abs(d) < 1e-10)
            return false;
        
        var t = ((b1.X - a1.X) * (b2.Y - b1.Y) - (b1.Y - a1.Y) * (b2.X - b1.X)) / d;
        var u = ((b1.X - a1.X) * (a2.Y - a1.Y) - (b1.Y - a1.Y) * (a2.X - a1.X)) / d;
        
        return t >= 0 && t <= 1 && u >= 0 && u <= 1;
    }
    
    /// <summary>
    /// Finds the intersection point of two line segments (if they intersect)
    /// </summary>
    public static Point? SegmentIntersectionPoint(Point a1, Point a2, Point b1, Point b2)
    {
        var d = (a2.X - a1.X) * (b2.Y - b1.Y) - (a2.Y - a1.Y) * (b2.X - b1.X);
        
        if (Math.Abs(d) < 1e-10)
            return null;
        
        var t = ((b1.X - a1.X) * (b2.Y - b1.Y) - (b1.Y - a1.Y) * (b2.X - b1.X)) / d;
        var u = ((b1.X - a1.X) * (a2.Y - a1.Y) - (b1.Y - a1.Y) * (a2.X - a1.X)) / d;
        
        if (t < 0 || t > 1 || u < 0 || u > 1)
            return null;
        
        return new Point(a1.X + t * (a2.X - a1.X), a1.Y + t * (a2.Y - a1.Y));
    }
    
    /// <summary>
    /// Calculates a point along a quadratic Bézier curve
    /// </summary>
    public static Point QuadraticBezier(Point start, Point control, Point end, double t)
    {
        t = Math.Clamp(t, 0, 1);
        var t2 = 1 - t;
        
        return new Point(
            t2 * t2 * start.X + 2 * t2 * t * control.X + t * t * end.X,
            t2 * t2 * start.Y + 2 * t2 * t * control.Y + t * t * end.Y
        );
    }
    
    /// <summary>
    /// Calculates a point along a cubic Bézier curve
    /// </summary>
    public static Point CubicBezier(Point start, Point control1, Point control2, Point end, double t)
    {
        t = Math.Clamp(t, 0, 1);
        var t2 = 1 - t;
        var t2_3 = t2 * t2 * t2;
        var t2_2_t = 3 * t2 * t2 * t;
        var t2_t_2 = 3 * t2 * t * t;
        var t_3 = t * t * t;
        
        return new Point(
            t2_3 * start.X + t2_2_t * control1.X + t2_t_2 * control2.X + t_3 * end.X,
            t2_3 * start.Y + t2_2_t * control1.Y + t2_t_2 * control2.Y + t_3 * end.Y
        );
    }
    
    /// <summary>
    /// Calculates the bounding box for a set of points
    /// </summary>
    public static Rect BoundingBox(IEnumerable<Point> points)
    {
        var pointList = points.ToList();
        if (pointList.Count == 0)
            return Rect.Zero;
        
        var minX = pointList.Min(p => p.X);
        var minY = pointList.Min(p => p.Y);
        var maxX = pointList.Max(p => p.X);
        var maxY = pointList.Max(p => p.Y);
        
        return new Rect(minX, minY, maxX - minX, maxY - minY);
    }
    
    /// <summary>
    /// Linearly interpolates between two points
    /// </summary>
    public static Point Lerp(Point start, Point end, double t)
    {
        t = Math.Clamp(t, 0, 1);
        return new Point(
            start.X + (end.X - start.X) * t,
            start.Y + (end.Y - start.Y) * t
        );
    }
    
    /// <summary>
    /// Gets the perpendicular point at a given distance from a line segment
    /// </summary>
    public static Point PerpendicularPoint(Point lineStart, Point lineEnd, double distance, bool leftSide = true)
    {
        var dx = lineEnd.X - lineStart.X;
        var dy = lineEnd.Y - lineStart.Y;
        var length = Math.Sqrt(dx * dx + dy * dy);
        
        if (length == 0)
            return lineStart;
        
        var nx = -dy / length;
        var ny = dx / length;
        
        if (!leftSide)
        {
            nx = -nx;
            ny = -ny;
        }
        
        var midX = (lineStart.X + lineEnd.X) / 2;
        var midY = (lineStart.Y + lineEnd.Y) / 2;
        
        return new Point(midX + nx * distance, midY + ny * distance);
    }
}

