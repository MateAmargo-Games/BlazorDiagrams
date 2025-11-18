using System.Diagnostics;

namespace BlazorDiagrams.Core.Services;

/// <summary>
/// Performance monitoring service for diagrams
/// </summary>
public class PerformanceMonitor
{
    private readonly Dictionary<string, List<long>> _metrics = new();
    private readonly Stopwatch _stopwatch = new();
    
    /// <summary>
    /// Maximum number of samples to keep per metric
    /// </summary>
    public int MaxSamples { get; set; } = 100;
    
    /// <summary>
    /// Starts timing an operation
    /// </summary>
    public void StartOperation(string operationName)
    {
        _stopwatch.Restart();
    }
    
    /// <summary>
    /// Ends timing an operation and records the result
    /// </summary>
    public long EndOperation(string operationName)
    {
        _stopwatch.Stop();
        var elapsed = _stopwatch.ElapsedMilliseconds;
        
        RecordMetric(operationName, elapsed);
        
        return elapsed;
    }
    
    /// <summary>
    /// Records a metric value
    /// </summary>
    public void RecordMetric(string metricName, long value)
    {
        if (!_metrics.ContainsKey(metricName))
        {
            _metrics[metricName] = new List<long>();
        }
        
        var list = _metrics[metricName];
        list.Add(value);
        
        // Keep only the most recent samples
        if (list.Count > MaxSamples)
        {
            list.RemoveAt(0);
        }
    }
    
    /// <summary>
    /// Gets the average value for a metric
    /// </summary>
    public double GetAverageMetric(string metricName)
    {
        if (!_metrics.ContainsKey(metricName) || _metrics[metricName].Count == 0)
        {
            return 0;
        }
        
        return _metrics[metricName].Average();
    }
    
    /// <summary>
    /// Gets the maximum value for a metric
    /// </summary>
    public long GetMaxMetric(string metricName)
    {
        if (!_metrics.ContainsKey(metricName) || _metrics[metricName].Count == 0)
        {
            return 0;
        }
        
        return _metrics[metricName].Max();
    }
    
    /// <summary>
    /// Gets the minimum value for a metric
    /// </summary>
    public long GetMinMetric(string metricName)
    {
        if (!_metrics.ContainsKey(metricName) || _metrics[metricName].Count == 0)
        {
            return 0;
        }
        
        return _metrics[metricName].Min();
    }
    
    /// <summary>
    /// Gets all metric names
    /// </summary>
    public IEnumerable<string> GetMetricNames()
    {
        return _metrics.Keys;
    }
    
    /// <summary>
    /// Gets a summary of all metrics
    /// </summary>
    public Dictionary<string, MetricSummary> GetAllMetricsSummary()
    {
        var summary = new Dictionary<string, MetricSummary>();
        
        foreach (var metricName in _metrics.Keys)
        {
            summary[metricName] = new MetricSummary
            {
                Name = metricName,
                Average = GetAverageMetric(metricName),
                Min = GetMinMetric(metricName),
                Max = GetMaxMetric(metricName),
                SampleCount = _metrics[metricName].Count
            };
        }
        
        return summary;
    }
    
    /// <summary>
    /// Clears all metrics
    /// </summary>
    public void Clear()
    {
        _metrics.Clear();
    }
    
    /// <summary>
    /// Clears a specific metric
    /// </summary>
    public void ClearMetric(string metricName)
    {
        _metrics.Remove(metricName);
    }
}

/// <summary>
/// Summary of a performance metric
/// </summary>
public class MetricSummary
{
    public string Name { get; set; } = "";
    public double Average { get; set; }
    public long Min { get; set; }
    public long Max { get; set; }
    public int SampleCount { get; set; }
    
    public override string ToString()
    {
        return $"{Name}: Avg={Average:F2}ms, Min={Min}ms, Max={Max}ms, Samples={SampleCount}";
    }
}

