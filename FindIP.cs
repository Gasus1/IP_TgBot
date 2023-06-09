using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Exceptions;

namespace IP_TgBot
{
    internal class FindIP
    {
        TelegramBotClient botClient = new TelegramBotClient("6186697071:AAG2YhSvanhsPwJS-si4CkY3sdv4dv8LRFI");
        CancellationToken cancellationToken = new CancellationToken();
        ReceiverOptions receiverOptions = new ReceiverOptions { AllowedUpdates = { } };  
        public async Task Start()
        {
            botClient.StartReceiving(HandlerUpdateAsync, HandlerError, receiverOptions, cancellationToken);
            var botMe = await botClient.GetMeAsync();
            Console.WriteLine($"Bot {botMe.Username} started");
            Console.ReadKey();

        }
        
        private Task HandlerError(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationTokken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Error in tg bot API:\n {apiRequestException.ErrorCode} " +
                    $"\n{apiRequestException.Message}", _ => exception.ToString()
            };
            Console.WriteLine(errorMessage);
            return Task.CompletedTask;
        }

        private async Task HandlerUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if(update.Type == UpdateType.Message && update?.Message?.Text != null)
            {
                await HandlerMessageAsync(botClient, update.Message);
            }
        }

        private async Task HandlerMessageAsync(ITelegramBotClient botClient, Message message)
        {
            if(message.Text == "/start")
            {
                IPDataClient iPDataClient = new IPDataClient();
                await botClient.SendTextMessageAsync(message.Chat.Id, "Enter /help");
                iPDataClient.PostUserDataAsync();
                return;
            }
            else
            if(message.Text == "/help")
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "/keyboard - calls keyboard \n"
                    + "MyIP - to see your IP adress \n"
                    + "MyCountry - to find your country by your IP adress \n"
                    + "MyRegion - to find your region by your IP adress \n"
                    + "MyCity - to find your city by your IP adress \n"
                    + "MyCoordinates - to find your coordinates by your IP adress \n"
                    + "MyProvider - to find your provider by your IP adress \n"
                    + "MyOrgs - to find your organisations by your IP adress \n" 
                    + "CheckIp - to find information about a user of a certain IP adress");
            }
            else
            if(message.Text == "/keyboard")
            {
                ReplyKeyboardMarkup replyKeyboardMarkup = new
                    (
                    new[]
                    {
                       new KeyboardButton[] { "MyIp", "MyCountry", "MyRegion", "MyCity"},
                       new KeyboardButton[] { "MyCoordinates", "MyProvider", "MyOrgs", "CheckIp"}
                    }
                    )
                {
                    ResizeKeyboard = true
                };
                await botClient.SendTextMessageAsync(message.Chat.Id, "Chose an option:", replyMarkup : replyKeyboardMarkup);
                return;
            }
            else
            if(message.Text == "MyIp")
            {
                IPDataClient iPDataClient = new IPDataClient();
                await botClient.SendTextMessageAsync(message.Chat.Id, $"Your IP is: {iPDataClient.GetQueryAsync().Result.query}");
                return;
            }
            else
            if(message.Text == "MyCountry")
            {
                IPDataClient iPDataClient = new IPDataClient();
                await botClient.SendTextMessageAsync(message.Chat.Id, $"Your country is: {iPDataClient.GetCountryAsync().Result.country}");
                return;
            }
            else
            if (message.Text == "MyRegion")
            {
                IPDataClient iPDataClient = new IPDataClient();
                await botClient.SendTextMessageAsync(message.Chat.Id, $"Your region is: {iPDataClient.GetRegionAsync().Result.regionName}");
                return;
            }
            else
            if (message.Text == "MyCity")
            {
                IPDataClient iPDataClient = new IPDataClient();
                await botClient.SendTextMessageAsync(message.Chat.Id, $"Your city is: {iPDataClient.GetCityAsync().Result.city}");
                return;
            }
            else
            if (message.Text == "MyCoordinates")
            {
                IPDataClient iPDataClient = new IPDataClient();
                await botClient.SendTextMessageAsync(message.Chat.Id, $"Your latitude is: {iPDataClient.GetCoordAsync().Result.lat}\nYour longitude is: {iPDataClient.GetCoordAsync().Result.lon}");
                return;
            }
            else
            if (message.Text == "MyProvider")
            {
                IPDataClient iPDataClient = new IPDataClient();
                await botClient.SendTextMessageAsync(message.Chat.Id, $"Your provider is: {iPDataClient.GetIspAsync().Result.isp}");
                return;
            }
            else
            if (message.Text == "MyOrgs")
            {
                IPDataClient iPDataClient = new IPDataClient();
                await botClient.SendTextMessageAsync(message.Chat.Id, $"Your organisations are: {iPDataClient.GetOrgsAsync().Result.org}, {iPDataClient.GetOrgsAsync().Result.As}");
                return;
            }
            else
            
            if (message.Text == "CheckIp")
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, $"Enter some IP:");
                return;
            }
            else
            {
                IPDataClient iPDataClient = new IPDataClient();
                var result = iPDataClient.GetDataAsync(message.Text);
                if (result.Result.status == "success")
                {
                    iPDataClient.PostFoundIPDataAsync(message.Text);
                    await botClient.SendTextMessageAsync(message.Chat.Id, $"IP: {result.Result.query}\n"
                        + $"Status:{result.Result.status}\n"
                        + $"Country: {result.Result.country}\n"
                        + $"Region: {result.Result.regionName}\n"
                        + $"City: {result.Result.city}\n"
                        + $"Latitude: {result.Result.lat}\n"
                        + $"Longitude: {result.Result.lon}\n"
                        + $"Provider: {result.Result.isp}\n"
                        + $"Organization: {result.Result.org}\n"
                        + $"AS: {result.Result.As}");
                    return;
                }
                else
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, $"Status: {result.Result.status}\n IP not found!");
                }
            }
        }
    }
}
