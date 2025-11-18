using System.Collections;
using System.Collections.Generic;
using Unity.Loading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class AstroApresentation : MonoBehaviour
{
    [Header("Astros")]
    [SerializeField] List<GameObject> Astros;
    [Header("Audios & Tempos")]
    [SerializeField] List<AudioSource> audiosAbout;
    [Header("Eventos")]
    public int atualIndex=0;
    [SerializeField] float delayToBigBang;
    [SerializeField] float delaytoLoadSkybox;
    [SerializeField] float loadingSkybox;
    public bool loadedSkybox;
    [Header("GameObject's effects")]
    [SerializeField] GameObject BigBangParticle;
    [SerializeField] GameObject particleMaterialDust;
    [SerializeField] PostProcessVolume fxEffect;

    [Header("Terrain, Luna & player")]
    [SerializeField] GameObject terrain;
    [SerializeField] Animator luna;
    [SerializeField] GameObject camPlayer;
    
    [Header("Efeitos e hud")]
    [SerializeField] GameObject cometa;
    [SerializeField] GameObject asteroid;
    [SerializeField] GameObject buttonsEnd;
    [SerializeField] GameObject cursor;

    private void Awake()
    {
        RenderSettings.skybox.SetFloat("_Exposure", 0);
    }
    IEnumerator Start()
    {
       
        yield return new WaitForSeconds(1);
        audiosAbout[0].Play();
        //Chama o contador de evento, pra depois que ele terminar de tocar o audio de intro
        //ele chame o audio e a a��o do sol
        StartCoroutine(afterAudioPlayEvent(audiosAbout[0]));
        StartCoroutine(Bigbang());
    }
    IEnumerator Bigbang()
    {
        yield return new WaitForSeconds(delayToBigBang);
        BigBangParticle.SetActive(true);
       
        yield return new WaitForSeconds(delaytoLoadSkybox);
        loadedSkybox = true;
        fxEffect.enabled = false;
    }
    public Vector3 positionCambaixa;
    private void Update()
    {
        if (loadedSkybox)
        {
            particleMaterialDust.SetActive(true);
            loadingSkybox += .2f*Time.deltaTime;
            RenderSettings.skybox.SetFloat("_Exposure", loadingSkybox);
            DynamicGI.UpdateEnvironment(); // Atualiza ilumina��o global
            if (loadingSkybox >= 1)
            {
                loadedSkybox = false;
              
                
            }
        }
    }
  

    /// <summary>
    /// Informe o audio que esta sendo escutado e o evento que ser� chamado ap�s o fim desse audio
    /// </summary>
    /// <param name="audio"></param>
    /// <param name="evento">evento que ser� chamado</param>
    /// <returns></returns>
    IEnumerator afterAudioPlayEvent(AudioSource audio)
    {
        yield return new WaitUntil(() => audio.isPlaying == false);
        luna.SetTrigger("Segurando");
        yield return new WaitForSeconds(1);
        atualIndex++;
        if (camPlayer.transform.position != positionCambaixa){
              luna.gameObject.SetActive(true);
                terrain.SetActive(true);
            camPlayer.transform.position = positionCambaixa;
        }
        showAstro();
    }

    public void showAstro()
    {
        if (atualIndex > audiosAbout.Count-1){
            //Final da apresentação
            buttonsEnd.SetActive(true);
            cursor.SetActive(true);
                 luna.SetTrigger("End");
            return;
        }
        if(atualIndex == audiosAbout.Count-1){
            //ultima fala, falando sobre plutão, asteroides e cometas
             StartCoroutine(ShowCometsAndAsteroids());
        }
        luna.SetTrigger("Talking");
        Astros[atualIndex].SetActive(true);
        audiosAbout[atualIndex].Play();
        Astros[atualIndex].GetComponent<Animator>().SetTrigger("Show");
        StartCoroutine(moveAstro(audiosAbout[atualIndex]));
    }

    IEnumerator moveAstro(AudioSource audio)
    {
        yield return new WaitUntil(() => audio.isPlaying == false);
          luna.SetTrigger("Segurando");
        yield return new WaitForSeconds(1);
        Astros[atualIndex].GetComponent<AstroOrbit>().enabled = true;
        //Chegou ao local
        yield return new WaitForSeconds(1);
        //A��o de continuar
        atualIndex++;
        showAstro();
    }

    IEnumerator ShowCometsAndAsteroids()
    {
        yield return new WaitForSeconds(14);
        //Solta o cometa, com calda de gelo
        cometa.SetActive(true);
        Destroy(cometa,10);
        yield return new WaitForSeconds(4);
        //solta o asteroide
        asteroid.SetActive(true);
        Destroy(asteroid,10);
    }

    public void EndOrRestart(int index){
        if(index == -1)
        Application.Quit();
        else
        SceneManager.LoadSceneAsync(index);
    }
}
