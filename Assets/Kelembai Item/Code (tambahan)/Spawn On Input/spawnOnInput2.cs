using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace VSX.UniversalVehicleCombat
{
    public class spawnOnInput2 : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] protected List<WaveController> waveControllers = new List<WaveController>();
        public List<WaveController> WaveControllers { get { return waveControllers; } }

        [SerializeField] protected bool loopWaves = false;
        [SerializeField] protected bool autoSpawnAllWaves = false;
        [SerializeField] protected bool spawnOnInput = false;

        protected int lastSpawnedWaveIndex = -1;
        protected bool wavesDestroyed = false;

        [Header("Events")]
        public UnityEvent onWavesDestroyed;

        private bool itemSpawned = false; // Initialize itemSpawned here

        // Code for accessing the skill input action
        public InputActionReference skill;

        private void OnEnable()
        {
            skill.action.started += SkillStarted;
        }

        private void OnDisable()
        {
            skill.action.started -= SkillStarted;
        }

        private void SkillStarted(InputAction.CallbackContext context)
        {
            Debug.Log("Skill activated");
            // Call the method for handling input
            OnInput();

        }

        public virtual void OnInput()
        {
            if (!itemSpawned)
            {
                // Check if the spawnOnInput flag is set and if it is, spawn waves
                if (spawnOnInput)
                {
                    //SpawnAllWaves();
                    itemSpawned = true;
                    Debug.LogWarning("have spawn on this time");
                    SpawnNextWave();
                }
                else
                {
                    Debug.LogWarning("Spawn on input is not enabled.");
                }
            }
            else
            {
                Debug.LogWarning("An item has already been spawned.");
            }
        }
        
        public virtual void SpawnNextWave()
        {
            int nextWaveSpawnIndex = lastSpawnedWaveIndex + 1;
            if (nextWaveSpawnIndex >= waveControllers.Count)
            {
                if (loopWaves)
                {
                    //ResetWaves();
                    nextWaveSpawnIndex = 0;
                }
                else
                {
                    return;
                }
            }

            SpawnWave(nextWaveSpawnIndex);
        }

        public virtual void SpawnWave(int index)
        {
            if (index < 0 || index >= waveControllers.Count) return;

            waveControllers[index].Spawn();
            lastSpawnedWaveIndex = index;
        }

        public virtual void SpawnAllWaves()
        {
            if (autoSpawnAllWaves)
            {
                for (int i = 0; i < waveControllers.Count; i++)
                {
                    SpawnWave(i);
                }

                lastSpawnedWaveIndex = waveControllers.Count - 1;
            }
        }
    }
}