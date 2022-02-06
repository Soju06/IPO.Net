# IPO.Net

국내 공모청약 라이브러리 | 38커뮤니티 크롤링

- 크롤링 특성상 사이트가 개편되면 작동하지 않을 수 있습니다.
- 자료의 오타등의 문제로 값이 정확하지 않으므로 참고용도로만 사용하십시오.

주의: 대부분 함수에는 설명이 없습니다. IPO.Net.Test 프로젝트의 예제 참고하십시오.



## 사용 가능한 데이터

IPO.Net는 다음의 데이터를 가져올 수 있습니다.

날자 기한, 가격 범위 형식을 지원합니다. 각각 ``DateTimeRange``  ``LongRange``

__작업 효율성을 위해 자료 이름은 한글로 표기되어있습니다. __

- 청구종목
- 승인종목
- 수요예측일정
- 수요예측결과
- 공모청약일정
- 신규상장
- 기업분석



## 예제

- 청약 예정 또는 진행중인 종목 가져오기

  ```cs	
  using IPO.Net; 
  using Npgg;
  using System.Text;
  
  // EUC-KR 인코딩을 사용하기 위함.
  Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
  
  using var ipo = new Ipo();
  
  // 공모청약일정 진행중이거나 예정인 항목을 가져옴.
  var table = await ipo.GetIPOSubscriptionSchedulesAsync(DateTimeRange.BackToEnd(DateTime.Now), false);
  
  ConsoleTable.TableColor = ConsoleColor.DarkGray;
  ConsoleTable.RowColor = ConsoleColor.White;
  ConsoleTable.ColumnColor = ConsoleColor.Yellow;
  ConsoleTable.Write(table.Select(row =>
      row == null ? default :
      new { 
          row.종목명, 
          공모주일정 = row.공모주일정.ToString("yyyy.MM.yy ~| MM.dd"),
          확정공모가 = row.확정공모가.ToString(),
          희망공모가 = row.희망공모가.ToString("{0} ~ {1}"),
          청약경쟁률 = row.청약경쟁률 != null ? $"{row.청약경쟁률}:1" : "",
          주간사 = string.Join(" ", row.주간사) 
      } 
  ).Append(new { 종목명 = "", 공모주일정 = "", 확정공모가 = "----------", 희망공모가 = "", 청약경쟁률 = "------------", 주간사 = "" }));
  ```

  ```
   +------------------+--------------------+------------+---------------+--------------+-------------------------------------+
   |           종목명 |         공모주일정 | 확정공모가 |    희망공모가 |   청약경쟁률 |                              주간사 |
   +------------------+--------------------+------------+---------------+--------------+-------------------------------------+
   |     유일로보틱스 | 2022.03.22 ~ 03.08 |            |   7600 ~ 9200 |              |                        한국투자증권 |
   |         보로노이 | 2022.03.22 ~ 03.08 |            | 50000 ~ 65000 |              |           한국투자증권 미래에셋증권 |
   |       대명에너지 | 2022.03.22 ~ 03.04 |            | 25000 ~ 29000 |              |               한국투자증권 삼성증권 |
   |         지투파워 | 2022.03.22 ~ 03.03 |            | 13500 ~ 16400 |              |                 한국투자증권 KB증권 |
   |       모아데이타 | 2022.02.22 ~ 02.28 |            | 24000 ~ 28000 |              |                        하나금융투자 |
   |    SK증권스팩7호 | 2022.02.22 ~ 02.23 |            |   2000 ~ 2000 |              |                              SK증권 |
   |             노을 | 2022.02.22 ~ 02.22 |            | 13000 ~ 17000 |              | 한국투자증권㈜한국투자증권 삼성증권 |
   |         비씨엔씨 | 2022.02.22 ~ 02.22 |            |  9000 ~ 11500 |              |                          NH투자증권 |
   |         풍원정밀 | 2022.02.22 ~ 02.18 |            | 13200 ~ 15200 |              |                            대신증권 |
   |           브이씨 | 2022.02.22 ~ 02.16 |            | 15000 ~ 19500 |              |                        한국투자증권 |
   | 스톤브릿지벤처스 | 2022.02.22 ~ 02.16 |            |  9000 ~ 10500 |              |                     KB증권 삼성증권 |
   | 하나금융스팩21호 | 2022.02.22 ~ 02.16 |            |   2000 ~ 2000 |              |                        하나금융투자 |
   |         퓨런티어 | 2022.02.22 ~ 02.15 |            | 11400 ~ 13700 |              |             유안타증권 신한금융투자 |
   |     한국스팩10호 | 2022.02.22 ~ 02.11 |       2000 |   2000 ~ 2000 |              |                        한국투자증권 |
   | 바이오에프디엔씨 | 2022.02.22 ~ 02.10 |      28000 | 23000 ~ 29000 |              |                          DB금융투자 |
   |     IBKS스팩17호 | 2022.02.22 ~ 02.09 |            |   2000 ~ 2000 |              |                         IBK투자증권 |
   |   인카금융서비스 | 2022.02.22 ~ 02.08 |      18000 | 23000 ~ 27000 |              |                        한국투자증권 |
   +------------------+--------------------+------------+---------------+--------------+-------------------------------------+
  ```

  콘솔 테이블 출력은 [Npgg.ConsoleTable]([Newp/Npgg.ConsoleTable: IEnumerable 인 리스트를 콘솔창에서 테이블형태로 깔끔하게 그려줍니다. (github.com)](https://github.com/Newp/Npgg.ConsoleTable))을 사용함.



## 테스트

공모청약 일정을 가져와 ``DataGridView``에 출력합니다.

![image](https://user-images.githubusercontent.com/34199905/152684081-567a7658-f1ab-4353-ad20-354f5d6472a2.png)



기업 분석 데이터를 가저옵니다.

해당 자료는 단순 테이블이 아니므로 ``DataGridView`` 특성상 전부 표시되지는 않습니다,

![image](https://user-images.githubusercontent.com/34199905/152684269-c5c1ac13-955f-4c89-bce0-c80af7e8640f.png)



## 문제 해결

- System.ArgumentException: ''euc-kr' is not a supported encoding name. 의 오류가 발생합니다.

  38커뮤니티는 기본 인코딩은 EUC-KR입니다.

  .Net Framework, .Net Core에서 추가 인코딩을 사용하려면 라이브러리를 호출하기 전에 다음의 코드가 필요합니다.

  

  ```cs
  Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
  ```

  



- IPO.Net에서 사용되는 ``DataTable``, ``DataColumn``, ``DataRow``는 ``System.Data``의 자료형이 아닙니다.



©️ Soju06 - MIT Licence