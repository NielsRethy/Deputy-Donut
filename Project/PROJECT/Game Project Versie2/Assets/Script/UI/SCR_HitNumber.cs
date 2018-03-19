using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SCR_HitNumber : MonoBehaviour {
    public GameObject TextPrefab;
    public GameObject Parent;
    public Transform Truck;
    public Camera CamTruck;
    public Text TruckText;

    public float TimeActive = 1;
    private float Active = 0;
    public float Offset = 2;
    private void Start()
    {
        TruckText.gameObject.active = false;

    }
    private void Update()
    {
        if(Active>0)
        {
            Active += Time.deltaTime;
            if (Active>0.5)
            {
                Color col = TruckText.color;

                col.a -= ((float)2 / TimeActive) * Time.deltaTime;
                TruckText.color = col;
            }
            if (Active>TimeActive)
            {
                Active = 0;
                TruckText.gameObject.active = false;
            }
        }
    }

    // Update is called once per frame
    public void DamageNumber (int number)
    {

            GameObject tempFloatingDamage = Instantiate(TextPrefab);
            tempFloatingDamage.transform.SetParent(Parent.transform, false);
            tempFloatingDamage.layer = 9;
            tempFloatingDamage.transform.Translate(new Vector3(0, Offset, 0));
            string text = number.ToString();
            tempFloatingDamage.GetComponent<SCR_LookAt>().Target = CamTruck.gameObject;

        tempFloatingDamage.GetComponent<TextMesh>().text = text;
            float dmg = 28 + number / 4 ;
        tempFloatingDamage.GetComponent<SCR_LookAt>().Target = CamTruck.gameObject;

        tempFloatingDamage.GetComponent<TextMesh>().fontSize =(int)dmg ;

        Active = Time.deltaTime;
        TruckText.text = number.ToString();
        TruckText.gameObject.active = true;

        Color col = TruckText.color;
        col.a = 1;
        TruckText.color = col;







    }
}
