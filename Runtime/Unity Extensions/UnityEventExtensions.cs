//-----------------------------------------------------------------------
// <copyright file="UnityEventExtensions.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if UNITY

namespace Lost
{
    using System;
    using UnityEngine.Events;

    public static class UnityEventExtensions
    {
        public static void SafeInvoke(this UnityEvent unityEvent)
        {
            if (unityEvent != null)
            {
                try
                {
                    unityEvent.Invoke();
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogException(ex);
                }
            }
        }
    }
}

#endif
