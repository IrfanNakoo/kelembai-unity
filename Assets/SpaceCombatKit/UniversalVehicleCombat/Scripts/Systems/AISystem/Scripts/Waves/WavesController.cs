using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VSX.UniversalVehicleCombat
{
    /// <summary>
    /// Manage a set of waves.
    /// </summary>
    public class WavesController : MonoBehaviour
    {
        [Header("General")]
        [SerializeField]
        protected List<WaveController> waveControllers = new List<WaveController>();
        public List<WaveController> WaveControllers { get { return waveControllers; } }


        [SerializeField]
        protected bool loopWaves = false;



        [SerializeField]
        protected bool autoSpawnAllWaves = false; // Renamed from SpawnAllWaves



        protected int lastSpawnedWaveIndex = -1;
        public int LastSpawnedWaveIndex { get { return lastSpawnedWaveIndex; } }

        protected bool wavesDestroyed = false;

        [Header("Events")]
        public UnityEvent onWavesDestroyed;

        protected virtual void Awake()
        {
            foreach (WaveController waveController in waveControllers)
            {
                waveController.onWaveDestroyed.AddListener(OnWaveDestroyed);
            }
        }

        /// <summary>
        /// Spawn a wave at a specific index in the list.
        /// </summary>
        /// <param name="index">The wave index to spawn.</param>
        public virtual void SpawnWave(int index)
        {
            if (index < 0 || index >= waveControllers.Count) return;

            waveControllers[index].Spawn();
            lastSpawnedWaveIndex = index;
        }




        /// <summary>
        /// Spawn all waves in the list.
        /// </summary>
        public virtual void SpawnAllWaves()
        {
            if (autoSpawnAllWaves)
            {
                // Iterate through each wave controller in the list
                for (int i = 0; i < waveControllers.Count; i++)
                {
                    // Spawn each wave controller
                    SpawnWave(i);
                }

                // Update the last spawned wave index to the index of the last wave
                lastSpawnedWaveIndex = waveControllers.Count - 1;
            }
        }




        /// <summary>
        /// Spawn a random wave in the list.
        /// </summary>
        public virtual void SpawnRandomWave()
        {
            SpawnWave(Random.Range(0, waveControllers.Count));
        }

        /// <summary>
        /// Spawn the next wave in the list.
        /// </summary>
        public virtual void SpawnNextWave()
        {
            int nextWaveSpawnIndex = lastSpawnedWaveIndex + 1;
            if (nextWaveSpawnIndex >= waveControllers.Count)
            {
                if (loopWaves)
                {
                    ResetWaves();
                    nextWaveSpawnIndex = 0;
                }
                else
                {
                    return;
                }
            }

            SpawnWave(nextWaveSpawnIndex);
        }

        public virtual void ResetWaves()
        {
            foreach (WaveController waveController in waveControllers)
            {
                waveController.ResetWave();
            }

            wavesDestroyed = false;
        }

        protected virtual void OnWaveDestroyed()
        {
            if (!wavesDestroyed)
            {
                wavesDestroyed = true;
                for (int i = 0; i < waveControllers.Count; ++i)
                {
                    if (!waveControllers[i].Destroyed)
                    {
                        wavesDestroyed = false;
                    }
                }

                if (wavesDestroyed)
                {
                    onWavesDestroyed.Invoke();
                }
            }
        }
    }
}