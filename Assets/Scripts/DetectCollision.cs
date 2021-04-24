using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    Rigidbody _rb;
    [SerializeField] SamplePlayerController samplePlayerScript;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            if(_rb.detectCollisions)
            {
                Invisible(true);
            }
            else if(!_rb.detectCollisions)
            {
                Invisible(false);
            }
        }
    }

    void OnCollisionStay(Collision collision)
    {
        // Debug.Log("衝突検出中");
    }

    // Forexample, Let animation control the rigidbody and ignore collisions.
    void DisableRagdoll(bool detectCollisions = true)
    {
        _rb.isKinematic = !detectCollisions;
        _rb.detectCollisions = detectCollisions;
    }

    // 衝突判定を無視して透明状態となり、壁を通り抜けるようにしたい時
    // 衝突検出(detectCollisions)を有効にするかどうか（デフォルトでは常に有効）
    void Invisible(bool detectCollisions)
    {
        _rb.isKinematic = detectCollisions;
        _rb.detectCollisions = !detectCollisions;
        if(detectCollisions)
        {
            samplePlayerScript.SetState(SamplePlayerController.State.Invisible);
        }
        else if(!detectCollisions)
        {
            samplePlayerScript.SetState(SamplePlayerController.State.Normal);
        }
    }
}
