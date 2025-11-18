using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextIntroduction : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] Animator Panel;
    [Header("Panels")]
    [SerializeField] GameObject PanelStart;
    [SerializeField] GameObject PanelWarning,PanelEndingPt1,PanelEndingPt2,PanelEndingPt3;
    [Header("Buttons")]
    [SerializeField] Button ButtonNext,ButtonStart;
    [Header("Audios & Times")]
    [SerializeField] AudioSource audioIntro;
    [Range(0.01f,100)]
    [SerializeField] float afterIntro=.1f, afterWarning=.1f, afterEndingPt1=.1f, afterEndingPt2 = .1f;
    [Header("Player")]
    public GameObject Cursor;
    IEnumerator Start()
    {
        //Ativa o painel de texto
        Panel.gameObject.SetActive(true);
        Panel.SetTrigger("Open");
        yield return new WaitForSeconds(.3f);
        //Solta o primeiro audio
        audioIntro.Play();
        yield return new WaitForSeconds(afterIntro);
        //Solta o painel de aviso
        PanelStart.SetActive(false);
        PanelWarning.SetActive(true);
        yield return new WaitForSeconds(afterWarning);
        //Solta o painel sobre movimentação
        PanelWarning.SetActive(false);
        PanelEndingPt1.SetActive(true);
        yield return new WaitForSeconds(afterEndingPt1);
        //Solta o painel sobre o botão
        PanelEndingPt1.SetActive(false);
        PanelEndingPt2.SetActive(true);
        ButtonNext.gameObject.SetActive(true);
        yield return new WaitForSeconds(afterEndingPt2);
        //Iniciar
        PanelEndingPt2.SetActive(false);
        PanelEndingPt3.SetActive(true);
        ButtonNext.interactable = true;
        Cursor.SetActive(true);
        StartCoroutine(ApresentationOfLuna());
    }
    [SerializeField] AudioSource LunaApresentation;

    IEnumerator ApresentationOfLuna()
    {
        yield return new WaitUntil(() => LunaApresentation.isPlaying);
        ActiveCursor(14.5f);
        StartCoroutine(ActiveButtonWithdelay(14.5f));
    }
  IEnumerator ActiveButtonWithdelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ButtonStart.gameObject.SetActive(true);
    }

    public void ActiveCursor(float delay)
    {
        StartCoroutine(CursorAction(delay, true));
    }
    public void DeactiveCursor(float delay)
    {
        StartCoroutine(CursorAction(delay, false));
    }
    IEnumerator CursorAction(float delay,bool ative)
    {
        yield return new WaitForSeconds(delay);
        Cursor.SetActive(ative);
    }
    public void LoadGameScene()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
