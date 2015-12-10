using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class AbilityButtonScript : MonoBehaviour, IPointerEnterHandler
{

    public Ability script;
    public Character myChar;
    public GridHandler map;

    public GameObject gm;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnPointerEnter(PointerEventData eventData)
    {
        gm.GetComponent<UIManager>().SetToolkitText("Abillity: " + script.Method.Name + ": " + Abilities.GetAbilityInfo(script.Method.Name, AbilityInfo.Description )+ "\nRange: " + Abilities.GetAbilityInfo(script.Method.Name, AbilityInfo.Range ));
    }
}
