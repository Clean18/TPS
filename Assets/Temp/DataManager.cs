using CustomUtility.IO;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class DataManager : MonoBehaviour
{
	public CsvTable PokeDataCSV;
	public CsvDictionary PokeDataDic;


	void Awake()
	{
		Init();
	}

	void Init()
	{
		CsvReader.Read(PokeDataCSV);
		CsvReader.Read(PokeDataDic);
	}
}

public enum PokeData
{
	DexNumber = 0,
	Name,
	Hp,
	Atk,
	Def,
	SpAtk,
	SpDef,
	Speed,
	Type1,
	Type2
}
