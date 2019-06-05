// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.3.0

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CoreBot
{
    public static class LuisHelper
    {
        public static async Task<BookingHotelDetail> ExecuteLuisQuery(IConfiguration configuration, ILogger logger, ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var bookingDetails = new BookingHotelDetail();

            try
            {
                // Create the LUIS settings from configuration.
                var luisApplication = new LuisApplication(
                    configuration["LuisAppId"],
                    configuration["LuisAPIKey"],
                    "https://" + configuration["LuisAPIHostName"]
                );

                var recognizer = new LuisRecognizer(luisApplication);

                // The actual call to LUIS
                var recognizerResult = await recognizer.RecognizeAsync(turnContext, cancellationToken);

                var (intent, score) = recognizerResult.GetTopScoringIntent();
                if (intent == "订酒店")
                {
                    // We need to get the result from the LUIS JSON which at every level returns an array.
                    if (recognizerResult.Entities["location"] == null || recognizerResult.Entities["datetimeV2"] == null)
                    {
                        Console.WriteLine("Error :");
                        Console.WriteLine("LUIS setting Error");
                        Console.WriteLine("LUIS model training uncertain,There are some entites missing.");
                        bookingDetails.Location = "北京市";
                        bookingDetails.StartDate = "2019-07-20";

                        
                    }
                       
                    bookingDetails.Location = recognizerResult.Entities["location"]?.ToString();
                    bookingDetails.StartDate = recognizerResult.Entities["datetimeV2"]?.ToString();

                }
                // logger.LogWarning($"Predidct by luis.ai result that your intent is {intent} with score {score}");
                Console.WriteLine(intent);
                Console.WriteLine(score);
            }
            catch (Exception e)
            {
                logger.LogWarning($"LUIS Exception: {e.Message} Check your LUIS configuration.");
            }

            return bookingDetails;
        }
    }
}
