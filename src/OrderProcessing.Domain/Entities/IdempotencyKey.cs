
namespace OrderProcessing.Domain.Entities;

public class IdempotencyKey
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Key { get; set; } = string.Empty;
    public string RequestName { get; set; } = string.Empty;
    public string ResponseData { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

//(
//    Id (PK, Guid or Long)
//    Key(string, Unique, Indexed)  <-- ده المفتاح اللي جاي من الـ Header
//    RequestName(string)           <-- اسم الكوماند(مثلاً "CreateOrderCommand")
//    ResponseData(string/json)     <-- النتيجة القديمة اللي رجعت أول مرة(عشان نرجعها تاني لو الطلب اتكرر)
//    CreatedAt(DateTime)           <-- تاريخ إنشائه(عشان نعمله Cleanup بعد فترة مثلاً 24 ساعة)
//)