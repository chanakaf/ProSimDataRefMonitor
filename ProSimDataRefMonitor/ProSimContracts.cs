using System;
using System.Threading.Tasks;

namespace ProSimDataRefMonitor
{
    public enum ProSimValueKind { Bool, Int, Double, String, Unknown }

    public struct ProSimValue
    {
        public ProSimValueKind Kind { get; }
        public object Raw { get; }

        public ProSimValue(ProSimValueKind kind, object raw)
        {
            Kind = kind; Raw = raw;
        }

        public string ValueAsString => Raw?.ToString() ?? string.Empty;
        public bool AsBool() => Raw is bool b ? b : string.Equals(ValueAsString, "1") || string.Equals(ValueAsString, "true", StringComparison.OrdinalIgnoreCase);
        public int AsInt() => Raw is int i ? i : int.TryParse(ValueAsString, out var v) ? v : 0;
        public double AsDouble() => Raw is double d ? d : double.TryParse(ValueAsString, out var v) ? v : 0.0;
    }

    public interface IProSimClient : IDisposable
    {
        string ModeName { get; }
        Task ConnectAsync();
        Task StartAsync(string[] dataRefs, Action<string, ProSimValue> onValue);
        Task StopAsync();
    }
}
