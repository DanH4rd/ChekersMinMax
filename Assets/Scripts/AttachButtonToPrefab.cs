using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttachButtonToPrefab : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Button btn1 = transform.Find("Checker").GetComponent<Button>();
        btn1.onClick.AddListener(() => UserInput.instance.checkerPicked(transform.gameObject));

        Button btn2 = transform.GetComponent<Button>();
        btn2.onClick.AddListener(() => UserInput.instance.fieldPicked(transform.gameObject));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
