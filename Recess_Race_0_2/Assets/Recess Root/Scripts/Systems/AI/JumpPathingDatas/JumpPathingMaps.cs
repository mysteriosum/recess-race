﻿using UnityEngine;
using System.Collections;

public class JumpPathingMaps {

	public static PathingMap jump_x1_ym4_Boumerang = new PathingMap(true, new bool[,] {
		{true,true,true},
		{true,true,true},
		{false,true,true},
		{false,true,true},
		{true,true,true},
		{true,true,false}
	});

	public static PathingMap jump_x1_y3 = new PathingMap(true, new bool[,] {
		{true,true},
		{true,true},
		{true,false},
		{true,false},
		{true,false}
	});
	
	public static PathingMap jump_x1_y4_noRun = new PathingMap(true, new bool[,] {
		{true,true},
		{true,true},
		{true,false},
		{true,false},
		{true,false},
		{true,false}
	});

	public static PathingMap jump_x1_ym1 = new PathingMap(true, new bool[,] {
		{true,true,true},
		{true,true,true},
		{false,true,true}
	});

	public static PathingMap jump_x1_ym2 = new PathingMap(true, new bool[,] {
		{true,true,true},
		{true,true,true},
		{false,true,true},
		{false,true,true}
	});

	public static PathingMap jump_x1_ym3 = new PathingMap(true, new bool[,] {
		{true,true,true},
		{true,true,true},
		{false,true,true},{false,true,true},{false,true,true}
	});

	public static PathingMap jump_x1_ym4 = new PathingMap(true, new bool[,] {
		{true,true,true},
		{true,true,true},
		{false,true,true},{false,true,true},{false,true,true},{false,true,true}
	});

	public static PathingMap jump_x1_ym5 = new PathingMap(true, new bool[,] {
		{true,true,true},
		{true,true,true},
		{false,true,true},{false,true,true},{false,true,true},{false,true,true},{false,true,true}
	});

    public static PathingMap jump_x1_ym6 = new PathingMap(true, new bool[,] {
		{true,true,true},
		{true,true,true},
		{false,true,true},
		{false,true,true},
		{false,true,true},
		{false,true,true},
		{false,true,true},
		{false,true,true}
	});


	public static PathingMap jump_x2_y1 = new PathingMap(true, new bool[,] {
		{true,true,true},
		{true,true,true},
		{true,true,true},
		{true,true,false}
	});

    public static PathingMap jump_x2_y2 = new PathingMap(true, new bool[,] {
        {true,true,true},
        {true,true,true},
        {true,true,true},
        {true,false,false},
        {true,false,false}
    });

    public static PathingMap jump_x2_y3 = new PathingMap(true, new bool[,] {
        {true,true,true},
        {true,true,true},
        {true,true,true},
        {true,true,false},
        {true,false,false},
        {true,false,false}
    });

    public static PathingMap jump_x2_y4 = new PathingMap(true, new bool[,] {
        {true,true,true},
        {true,true,true},
        {true,true,false},
        {true,true,false},
        {true,false,false},
        {true,false,false}
    });

	public static PathingMap jump_x2_ym10 = new PathingMap(true, new bool[,] {
		{true,true,true},
		{true,true,true},
		{false,true,true},{false,true,true},{false,true,true},{false,true,true},{false,true,true},
		{false,true,true},{false,true,true},{false,true,true},{false,true,true},{false,true,true}
	});

	public static PathingMap jump_x2_ym11 = new PathingMap(true, new bool[,] {
		{true,true,true},
		{true,true,true},
		{false,true,true},{false,true,true},{false,true,true},{false,true,true},{false,true,true},
		{false,true,true},{false,true,true},{false,true,true},{false,true,true},{false,true,true},
		{false,true,true}
	});

	public static PathingMap jump_x2_ym12 = new PathingMap(true, new bool[,] {
		{true,true,true},
		{true,true,true},
		{false,true,true},{false,true,true},{false,true,true},{false,true,true},{false,true,true},
		{false,true,true},{false,true,true},{false,true,true},{false,true,true},{false,true,true},
		{false,true,true},{false,true,true}
	});
	
