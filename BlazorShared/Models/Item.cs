﻿using System;
# if (WINDOWS && NET6_0_OR_GREATER)
using FreeSql.DataAnnotations;
#endif

namespace BlazorShared.Models;

public class Item
{

#if (WINDOWS && NET6_0_OR_GREATER)
[Column(IsPrimary = false)]
#endif
    public string? fId { get; set; }

    public string? Id { get; set; }
    public string? Text { get; set; }
    public string? Description { get; set; }
}
