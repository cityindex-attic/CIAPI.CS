/*
 * Copyright (c) 2004-2011 Weswit S.r.l., Via Campanini, 6 - 20124 Milano, Italy.
 * All rights reserved.
 * www.lightstreamer.com
 *
 * This software is the confidential and proprietary information of
 * Weswit s.r.l.
 * You shall not disclose such Confidential Information and shall use it
 * only in accordance with the terms of the license agreement you entered
 * into with Weswit s.r.l.
 */

using System;
using System.Threading;
using System.Collections;

namespace Lightstreamer.Adapters.StockListDemo.Feed {

	/// <summary>
	/// Used by the Stock List Adapter to receive data from the simulated broadcast feed in an
	/// asynchronous way, through the OnEvent method.
	/// </summary>
	public interface IExternalFeedListener {
		
		/// <summary>
		/// Called by the feed for each update event occurrence on some stock.
		/// If isSnapshot is true, then the event contains a full snapshot,
		/// with the current values of all fields for the stock.
		/// </summary>
		void OnEvent(string itemName, IDictionary currentValues, bool isSnapshot);
	}

	/// <summary>
	/// Used by the Stock List Adapter, it simulates an external data feed that supplies quote values for all the
	/// stocks needed for the demo.
	/// </summary>
	public class ExternalFeedSimulator {

		/// <summary>
		/// Used to automatically generate the updates for the 30 stocks:
		/// mean and standard deviation of the times between consecutive
		/// updates for the same stock.
		/// </summary>
		private double [] _updateTimeMeans;		
		private double [] _updateTimeStdDevs;

		/// <summary>
		/// Used to generate the initial field values for the 30 stocks
		/// </summary>
		private double [] _refprices;		
		private double [] _openprices;		
		private double [] _minprices;		
		private double [] _maxprices;
		private string [] _stockNames;
		
		/// <summary>
		/// Used to keep the contexts of the 30 stocks.
		/// </summary>
		private IDictionary _stockGenerators;

		private IExternalFeedListener _listener;

		private IList _snapshotQueue;
		private Thread _snapshotSender;
		
		public ExternalFeedSimulator() {
			_stockGenerators= new Hashtable();
			_snapshotQueue= new ArrayList();

			_updateTimeMeans = new double [] {30000, 500, 3000, 90000,
												 7000, 10000, 3000, 7000,
												 7000, 7000, 500, 3000,
												 20000, 20000, 20000, 30000,
												 500, 3000, 90000, 7000,
												 10000, 3000, 7000, 7000,
												 7000, 500, 3000, 20000,
												 20000, 20000 };
		
			_updateTimeStdDevs = new double [] {6000, 300, 1000, 1000,
												   100, 5000, 1000, 3000,
												   1000, 6000, 300, 1000,
												   1000, 4000, 1000, 6000,
												   300, 1000, 1000, 100,
												   5000, 1000, 3000, 1000,
												   6000, 300, 1000, 1000,
												   4000, 1000 };

			_refprices = new double [] {3.04, 16.09, 7.19, 3.63, 7.61,
										   2.30, 15.39, 5.31, 4.86, 7.61,
										   10.41, 3.94, 6.79, 26.87, 2.27,
										   13.04, 6.09, 17.19, 13.63, 17.61,
										   11.30, 5.39, 15.31, 14.86, 17.61,
										   5.41, 13.94, 16.79, 6.87,
										   11.27 };
		
			_openprices = new double [] {3.10, 16.20, 7.25, 3.62, 7.65,
											2.30, 15.85, 5.31, 4.97, 7.70,
											10.50, 3.95, 6.84, 27.05, 2.29,
											13.20, 6.20, 17.25, 13.62,
											17.65, 11.30, 5.55, 15.31,
											14.97, 17.70, 5.42, 13.95,
											16.84, 7.05, 11.29 };
		
			_minprices = new double [] {3.09, 15.78, 7.15, 3.62, 7.53,
										   2.28, 15.60, 5.23, 4.89, 7.70,
										   10.36, 3.90, 6.81, 26.74, 2.29,
										   13.09, 5.78, 17.15, 13.62, 17.53,
										   11.28, 5.60, 15.23, 14.89, 17.70,
										   5.36, 13.90, 16.81, 6.74,
										   11.29 };
		
			_maxprices = new double [] {3.19, 16.20, 7.26, 3.71, 7.65,
										   2.30, 15.89, 5.31, 4.97, 7.86,
										   10.50, 3.95, 6.87, 27.05, 2.31,
										   13.19, 6.20, 17.26, 13.71, 17.65,
										   11.30, 5.89, 15.31, 14.97, 17.86,
										   5.50, 13.95, 16.87, 7.05,
										   11.31 };

			_stockNames = new string [] {"Anduct", "Ations Europe", 
											"Bagies Consulting", "BAY Corporation", 
											"CON Consulting", "Corcor PLC",
											"CVS Asia", "Datio PLC", 
											"Dentems", "ELE Manufacturing", 
											"Exacktum Systems", "KLA Systems Inc", 
											"Lted Europe", "Magasconall Capital", 
											"MED", "Mice Investments", 
											"Micropline PLC", "Nologicroup Devices", 
											"Phing Technology", "Pres Partners", 
											"Quips Devices", "Ress Devices", 
											"Sacle Research", "Seaging Devices", 
											"Sems Systems, Inc", "Softwora Consulting", 
											"Systeria Develop", "Thewlec Asia", 
											"Virtutis", "Yahl" };
		}