	public static PathingMap jump_x2_ym14 = new PathingMap(true, new bool[,] {
		{true,true,true,true},
		{true,true,true,true},
		{false,true,true,true},
		{false,false,true,true},{false,false,true,true},{false,false,true,true},{false,false,true,true},{false,false,true,true},
		{false,false,true,true},{false,false,true,true},{false,false,true,true},{false,false,true,true},{false,false,true,true},
		{false,false,true,true},{false,false,true,true},{false,false,true,true}
	});
	
	public static PathingMap jump_x2_ym16 = new PathingMap(true, new bool[,] {
		{true,true,true,true},
		{true,true,true,true},
		{false,true,true,true},
		{false,false,true,true},{false,false,true,true},{false,false,true,true},{false,false,true,true},{false,false,true,true},
		{false,false,true,true},{false,false,true,true},{false,false,true,true},{false,false,true,true},{false,false,true,true},
		{false,false,true,true},{false,false,true,true},{false,false,true,true},{false,false,true,true},{false,false,true,true}
	});

	public static PathingMap jump_x2_ym7 = new PathingMap(true, new bool[,] {
		{true,true,true,true},
		{true,true,true,true},
		{false,true,true,true},
		{false,false,true,true},{false,false,true,true},{false,false,true,true},{false,false,true,true},{false,false,true,true},
		{false,false,true,true}
	});

	public static PathingMap jump_x2_ym8 = new PathingMap(true, new bool[,] {
		{true,true,true,true},
		{true,true,true,true},
		{false,true,true,true},
		{false,false,true,true},{false,false,true,true},{false,false,true,true},{false,false,true,true},{false,false,true,true},
		{false,false,true,true},{false,false,true,true}
	});

	public static PathingMap jump_x2_ym4 = new PathingMap(true, new bool[,] {
		{true,true,true,true},
		{true,true,true,true},
		{false,true,true,true},
		{false,false,true,true},{false,false,true,true},{false,false,true,true}
	});

	public static PathingMap jump_x2_ym3 = new PathingMap(true, new bool[,] {
		{true,true,true,true},
		{true,true,true,true},
		{false,true,true,true},
		{false,false,true,true},{false,false,true,true}
	});


	public static PathingMap jump_x8_y4 = new PathingMap(true, new bool[,] {
		{false,false,false,false,true,true,true,true,true},
		{false,false,true,true,true,true,true,true,true},
		{false,true,true,true,true,true,true,true,true},
		{false,true,true,true,true,false,false,false,false},
		{true,true,true,true,false,false,false,false,false},
		{true,true,true,false,false,false,false,false,false},
		{true,true,false,false,false,false,false,false,false}
	});
	

	public static PathingMap jump_x5_y2 = new PathingMap(true, new bool[,] {
		{true,true,true,true,true,false},
		{true,true,true,true,true,true},
		{true,true,true,true,true,true},
		{true,true,false,false,true,true},
		{true,true,false,false,false,false},
		{true,true,false,false,false,false}
	});

	public static PathingMap jump_x5_y3 = new PathingMap(true, new bool[,] {
		{true,true,true,true,true,true},
		{true,true,true,true,true,true},
		{true,true,true,true,true,true},
		{true,true,false,false,false,false},
		{true,true,false,false,false,false},
		{true,true,false,false,false,false}
	});

	public static PathingMap jump_x5_y3_loopedBetter = new PathingMap(true, new bool[,] {
		{false,false,true,true,true,true},
		{false,true,true,true,true,true},
		{false,true,true,true,true,true},
		{true,true,true,true,false,false},
		{true,true,true,false,false,false},
		{true,true,false,false,false,false}
	});

	public static PathingMap jump_x3_ym1 = new PathingMap(true, new bool[,] {
		{true,true,true,true},
		{true,true,true,true},
		{false,true,true,true},
	});

