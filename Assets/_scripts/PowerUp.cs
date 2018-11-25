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

        //Set a random velocity
        Vector3 vel = Random.onUnitSphere;

        vel.z = 0;
        vel.Normalize();

        vel *= Random.Range(driftMinMax.x, driftMinMax.y);
        rigid.velocity = vel;

        //Sets rotation of this GameObject to R:[0,0,0]
        transform.rotation = Quaternion.identity;

        rotPerSecond = new Vector3(Random.Range(rotMinMax.x, rotMinMax.y),
                                     Random.Range(rotMinMax.x, rotMinMax.y),
                                     Random.Range(rotMinMax.x, rotMinMax.y));

        birthTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () 
    {
        cube.transform.rotation = Quaternion.Euler(rotPerSecond * Time.time);

        //power up gets faded-out over time
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
            //fades letters
            c = letter.color;
            c.a = 1f - (u * 0.5f);
            letter.color = c;
        }
        if (!bndCheck.isOnScreen)
        {
            //when powerup leaves the bounds it is destroyed
            Destroy(gameObject);
        }
	}
    public void SetType(WeaponType wt)
    {
        WeaponDefinition def = Main.GetWeaponDefinition(wt);
        cubeRend.material.color = def.color;
        letter.text = def.letter;
        type = wt;
    }
    public void AbsorbedBy (GameObject target)
    {
        //called by hero class when powerup collected
        Destroy(this.gameObject);
    }
}
