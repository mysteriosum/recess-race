using UnityEngine;
using System.Collections;

public class JumpPathingMaps {
	
    public static bool[,] fullJump = new bool[,] { 
  {true,true,true,false,false,false,false,true,true,true,true,false,false},
{false,false,false,true,true,true,false,false,false,false,false,true,true},
{false,false,false,false,false,true,true,true,false,false,false,false,false},
{true,true,false,false,false,false,false,true,true,false,false,false,false},
{false,true,true,false,false,false,false,false,true,true,false,false,false},
{false,true,true,true,false,false,false,true,true,true,false,false,false},
{true,true,true,false,false,false,true,true,true,false,false,false,false}
    };
}
