using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace VSX.UniversalVehicleCombat.Radar
{
    /// <summary>
    /// Select a target from a list of trackables.
    /// </summary>
    public class TargetSelector : MonoBehaviour
    {
        /// The list of trackables this selector is working with.
        protected List<Trackable> trackables = new List<Trackable>();

        [Header("Selection Criteria")]

        // The teams that can be selected
        [SerializeField]
        protected List<Team> selectableTeams = new List<Team>();
        public List<Team> SelectableTeams
        {
            get { return selectableTeams; }
            set { selectableTeams = value; }
        }

        // The types that can be selected
        [SerializeField]
        protected List<TrackableType> selectableTypes = new List<TrackableType>();
        public List<TrackableType> SelectableTypes
        {
            get { return selectableTypes; }
            set { selectableTypes = value; }
        }

        [Tooltip("By default, targets at this depth are available for selection. Set to -1 for all depths.")]
        [SerializeField]
        protected int defaultDepth = -1;

        // Always look for the highest depth child
        [SerializeField]
        protected bool selectHighestDepthChild = false;

        [Header("General")]

        // Whether or not to automatically scan for a target when none is selected
        [SerializeField]
        protected bool scanEveryFrame = true;

        [SerializeField]
        protected bool defaultToFrontMostTarget = true;

        [SerializeField]
        protected float frontTargetAngle = 10;

        [SerializeField]
        protected Transform frontTargetReference;

        [SerializeField]
        protected bool callSelectEventOnTarget = true;

        protected Trackable selectedTarget;
        public Trackable SelectedTarget
        {
            get { return selectedTarget; }
            set { selectedTarget = value; }
        }

        [Header("Audio")]

        [SerializeField]
        protected bool audioEnabled = true;
        public bool AudioEnabled
        {
            get { return audioEnabled; }
            set { audioEnabled = value; }
        }

        [SerializeField]
        protected AudioSource selectedTargetChangedAudio;


        [Header("Events")]

        // Selected target changed event
        public TrackableEventHandler onSelectedTargetChanged;


        // Called when the component is first added to a gameobject, or the component is reset in the inspector
        protected virtual void Reset()
        {
            frontTargetReference = transform;
        }


        // Get the index of the currently selected target in the list
        protected virtual int GetSelectedTargetIndex()
        {

            if (selectedTarget == null) return -1;

            for (int i = 0; i < trackables.Count; ++i)
            {
                if (trackables[i] == selectedTarget)
                {
                    return i;
                }
            }

            return -1;
        }


        /// <summary>
        /// Select the first selectable target.
        /// </summary>
        public virtual void SelectFirstSelectableTarget(int depth = -1)
        {
            for (int i = 0; i < trackables.Count; ++i)
            {
                if (depth != -1 && trackables[i].Depth != depth) continue;

                if (IsSelectable(trackables[i]))
                {
                    SetSelected(trackables[i]);
                    return;
                }
            }

            if (selectedTarget != null) SetSelected(null);
        }


        public virtual Trackable GetFrontMostTarget(Vector3 position, Vector3 forwardDirection, int depth = -1)
        {
            float minAngle = 180;

            // Get the target that is nearest the forward vector of the tracker
            int index = -1;
            for (int i = 0; i < trackables.Count; ++i)
            {
                if (depth != -1 && trackables[i].Depth != depth) continue;

                if (IsSelectable(trackables[i]))
                {
                    float angle = Vector3.Angle(trackables[i].transform.position - position, forwardDirection);

                    if (angle < minAngle)
                    {
                        minAngle = angle;
                        index = i;
                    }
                }
            }

            // Select the target
            if (index != -1)
            {
                return (trackables[index]);
            }
            else
            {
                return null;
            }
        }


        // Check if a target is selectable
        public virtual bool IsSelectable(Trackable target)
        {

            // Check if the team is selectable
            if (selectableTeams.Count > 0)
            {
                bool teamFound = false;
                for (int i = 0; i < selectableTeams.Count; ++i)
                {
                    if (selectableTeams[i] == target.Team)
                    {
                        teamFound = true;
                        break;
                    }
                }
                if (!teamFound) return false;
            }

            // Check if the type is selectable 
            if (selectableTypes.Count > 0)
            {
                bool typeFound = false;
                for (int i = 0; i < selectableTypes.Count; ++i)
                {
                    if (selectableTypes[i] == target.TrackableType)
                    {
                        typeFound = true;
                        break;
                    }
                }
                if (!typeFound) return false;
            }

            return true;

        }

        /// <summary>
        /// Called when the Tracker stops tracking a target.
        /// </summary>
        /// <param name="untrackedTrackable"></param>
        public virtual void OnStoppedTracking(Trackable trackable)
        {
            if (trackable == selectedTarget)
            {
                SetSelected(null);
            }
        }



        public virtual void Select(Trackable target)
        {
            if (target == selectedTarget) return;

            if (target != null && !IsSelectable(target)) return;
        }


        // Select a target
        protected virtual void SetSelected(Trackable newSelectedTarget)
        {

            if (newSelectedTarget == selectedTarget) return;

            // Unselect the currently selected target
            if (selectedTarget != null)
            {
                selectedTarget.Unselect();
            }

            if (newSelectedTarget != null)
            {
                // If toggled, select the highest depth child in the hierarchy.
                if (selectHighestDepthChild)
                {
                    for (int i = 0; i < 1000; ++i)
                    {
                        if (newSelectedTarget.ChildTrackables.Count > 0)
                        {
                            SetSelected(newSelectedTarget.ChildTrackables[0]);
                            return;
                        }
                    }
                }
            }

            // Play audio
            if (audioEnabled && selectedTargetChangedAudio != null)
            {
                // If new target is not null and is different from previous, play audio
                if (newSelectedTarget != null && newSelectedTarget != selectedTarget)
                {
                    selectedTargetChangedAudio.Play();
                }
            }

            // Update the target 
            selectedTarget = newSelectedTarget;

            // Call select event on the new target
            if (selectedTarget != null && callSelectEventOnTarget)
            {
                selectedTarget.Select();
            }

            // Call the event
            onSelectedTargetChanged.Invoke(selectedTarget);

        }


        /// <summary>
        /// Cycle back or forward through the targets list.
        /// </summary>
        /// <param name="forward">Whether to cycle forward.</param>
        public virtual void Cycle(bool forward, bool searchLocal = true, int depth = -1)
        {

            // Get the index of the currently selected target
            int index = GetSelectedTargetIndex();

            // If the selected target is null or doesn't exist in the list, just get the first selectable target
            if (index == -1)
            {
                SelectFirstSelectableTarget(depth);
                return;
            }

            List<Trackable> targetsList = trackables;
            Trackable rootTrackable = selectedTarget;
            if (searchLocal && depth > 0)
            {
                while (true)
                {
                    if (rootTrackable.ChildTrackables.Count == 0) return;

                    if (rootTrackable.ChildTrackables[0].Depth == depth)
                    {
                        break;
                    }
                    else
                    {
                        rootTrackable = rootTrackable.ChildTrackables[0];
                    }
                }

                targetsList = rootTrackable.ChildTrackables;
            }


            // Step through the targets in the specified direction looking for the next selectable one
            int direction = forward ? 1 : -1;
            for (int i = 0; i < targetsList.Count; ++i)
            {

                index += direction;

                // Wrap at the end
                if (index >= targetsList.Count)
                {
                    index = 0;
                }

                // Wrap at the beginning
                else if (index < 0)
                {
                    index = targetsList.Count - 1;
                }

                if (depth != -1 && targetsList[index].Depth != depth) continue;

                // Select the target if it's selectable
                if (IsSelectable(targetsList[index]))
                {
                    SetSelected(targetsList[index]);
                    return;
                }
            }

            if (selectedTarget != null) SetSelected(null);

        }


        /// <summary>
        /// Select the nearest target to the tracker.
        /// </summary>
        public virtual void SelectNearest(int depth = -1)
        {
            // Find the index of the target that is nearest
            float minDist = float.MaxValue;
            int index = -1;
            for (int i = 0; i < trackables.Count; ++i)
            {
                if (depth != -1 && trackables[i].Depth != depth) continue;

                if (IsSelectable(trackables[i]))
                {
                    float dist = Vector3.Distance(trackables[i].transform.position, transform.position);

                    if (dist < minDist)
                    {
                        minDist = dist;
                        index = i;
                    }
                }
            }

            // Select the target
            if (index != -1)
            {
                SetSelected(trackables[index]);
            }
            else
            {
                if (selectedTarget != null) SetSelected(null);
            }
        }


        /// <summary>
        /// Select the target closest to the front of the tracker, within a specified angle.
        /// </summary>
        public virtual void SelectFront(int depth = -1)
        {
            Trackable frontTrackable = GetFrontMostTarget(transform.position, transform.forward, depth);
            if (frontTrackable != null)
            {
                float angle = Vector3.Angle(frontTrackable.transform.position - transform.position, transform.forward);
                if (angle < frontTargetAngle)
                {
                    SetSelected(frontTrackable);
                }
            }
        }



        public virtual void SelectFront(Vector3 position, Vector3 forwardDirection, int depth = -1)
        {
            Trackable frontTrackable = GetFrontMostTarget(position, forwardDirection, depth);
            if (frontTrackable != null)
            {
                float angle = Vector3.Angle(frontTrackable.transform.position - position, forwardDirection);
                if (angle < frontTargetAngle)
                {
                    SetSelected(frontTrackable);
                }
            }
        }



        // Called every frame
        protected virtual void Update()
        {
            // If toggled, always look for a new target when none is selected
            if (scanEveryFrame && selectedTarget == null)
            {
                if (defaultToFrontMostTarget)
                {
                    Trackable frontMostTarget = GetFrontMostTarget(transform.position, transform.forward, defaultDepth);
                    if (frontMostTarget != null) SetSelected(frontMostTarget);
                }
                else
                {
                    SelectFirstSelectableTarget(defaultDepth);
                }
            }
        }
    }
}
