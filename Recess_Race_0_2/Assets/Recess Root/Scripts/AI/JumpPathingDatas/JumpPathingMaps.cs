using UnityEngine;
using System.Collections;

public class JumpPathingMaps {

	public static PathingMap jump_x6_y3 = new PathingMap(true, new bool[,] { 
		{false,false,false,false,false,false,false},
		{false,false,true,true,true,true,true},
		{false,true,true,true,true,true,true},
		{true,true,true,false,false,false,false},
		{true,true,false,false,false,false,false},
		{true,true,false,false,false,false,false}
	});
	
}
