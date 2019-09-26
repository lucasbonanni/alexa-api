using System;
using System.Linq;

namespace AlexaSkill.Data
{
    public class Requests
    {
        public Request Create(Request request)
        {
            using (var db = new AwesomeAlexaSkillEntities())
            {
                var member = db.Members.FirstOrDefault(m => m.AlexaUserId == request.UserId);

                if (member == null)
                {
                    request.Member = new Member() { AlexaUserId = request.UserId, CreatedDate = DateTime.UtcNow, LastRequestDate = DateTime.UtcNow, RequestCount = 1 };

                    db.Requests.Add(request);
                }
                else
                {
                    member.Requests.Add(request);
                }

                db.SaveChanges();
            }

            return request;
        }
    }
}
