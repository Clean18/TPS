using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chikorita : MonoBehaviour
{
	public DataManager Data;

	[SerializeField] private int dexNumber;
	[SerializeField] private string pokeName;
	[SerializeField] private int hp;
	[SerializeField] private int atk;
	[SerializeField] private int def;
	[SerializeField] private int spAtk;
	[SerializeField] private int spDef;
	[SerializeField] private int speed;
	[SerializeField] private string type1;
	[SerializeField] private string type2;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Init(4);
		}
	}

	void Init(int DexNumber)
	{
		//dexNumber = int.Parse(Data.MonsterCSV.GetData(DexNumber, (int)PokeData.DexNumber));
		//pokeName = Data.MonsterCSV.GetData(DexNumber, (int)PokeData.Name);
		//hp = int.Parse(Data.MonsterCSV.GetData(DexNumber, (int)PokeData.Hp));
		//atk = int.Parse(Data.MonsterCSV.GetData(DexNumber, (int)PokeData.Atk));
		//def = int.Parse(Data.MonsterCSV.GetData(DexNumber, (int)PokeData.Def));
		//spAtk = int.Parse(Data.MonsterCSV.GetData(DexNumber, (int)PokeData.SpAtk));
		//spDef = int.Parse(Data.MonsterCSV.GetData(DexNumber, (int)PokeData.SpDef));
		//speed = int.Parse(Data.MonsterCSV.GetData(DexNumber, (int)PokeData.Speed));
		//type1 = Data.MonsterCSV.GetData(DexNumber, (int)PokeData.Type1);
		//type2 = Data.MonsterCSV.GetData(DexNumber, (int)PokeData.Type2);

		dexNumber = DexNumber;
		pokeName = Data.PokeDataDic.GetData(DexNumber.ToString(), "이름") ?? "null";
		hp = int.Parse(Data.PokeDataDic.GetData(DexNumber.ToString(), "체력"));
		atk = int.Parse(Data.PokeDataDic.GetData(DexNumber.ToString(), "공격"));
		def = int.Parse(Data.PokeDataDic.GetData(DexNumber.ToString(), "방어"));
		spAtk = int.Parse(Data.PokeDataDic.GetData(DexNumber.ToString(), "특수공격"));
		spDef = int.Parse(Data.PokeDataDic.GetData(DexNumber.ToString(), "특수방어"));
		speed = int.Parse(Data.PokeDataDic.GetData(DexNumber.ToString(), "스피드"));
		type1 = Data.PokeDataDic.GetData(DexNumber.ToString(), "타입1") ?? "null";
		type2 = Data.PokeDataDic.GetData(DexNumber.ToString(), "타입2") ?? "";
	}
}
