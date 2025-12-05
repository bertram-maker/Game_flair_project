using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//main
public class BallController : MonoBehaviour
{
    //My Rigibody
    public Rigidbody2D RB;
    //My starting velocity. This should be set in the editor
    public Vector2 StartVel;
    //My starting position, where I respawn into. I set this in Start()
    public Vector3 StartPos;
    //This velocity controls how fast the ball should go
    private Vector2 IncreasedVel;
    //Is the cap for how fast the ball can go
    public float BallSpeedCap;
    //the cap for how far the angle can be
    public float angleCap;
    //the cap for the paddle speed
    public float PaddleSpeedCap;
    
    //paddle controller as a variable
    public PaddleController PC;
    
    //saves the score text as a variable
    public TextMeshPro ScoreDisplay;
    //score
    public static float score = 0;
    //score multiplier
    private float mult;
    private float startMult = 0;

    //saves the orginal angle
    private float StartAngle;
    //saves the original paddle speed
    private float PaddleSpeed;
    
    void Start()
    {
        //I record where I started, so I can respawn there
        StartPos = transform.position;
        //I check my StartVelocity variable and set that to be my velocity
        RB.linearVelocity = StartVel;
        IncreasedVel = StartVel;

        StartAngle = PC.angle;
        PaddleSpeed = PC.Speed;

        ScoreDisplay.text = "Score: " + score;
        mult = startMult;
    }

    void Update()
    {
        //If I'm off-screen, I respawn with my initial position & speed
        if (transform.position.y < -10)
        {
            transform.position = StartPos;
            RB.linearVelocity = StartVel;
            PC.angle = StartAngle;
            PC.Speed = PaddleSpeed;
            mult = startMult;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //If I hit something, I'm going to bounce. Let's calculate my new velocity
        Vector2 vel = RB.linearVelocity;
        
        //Did I hit the paddle?
        PaddleController pc = other.gameObject.GetComponent<PaddleController>();
        if (pc != null)
        { 
            //If so, I should bounce back up
            vel.y *= -1;
            //I should also be aimed based on where I hit the paddle
            //I ask the paddle to calculate this for me
            vel.x = pc.BounceAngle(this);
            
            //make ball speed up vertically
            if (vel.y <= BallSpeedCap && vel.y > (BallSpeedCap * -1))
            {
                if (vel.y > 0)
                {
                    vel.y += 1;
                }
                else
                {
                    vel.y -= 1;
                }
            }

            //make the ball speed up horizontally
            if (vel.x <= BallSpeedCap && vel.x > (BallSpeedCap * -1))
            {
                //angle
                if (PC.angle <= angleCap)
                {
                    PC.angle += 1;
                }
                
                //paddle speed. Maybe move this to the end and make it so that when vel.x/vel.y gets high enough it speeds up the paddle
                if (PC.Speed < PaddleSpeedCap)
                {
                    PC.Speed += 1;
                }

                //x velocity
                if (vel.x > 0)
                {
                    vel.x += 1;
                }
                else
                {
                    vel.x -= 1;
                }
            }
            mult = startMult;
        }

        //Did I hit a brick?
        BrickController bc = other.gameObject.GetComponent<BrickController>();
        if (bc != null)
        {
            
            //If so, I bounce vertically
            //MINOR BUG: if I hit a brick from the side I should bounce horizontally
            vel.y *= -1;
            //Also I tell the brick to break
            bc.Break();
            mult += 1;
            score += 100 * mult;
            ScoreDisplay.text = "Score: " + score;
        }

        //If I hit a vertical wall, I bounce horizontally
        if (other.gameObject.CompareTag("VWall"))
        {
            vel.x *= -1;
        }
        
        //If I hit a horizontal wall (the roof), I bounce vertically
        if (other.gameObject.CompareTag("HWall"))
        {
            vel.y *= -1;
        }

        //Now that I've calculated any bouncing I need to do, plug that into my rigidbody
        RB.linearVelocity = vel;
    }
}
