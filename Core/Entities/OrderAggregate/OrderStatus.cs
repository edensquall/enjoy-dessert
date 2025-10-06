using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Core.Entities.OrderAggregate
{
    public enum OrderStatus
    {
        [Description("待處理")]
        Pending,
        [Description("已付款")]
        PaymentRecevied,
        [Description("付款失敗")]
        PaymentFailed,
        [Description("待取貨")]
        PendingPickup,
        [Description("已完成")]
        Complete
    }
}