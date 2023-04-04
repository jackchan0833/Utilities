using System;
using System.Timers;

namespace JC.Utilities.WPF.Extensions
{
    /// <summary>
    /// Represents the <see cref="System.Timers.Timer"/> extension.
    /// </summary>
    public class ExtTimer
    {
        /// <summary>
        /// The interval of timer.
        /// </summary>
        public double Interval { get; private set; }
        /// <summary>
        /// Ths state of timer.
        /// </summary>
        public bool IsRunning { get; private set; } = false;
        private Timer _Timer;
        /// <summary>
        /// Constructs the <see cref="ExtTimer"/>.
        /// </summary>
        public ExtTimer(TimeSpan timeSpan, ElapsedEventHandler timerTickEvent)
        {
            Interval = timeSpan.TotalMilliseconds;
            _Timer = new Timer(Interval);
            _Timer.Elapsed += timerTickEvent;
        }

        /// <summary>
        /// Start the timer.
        /// </summary>
        public void Start()
        {
            IsRunning = true;
            try
            {
                _Timer.Start();
            }
            catch
            {
                IsRunning = false;
            }
        }
        /// <summary>
        /// Stop the timer.
        /// </summary>
        public void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                _Timer.Stop();
            }
        }
    }
}
