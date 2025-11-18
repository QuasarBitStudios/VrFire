using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class AstroOrbit : MonoBehaviour
{

    [SerializeField] float Speed;
    [SerializeField] Transform Sun,CloneOnHand;
    [SerializeField] Vector3 Axis;
    public bool NotTransformParent;
    public enum Tipo {Segue,Rotaciona}
    public Tipo OrbitType;
    private void Start()
    {
          myOldScale = transform.localScale;
        if (OrbitType == Tipo.Segue)
        {
            //GetComponent<Animator>().enabled = false;
           if(NotTransformParent == false)
            Sun.transform.parent = null;
            transform.localScale = Sun.localScale;
            if (Sun.GetComponent<AstroOrbit>() != null)
                Sun.GetComponent<AstroOrbit>().EnableTrail();
        }
        float tempo = CalcularTempoDaVolta(Speed);
        if (trail != null)
            trail.time = tempo+12;
    }
    float CalcularTempoDaVolta(float velocidadeAngular)
    {
        return 360f / velocidadeAngular;
    }
public bool MovingoToCam;
public bool MovingToOrbit;
public float realSpeed;
public GameObject Particle;
public bool Sol, MovendoInfinito;
    void Update()
    {
        if (MovendoInfinito)
        {
            transform.Translate(0,0, Speed *3* Time.deltaTime);
            return;
        }

        if(MovingoToCam){
             transform.position = Vector3.MoveTowards(transform.position, CloneOnHand.transform.position, realSpeed * Time.deltaTime);
        if(trail != null){
        trail.enabled = false;
        //trail.Clear();
        }
           transform.localScale = CloneOnHand.localScale;
           if(Particle != null)
           Particle.SetActive(false);
return;
        }
        
        if(MovingToOrbit){
         transform.position = Vector3.MoveTowards(transform.position, myOldPosition, realSpeed * Time.deltaTime);
   if(transform.position == myOldPosition){
       if(Particle != null)
           Particle.SetActive(true);
           EnableTrail();
    MovingToOrbit = false;
    return;
   }

        }
        
              transform.localScale = myOldScale;
        if (OrbitType == Tipo.Segue)
        {
            transform.position = Vector3.MoveTowards(transform.position, Sun.position, Speed * Time.deltaTime);
       
        }
        else if (OrbitType == Tipo.Rotaciona && Sun.GetComponent<AstroOrbit>().MovingoToCam==false && Sun.GetComponent<AstroOrbit>().MovingToOrbit == false)
            transform.RotateAround(Sun.position, Axis, Speed * Time.deltaTime);

    }
    public TrailRenderer trail;
    public void EnableTrail()
    {
        if (trail != null)
            trail.enabled = true;
    }
    public void MoveToSun(float delay){
        if(Sol)return;
        myOldPosition =transform.position;
           transform.rotation = CloneOnHand.transform.rotation;
        MovingoToCam = true;
        StartCoroutine(Movin(delay));
    }
    public Vector3 myOldPosition, myOldScale, myWay;
    IEnumerator Movin(float delay)
    {
        yield return new WaitForSeconds(delay);
        MovingToOrbit = true;
        MovingoToCam = false;
    }
   public void SetTransparency()
    {
        StartCoroutine(DelayToInfinito());
    }
   IEnumerator DelayToInfinito()
    {
        yield return new WaitForSeconds(11);
        MovendoInfinito = true;
    }
}
