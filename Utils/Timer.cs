using System;
using Raylib_cs;

namespace RaylibExtendedCS
{
	public class Timer
	{
		private double _startTime;
		private double _endTime;
		private double _interval;
		private readonly bool _repeat;

		public Action OnDone;

		public double TimeLeft => _endTime - _startTime;
		public double Interval
		{
			get => _interval;
			set => _interval = value;
		}

		public Timer(double interval, bool runOnce = false)
		{
			_startTime = Raylib.GetTime();
			_endTime = _startTime + interval;
			_repeat = !runOnce;
		}

		public bool IsDone()
		{
			if (Raylib.GetTime() > _endTime)
			{
				if (_repeat) // restart the timer if configured
				{
					_startTime = _endTime;
					_endTime += _interval;
				}
				OnDone?.Invoke();
				return true;
			}
			return false;
		}
	}

	public class TimerHeap : Heap<Timer>
	{
		// TODO this will probably run every frame so this might needs some optimalisations
		public bool RunEvery(object owner, double interval)
		{
			Timer? timer = GetExisting(owner);
			if (timer == null) // the owner doesn't have a timer assigned yet
			{
				timer = new Timer(interval,false);
				Register(owner,timer);
			}
			return timer.IsDone();
		}
	}
}
