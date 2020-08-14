using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class JoinButton : MonoBehaviour
{
	private Text buttonText;
	public int numberofmatchs; 
	private MatchInfoSnapshot match;
	public MatchInfoSnapshot[] matchs;
	public Text inputField;

	private void Awake()
	{
		buttonText = GetComponentInChildren<Text>();
	
	//	GetComponent<Button>().onClick.AddListener(JoinMatch);
		
	}

	public void Initialize(MatchInfoSnapshot match, Transform panelTransform)
	{
		this.match = match;
		buttonText.text = match.name;
		transform.SetParent(panelTransform);
		transform.localScale = Vector3.one;
		transform.localRotation = Quaternion.identity;
		transform.localPosition = Vector3.zero;
		numberofmatchs++;
	}

	public void JoinMatch()
	{
		
		FindObjectOfType<CustomNetworkManager>().JoinMatch(match);
	}

	public void Joinwithname()
    {
		matchs = null; 
		for (int i = 0; i< numberofmatchs; i++)
        {
		
        }
    }



	public void JoinWithNameOfRoom( string nameOfRom)
    {
		FindObjectOfType<CustomNetworkManager>().JoinMatchs(match,nameOfRom);
	}
	
}