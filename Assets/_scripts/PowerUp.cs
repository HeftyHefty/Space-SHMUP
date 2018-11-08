using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Header("Ser in Inspector")]
    public Vector2 rotMinMax = new Vector2(15, 19);
    public Vector2 driftMinMax = new Vector2(0.25f, 2);
    public float lifeTime = 6f;
    public float fadeTime = 4f;

    [Header("Set Dynamically")]
    public WeaponType type;
    public GameObject cube;
    public TextMesh letter;
    public Vector3 rotPerSecond;
    public float birthTime;

    private Rigidbody rigid;
    private BoundsCheck bndCheck;
    private Renderer cubeRend;

	void Awake () 
    {
        cube = transform.Find("Cube").gameObject;

        letter = GetComponent<TextMesh>();
        rigid = GetComponent<Rigidbody>();
        bndCheck = GetComponent<BoundsCheck>();
        cubeRend = cube.GetComponent<Renderer>();

        //Set random velocity
        Vector3 vel = Random.onUnitSphere;

        vel.z = 0;
        vel.Normalize();

        vel *= Random.Range(driftMinMax.x, driftMinMax.y);
        rigid.velocity = vel;

        //Set the rotation of this GameObject to R:[0,0,0]
        transform.rotation = Quaternion.identity;
        //quaterion.id is equal to no rotation

        rotPerSecond = new Vector3(Random.Range(rotMinMax.x, rotMinMax.y),
                                     Random.Range(rotMinMax.x, rotMinMax.y),
                                     Random.Range(rotMinMax.x, rotMinMax.y));

        birthTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () 
    {
        cube.transform.rotation = Quaternion.Euler(rotPerSecond * Time.time);

        //fade out the power up over time
        float u = (Time.time - (birthTime + lifeTime)) / fadeTime;

        if (u >= 1)
        {
            Destroy(this.gameObject);
            return;
        }

        if (u > 0)
        {
            Color c = cubeRend.material.color;
            c.a = 1f - u;
            cubeRend.material.color = c;
            //fade the letter as well, just not as much
            c = letter.color;
            c.a = 1f - (u * 0.5f);
            letter.color = c;
        }
        if (!bndCheck.isOnScreen)
        {
            //if powerup has left the screen, destroy it
            Destroy(gameObject);
        }
	}
    public void SetType(WeaponType wt)
    {
        WeaponDefinition def = Main.GetWeaponDefinition(wt);
        //set color of the cube child
        cubeRend.material.color = def.color;
        letter.text = def.letter;
        type = wt;
    }
    public void AbsorbedBy (GameObject target)
    {
        //this is called by the hero class whe powerup is collected
        Destroy(this.gameObject);
    }
}
