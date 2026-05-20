using System;
using UnityEngine;
using UsefulTools.Utility.Runtime.Utility;

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager Instance;

    [SerializeField] private SeObject _seObject;
    [SerializeField] private int _bufferCount = 10;

    private RecycleBuffer<SeObject> _recycleBuffer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// SE再生用のオブジェクトを取得する
    /// </summary>
    /// <returns></returns>
    public SeObject GetSeObject()
    {
        if (_recycleBuffer == null)
        {
            Debug.LogWarning("[SoundEffectManager]初期化前にSeObjectの取得を試みました。");
            return null;
        }

        return _recycleBuffer.Get();
    }

    private void Initialize()
    {
        SeObject[] seObjects = new SeObject[_bufferCount];

        for (int i = 0; i < _bufferCount; i++)
        {
            seObjects[i] = Instantiate(_seObject, transform);
        }

        _recycleBuffer = new RecycleBuffer<SeObject>(seObjects);
    }
}