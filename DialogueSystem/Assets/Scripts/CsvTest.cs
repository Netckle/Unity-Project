using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CsvTest : MonoBehaviour 
{
	void Start () 
	{		
		var writter = new CsvFileWriter("Assets/Resources/Quest.csv");

		// Making Index Row
		List<string> columns = new List<string>() { "Index", "Text", "Check" };

		writter.WriteRow(columns);
		columns.Clear();

		columns.Add("0");
		columns.Add("NPC와 다시 한 번 대화하라.");
		columns.Add("No");
	}
}
