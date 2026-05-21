using UnityEngine;
using UsefulTools.Utility.Runtime.Utility;

//
//　基本的にクラス中の変数やメソッドの定義順は「自分以外の誰がどの程度見るか」で判断する
//　多くの人が見る可能性のある物ほど上に、自分しか見ないものは下に纏める。
//

/// <summary>
/// 音声エフェクトを再生するためのクラスを取得するためのオブジェクトプール
/// </summary>
public class SoundEffectPool : MonoBehaviour
{
    //パブリックなフィールド変数は他の作業をしている人が見に来る可能性が高いので、上に持ってくる
    public static SoundEffectPool Instance;
    
    //シリアライズフィールドはインスペクターの変更とかかわり、他の人が触る値なのでpublicの下ぐらいの上の方に置く
    [SerializeField] private SeObject _seObject;
    [SerializeField] private int _bufferCount = 10;

    //プライベートなフィールド変数はパブリックメソッドの下にしてもいいが、
    //変数は変数、メソッドはメソッドで纏めた方が見やすいと思っているので変数の中で一番下に置いている
    private RecycleBuffer<SeObject> _recycleBuffer;

    //Unityのイベント関数、ライフサイクルに関する物は処理の起点になることが多いため、メソッドの中では比較的上の方にある方がよい。
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

    //パブリックメソッドは外部に公開するため、他の人が見に来る可能性が高いので上の方に固めておくとよい
    
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

    //基本的にこのクラスで作業する人しか見に来ないため、クラス内部でのみ使うプライベートメソッドは一番下に置く。
    
    /// <summary>
    /// 初期化を行う
    /// </summary>
    private void Initialize()
    {
        SeObject[] seObjects = new SeObject[_bufferCount];

        for (int i = 0; i < _bufferCount; i++)
        {
            seObjects[i] = Instantiate(_seObject, transform);
        }

        //オブジェクトプールを作成
        _recycleBuffer = new RecycleBuffer<SeObject>(seObjects);
    }
}