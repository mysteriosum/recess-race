using UnityEngine;
using System.Collections;

public class ItemInfo {

	public string name;
	public int index;
	
	public delegate void FunctionDel();
	
	public FunctionDel function;
	
	public ItemInfo (string name, int index, FunctionDel function)
	{
		this.name = name;
		this.index = index;
		this.function = function;
	}

}
