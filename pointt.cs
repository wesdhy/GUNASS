using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class pointt : MonoBehaviour
{

    public Text m_pointtext;

    public static int points = 0;

    public static void Onpoint(int pt)
    {
        points += pt;


        if (points== 12)
        {
            SceneManager.LoadScene("bb");
        }
        if (points == 28)
        {
            SceneManager.LoadScene("Demo");
        }


    }


    void Update()
    {
        m_pointtext.text = points.ToString();
    }
}
