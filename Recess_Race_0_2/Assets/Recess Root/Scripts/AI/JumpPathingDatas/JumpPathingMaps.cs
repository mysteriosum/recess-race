using UnityEngine;
using System.Collections;

public class JumpPathingMaps {


	public static PathingMap jump_x2_yMinus16 = new PathingMap(true, new bool[,] { 
    {true,true,true},
{false,true,true},
{false,true,true},
{false,true,true},
{false,true,true},
{false,true,true},
{false,true,true},
{false,true,true},
{false,true,true},
{false,true,true},
{false,true,true},
{false,true,true},
{false,true,true},
{false,true,true},
{false,true,true},
{false,true,true},
{false,true,true}
	});

    public static PathingMap jump_x2_yMinus15 = new PathingMap(true, new bool[,] { 
    {true,true,true},
{false,true,true},
{false,true,true},
{false,true,true},
{false,true,true},
{false,true,true},
{false,true,true},
{false,true,true},
{false,true,true},
{false,true,true},
{false,true,true},
{false,true,true},
{false,true,true},
{false,true,true},
{false,true,true},
{false,true,true}
	});

    public static PathingMap jump_x2_yMinus8 = new PathingMap(true, new bool[,] { 
    {true,true,true},
{false,true,true},
{false,true,true},
{false,true,true},
{false,true,true},
{false,true,true},
{false,true,true},
{false,true,true},
{false,true,true}
	});

    public static PathingMap jump_x2_yMinus7 = new PathingMap(true, new bool[,] { 
    {true,true,true},
{false,true,true},
{false,true,true},
{false,true,true},
{false,true,true},
{false,true,true},
{false,true,true},
{false,true,true}
	});

    public static PathingMap jump_x2_yMinus6 = new PathingMap(true, new bool[,] { 
    {true,true,true},
{false,true,true},
{false,true,true},
{false,true,true},
{false,true,true},
{false,true,true},
{false,true,true}
	});

	public static PathingMap jump_x2_yMinus4 = new PathingMap(true, new bool[,] { 
		{true,true,true},
		{false,true,true},
		{false,true,true},
		{false,true,true},
		{false,true,true}
	});

    public static PathingMap jump_x2_yMinus3 = new PathingMap(true, new bool[,] { 
		{true,true,true},
		{false,true,true},
		{false,true,true},
		{false,true,true}
	});

    public static PathingMap jump_x2_yMinus2 = new PathingMap(true, new bool[,] { 
		{true,true,true},
		{false,true,true},
		{false,true,true}
	});

    public static PathingMap jump_x2_yMinus1 = new PathingMap(true, new bool[,] { 
		{true,true,true},
		{false,true,true}
	});


    public static PathingMap jump_x12_y0 = new PathingMap(true, new bool[,] { 
{false,false,false,true,true,true,true,true,false,false,false,false,false},
{false,true,true,true,true,true,true,true,true,true,false,false,false},
{false,true,true,true,false,false,false,true,true,true,true,false,false},
{true,true,true,false,false,false,false,false,false,true,true,true,false},
{true,true,false,false,false,false,false,false,false,false,true,true,false},
{true,false,false,false,false,false,false,false,false,false,false,true,true}
    });


    public static PathingMap jump_x4_y0 = new PathingMap(true, new bool[,] { 
    {false,false,false,false,false},
{false,false,true,true,true},
{false,true,true,true,true},
{true,true,true,true,true},
{true,true,false,true,true},
{true,false,false,true,true}
    });

    public static PathingMap jump_x7_y0 = new PathingMap(true, new bool[,] { 
    {false,false,false,false,false,false,false,false},
{false,false,true,true,true,true,false,false},
{false,true,true,true,true,true,true,false},
{true,true,true,false,false,true,true,true},
{true,true,false,false,false,false,true,true},
{true,false,false,false,false,false,false,true}
    });


