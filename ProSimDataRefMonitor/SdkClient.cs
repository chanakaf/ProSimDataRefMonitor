using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProSimSDK; // <-- make sure the project references the DLL that defines ProSimConnect and DataRef

namespace ProSimDataRefMonitor
{
	public sealed class SdkClient : IProSimClient, IDisposable
	{
		private readonly string _host;
		private ProSimConnect _client;
		private readonly List<DataRef> _subs = new List<DataRef>();
		private Action<string, ProSimValue> _sink;

		public string ModeName => "SDK";

		public SdkClient(string host)
		{
			_host = host;
		}

		public Task ConnectAsync()
		{
			// Direct connection exactly like your example
			_client = new ProSimConnect();
			// These are optional, but nice to have if you want to reflect status in the UI:
			_client.onConnect += OnConnected;
			_client.onDisconnect += OnDisconnected;

			_client.Connect(_host);
			return Task.CompletedTask;
		}

		public Task StartAsync(string[] dataRefs, Action<string, ProSimValue> onValue)
		{
			// C# 7.3-friendly default sink (avoid '??' with a lambda)
			if (onValue != null)
				_sink = onValue;
			else
				_sink = delegate (string n, ProSimValue v) { };

			// Clean any previous subscriptions
			CleanupSubscriptions();

			// Create a DataRef per name and hook its onDataChange event
			// The '100' below matches your example's period (ms). Adjust if needed.
			foreach (var name in dataRefs)
			{
				if (string.IsNullOrWhiteSpace(name)) continue;

				try
				{
					var dr = new DataRef(name, 100, _client); // read-only subscription like your sample
					dr.onDataChange += OnDataRefChanged;
					_subs.Add(dr);
				}
				catch
				{
					// If one DataRef fails, keep going so others can still work
				}
			}

			return Task.CompletedTask;
		}

		public Task StopAsync()
		{
			CleanupSubscriptions();
			return Task.CompletedTask;
		}

		public void Dispose()
		{
			try { StopAsync().GetAwaiter().GetResult(); } catch { }
		}

		// ==== handlers ====

		private void OnConnected()
		{
			// Optional: you can surface this to the UI if you want
			// e.g., via another callback or status flag.
		}

		private void OnDisconnected()
		{
			// Optional: update UI status if desired
		}

		private void OnDataRefChanged(DataRef dr)
		{
			// The SDK DataRef exposes .name and .value in your example
			object val = dr.value;
			var kind = GuessKind(val);
			_sink(dr.name, new ProSimValue(kind, val));
		}

		// ==== helpers ====

		private void CleanupSubscriptions()
		{
			for (int i = 0; i < _subs.Count; i++)
			{
				try { _subs[i].onDataChange -= OnDataRefChanged; } catch { }
				try
				{
					var disp = _subs[i] as IDisposable;
					if (disp != null) disp.Dispose();
				}
				catch { }
			}
			_subs.Clear();
		}

		private static ProSimValueKind GuessKind(object v)
		{
			if (v is bool) return ProSimValueKind.Bool;
			if (v is int || v is long) return ProSimValueKind.Int;
			if (v is double || v is float || v is decimal) return ProSimValueKind.Double;
			if (v is string) return ProSimValueKind.String;
			return ProSimValueKind.Unknown;
		}
	}
}
