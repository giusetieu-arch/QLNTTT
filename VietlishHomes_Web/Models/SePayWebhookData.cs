// ===== [MỚI - SEPAY WEBHOOK MODEL] =====
// File này chứa model ánh xạ dữ liệu JSON mà SePay gửi về khi phát hiện giao dịch
// Tham khảo: https://my.sepay.vn/userManual/transaction/webhook
using System.Text.Json.Serialization;

namespace VietlishHomes_Web.Models;

/// <summary>
/// Model nhận dữ liệu webhook từ SePay khi có giao dịch ngân hàng phát sinh
/// SePay gửi POST request với Content-Type: application/json
/// </summary>
public class SePayWebhookData
{
    /// <summary>ID giao dịch nội bộ của SePay</summary>
    [JsonPropertyName("id")]
    public long Id { get; set; }

    /// <summary>Tên ngân hàng / cổng thanh toán (ACB, MB, VCB...)</summary>
    [JsonPropertyName("gateway")]
    public string? Gateway { get; set; }

    /// <summary>Thời gian giao dịch theo ngân hàng (yyyy-MM-dd HH:mm:ss)</summary>
    [JsonPropertyName("transactionDate")]
    public string? TransactionDate { get; set; }

    /// <summary>Số tài khoản ngân hàng nhận tiền</summary>
    [JsonPropertyName("accountNumber")]
    public string? AccountNumber { get; set; }

    /// <summary>Tài khoản phụ (nếu có)</summary>
    [JsonPropertyName("subAccount")]
    public string? SubAccount { get; set; }

    /// <summary>Nội dung chuyển khoản – dùng để đối chiếu mã hóa đơn</summary>
    [JsonPropertyName("content")]
    public string? Content { get; set; }

    /// <summary>Loại giao dịch: "in" = tiền vào, "out" = tiền ra</summary>
    [JsonPropertyName("transferType")]
    public string? TransferType { get; set; }

    /// <summary>Số tiền giao dịch (VNĐ)</summary>
    [JsonPropertyName("transferAmount")]
    public decimal TransferAmount { get; set; }

    /// <summary>Số dư lũy kế</summary>
    [JsonPropertyName("accumulated")]
    public decimal? Accumulated { get; set; }

    /// <summary>Mã tham chiếu giao dịch từ ngân hàng</summary>
    [JsonPropertyName("referenceCode")]
    public string? ReferenceCode { get; set; }

    /// <summary>Mô tả thêm</summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }
}
