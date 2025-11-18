using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class AstroChoice : MonoBehaviour
{
    [Header("Astros")]
    [SerializeField] List<GameObject> Astros;
    [Header("Audios & Tempos")]
    [SerializeField] List<AudioSource> audiosAbout;
    [Header("Eventos")]
    public int atualIndex = 0;
    [SerializeField] float delayToBigBang;
    [SerializeField] float delaytoLoadSkybox;
    [SerializeField] float loadingSkybox;
    public bool loadedSkybox;
    [Header("GameObject's effects")]
    [SerializeField] GameObject BigBangParticle;
    [SerializeField] GameObject particleMaterialDust;

    [Header("Astros & player")]

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
       Invoke(nameof(ActionBigBangEnd),4.81f);
        yield return new WaitForSeconds(delaytoLoadSkybox);
        loadedSkybox = true;
    
    }
   void ActionBigBangEnd(){
      foreach(GameObject astros in Astros)
            astros.SetActive(true);
            Destroy(BigBangParticle,.5f);
   }
    private void Update()
    {
        if (loadedSkybox)
        {
            particleMaterialDust.SetActive(true);
            loadingSkybox += .2f * Time.deltaTime;
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

        yield return new WaitForSeconds(1);
        atualIndex++;

        showAstro();
    }

    public void showAstro()
    {
       

        if (atualIndex > audiosAbout.Count - 1)
        {
            //Final da apresentação
            buttonsEnd.SetActive(true);
            cursor.SetActive(true);

            return;
        }
       
        if (atualIndex == audiosAbout.Count - 1)
        {
            //ultima fala, falando sobre plutão, asteroides e cometas
            Astros[atualIndex].GetComponent<AstroOrbit>().SetTransparency();
            StartCoroutine(ShowCometsAndAsteroids());
        }
        audiosAbout[atualIndex].Play();
                StartCoroutine(moveAstro(audiosAbout[atualIndex]));

        Astros[atualIndex].GetComponent<AstroOrbit>().MoveToSun(audiosAbout[atualIndex].clip.length);

    }

    IEnumerator moveAstro(AudioSource audio)
    {
        yield return new WaitUntil(() => audio.isPlaying == false);
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
        Destroy(cometa, 30);
        yield return new WaitForSeconds(4);
        //solta o asteroide
        asteroid.SetActive(true);
        Destroy(asteroid, 60);
        yield return new WaitForSeconds(6);
        asteroidsRain.SetActive(true);
        Destroy(asteroidsRain, 54);
    }
public GameObject asteroidsRain;
    public void EndOrRestart(int index)
    {
        if (index == -1)
            Application.Quit();
        else
            SceneManager.LoadSceneAsync(index);
    }
}
