using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Train : MonoBehaviour {
    //SPLINE
    public int StartIndex = 0;
    public List<Transform> Points = new List<Transform>();
    public SCR_MakePath MakePathScript;
    public int inter;
    public bool Loops = true;
    public GameObject[] CalPoints;
    private int Interpolation;

    //TRAIN
    public float speed = 5;
    public float WeakForce = 100;
    public float StrongForce = 2000;
    private int _CurentWayPoint = 0;
    private bool _OnTrack = true;
    private Vector3 _Old,_New;

    void Start()
    {
        Points = MakePathScript.GetPath();

        _CurentWayPoint = StartIndex;
        //StartFromIndex();
        
        Interpolation = inter +1;

        if (Loops)
        {
            CalPoints = new GameObject[Points.Count * Interpolation ];
        }
        else
        {
            CalPoints = new GameObject[Points.Count * Interpolation - Interpolation];

        }
        for (int i = 0; i < CalPoints.Length; i++)
        {
            CalPoints[i] = new GameObject();
        }



        //calculate points
        int index = 0;
        for (int i = 0; i < Points.Count; i++)
        {        
            if ((i == 0 || i == Points.Count - 2 || i == Points.Count - 1) && !Loops)
            {
                continue;
            }
            else
            {
                Vector3 p0 = Points[ClampListPos(i - 1)].position;
                Vector3 p1 = Points[i].position;
                Vector3 p2 = Points[ClampListPos(i + 1)].position;
                Vector3 p3 = Points[ClampListPos(i + 2)].position;
                Vector3 lastPos = p1;

                float resolution = 1 / (float)Interpolation;

                int loops = Mathf.FloorToInt(1f / resolution);
                
                for (int b = 1; b <= loops; b++)
                {
                    float t = b * resolution;

                    Vector3 newPos = GetCatmullRomPosition(t, p0, p1, p2, p3);
                    CalPoints[index].transform.position = newPos;
                    index++;
                    lastPos = newPos;
                }
            }           
        }

        int previousIndex = (_CurentWayPoint - 1)% CalPoints.Length;
        int nextIndex = (_CurentWayPoint + 1)% CalPoints.Length;

        transform.position = CalPoints[_CurentWayPoint].transform.position;
         _Old = CalPoints[ClampListPos(previousIndex)].transform.position - CalPoints[_CurentWayPoint].transform.position;
         _New= CalPoints[_CurentWayPoint].transform.position - CalPoints[nextIndex].transform.position;
    }
    void Update ()
    {
       
        Vector3 interpolated = new Vector3( 0,0,0);
        float length,currLen;
        if (_OnTrack)
        {
            length = (CalPoints[_CurentWayPoint].transform.position - CalPoints[clampListCal(_CurentWayPoint - 1)].transform.position).magnitude;
            currLen = (transform.position - CalPoints[clampListCal( _CurentWayPoint-1)].transform.position).magnitude;
            interpolated = Vector3.Lerp(_Old, _New, currLen / length);
            float step = speed * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, CalPoints[_CurentWayPoint].transform.position, step);
            transform.rotation = Quaternion.LookRotation(interpolated);

            if (transform.position == CalPoints[_CurentWayPoint].transform.position)
            {
                _CurentWayPoint++;
                 
                    if (_CurentWayPoint > CalPoints.Length-1)
                    {
                       if (Loops)
                        {
                            _CurentWayPoint = 0;
                        }
                       else
                       {
                        _CurentWayPoint = 1;
                        transform.position = CalPoints[0].transform.position;

                       }
                    }
                _Old = _New;
                _New = CalPoints[clampListCal( _CurentWayPoint-1)].transform.position - CalPoints[clampListCal( _CurentWayPoint +1)].transform.position;
            }
            }
        
      


	}

    private void StartFromIndex()
    {
        List<Transform> nodes = Points;
        int index = StartIndex;
        for (int i = 0; i < nodes.Count; i++)
        {
            Points[i] = nodes[index];
            ++index;
            if (index >= nodes.Count)
            {
                index = 0;
            }

        }
    }

    private void OnDrawGizmos()
    {
        Interpolation = inter +1;

        Points = MakePathScript.GetPath();

        Gizmos.color = Color.white;
        for (int i = 0; i < Points.Count; i++)
        {
            if ((i == 0 || i == Points.Count - 2 || i == Points.Count - 1) && !Loops)
            {
                continue;
            }
            ShowSpline(i);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.other.tag == "PoliceCar" || collision.other.tag == "DonutTruck")
        {

            Vector3 posTr = transform.position;
            Vector3 posCol = collision.other.transform.position;
            Vector3 impact = (posCol - posTr).normalized;
            impact.y = 0;
            float angle = Vector3.Angle(-impact, transform.forward);
            if (angle < 20)
            {
                impact = collision.other.transform.position - transform.position;

                collision.other.GetComponent<Rigidbody>().AddForce(impact * StrongForce, ForceMode.Impulse);
            }
            else
            {
                collision.other.GetComponent<Rigidbody>().AddForce(impact * WeakForce, ForceMode.Impulse);

            }
        }
    }
    void ShowSpline(int pos)
    {
        Vector3 p0 = Points[ClampListPos(pos - 1)].position;
        Vector3 p1 = Points[pos].position;
        Vector3 p2 = Points[ClampListPos(pos + 1)].position;
        Vector3 p3 = Points[ClampListPos(pos + 2)].position;

        Vector3 lastPos = p1;

        float resolution =1/(float)Interpolation;

        int loops = Mathf.FloorToInt(1f / resolution);
        int index = 0;
        for (int i = 1; i <= loops; i++)
        {
            float t = i * resolution;
            Vector3 newPos = GetCatmullRomPosition(t, p0, p1, p2, p3);
            Gizmos.DrawLine(lastPos, newPos);
            lastPos = newPos;
        }
    }
    int ClampListPos(int pos)
    {
        if (pos < 0)
        {
            pos = Points.Count - 1;
        }
        if (pos > Points.Count)
        {
            pos = 1;
        }
        else if (pos > Points.Count - 1)
        {
            pos = 0;
        }
        return pos;
    }
    int clampListCal(int pos)
    {
        if (pos < 0)
        {
            pos = CalPoints.Length - 1;
        }
        if (pos > CalPoints.Length)
        {
            pos = 1;
        }
        else if (pos > CalPoints.Length - 1)
        {
            pos = 0;
        }
        return pos;
    }
    Vector3 GetCatmullRomPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        Vector3 a = 2f * p1;
        Vector3 b = p2 - p0;
        Vector3 c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
        Vector3 d = -p0 + 3f * p1 - 3f * p2 + p3;
        Vector3 pos = 0.5f * (a + (b * t) + (c * t * t) + (d * t * t * t));

        return pos;
    }
}