    public static PathingMap jump_x5_y1 = new PathingMap(true, new bool[,] { 
    {false,false,false,false,false,false,false},
{false,false,true,true,true,true,false},
{false,true,true,true,true,true,true},
{true,true,true,false,false,true,true},
{true,true,false,false,false,false,true},
{true,false,false,false,false,false,false}
    });
	
	public static PathingMap jump_x12_y1 = new PathingMap(true, new bool[,] { 
		{false,false,false,true,true,true,true,true,true,false,false,false,false},
		{false,false,true,true,true,true,true,true,true,true,true,false,false},
		{false,true,true,true,false,false,false,false,true,true,true,false,false},
		{true,true,true,false,false,false,false,false,false,true,true,true,false},
		{true,true,false,false,false,false,false,false,false,false,true,true,true},
		{true,true,false,false,false,false,false,false,false,false,false,false,false}
	});

    public static PathingMap jump_x5_y2 = new PathingMap(true, new bool[,] { 
{false,false,true,true,true,true},
{false,true,true,true,true,true},
{true,true,true,false,true,true},
{true,true,false,false,false,false},
{true,true,false,false,false,false}
    });

    public static PathingMap jump_x6_y2 = new PathingMap(true, new bool[,] { 
   {false,false,true,true,true,true,false},
{false,true,true,true,true,true,true},
{true,true,true,false,false,true,true},
{true,true,false,false,false,false,false},
{true,true,false,false,false,false,false}
    });

	public static PathingMap jump_x9_y2 = new PathingMap(true, new bool[,] { 
		{false,false,false,true,true,true,true,true,false,false},
		{false,false,true,true,true,true,true,true,true,false},
		{false,true,true,true,false,false,false,true,true,true},
		{true,true,true,false,false,false,false,false,true,true},
		{true,true,false,false,false,false,false,false,false,false},
		{true,true,false,false,false,false,false,false,false,false}
	});

	public static PathingMap jump_x10_y2 = new PathingMap(true, new bool[,] { 
		{false,false,false,true,true,true,true,true,false,false,false},
		{false,false,true,true,true,true,true,true,true,false,false},
		{false,true,true,true,false,false,false,true,true,true,false},
		{true,true,true,false,false,false,false,false,true,true,true},
		{true,true,false,false,false,false,false,false,false,false,false},
		{true,true,false,false,false,false,false,false,false,false,false}
	});

	public static PathingMap jump_x11_y2 = new PathingMap(true, new bool[,] { 
		{false,false,false,true,true,true,true,true,true,false,false,false},
		{false,false,true,true,true,true,true,true,true,true,false,false},
		{false,true,true,true,false,false,false,false,true,true,true,false},
		{true,true,true,false,false,false,false,false,false,true,true,true},
		{true,true,false,false,false,false,false,false,false,false,false,false},
		{true,true,false,false,false,false,false,false,false,false,false,false}
	});


	public static PathingMap jump_x6_y3 = new PathingMap(true, new bool[,] { 
		{false,false,false,false,false,false,false},
		{false,false,true,true,true,true,true},
		{false,true,true,true,true,true,true},
		{true,true,true,false,false,false,false},
		{true,true,false,false,false,false,false},
		{true,true,false,false,false,false,false}
	});

	
	public static PathingMap jump_x8_y4 = new PathingMap(true, new bool[,] { 
		{false,false,false,true,true,true,true,true,false},
		{false,false,true,true,true,true,true,true,true},
		{false,true,true,true,false,false,false,false,false},
		{true,true,true,false,false,false,false,false,false},
		{true,true,false,false,false,false,false,false,false},
		{true,true,false,false,false,false,false,false,false}
	});

	public static PathingMap jump_x9_y4 = new PathingMap(true, new bool[,] { 
		{false,false,false,true,true,true,true,true,true,false},
		{false,false,true,true,true,true,true,true,true,true},
		{false,true,true,true,false,false,false,false,false,false},
		{true,true,true,false,false,false,false,false,false,false},
		{true,true,false,false,false,false,false,false,false,false},
		{true,true,false,false,false,false,false,false,false,false}
	});
}
