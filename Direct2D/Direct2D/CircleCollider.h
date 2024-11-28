#pragma once
#include "stdafx.h"
#include "Direct2D.h"
#include <iostream>
#include <conio.h>
#include <cmath>

//D2D1_POINT_2F operator-() // Point ������ �������̵� ��
//{
//
//}

// �浹�� ������.
D2D1_POINT_2F ReturnCircleToCircleReverseDir(D2D1_ELLIPSE PC, D2D1_ELLIPSE OC)
{
	D2D1_POINT_2F nDir;
	D2D1_POINT_2F PC_Point = D2D1::Point2F(PC.point.x, PC.point.y);
	D2D1_POINT_2F OC_Point = D2D1::Point2F(OC.point.x, OC.point.y);
	//D2D1_POINT_2F pDir = D2D1::Point2F(OC_Point.x - PC_Point.x, OC_Point.y - PC_Point.y);
	float nx = PC_Point.x - OC_Point.x;
	float ny = PC_Point.y - OC_Point.y;
	float nnx = nx / sqrt((pow(nx, 2) + pow(ny, 2)));
	float nny = ny / sqrt((pow(nx, 2) + pow(ny, 2)));
	D2D1_POINT_2F pDir = D2D1::Point2F(nnx, nny);
	nDir = pDir;

	_cprintf("Dir: (%f, %f)\n", pDir.x, pDir.y);
	return nDir;
}

bool CheckCollider1(D2D1_ELLIPSE PC, D2D1_ELLIPSE OC)
{
	// �� ���� �߽���ǥ
	float PC_x = PC.point.x;
	float PC_y = PC.point.y;
	float OC_x = OC.point.x;
	float OC_y = OC.point.y;
	// �� ���� ������
	float PC_Radius = PC.radiusX;
	float OC_Radius = OC.radiusX;
	// �� �� ������ �Ÿ�(�߽���ǥ����)
	float nDistance = sqrt(pow((PC_x - OC_x), 2) + pow((PC_y - OC_y), 2));
	// �� ���� �������� ��
	float SumRadius = PC_Radius + OC_Radius;

	//_cprintf("C1: (%f, %f), C2: (%f, %f), CircleDistance: %f\n", PC_x, PC_y, OC_x, OC_y, nDistance);

	if (nDistance + 0.1f > SumRadius) { return false; } // �浹X
	else { return true; } // �浹O
}

//int CheckCollider2(D2D1_ELLIPSE PC, D2D1_ELLIPSE OC)
//{
//	// �� ���� �߽���ǥ
//	float PC_x = PC.point.x;
//	float PC_y = PC.point.y;
//	float OC_x = OC.point.x;
//	float OC_y = OC.point.y;
//	// �� ���� ������
//	float PC_Radius = PC.radiusX;
//	float OC_Radius = OC.radiusX;
//	// �� �� ������ �Ÿ�(�߽���ǥ����)
//	float nDistance = sqrt(pow((PC_x - OC_x), 2) + pow((PC_y - OC_y), 2));
//	// �� ���� �������� ��
//	float SumRadius = PC_Radius + OC_Radius;
//
//	_cprintf("C1: (%f, %f), C2: (%f, %f), CircleDistance: %f\n", PC_x, PC_y, OC_x, OC_y, nDistance);
//
//	if (nDistance + 1 > SumRadius) { return true; } // �浹X
//	else { return false; } // �浹O
//	return true;
//}