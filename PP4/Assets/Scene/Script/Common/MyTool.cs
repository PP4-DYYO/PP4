////////////////////////////////////////////////////////////////////////////////////////////////////
//
//2017/8/13～
//製作者　京都コンピュータ学院　ゲーム学科　３回生　奥田裕也
//
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 便利な機能を集めるクラス
/// </summary>
public class MyTool
{
	/// <summary>
	/// ランダムに並べた重複しない配列を生成するメソッド
	/// </summary>
	/// <returns>ランダムな数値配列</returns>
	/// <param name="min">最小</param>
	/// <param name="max">最大</param>
	public static int[] DoNotOverlapRandom(int min, int max)
	{
		var array = new int[max - min];

		//配列に連番で代入
		for (var i = min; i < max; i++)
		{
			array[i - min] = i;
		}

		//ランダム配列作成
		int num1;
		int num2;
		int work;
		for (int i = 0; i < array.Length * array.Length; i++)
		{
			num1 = Random.Range(min, max);  //ランダム数値１
			num2 = Random.Range(min, max);  //ランダム数値２

			//交換
			work = array[num1];
			array[num1] = array[num2];
			array[num2] = work;
		}

		return array;
	}
}
