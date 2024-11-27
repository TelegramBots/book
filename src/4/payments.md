# Bot Payments API & Telegram Stars

[![Bot Payments API](https://img.shields.io/badge/Bot_Payments_API-Physical_Goods_-blue.svg?style=flat-square)](https://core.telegram.org/bots/payments)
[![Bot Payments Stars](https://img.shields.io/badge/Bot_Payments_API-Digital_Goods-blue.svg?style=flat-square)](https://core.telegram.org/bots/payments)

Telegram offers a safe, simple and unified payment system for goods and services.

Due to Google/Apple policies, there is a distinction between:
- **Digital Goods & Services**, which can be paid using [Telegram Stars](https://telegram.org/blog/telegram-stars) (XTR) only
- **Physical Goods**, which can be paid using regular currencies, and can request more details like a shipping address.

Both process are similar, so we will demonstrate how to do a Telegram Stars payment (simpler) and give you some info about the difference for Physical Goods.

## Important notes for physical goods

Before starting, you need to talk to [@BotFather](https://t.me/BotFather), select one of the supported
[Payment providers](https://core.telegram.org/bots/payments#supported-payment-providers)
(you need to open an account on the provider website), and complete the connection procedure
linking your bot with your provider account.
>It is recommended to start with the [Stripe TEST MODE](https://core.telegram.org/bots/payments#testing-payments-the-39stripe-test-mode-39-provider)
provider so you can test your bot with fake card numbers before going live.

Price amounts are expressed as integers with some digits at the end for the "decimal" part.  
For example, in USD currency there are 2 digits for cents, so 12345 means $123.45 ; With Telegram Stars (XTR), there are no extra digits.  
See ["exp" in this table](https://core.telegram.org/bots/payments/currencies.json), to determine the number of decimal digits for each currency.

## Sending an invoice
[![send invoice method](https://img.shields.io/badge/Bot_API_method-sendInvoice-blue.svg?style=flat-square)](https://core.telegram.org/bots/api#sendinvoice)

When your bot is ready to issue a payment for the user to complete, it will send an invoice:
```csharp
await bot.SendInvoice(
    chatId: chatId,                         // same as userId for private chat
    title: "Product Title",
    description: "Product Detailed Description",
    payload: "InternalProductID",           // not sent nor shown to user
    currency: "XTR",                        // 3-letters ISO 4217 currency
    prices: [("Price", 500)],               // only one price for XTR
    photoUrl: "https://cdn.pixabay.com/photo/2012/10/26/03/16/painting-63186_1280.jpg",
);
```

Alternatively, you can instead generate an URL for that payment with [`CreateInvoiceLink`](https://core.telegram.org/bots/api#createinvoicelink),
or if your bot supports [Inline Mode](../3/inline.md), you can [send invoices as inline results](https://core.telegram.org/bots/api#inputinvoicemessagecontent) ("via YourBot").

With Physical Goods, you can specify [more parameters](https://core.telegram.org/bots/api#sendinvoice) like:
- the `providerToken` obtained by BotFather (something like "1234567:TEST:aBcDeFgHi")
- several price lines detailing the total price
- some suggested tips
- the need for extra information about the user, including a shipping address
- if the price is **flexible** depending on the shipping address/method

## Handling the `ShippingQuery` Update

[![Shipping Query Update](https://img.shields.io/badge/Bot_API_update-ShippingQuery-blue.svg?style=flat-square)](https://core.telegram.org/bots/api#shippingquery)
[![Answer Shipping Query method](https://img.shields.io/badge/Bot_API_method-answerShippingQuery-blue.svg?style=flat-square)](https://core.telegram.org/bots/api#answershippingquery)

This update is received only for Physical Goods, if you specified a **flexible** price.
Otherwise you can skip to the next section.

`update.ShippingQuery` would contain information like the current shipping address for the user, and can be received again if the user changes the address.

You should check if the address is supported, and reply using `bot.AnswerShippingQuery` with an error message or a list of shipping options with associated additional price lines:
```csharp
var shippingOptions = new List<ShippingOption>();
shippingOptions.Add(new() { Title = "DHL Express", Id = "dhl-express",
    Prices = [("Shipping", 1200)] });
shippingOptions.Add(new() { Title = "FedEx Fragile", Id = "fedex-fragile",
    Prices = [("Packaging", 500), ("Shipping", 1800)] });
await bot.AnswerShippingQuery(shippingQuery.Id, shippingOptions);
```

## Handling the `PreCheckoutQuery` Update

[![Pre Checkout Query Update](https://img.shields.io/badge/Bot_API_update-PreCheckoutQuery-blue.svg?style=flat-square)](https://core.telegram.org/bots/api#precheckoutquery)
[![Answer Pre Checkout Query method](https://img.shields.io/badge/Bot_API_method-answerPreCheckoutQuery-blue.svg?style=flat-square)](https://core.telegram.org/bots/api#answerprecheckoutquery)

This update is received when the user has entered their payment information and confirmed the final Pay button.

`update.PreCheckoutQuery` contains all the requested information for the order, so you can validate that all is fine before actual payment

You must reply within 10 seconds with:
```csharp
if (confirm)
    await bot.AnswerPreCheckoutQuery(preCheckoutQuery.Id); // success
else
    await bot.AnswerPreCheckoutQuery(preCheckoutQuery.Id, "Can't process your order: <REASON>");
```

## Handling the `SuccessfulPayment` <u>Message</u>

[![Successful Payment Message](https://img.shields.io/badge/Bot_API_message-SuccessfulPayment-blue.svg?style=flat-square)](https://core.telegram.org/bots/api#successfulpayment)

If you confirmed the order in the step above, Telegram requests the payment with the payment provider.

If the payment is successfully processed, you will receive a private Message of type `SuccessfulPayment` from the user, and you must then proceed with delivering the goods or services to the user.

The `message.SuccessfulPayment` structure contains all the same previous information, plus two payment identifiers from Telegram and from the Payment Provider.

You should store these ChargeId strings for traceability of the transaction in case of dispute, or refund _(possible with [RefundStarPayment](https://core.telegram.org/bots/api#refundstarpayment))_.

## Full example code for Telegram Stars transaction

```csharp
using Telegram.Bot;
using Telegram.Bot.Types;

var bot = new TelegramBotClient("YOUR_BOT_TOKEN");
bot.OnUpdate += OnUpdate;
Console.ReadKey();

async Task OnUpdate(Update update)
{
   switch (update)
   {
      case { Message.Text: "/start" }:
         await bot.SendInvoice(update.Message.Chat,
            "Unlock feature X", "Will give you access to feature X of this bot", "unlock_X",
            "XTR", [("Price", 200)], photoUrl: "https://cdn-icons-png.flaticon.com/512/891/891386.png");
         break;
      case { PreCheckoutQuery: { } preCheckoutQuery }:
         if (preCheckoutQuery is { InvoicePayload: "unlock_X", Currency: "XTR", TotalAmount: 200 })
            await bot.AnswerPreCheckoutQuery(preCheckoutQuery.Id); // success
         else
            await bot.AnswerPreCheckoutQuery(preCheckoutQuery.Id, "Invalid order");
         break;
      case { Message.SuccessfulPayment: { } successfulPayment }:
         System.IO.File.AppendAllText("payments.log", $"{DateTime.Now}: " +
            $"User {update.Message.From} paid for {successfulPayment.InvoicePayload}: " +
            $"{successfulPayment.TelegramPaymentChargeId} {successfulPayment.ProviderPaymentChargeId}\n");
         if (successfulPayment.InvoicePayload is "unlock_X")
            await bot.SendMessage(update.Message.Chat, "Thank you! Feature X is unlocked");
         break;
   };
}
```