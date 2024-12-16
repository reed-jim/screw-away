using UnityEngine;

public class ObjectPoolingEverythingInitializer : MonoBehaviour
{
    [SerializeField] private GameObject vehicleEngineSoundPrefab;
    [SerializeField] private GameObject hitObstacleSoundPrefab;
    [SerializeField] private GameObject getInVehicleSoundPrefab;
    [SerializeField] private GameObject vehicleMoveOutSoundPrefab;

    private void Start()
    {
        InitPool();
    }

    private void InitPool()
    {
        RegisterPool<AudioSource>(GameConstants.VEHICLE_ENGINE_SOUND, vehicleEngineSoundPrefab, 3);
        RegisterPool<AudioSource>(GameConstants.HIT_OBSTACLE_SOUND, hitObstacleSoundPrefab, 3);
        RegisterPool<AudioSource>(GameConstants.GET_IN_VEHICLE_SOUND, getInVehicleSoundPrefab, 3);
        RegisterPool<AudioSource>(GameConstants.VEHICLE_MOVE_OUT_SOUND, vehicleMoveOutSoundPrefab, 3);
    }

    private void RegisterPool<T>(string key, GameObject prefab, int poolSize)
    {
        ObjectPool pool = new ObjectPool();

        pool.Init<T>(prefab, poolSize);

        ObjectPoolingEverything.AddToPool(key, pool);
    }
}