		/// <summary>
		/// Starts generating update events for the stocks. Sumulates attaching
		/// and reading from an external broadcast feed.
		/// </summary>
		public void Start() {
			if (_snapshotSender != null) return;

			for (int i = 0; i < 30; i++) {
				string itemName= "item" + (i + 1);
				ExternalFeedProducer myProducer = new ExternalFeedProducer(itemName, 
					_openprices[i], _refprices[i], _minprices[i], _maxprices[i], 
					_updateTimeMeans[i], _updateTimeStdDevs[i], _stockNames[i]);
				
				_stockGenerators[itemName]= myProducer;
				myProducer.SetFeedListener(_listener);
				myProducer.Start();
			}

			_snapshotSender= new Thread(new ThreadStart(Run));
			_snapshotSender.Start();
		}

		private void Run() {
			IList snapshots= new ArrayList();
			do {
				lock (_snapshotQueue) {
					if (_snapshotQueue.Count == 0) 
						Monitor.Wait(_snapshotQueue);

					snapshots.Clear();
					while (_snapshotQueue.Count > 0) {
						ExternalFeedProducer myProducer= (ExternalFeedProducer) _snapshotQueue[0];
						snapshots.Add(myProducer);
						_snapshotQueue.RemoveAt(0);
					}
				}

				foreach (ExternalFeedProducer myProducer in snapshots) {
					_listener.OnEvent(myProducer.GetItemName(), myProducer.GetCurrentValues(true), true);
				}

			} while (true);
		}

		/// <summary>
		/// Sets an internal listener for the update events.
		/// Since now, the update events were ignored.
		/// </summary>
		public void SetFeedListener(IExternalFeedListener listener) {
			_listener= listener;

			foreach (ExternalFeedProducer myProducer in _stockGenerators.Values) {
				myProducer.SetFeedListener(listener);
			}
		}

		/// <summary>
		/// Forces sending an event with a full snapshot for a stock.
		/// </summary>
		public void SendCurrentValues(string itemName) {
			ExternalFeedProducer myProducer= (ExternalFeedProducer) _stockGenerators[itemName];
			if (myProducer == null) return;

			lock (_snapshotQueue) {
				_snapshotQueue.Add(myProducer);
				Monitor.Pulse(_snapshotQueue);
			}
		}
	}

	public class ExternalFeedProducer {
		public string _itemName;
		private int _open, _refer, _last, _min, _max, _other;
		private double _mean, _stddev;
		private string _stockName;
		
		private Random _random;
		private bool _haveNextNextGaussian;
		private double _nextNextGaussian;

		private IExternalFeedListener _listener;
		private Thread _thread;

		/// <summary>
		/// Initializes stock data based on the already prepared values. 
		/// </summary>
		public ExternalFeedProducer(string name, 
			double openPrice, double referPrice, double minPrice, double maxPrice, 
			double updateTimeMean, double updateTimeStdDev, string stockName) {
			_itemName = name;
			_open = (int) Math.Round(openPrice * 100);
			_refer = (int) Math.Round(referPrice * 100);
			_min = (int) Math.Ceiling(minPrice * 100);
			_max = (int) Math.Floor(maxPrice * 100);
			_last = _open;
			_mean = updateTimeMean;
			_stddev = updateTimeStdDev;
			_stockName = stockName;
			
			_random = new Random();
			_haveNextNextGaussian= false;
			_nextNextGaussian= 0.0;

			ComputeNewValues();
		}

		public string GetItemName() {
			return _itemName;
		}

		public void SetFeedListener(IExternalFeedListener listener) {
			lock (this) {
				_listener = listener;
			}
		}

		public void Start() {
			lock (this) {
				if (_thread != null) return;

				_thread = new Thread(new ThreadStart(Run));
				_thread.Start();
			}
		}