	public static PathingMap jump_x3_ym2 = new PathingMap(true, new bool[,] {
		{true,true,true,true},
		{true,true,true,true},
		{false,true,true,true},
		{false,false,true,true}
	});

	public static PathingMap jump_x4_y1_noRun = new PathingMap(true, new bool[,] {
		{true,true,true,true,false},
		{true,true,true,true,true},
		{true,true,true,true,true},
		{true,true,false,true,true},
		{true,true,false,false,false}
	});

	public static PathingMap jump_x4_y2_norun = new PathingMap(true, new bool[,] {
		{true,true,true,true,false},
		{true,true,true,true,true},
		{true,true,true,true,true},
		{true,true,false,false,false},
		{true,true,false,false,false}
	});

	public static PathingMap jump_x4_y3 = new PathingMap(true, new bool[,] {
		{true,true,true,true,true},
		{true,true,true,true,true},
		{true,true,true,true,true},
		{true,true,false,false,false},
		{true,true,false,false,false},
		{true,true,false,false,false}
	});

	public static PathingMap jump_x4_y4 = new PathingMap(true, new bool[,] {
		{true,true,true,true,true},
		{true,true,true,true,true},
		{true,true,true,true,false},
		{true,true,false,false,false},
		{true,true,false,false,false},
		{true,true,false,false,false}
	});

	public static PathingMap jump_x4_ym3 = new PathingMap(true, new bool[,] {
		{true,true,true,true,false},
		{true,true,true,true,true},
		{false,true,true,true,true},
		{false,false,false,true,true},
		{false,false,false,false,true}
	});

	public static PathingMap jump_x5_ym2 = new PathingMap(true, new bool[,] {
		{true,true,true,true,false,false},
		{true,true,true,true,true,true},
		{false,true,true,true,true,true},
		{false,false,false,true,true,true}
	});




	//--------------------------------------------------------------------------------
	//---------------------------------------7----------------------------------------
	//--------------------------------------------------------------------------------

	public static PathingMap jump_x7_ym1 = new PathingMap(true, new bool[,] {
		{false,true,true,true,true,false,false,false},
		{false,true,true,true,true,true,true,false},
		{true,true,true,true,true,true,true,true},
		{true,true,true,false,true,true,true,true},
		{true,true,true,false,false,true,true,true},
		{true,true,false,false,false,false,true,true},
		{false,false,false,false,false,false,false,true},
		{false,false,false,false,false,false,false,false}
	});
	
	public static PathingMap jump_x7_y0 = new PathingMap(true, new bool[,] {
		{false,false,true,true,true,true,false,false},
		{false,true,true,true,true,true,true,true},
		{true,true,true,true,true,true,true,true},
		{true,true,true,false,false,true,true,true},
		{true,true,true,false,false,false,true,true},
		{true,true,false,false,false,false,false,true}
	});
	
	public static PathingMap jump_x7_y2 = new PathingMap(true, new bool[,] {
		{false,false,true,true,true,true,true,true},
		{false,true,true,true,true,true,true,true},
		{false,true,true,true,true,true,true,true},
		{true,true,true,true,false,false,true,true},
		{true,true,true,false,false,false,false,false},
		{true,true,false,false,false,false,false,false}
	});

	public static PathingMap jump_x7_y3 = new PathingMap(true, new bool[,] {
		{false,false,false,true,true,true,true,false},
		{false,true,true,true,true,true,true,true},
		{false,true,true,true,true,true,true,true},
		{true,true,true,true,false,false,false,false},
		{true,true,true,false,false,false,false,false},
		{true,true,false,false,false,false,false,false}
	});
	
	public static PathingMap jump_x7_y4 = new PathingMap(true, new bool[,] {
		{false,false,true,true,true,true,true,true},
		{false,true,true,true,true,true,true,true},
		{false,true,true,true,true,false,false,false},
		{true,true,true,true,false,false,false,false},
		{true,true,true,false,false,false,false,false},
		{true,true,false,false,false,false,false,false}
	});

