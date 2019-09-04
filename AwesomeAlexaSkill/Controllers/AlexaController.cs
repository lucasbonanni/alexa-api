using System.Web.Http;

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
        public dynamic AwesomeAlexaSkill(dynamic request)
        {
            //if(request.Session.Application.ApplicationId != appId)
            //{
            //    throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);
            //}

            return new
            {
                version = "1.0",
                sessionAttributes = new { },
                response = new
                {
                    outputSpeech = new
                    {
                        type = "PlainText",
                        text = "Hello world"
                    },
                    card = new
                    {
                        type = "Simple",
                        title = "AwesomeAlexaSkill",
                        content = "Intelligence is the ability to adapt to change."
                    },
                    shouldEndSession = true
                }
            };
        }
    }
}
