using System;
using System.Web.Http;
using AlexaSkill.Data;

namespace AwesomeAlexaSkill.Controllers
{
    public class AlexaController : ApiController
    {
        /// <summary>
        /// Here should be the app id in order to identify the request
        /// so one controller or one service could manage multiple skills
        /// </summary>
        private string appId = "";


        [HttpPost,Route("api/alexa/awesome")]
        public dynamic AwesomeAlexaSkill(dynamic alexaRequest)
        {
            //app id verification

            //if(request.Session.Application.ApplicationId != appId)
            //{
            //    throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);
            //}

            //seconds request verification

            //var totalSeconds = (DateTime.UtcNow - request.Request.Timestamp).TotalSeconds;
            //if(totalSeconds <= 0 || totalSeconds > 150)
            //    throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);


                var request = new Requests().Create(new Request
                {
                    MemberId = (alexaRequest.Session.Attributes == null) ? 0 : alexaRequest.Session.Attributes.MemberId,
                    Timestamp = alexaRequest.Request.Timestamp,
                    Intent = (alexaRequest.Request.Intent == null) ? "" : alexaRequest.Request.Intent.Name,
                    AppId = alexaRequest.Session.Application.ApplicationId,
                    RequestId = alexaRequest.Request.RequestId,
                    SessionId = alexaRequest.Session.SessionId,
                    UserId = alexaRequest.Session.User.UserId,
                    IsNew = alexaRequest.Session.New,
                    Version = alexaRequest.Version,
                    Type = alexaRequest.Request.Type,
                    Reason = alexaRequest.Request.Reason,
                    SlotsList = alexaRequest.Request.Intent.GetSlots(),
                    DateCreated = DateTime.UtcNow
                });

                AlexaResponse response = null;

                switch (request.Type)
                {
                    case "LaunchRequest":
                        response = LaunchRequestHandler(request);
                        break;
                    case "IntentRequest":
                        response = IntentRequestHandler(request);
                        break;
                    case "SessionEndedRequest":
                        response = SessionEndedRequestHandler(request);
                        break;
                }

                return response;
            }

            private AlexaResponse LaunchRequestHandler(Request request)
            {
                var response = new AlexaResponse("Welcome to Plural sight. What would you like to hear, the Top Courses or New Courses?");
                response.Session.MemberId = request.MemberId;
                response.Response.Card.Title = "Pluralsight";
                response.Response.Card.Content = "Hello\ncruel world!";
                response.Response.Reprompt.OutputSpeech.Text = "Please pick one, Top Courses or New Courses?";
                response.Response.ShouldEndSession = false;

                return response;
            }

            private AlexaResponse IntentRequestHandler(Request request)
            {
                AlexaResponse response = null;

                switch (request.Intent)
                {
                    case "Action1":
                        //response = Action1(request);
                        break;
                    case "Action2":
                        //response = Action1(request);
                        break;
                    case "AMAZON.CancelIntent":
                    case "AMAZON.StopIntent":
                        response = CancelOrStopIntentHandler(request);
                        break;
                    case "AMAZON.HelpIntent":
                        response = HelpIntent(request);
                        break;
                }

                return response;
            }

            private AlexaResponse HelpIntent(Request request)
            {
                var response = new AlexaResponse("To use the Plural sight skill, you can say, Alexa, ask Plural sight for top courses, to retrieve the top courses or say, Alexa, ask Plural sight for the new courses, to retrieve the latest new courses. You can also say, Alexa, stop or Alexa, cancel, at any time to exit the Plural sight skill. For now, do you want to hear the Top Courses or New Courses?", false);
                response.Response.Reprompt.OutputSpeech.Text = "Please select one, top courses or new courses?";
                return response;
            }

            private AlexaResponse CancelOrStopIntentHandler(Request request)
            {
                return new AlexaResponse("Thanks for listening, let's talk again soon.", true);
            }




            private AlexaResponse SessionEndedRequestHandler(Request request)
            {
                return null;
            }
        }
}
