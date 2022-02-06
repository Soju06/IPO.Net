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