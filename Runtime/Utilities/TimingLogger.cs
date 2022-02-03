//-----------------------------------------------------------------------
// <copyright file="TimingLogger.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if UNITY

namespace Lost
{
    using System;
    using UnityEngine;

    public class TimingLogger : IDisposable
    {
        private DateTime startTime;
        private string message;

        /// <summary>
        /// Initializes a new instance of the <see cref="Lost.TimingLogger"/> class.
        /// </summary>
        /// <param name='message'>The message to print to the logger (along with timing info).</param>
        public TimingLogger(string message)
        {
            this.message = message;
            this.startTime = DateTime.Now;
        }

        /// <summary>
        /// Releases all resource used by the <see cref="Lost.TimingLogger"/> object.
        /// </summary>
        /// <remarks>
        /// Call <see cref="Dispose"/> when you are finished using the <see cref="LostEngine.LostTimingLogger"/>. The
        /// <see cref="Dispose"/> method leaves the <see cref="LostEngine.LostTimingLogger"/> in an unusable state. After
        /// calling <see cref="Dispose"/>, you must release all references to the <see cref="LostEngine.LostTimingLogger"/> so
        /// the garbage collector can reclaim the memory that the <see cref="LostEngine.LostTimingLogger"/> was occupying.
        /// </remarks>
        public void Dispose()
        {
            Debug.LogFormat($"{this.message} took {GetTimeAsString(this.startTime, DateTime.Now)}");
        }

        public static string GetTimeAsString(DateTime start, DateTime end)
        {
            var timeSpan = end.Subtract(start);

            if (timeSpan.TotalHours >= 1.0)
            {
                return $"{timeSpan.TotalHours:0.00} Hours";
            }
            else if (timeSpan.TotalMinutes >= 1.0)
            {
                return $"{timeSpan.TotalMinutes:0.00} Minutes";
            }
            else if (timeSpan.TotalSeconds >= 1.0)
            {
                return $"{timeSpan.TotalSeconds:0.00} Seconds";
            }
            else
            {
                return $"{timeSpan.TotalMilliseconds:0.00} Milliseconds";
            }
        }
    }
}

#endif
