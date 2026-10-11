using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace OrderProcessing.WebApi.Infrastructure;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // 1. تسجيل الخطأ في الـ Logs لتسهيل تتبعه (Tracing)
        _logger.LogError(
            exception,
            "An unhandled exception occurred: {Message}",
            exception.Message);

        // 2. إنشاء الـ ProblemDetails بنفس الهيكل الذي نستخدمه في ResultExtensions
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Server Error",
            Detail = "An unexpected error occurred while processing your request.",
            Instance = httpContext.Request.Path
        };

        // يمكننا إضافة بعض التفاصيل الإضافية في بيئة التطوير فقط (Development)
        // لتسهيل الـ Debugging، ولكن يجب إخفاؤها في بيئة الإنتاج (Production) لأسباب أمنية.
        var env = httpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();
        if (env.IsDevelopment())
        {
            problemDetails.Extensions["exceptionMessage"] = exception.Message;
            problemDetails.Extensions["stackTrace"] = exception.StackTrace;
        }

        // 3. ضبط الـ Response ليطابق الـ ProblemDetails
        httpContext.Response.StatusCode = problemDetails.Status.Value;
        httpContext.Response.ContentType = "application/problem+json";

        // 4. إرسال الرد
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        // إرجاع true يخبر الـ Pipeline أننا تعاملنا مع الخطأ ولن يمرر لباقي الـ Middlewares
        return true;
    }
}