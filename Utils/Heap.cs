using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace RaylibExtendedCS
{
	public class Heap<T>
	{
		private Dictionary<object,T> _pool;

		public Heap(){
			_pool = new Dictionary<object, T>();
		}

		// Get the reserved object of an owner, with filter
		protected T GetExisting(object owner)
		{
			if (_pool.ContainsKey(owner))
			{
				return _pool[owner];
			}
			return default;
		}

		public bool Register(object owner, T reference)
		{
			if (_pool.ContainsKey(owner))
			{
				Raylib.TraceLog(TraceLogLevel.LOG_WARNING,$"Owner {owner} already has a reservation!");
				return false;
			}
			_pool.Add(owner,reference);
			Raylib.TraceLog(TraceLogLevel.LOG_DEBUG, $"Allocated {reference} for {owner}");
			return true;
		}
	}
}
