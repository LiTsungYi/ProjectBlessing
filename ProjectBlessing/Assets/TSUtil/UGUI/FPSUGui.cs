using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FPSUGui : MonoBehaviour 
{	
	public  float updateInterval = 0.5F;
	public bool isRunAtRelease = false;
	
	private float accum   = 0; // FPS accumulated over the interval
	private int   frames  = 0; // Frames drawn over the interval
	private float timeleft; // Left time for current interval
	
	private Text text;
	
	// Use this for initialization
	void Start () 
	{
		text = GetComponent<Text>();
		if( !text )
		{
			Debug.Log("UtilityFramesPerSecond needs a GUIText component!");
			enabled = false;
			return;
		}
		timeleft = updateInterval;
		
		text.gameObject.SetActive(false);
		if(isRunAtRelease || UnityEngine.Debug.isDebugBuild)
		{
			text.gameObject.SetActive(true);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{	   
		timeleft -= Time.deltaTime;
		accum += Time.timeScale/Time.deltaTime;
		++frames;
		
		// Interval ended - update GUI text and start new interval
		if( timeleft <= 0.0 )
		{
			// display two fractional digits (f2 format)
			float fps = accum/frames;
			string format = System.String.Format("FPS {0:F2} ",fps);
			text.text = format;
			
			if(fps < 30)
			{
				text.color = Color.yellow;
			}
			else
			{
				if(fps < 10)
				{
					text.color = Color.red;
				}
				else
				{
					text.color = Color.green;
				}
			}
			//	DebugConsole.Log(format,level);
			timeleft = updateInterval;
			accum = 0.0F;
			frames = 0;
		}
	}
}
