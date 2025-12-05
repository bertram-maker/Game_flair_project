using UnityEngine;

public class BlockerController : MonoBehaviour
{
    public float speed;
    public float start;
    public float end;
    private float negSpeed;
    private float posSpeed;
    public PaddleController PC;
    
    
    void Start()
    {
        posSpeed = speed;
        negSpeed = speed * -1;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        
        //moves block to the left
        if (transform.position.x >= start)
        {
            speed = negSpeed;
        }
        
        //moves block to the right
        if (transform.position.x <= end)
        {
            speed = posSpeed;
        }
        
        pos += new Vector3(speed * Time.deltaTime, 0, 0);
        
        //Plug in the position I calculated to my transform
        transform.position = pos;
    }
    
    public float BounceAngle(BallController ball)
    {
        //ball now aims
        return (ball.transform.position.x - transform.position.x) * PC.angle;
    }
}
