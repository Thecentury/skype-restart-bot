using System;
using System.ServiceProcess;
using System.Threading;

namespace SkypeRestartBot
{
	public sealed class FakeServiceController : IServiceController
	{
		private readonly IRandomNumberGenerator _rnd;

		private TimeSpan _minDelay = TimeSpan.FromSeconds( 15 );
		public TimeSpan MinDelay
		{
			get { return _minDelay; }
			set { _minDelay = value; }
		}

		private TimeSpan _maxDelay = TimeSpan.FromSeconds( 30 );
		public TimeSpan MaxDelay
		{
			get { return _maxDelay; }
			set { _maxDelay = value; }
		}

		private double _runningProbability = 0.4;
		public double RunningProbability
		{
			get { return _runningProbability; }
			set { _runningProbability = value; }
		}

		private double _stoppedProbability = 0.4;
		public double StoppedProbability
		{
			get { return _stoppedProbability; }
			set { _stoppedProbability = value; }
		}

		private double _failedProbability = 0.2;
		public double FailedProbability
		{
			get { return _failedProbability; }
			set { _failedProbability = value; }
		}

		public FakeServiceController( IRandomNumberGenerator rnd )
		{
			if ( rnd == null )
			{
				throw new ArgumentNullException( "rnd" );
			}
			_rnd = rnd;
		}

		public ServiceControllerStatus Status
		{
			get
			{
				double sleepDelay = ( _rnd.NextDouble()*( _maxDelay - _minDelay ).TotalSeconds ) + _minDelay.TotalSeconds;
				TimeSpan sleepDuration = TimeSpan.FromSeconds( sleepDelay );

				Thread.Sleep( sleepDuration );

				double value = _rnd.NextDouble();
				if ( value < _runningProbability )
				{
					return ServiceControllerStatus.Running;
				}
				if ( value < _runningProbability + _stoppedProbability )
				{
					return ServiceControllerStatus.Stopped;
				}

				throw new Exception();
			}
		}
	}
}