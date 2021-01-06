using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerOrbs : MonoBehaviour
{
    [Required][SerializeField] private PlayerMainController playerController;
    [SerializeField] private GameObject orbTemplate;
    [SerializeField] private RectTransform orbsParent;
    private List<Image> orbImages = new List<Image>();

    private void Start()
    {
        int orbs = playerController.maxPowerOrbs;
        for (int i = 0; i < orbs; i++)
        {
            GameObject orbObj = Instantiate(orbTemplate, orbsParent);
            Image orbImage = orbObj.transform.Find("Power Orb Meter").GetComponent<Image>();
            orbImages.Add(orbImage);
        }

        SetOrbsFill(playerController.currentPowerOrbMeter);
        playerController.hasChangedPowerOrbMeter += SetOrbsFill;
    }

    public void SetOrbsFill(int totalOrbsValue)
    {
        for (int i = 0; i < orbImages.Count; i++)
        {
            Image currentOrb = orbImages[i];
            int orbMinValue = PlayerMainController.ORB_CAPACITY * i;

            int orbValue = totalOrbsValue - orbMinValue;

            if (orbValue > PlayerMainController.ORB_CAPACITY)
                orbValue = PlayerMainController.ORB_CAPACITY;

            float orbFill = (float)orbValue / PlayerMainController.ORB_CAPACITY;

            currentOrb.fillAmount = orbFill;

        }
    }
}
