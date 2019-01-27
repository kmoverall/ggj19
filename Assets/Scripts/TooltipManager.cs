using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{
    public GameObject tooltipPanel;
    public Text tooltipText;
    public Canvas mainCanvas;

    public GameObject launchMenu;
    public Text launchHeader;

    private static GameObject TooltipPanel;
    public static Text TooltipText;
    public static Canvas MainCanvas;

    public static GameObject LaunchMenu;
    public static Text LaunchHeader;

    void Awake()
    {
        TooltipPanel = tooltipPanel;
        TooltipText = tooltipText;
        MainCanvas = mainCanvas;
        LaunchMenu = launchMenu;
        LaunchHeader = launchHeader;
    }

    private void Update()
    {
        TooltipPanel.GetComponent<TooltipPlacer>().basePosition = Input.mousePosition;
    }

    public static void Show(string message)
    {
        TooltipPanel.SetActive(true);
        TooltipText.text = message;
    }

    public static void Hide()
    {
        TooltipPanel.SetActive(false);
    }

    public static void ShowLaunch(string name, Vector3 pos)
    {
        LaunchMenu.GetComponent<TooltipPlacer>().basePosition = pos;
        LaunchMenu.SetActive(true);
        LaunchHeader.text = name;
    }
}
