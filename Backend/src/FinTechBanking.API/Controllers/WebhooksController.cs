using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using FinTechBanking.Core.Interfaces;

namespace FinTechBanking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WebhooksController : ControllerBase
{
    private readonly IMessageBroker _messageBroker;

    public WebhooksController(IMessageBroker messageBroker)
    {
        _messageBroker = messageBroker;
    }

    [HttpPost("sicoob")]
    public async Task<IActionResult> ReceiveSicoobWebhook([FromBody] JsonElement payload)
    {
        try
        {
            // Validate webhook signature (TODO: implement signature validation)
            
            // Extract event type and transaction data
            var eventType = payload.GetProperty("event").GetString();
            var transactionData = payload.GetProperty("data");

            // Publish to webhook queue for processing
            await _messageBroker.PublishAsync("webhook-events", new
            {
                EventType = eventType,
                Payload = transactionData.GetRawText(),
                ReceivedAt = DateTime.UtcNow,
                Source = "SICOOB"
            });

            return Ok(new { message = "Webhook received successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error processing webhook", error = ex.Message });
        }
    }
}

