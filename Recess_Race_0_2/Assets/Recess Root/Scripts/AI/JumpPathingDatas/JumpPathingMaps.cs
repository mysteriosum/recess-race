﻿using UnityEngine;
using System.Collections;

public class JumpPathingMaps {

	public static PathingMap jump_x1_ym4 = new PathingMap(true, new bool[,] {
		{true,true,true},
		{true,true,true},
		{false,true,true},
		{true,true,true},
		{true,true,true},
		{true,true,false}
	});

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

	public static PathingMap jump_x7_y4 = new PathingMap(true, new bool[,] {
		{false,false,true,true,true,true,true,true},
		{false,true,true,true,true,true,true,true},
		{false,true,true,true,true,false,false,false},
		{true,true,true,true,false,false,false,false},
		{true,true,true,false,false,false,false,false},
		{true,true,false,false,false,false,false,false}
	});

	public static PathingMap jump_x5_y2 = new PathingMap(true, new bool[,] {
		{false,true,true,true,true,false},
		{false,true,true,true,true,true},
		{true,true,true,true,true,true},
		{true,true,true,false,true,true},
		{true,true,false,false,false,false},
		{true,true,false,false,false,false}
	});
}
