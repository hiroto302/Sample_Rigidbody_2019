using UnityEngine;
using UnityEngine.SceneManagement;

// Playerの移動操作
public class SamplePlayerController : MonoBehaviour
{
    // Playerの移動に関する変数群
    Transform _transfrom;
    Rigidbody _rigidbody;
    Vector3 _inputVector;      // 入力値
    Vector3 _velocity;         // 前方方向の加速度
    [SerializeField]
    float _moveSpeed = 2.0f;   // 移動速度

    // Playerの状態
    public enum State
    {
        Normal,
        Invisible // 物理挙動を無視した動き
    }
    public State currentState;


    void Awake()
    {
        // 変数の取得
        _transfrom = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        /* transform を 利用した時の注意点 (どのSpace空間(Self or World) を利用していか意識する)
        example)    Move the object forward along its z axis 1 unit/second
                    transform.Translate(Vector3.forward * Time.deltaTime);

                    Move the object forward along its z axis 1 unit/second
                    transform.Translate(Vector3.forward * Time.deltaTime, Space.World);

        「transform.forward」について
            Unlike Vector3.forward, Transform.forward moves the GameObject while also considering its rotation

        「Space.World」について
        Returns a normalized vector representing the blue axis of the transform in world space.

        下記は、同様の結果を得ることができる
            transform.Translate(Vector3.forward * _moveSpeed * Time.deltaTime, Space.Self);
            transform.Translate(transform.forward * _moveSpeed * Time.deltaTime, Space.World);
        */

        // 速度の初期化
        _velocity = Vector3.zero;
        // A, D, 左右の矢印キー
        float hor = Input.GetAxis("Horizontal");
        // W, S, 上下の矢印キー
        float ver = Input.GetAxis("Vertical");
        // 入力値を代入
        _inputVector = new Vector3(hor, 0, ver);

        if(currentState == State.Invisible && _inputVector.magnitude > 0.1f)
        {
            transform.LookAt(transform.position + _inputVector.normalized);
            // ワールド座標を基準に、現在の座標からの相対的な移動
            transform.Translate(transform.forward * _moveSpeed * Time.deltaTime, Space.World);
        }
    }

    void FixedUpdate()
    {
        if(_inputVector.sqrMagnitude > 0 && currentState == State.Normal)
        {
            // 移動方向の決定 : Rotates the transform so the forward vector points at worldPosition.
            transform.LookAt(transform.position + _inputVector.normalized);
            if(_inputVector.magnitude > 0.1f)
            {
                _velocity += transform.forward * _moveSpeed;
                _rigidbody.velocity = _velocity;
            }
        }
    }

    public void SetState(State state)
    {
        currentState = state;
        if(state == State.Invisible)
        {
            _rigidbody.velocity = Vector3.zero;
        }
    }
}
