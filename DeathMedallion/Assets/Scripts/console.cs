using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class console : MonoBehaviour
{

    bool isInConsoleMode = false;
    [SerializeField] InputField consoleLine;
    [SerializeField] GameObject skeleton;
    [SerializeField] GameObject spider;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            ChangeConsoleState();
        }


        if (isInConsoleMode)
        {
            EventSystem.current.SetSelectedGameObject(consoleLine.gameObject, null);
            consoleLine.OnPointerClick(new PointerEventData(EventSystem.current));
            if (Input.GetKeyDown(KeyCode.Return))
            {
                var command = consoleLine.GetComponent<InputField>().text;
                consoleLine.GetComponent<InputField>().text = "";
                
                switch (command)
                {
                    case "killall":
                        foreach(Enemy enm in FindObjectsOfType<Enemy>())
                        {
                            enm.Health = 0;
                        }
                        break;
                    case "godmode":
                        GameObject.Find("Hero").GetComponent<Player>().SetGodMode(!GameObject.Find("Hero").GetComponent<Player>().godMode);
                        break;
                    case "kill":
                        Enemy nearestEnemy = new Enemy();
                        float minDistance = float.MaxValue;
                        foreach (Enemy enemy in FindObjectsOfType<Enemy>())
                        {
                            if(Vector2.Distance(enemy.gameObject.transform.position, GameObject.Find("Hero").transform.position) < minDistance && enemy.Health > 0)
                            {
                                minDistance = Vector2.Distance(enemy.gameObject.transform.position, GameObject.Find("Hero").transform.position);
                                nearestEnemy = enemy;
                            }
                        }
                        nearestEnemy.Health = 0;
                        break;
                    case "resurrect":

                        break;
                    case "spawn skeleton":
                        Instantiate(skeleton, (Vector2)GameObject.Find("Hero").transform.position + new Vector2(3, 0), Quaternion.identity);
                        break;
                }
                ChangeConsoleState();

            }
        }
    }
    void ChangeConsoleState()
    {
        Time.timeScale = Time.timeScale == 0 ? 1f : 0f;
        isInConsoleMode = !isInConsoleMode;
        Info.IsGameOn = !isInConsoleMode;
        consoleLine.gameObject.SetActive(isInConsoleMode);
    }
}
