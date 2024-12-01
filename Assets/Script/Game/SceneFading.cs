using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFading : MonoBehaviour
{
    public Texture2D fadeOutTexture;    
    public float dafingSpeed = 0.8f;      

    private int depthToDraw = -1000;      
    private float alpha = 1.0f;         
    private int fadeDirection = -1;           

    void Start()
    {
        SceneManager.sceneLoaded += SceneManagerSceneLoaded; ;
    }

    void OnGUI()
    {
        alpha += fadeDirection * dafingSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = depthToDraw;                                                              
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);       
    }

    //плавный переход через текстуру
    public float StartFading(int direction)
    {
        fadeDirection = direction;
        return (dafingSpeed);
    }

    //запускаем
    void SceneManagerSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartFading(-1);
    }


}
