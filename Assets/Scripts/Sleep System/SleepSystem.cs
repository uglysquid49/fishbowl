using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;

public class SleepSystem : MonoBehaviour
{
    [Header("Interaction")]
    public bool Caninteract = true;
    [SerializeField] private GameObject BlackPanel;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI Subtext;
    private string Holder;
    private float WriteSpeed = 0.25f;
    
    void Update()
    {
        

        if (Caninteract == true)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 5f))
            {
                if (hit.collider.CompareTag("Bed"))
                {
                    if (Input.GetKey("Interact"))
                    {
                        // sleep
                        StartCoroutine(SleepCO());
                    }
                }
            }
        }
    }

    IEnumerator SleepCO()
    {
        BlackPanel.SetActive(true);
        Caninteract = false;

        yield return new WaitForSeconds(1f);

        // write night

        Subtext.text = "";
        Holder = "Night 12";
        foreach(char c in Holder)
        {
            Subtext.text += c;
            yield return new WaitForSeconds(WriteSpeed);
        }

        // write night

        yield return new WaitForSeconds(1f);

        // loading "next scene"
        Debug.Log("loaded the next scene");
    }
}
