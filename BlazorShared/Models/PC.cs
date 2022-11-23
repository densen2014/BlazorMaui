// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************
# if (WINDOWS && NET6_0_OR_GREATER)
using FreeSql.DataAnnotations;
#endif
using BootstrapBlazor.Components;
using Newtonsoft.Json;
using System.ComponentModel;

namespace AmeApi;

[AutoGenerateClass(Searchable = true, Filterable = true, Sortable = true)]
public class PC
{
#if (WINDOWS && NET6_0_OR_GREATER)
   [Column(IsIdentity = true)]
#endif
    [DisplayName("序号")]
    [AutoGenerateColumn(Ignore = true)]
    public int ID { get; set; }

    [DisplayName("状态")]
    public string? Status { get; set; }

    [DisplayName("名称")]
    [AutoGenerateColumn(TextWrap = true)]
    public string? Name { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    [JsonProperty]
    [DisplayName("更新时间")]
    [AutoGenerateColumn(FormatString = "MM-dd HH:mm:ss")]
    public DateTime StatusDate { get; set; } = DateTime.Now;


    /// <summary>
    /// 远端配置文件最后更新时间
    /// </summary>
    [JsonProperty]
    [DisplayName("配置文件")]
    [AutoGenerateColumn(FormatString = "MM-dd HH:mm:ss")]
    public DateTime? ProfileDate { get; set; }

    [AutoGenerateColumn(Ignore = true)]
    public string? GUID { get; set; }

    [DisplayName("描述")]
    [AutoGenerateColumn(TextWrap = true)]
    public string? Description { get; set; }

#if (WINDOWS && NET6_0_OR_GREATER)
   [Navigate(nameof(Record.PcID))]
#endif
    [AutoGenerateColumn(Ignore = true)]
    public virtual List<Record>? records { get; set; }
}

[AutoGenerateClass(Searchable = true, Filterable = true, Sortable = true)]
public class Record
{
#if (WINDOWS && NET6_0_OR_GREATER)
   [Column(IsIdentity = true)]
#endif
    [DisplayName("序号")]
    public int ID { get; set; }

    [DisplayName("机器序号")]
    [AutoGenerateColumn(Ignore = true)]
    public int PcID { get; set; }

    [DisplayName("名称")]
    public string? Name { get; set; }

    /// <summary>
    /// 日期
    /// </summary>
    [JsonProperty]
    [DisplayName("日期")]
    [AutoGenerateColumn(FormatString = "yyyy-MM-dd HH:mm:ss")]
    public DateTime OrderDate { get; set; } = DateTime.Now;

    [DisplayName("状态")]
    public string? Status { get; set; }

    [DisplayName("描述")]
    public string? Description { get; set; }

#if (WINDOWS && NET6_0_OR_GREATER)
[Navigate(nameof(PcID))]
#endif
    [AutoGenerateColumn(Ignore = true)]
    public virtual PC? PCs { get; set; }
}

public class PcStatus
{
    [DisplayName("机器")]
    public int Count { get; set; }

    [DisplayName("工作中")]
    public int Working { get; set; }

    [DisplayName("闲置")]
    public int UnWorking { get; set; }

    [DisplayName("在线")]
    public int Online { get; set; }

    [DisplayName("离线")]
    public int Offline { get; set; }

    [AutoGenerateColumn(Ignore = true)]
    public List<PC>? PCs { get; set; }

}
