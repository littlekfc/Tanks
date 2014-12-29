using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Tanks.Players;

namespace Tanks.Battle.Capture
{
    public abstract class CapturableObject : TObject, ICapturable
    {
        private IDictionary<Team.TeamID, ICollection<ICaptor>> captors = new Dictionary<Team.TeamID, ICollection<ICaptor>>();

        [SerializeField]
        private CapturePolicy capturePolicy;
        public ICapturePolicy CapturePolicy
        {
            get
            {
                return capturePolicy;
            }
            set
            {
                capturePolicy = value as CapturePolicy;
            }
        }

        [SerializeField]
        private CaptorSelectionPolicy captorSelectionPolicy;
        public ICaptorSelectionPolicy CaptorSelectionPolicy
        {
            get
            {
                return captorSelectionPolicy;
            }
            set
            {
                captorSelectionPolicy = value as CaptorSelectionPolicy;
            }
        }

        public TObject Object
        {
            get { return this; }
        }

        protected virtual void Awake()
        {
            CapturePolicy.onCaptured += OnCaptured;
        }

        public void BeginCapturing(ICaptor captor)
        {
            var team_id = captor.Owner;

            if (!captors.ContainsKey(team_id))
                captors.Add(team_id, new List<ICaptor>());

            captors[team_id].Add(captor);

            captor.CapturingObjects.Add(this);
        }

        public void EndCapturing(ICaptor captor)
        {
            var team_id = captor.Owner;

            if (captors.ContainsKey(team_id))
            {
                captors[team_id].Remove(captor);
            }

            captor.CapturingObjects.Remove(this);
        }

        /// <summary>
        /// Remove all the captor lists which are empty from captors.
        /// </summary>
        private void RemoveEmptyCaptorLists()
        {
            foreach (var captor_list in captors)
            {
                if (captor_list.Value.Count == 0)
                {
                    captors.Remove(captor_list);
                }
            }
        }

        private void Update()
        {
            RemoveEmptyCaptorLists();

            var selected_captor = CaptorSelectionPolicy.SelectCaptorFrom(captors);
            if (selected_captor != null && selected_captor.Owner != Owner)
            {
                CapturePolicy.CaptureBy(selected_captor);
            }
        }

        private void OnCaptured(Team.TeamID new_owner)
        {
            Owner = new_owner;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            foreach (var captor_list in captors.Values)
                foreach (var captor in captor_list)
                    EndCapturing(captor);
        }
    }
}