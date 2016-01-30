using UnityEngine;
using System.Collections;
using System.Collections.Generic;

static public class TSExtension
{
	static public T[] GetComponentsForInterface<T>(this GameObject target)
	{
		IList list = target.GetComponents(typeof(T));
		T[] ret = new T[list.Count];
		list.CopyTo(ret, 0);
		return ret;
	}
	
	static public string AddChangeLineSymbol(this string target, int charOfLine)
	{
		char[] chars = target.ToCharArray();
		List<char> resultStr = new List<char>();
		
		int cnt = 0;
		int lastIdx = 0;
		int nowCnt = 0;
		bool isRichtext = false;
		
		for(int i = 0; i < chars.Length; i++)
		{
			switch(chars[i])
			{
			case '\n':
			case ' ':			
				cnt = i - lastIdx + 1;

				if(nowCnt > charOfLine)
				{
					resultStr.Add('\n');
					nowCnt = cnt;
				}
				else
				{
					nowCnt++;
				}
				
				resultStr.AddRange(target.Substring(lastIdx, cnt));
				lastIdx = i+1;
				
				if(chars[i] == '\n')
				{
					nowCnt = 0;
				}
				break;
				
			case '<':
				isRichtext = true;
				break;
				
			case '>':
				isRichtext = false;
				break;
				
			default:
				if(!isRichtext)
				{
					nowCnt++;
				}
				break;
			}
		}
		
		cnt = chars.Length - lastIdx;
		if(nowCnt > charOfLine)
		{
			resultStr.Add('\n');
		}
		resultStr.AddRange(target.Substring(lastIdx, cnt));
		
		return new string(resultStr.ToArray());
	}
	
	static public Transform FindGrandChild(this Transform target, string name)
	{
		var child = target.FindChild(name);
		if(null != child)
		{
			return child;
		}
		
		for(int i = 0; i < target.childCount; i++)
		{
			child = target.GetChild(i).FindGrandChild(name);
			if (child != null)
			{
				return child;
			}
		}
		return null;
	}
}
