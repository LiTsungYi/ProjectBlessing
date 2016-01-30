using UnityEngine;
using System.Collections;

public class ExampleDataView : GoogleDataBase<ExampleData>
{

}

[System.Serializable]
public class ExampleData
{
	public int dataInt;
	public string dataString;
}