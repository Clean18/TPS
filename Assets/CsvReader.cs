using System.Collections.Generic;          
using System.IO;                           
using UnityEngine;
using CustomUtility;
using CustomUtility.IO;

namespace CustomUtilityy
{
    namespace IO                          
    {
        // CsvReader 클래스는 CSV 파일을 읽고, 다양한 형태로 데이터를 저장할 수 있는 기능을 제공하는 정적 클래스
        public static class CsvReader
        {
            // 현재 실행 환경에 따라 CSV 파일을 불러올 기본 경로를 반환하는 프로퍼티
            public static string BasePath
            {
                get
                {
#if UNITY_EDITOR                                       // 유니티 에디터 환경인 경우
                    return Application.dataPath + Path.DirectorySeparatorChar;  // Assets 폴더 경로 반환
#else                                                  // 빌드된 애플리케이션 환경인 경우
                    return Application.persistentDataPath + Path.DirectorySeparatorChar; // 해당 플랫폼 저장 경로 반환
#endif
                }
            }

            // 전달받은 Csv 객체를 기준으로 CSV 파일을 읽고 데이터 구조에 맞게 로드하는 메서드
            public static void Read(Csv csv)
            {
                // 파일 경로 유효성 검사 및 내용이 비어있는지 검사, 실패하면 리턴
                if (!IsValidPath(csv) || 
                    !IsValidEmpty(csv, out string[] lines))
                    return;

                bool isReadSuccessful; // 로드 성공 여부 저장용 변수

                // Csv 객체의 실제 타입에 따라 다르게 처리
                switch (csv)
                {
                    case CsvTable table: // CsvTable 타입인 경우
                        isReadSuccessful = ReadToTable(table, lines); // 2D 배열 형태로 로드
                        break;
                    case CsvDictionary dictionary: // CsvDictionary 타입인 경우
                        isReadSuccessful = ReadToDictionary(dictionary, lines); // Dictionary 형태로 로드
                        break;
                    default: // 그 외의 타입은 실패 처리
                        isReadSuccessful = false;
                        break;
                }

                // 결과를 로그로 출력
                PrintResult(csv, isReadSuccessful);
            }

            // CSV 내용을 CsvDictionary 형태로 읽어들이는 메서드
            private static bool ReadToDictionary(CsvDictionary csv, string[] lines)
            {
                string[] fieldsIndex = lines[0].Split(csv.SplitSymbol);  // 첫 줄에서 열 이름 추출
                int columns = fieldsIndex.Length;                        // 열 개수 저장
                csv.Dict = new Dictionary<string, Dictionary<string, string>>(); // 새 Dictionary 초기화

                for (int r = 1; r < lines.Length; r++) // 두 번째 줄부터 데이터 줄 순회
                {
                    string[] fields = lines[r].Split(csv.SplitSymbol); // 현재 줄에서 값 분리

                    if (fields.Length < columns) // 열 개수가 모자라면 오류 처리
                    {
                        return false;
                    }

                    string rowKey = fields[0]; // 첫 번째 값은 행의 키로 사용
                    csv.Dict[rowKey] = new Dictionary<string, string>(columns); // 내부 딕셔너리 생성

                    for (int c = 1; c < columns; c++) // 나머지 열은 키-값 형태로 저장
                    {
                        csv.Dict[rowKey][fieldsIndex[c]] = fields[c];
                    }
                }

                return true; // 성공적으로 로드되었으면 true 반환
            }

            // CSV 내용을 2차원 배열 형태로 읽어들이는 메서드
            private static bool ReadToTable(CsvTable csv, string[] lines)
            {
                string[] firstLineFields = lines[0].Split(csv.SplitSymbol); // 첫 줄에서 열 개수 추출
                int rows = lines.Length;                                    // 전체 줄 수 저장
                int columns = firstLineFields.Length;                       // 열 개수 저장

                csv.Table = new string[rows, columns]; // 2차원 배열 초기화

                for (int r = 0; r < rows; r++) // 각 줄마다 반복
                {
                    string[] fields = lines[r].Split(csv.SplitSymbol); // 현재 줄에서 값 추출
                    if (fields.Length < columns) // 열 수가 부족하면 실패 처리
                    {
                        return false;
                    }

                    for (int c = 0; c < columns; c++) // 각 열에 값 저장
                    {
                        csv.Table[r, c] = fields[c];
                    }
                }

                return true; // 성공적으로 읽었으면 true 반환
            }

            // 지정된 경로에 파일이 존재하는지 확인하는 메서드
            private static bool IsValidPath(Csv csv)
            {
                if (!File.Exists(csv.FilePath)) // 파일이 존재하지 않으면
                {
#if UNITY_EDITOR
                    Debug.LogError($"Error: CSV file not found at path: {csv.FilePath}"); // 에디터에서 에러 출력
#endif
                    return false; // 유효하지 않음
                }
                return true; // 파일이 존재하면 true 반환
            }

            // 파일이 비어있는지 검사하고, 내용을 읽어오는 메서드
            private static bool IsValidEmpty(Csv csv, out string[] lines)
            {
                lines = File.ReadAllLines(csv.FilePath); // 모든 줄을 읽어 배열로 저장

                if (lines.Length == 0) // 한 줄도 없다면 비어있는 파일
                {
#if UNITY_EDITOR
                    Debug.LogError($"Error: CSV file at path {csv.FilePath} is empty."); // 에디터에서 에러 출력
#endif
                    return false;
                }
                return true; // 줄이 있으면 true 반환
            }

            // 로딩 결과에 따라 성공 또는 실패 메시지를 출력하는 메서드
            private static void PrintResult(Csv csv, bool result)
            {
#if UNITY_EDITOR
                if (result) // 성공했으면
                {
                    Debug.Log($"Successfully loaded CSV file from path: {csv.FilePath}"); // 성공 메시지 출력
                }
                else // 실패했으면
                {
                    Debug.LogError($"Error: CSV file at path {csv.FilePath} has inconsistent column lengths."); // 열 개수 불일치 메시지 출력
                }
#endif
            }
        }
    }
}
