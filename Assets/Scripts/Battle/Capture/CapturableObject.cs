using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Tanks.Players;

namespace Tanks.Battle.Capture
{
    public abstract class CapturableObject : TObject, ICapturable
    {
        public class CaptorList
        {
            public ICollection<ICaptor> captors = new List<ICaptor>();
            public Team.TeamID teamId = Team.TeamID.NONE;
        }

        private List<CaptorList> captorLists = new List<CaptorList>();

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
            var captor_list = captorLists.FirstOrDefault(t => t.teamId == team_id);

            if (captor_list == null)
            {
                captor_list = new CaptorList
                {
                    teamId = team_id
                };
                captorLists.Add(captor_list);
            }

            captor_list.captors.Add(captor);
            captor.CapturingObjects.Add(this);
        }

        public void EndCapturing(ICaptor captor)
        {
            var team_id = captor.Owner;
            var captor_list = captorLists.FirstOrDefault(t => t.teamId == team_id);

            if (captor_list != null)
            {
                captor_list.captors.Remove(captor);
            }

            captor.CapturingObjects.Remove(this);
        }

        /// <summary>
        /// Remove all the captor lists which are empty from captors.
        /// </summary>
        private void RemoveEmptyCaptorLists()
        {
            captorLists.RemoveAll(t => t.captors.Count == 0);
        }

        private void Update()
        {
            RemoveEmptyCaptorLists();

            var selected_captor = CaptorSelectionPolicy.SelectCaptorFrom(captorLists);
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

            foreach (var captor_list in captorLists)
                foreach (var captor in captor_list.captors)
                    EndCapturing(captor);
        }
    }
}