using UnityEngine;
using System.Collections;

public class JumpPathingMaps {


    public static PathingMap jump_x12_y0 = new PathingMap(new bool[,] { 
{false,false,false,true,true,true,true,true,false,false,false,false,false},
{false,true,true,true,true,true,true,true,true,true,false,false,false},
{false,true,true,true,false,false,false,true,true,true,true,false,false},
{true,true,true,false,false,false,false,false,false,true,true,true,false},
{true,true,false,false,false,false,false,false,false,false,true,true,false},
{true,false,false,false,false,false,false,false,false,false,false,true,true}
    });


    public static PathingMap jump_x4_y0 = new PathingMap(new bool[,] { 
    {false,false,false,false,false},
{false,false,true,true,true},
{false,true,true,true,true},
{true,true,true,true,true},
{true,true,false,true,true},
{true,false,false,true,true}
    });

    public static PathingMap jump_x7_y0 = new PathingMap(new bool[,] { 
    {false,false,false,false,false,false,false,false},
{false,false,true,true,true,true,false,false},
{false,true,true,true,true,true,true,false},
{true,true,true,false,false,true,true,true},
{true,true,false,false,false,false,true,true},
{true,false,false,false,false,false,false,true}
    });

    
    public static PathingMap jump_x5_y1 = new PathingMap(new bool[,] { 
    {false,false,false,false,false,false,false},
{false,false,true,true,true,true,false},
{false,true,true,true,true,true,true},
{true,true,true,false,false,true,true},
{true,true,false,false,false,false,true},
{true,false,false,false,false,false,false}
    });
}
