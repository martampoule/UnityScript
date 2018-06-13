using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tasto : MonoBehaviour {

	private const int NUM_GIORNI = 7;

	private float scaling = 0.0003f;
	private float damping = 0.05f;
	private float ds = 0;
	private int currentDataSet = 0;

	string[] text = {
		"03.05.2018",     // 0
		"04.05.2018",     // 1
		"05.05.2018",     // 2
		"06.05.2018", 
		"07.05.2018",
		"08.05.2018",
		"09.05.2018",
	};
		
	void Start () {
		LoadData (currentDataSet);
		AggiornaLabel(currentDataSet);
		AggiornaImg(currentDataSet);
	}
		
	void AggiornaImg(int t){
		string path = "Img/img" + t.ToString();
		Material mat = (Material)Resources.Load(path, typeof(Material));
		GameObject.Find ("Img").GetComponent<MeshRenderer>().material = mat;
	}
		
	void AggiornaLabel(int t){
		TextMesh textObject = GameObject.Find("MyText").GetComponent<TextMesh>();
		textObject.text = text[t];
	}

	void LoadData(int t){
		string path = "Data/out" + t.ToString();
		TextAsset data = (TextAsset)Resources.Load(path, typeof(TextAsset));
		ds = ParseData(data);
	}


	float ParseData(TextAsset data){
		int value = 0;
		string[] lines = data.text.Split ('\n');
		foreach (string line in lines) { 
			string[] chunks = line.Split('\t');

			string k = chunks [0];
			if (k == this.name){
				value = int.Parse (chunks [1]);
				break;
			}
		}			
		return Mathf.Max(value * scaling, 0.001f);
	}

	void Update () {

		Vector3 s = this.transform.localScale;
		s.y += (ds - s.y) * damping;

		this.transform.localScale = s;

		// calcola l'altezza (non scale)
		//float h = this.GetComponents<Renderer> () [0].bounds.size.y;

		//Debug.Log (h);

		// allinea il box in basso (non in centro)
		Vector3 p = this.transform.position;
		p.y = this.transform.lossyScale.y / 2.0f;
		//p.y = GetComponent<BoxCollider>().bounds.extents.y / 2.0f;
		this.transform.position = p;

	}


	void OnGUI(){
		Event e = Event.current;
		if (e.type == EventType.MouseDown){			
			currentDataSet = (currentDataSet + 1) % NUM_GIORNI;

			// carica i dati per i tasti:
			LoadData (currentDataSet);
			AggiornaLabel(currentDataSet);
			AggiornaImg(currentDataSet);
		}			
	}
}