		private void Run() {
			do {
				int waitMillis= ComputeNextWaitTime();
				Thread.Sleep(waitMillis);

				ComputeNewValues();
				if (_listener != null) 
					_listener.OnEvent(_itemName, GetCurrentValues(false), false);

			} while (true);
		}

		/// <summary>
		/// Decides, for ease of simulation, the time at which the next
		/// update for the stock will happen.
		/// </summary>
		public int ComputeNextWaitTime() {
			lock (this) {
				int millis;
				do {
					millis = (int) Gaussian(_mean, _stddev);
				} while (millis <= 0);
				return millis;
			}
		}

		/// <summary>
		/// Changes the current data for the stock. This stuff is to ensure that new prices follow a random
		/// but nondivergent path, centered around the reference price
		/// </summary>
		public void ComputeNewValues() {
			lock (this) {
				double limit = _refer / 4.0;
				double relDist = (_last - _refer) / limit;
				
				int direction = 1;
				if (relDist < 0) {
					direction = -1;
					relDist = -relDist;
				}
				if (relDist > 1) {
					relDist = 1.0;
				}
				
				double weight = (relDist * relDist * relDist);
				double prob = (1.0 - weight) / 2.0;
				
				bool goFarther = (_random.NextDouble() < prob);
				if (!goFarther) {
					direction *= -1;
				}
				
				int jump = _refer / 100;
				int difference = Uniform(0, jump) * direction;
				int gap = _refer / 250;
				
				int delta;
				if (gap > 0) {
					do {
						delta = Uniform(-gap, gap);
					} while (delta == 0);
				} 
				else {
					delta = 1;
				}
				
				_last += difference;
				_other = _last + delta;
				
				if (_last < _min) {
					_min = _last;
				}
				
				if (_last > _max) {
					_max = _last;
				}
			}
		}

		/// <summary>
		/// Picks the stock field values and stores them in a field/value
		/// Hashtable. If fullData is false, then only the fields whose value
		/// is just changed are considered (though this check is not strict).
		/// </summary>
		public IDictionary GetCurrentValues(bool fullData) {
			lock (this) {
				IDictionary eventData = new Hashtable();

				string time = DateTime.Now.ToString("s");
				eventData["time"] = time.Substring(time.Length - 8);
				// this yields us a "HH:mm:ss" format

				AddDecField("last_price", _last, eventData);
				
				if (_other > _last) {
					AddDecField("ask", _other, eventData);
					AddDecField("bid", _last, eventData);
				} 
				else {
					AddDecField("ask", _last, eventData);
					AddDecField("bid", _other, eventData);
				}
				
				int quantity = Uniform(1, 200) * 500;
				eventData["bid_quantity"]= quantity.ToString();
				
				quantity = Uniform(1, 200) * 500;
				eventData["ask_quantity"]= quantity.ToString();
				
				double v = ((double) (_last - _refer)) / ((double) _refer) * 100.0;
				AddDecField("pct_change", (int) (v * 100.0), eventData);
				
				if ((_last == _min) || fullData) {
					AddDecField("min", _min, eventData);
				}
				if ((_last == _max) || fullData) {
					AddDecField("max", _max, eventData);
				}
				
				if (fullData) {
					eventData["stock_name"] = _stockName;
					AddDecField("ref_price", _refer, eventData);
					AddDecField("open_price", _open, eventData);
				}
				
				return eventData;
			}
		}

		private static void AddDecField(string fld, int val100, IDictionary target) {
			double v = (((double) val100) / 100);
			target[fld]= v.ToString().Replace(',','.');
		}

		private double Gaussian(double mean, double stddev) {
			lock (this) {
				double b= 0.0;
				
				if (_haveNextNextGaussian) {
					_haveNextNextGaussian = false;
					b= _nextNextGaussian;
				
				} 
				else {
					double v1, v2, s;
					do { 
						v1 = 2.0 * _random.NextDouble() - 1.0;
						v2 = 2.0 * _random.NextDouble() - 1.0;
						s = v1 * v1 + v2 * v2;
					} while (s >= 1.0 || s == 0.0);
					double multiplier = Math.Sqrt(-2.0 * Math.Log(s)/s);
					_nextNextGaussian = v2 * multiplier;
					_haveNextNextGaussian = true;
					b= v1 * multiplier;
				}

				double val= b * stddev + mean;
				return val;
			}
		}

		private int Uniform(int min, int max) {
			lock (this) {
				int b = _random.Next(max + 1 - min);
				
				int val= b + min;
				return b + min;
			}
		}
	}

}