	public static PathingMap jump_x7_y0_headbump = new PathingMap(true, new bool[,] {
		{false,true,true,true,true,true,true,false},
		{false,true,true,true,true,true,true,true},
		{true,true,true,true,false,true,true,true},
		{true,true,true,false,false,false,true,true},
		{true,true,false,false,false,false,false,true}
	});


	public static PathingMap jump_x8_y0_noRun = new PathingMap(true, new bool[,] {
		{true,true,true,true,true,true,false,false,false},
		{true,true,true,true,true,true,true,false,false},
		{true,true,true,true,true,true,true,true,false},
		{true,true,false,false,false,true,true,true,true},
		{true,true,false,false,false,false,true,true,true},
		{true,true,false,false,false,false,false,true,true}
	});

	

	//--------------------------------------------------------------------------------
	//---------------------------------------10---------------------------------------
	//--------------------------------------------------------------------------------

	public static PathingMap jump_x10_y3 = new PathingMap(true, new bool[,] {
		{false,false,false,false,true,true,true,true,true,false,false},
		{false,false,true,true,true,true,true,true,true,true,true},
		{false,true,true,true,true,true,true,true,true,true,true},
		{false,true,true,true,true,false,false,false,false,true,true},
		{true,true,true,true,false,false,false,false,false,false,false},
		{true,true,true,false,false,false,false,false,false,false,false},
		{true,true,false,false,false,false,false,false,false,false,false}
	});

	//--------------------------------------------------------------------------------
	//---------------------------------------11---------------------------------------
	//--------------------------------------------------------------------------------

	public static PathingMap jump_x11_y0 = new PathingMap(true, new bool[,] {
		{false,false,false,false,true,true,true,true,true,false,false,false},
		{false,false,true,true,true,true,true,true,true,true,false,false},
		{false,true,true,true,true,true,true,true,true,true,true,false},
		{false,true,true,true,true,false,false,false,true,true,true,false},
		{true,true,true,true,false,false,false,false,false,true,true,true},
		{true,true,true,false,false,false,false,false,false,true,true,true},
		{true,true,false,false,false,false,false,false,false,false,true,true}
	});

	public static PathingMap jump_x12_y0 = new PathingMap(true, new bool[,] {
		{false,false,false,false,true,true,true,true,true,false,false,false,false},
		{false,false,true,true,true,true,true,true,true,true,true,false,false},
		{false,true,true,true,true,true,true,true,true,true,true,true,false},
		{false,true,true,true,true,false,false,false,false,true,true,true,true},
		{true,true,true,true,false,false,false,false,false,false,true,true,true},
		{true,true,true,false,false,false,false,false,false,false,false,true,true},
		{true,true,false,false,false,false,false,false,false,false,false,true,true}
	});

	public static PathingMap jump_x12_y1 = new PathingMap(true, new bool[,] {
		{false,false,false,false,true,true,true,true,true,false,false,false,false},
		{false,false,true,true,true,true,true,true,true,true,true,false,false},
		{false,true,true,true,true,true,true,true,true,true,true,true,false},
		{false,true,true,true,true,false,false,false,false,true,true,true,true},
		{true,true,true,true,false,false,false,false,false,false,true,true,true},
		{true,true,true,false,false,false,false,false,false,false,false,true,true},
		{true,true,false,false,false,false,false,false,false,false,false,false,false}
	});

	public static PathingMap jump_x13_y0 = new PathingMap(true, new bool[,] {
		{false,false,false,false,false,false,false,false,false,false,false,false,false,false},
		{false,false,false,false,true,true,true,true,true,false,false,false,false,false},
		{false,false,true,true,true,true,true,true,true,true,true,false,false,false},
		{false,true,true,true,true,true,true,true,true,true,true,true,false,false},
		{false,true,true,true,true,false,false,false,false,true,true,true,true,false},
		{true,true,true,true,false,false,false,false,false,false,true,true,true,true},
		{true,true,true,false,false,false,false,false,false,false,false,true,true,true},
		{true,true,false,false,false,false,false,false,false,false,false,false,true,true}
	});
}
