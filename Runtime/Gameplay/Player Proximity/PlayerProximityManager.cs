//-----------------------------------------------------------------------
// <copyright file="PlayerProximityManager.cs" company="Lost Signal">
//     Copyright (c) Lost Signal. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if UNITY

namespace Lost
{
    using UnityEngine;

    public class PlayerProximityManager : MonoBehaviour, IUpdate
    {
        private static PlayerProximityManager instance;

        private PlayerProximityList playerProximityList = new PlayerProximityList("Player Proximity List", 1000);

        public static PlayerProximityManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = SingletonUtil.CreateSingleton<PlayerProximityManager>("Player Proximity Manager");
                }

                return instance;
            }
        }

        public void Register(PlayerProximity playerProximity)
        {
            Transform playerProximityTransform = playerProximity.ProximityTransform;

            this.playerProximityList.Add(
                playerProximity.GetInstanceID(), 
                new PlayerProximityItem
                {
                    WorldToLocal = playerProximityTransform.worldToLocalMatrix,
                    Area = playerProximity.Area,
                    IsDynamic = playerProximity.IsDynamic,
                    PlayerProximity = playerProximity,
                    Transform = playerProximityTransform,
                    IsInProximity = false,
                },
                playerProximity);
        }

        public void Unregister(PlayerProximity playerProximity)
        {
            this.playerProximityList.Remove(playerProximity.GetInstanceID());
        }

        public void OnUpdate(float deltaTime)
        {
            this.playerProximityList.RunAllOverXSeconds(deltaTime, 1.0f);
        }

        private void Awake() => ActivationManager.Register(this);
    }
}

#endif