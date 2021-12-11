//-----------------------------------------------------------------------
// <copyright file="CoroutineRunner.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if UNITY

namespace Lost
{
    using UnityEngine;

    public class CoroutineRunner : MonoBehaviour
    {
        private static CoroutineRunner instance;

        public static CoroutineRunner Instance
        {
            get
            {
                if (instance == null)
                {
                    var newGameObject = new GameObject("CoroutineRunner", typeof(CoroutineRunner));
                    DontDestroyOnLoad(newGameObject);
                    instance = newGameObject.GetComponent<CoroutineRunner>();
                }

                return instance;
            }
        }
    }
}

#endif
