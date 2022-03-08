using UnityEngine;

public class PointsMover : MonoBehaviour
{
    private ShaderScreenSizeRefresher _target;
    private bool _halt;
    
    private void Awake()
    {
        _target = GetComponent<ShaderScreenSizeRefresher>();

        aPos = _target.trianglePointA;
        bPos = _target.trianglePointB;
        cPos = _target.trianglePointC;
    }

    [SerializeField]
    private Vector2 aVel, bVel, cVel;

    private Vector2 aPos, bPos, cPos;
    

    private Vector2 PingPong2(Vector2 vec)
    {
        return new Vector2(Mathf.PingPong(vec.x, 1), Mathf.PingPong(vec.y, 1));
    }
    
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            _halt = !_halt;
        
        if(_halt) return;
        
        aPos += aVel * Time.deltaTime;
        bPos += bVel * Time.deltaTime;
        cPos += cVel * Time.deltaTime;
        
        _target.trianglePointA = PingPong2(aPos);
        _target.trianglePointB = PingPong2(bPos);
        _target.trianglePointC = PingPong2(cPos);
    }
}
