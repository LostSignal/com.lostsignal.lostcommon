//-----------------------------------------------------------------------
// <copyright file="SingletonUtil.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using UnityEngine;

    public static class SingletonUtil
    {
        private static GameObject rootInstance;

        public static T CreateSingleton<T>(string name)
            where T : MonoBehaviour
        {
            if (rootInstance == null)
            {
                rootInstance = new GameObject("Lost Singletons");
                GameObject.DontDestroyOnLoad(rootInstance);
                rootInstance.transform.Reset();
            }

            var singleton = new GameObject(name, typeof(T));
            singleton.transform.SetParent(rootInstance.transform);
            singleton.transform.Reset();

            return singleton.GetComponent<T>();
        }
    }
}
