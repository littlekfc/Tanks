using UnityEngine;
using System.Collections;

using Tanks.Players;
using Tanks.Resources;

namespace Tanks.Battle
{
    public class ResourceGenerator : TObject
    {
        private Timer generationTimer = null;

        public Resource generatingResource = new Resource();
        public float speed = 1.0f;

        private void Awake()
        {
            if (speed > 0.0f)
            {
                generationTimer = AddComponent<Timer>();
                generationTimer.IsRepeating = true;
                generationTimer.onTick += OnResourceGenerated;
                generationTimer.Reset(1 / speed);
            }

            onOwnerChanged += OnOwnerChanged;
        }

        private void OnOwnerChanged(Team.TeamID owner)
        {
            if (owner != Team.TeamID.NONE)
                generationTimer.enabled = true;
            else
                generationTimer.enabled = false;
        }

        private void OnResourceGenerated()
        {
            var resource_receiver = ResourceManager.Instance.ResourceFor(Owner);
            if (resource_receiver != null)
                resource_receiver.Add(generatingResource);
        }
    }
}