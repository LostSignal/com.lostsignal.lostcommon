//-----------------------------------------------------------------------
// <copyright file="ActivationManager.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class ActivationManager : MonoBehaviour
    {
        private static ActivationManager instance;
        private static bool isInitialized;

        private List<MonoBehaviour> monoBehaviours = new List<MonoBehaviour>(1000);
        private List<IAwake> awakes = new List<IAwake>(1000);
        private List<IStart> starts = new List<IStart>(1000);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Register(MonoBehaviour monoBehaviour)
        {
            (isInitialized ? instance : Initialize()).monoBehaviours.Add(monoBehaviour);
        }

        private static ActivationManager Initialize()
        {
            isInitialized = true;
            instance = SingletonUtil.CreateSingleton<ActivationManager>("Activation Manager");

            // HACK [bgish]: It seems that adding an element for the first time is causing some jit-ing, so doing it now.
            instance.monoBehaviours.Add(null);
            instance.monoBehaviours.Clear();

            return instance;
        }
    }
}
