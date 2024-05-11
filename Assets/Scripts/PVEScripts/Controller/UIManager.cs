using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Text startTextButton;
    public Text loadingText;
    public GameObject loadingPanel;
    public Slider loadingSlider;
    private bool bigger;
    private bool smaller;

    private void Awake()
    {
        Time.timeScale = 1;
    }
    private void Update()
    {
        ChangeStartTextAlpha();
    }

    public void OldGame()
    {
        StartCoroutine(LoadLevel());
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadLevel()
    {
        loadingPanel.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        operation.allowSceneActivation = false;
        while(!operation.isDone)
        {
            loadingSlider.value = operation.progress;
            loadingText.text = "Loading~" + operation.progress * 100 + "%";
            if(operation.progress>=0.9f)
            {
                loadingSlider.value = 1;
                loadingText.text= "Loading~" +  "100%";
                if(Input.anyKeyDown)
                {
                    operation.allowSceneActivation = true;
                }
            }
            yield return null;
        }
    }
    void ChangeStartTextAlpha()
    {
        float alpha = startTextButton.color.a;
        if(Mathf.Abs(alpha-1)<0.01f)
        {
            bigger = false;
            smaller = true;
        }
        if(Mathf.Abs(alpha-0.4f)<0.01f)
        {
            bigger = true;
            smaller = false;
        }
        if(alpha>0.4&&smaller)
        {
            alpha-=Time.deltaTime;
            startTextButton.color=new Color(1,1,1,alpha);
        }
        else if(alpha<1&&bigger)
        {
            alpha+=Time.deltaTime;
            startTextButton.color = new Color(1, 1, 1, alpha);
        }
    }
}
