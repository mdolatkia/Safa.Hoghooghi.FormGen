using DataAccess;
using ModelEntites;
using MyModelManager;

using ProxyLibrary.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModelManager
{
    public class BizActivity
    {
        public List<ActivityDTO> GetActivities(int processID, bool withDetails)
        {
            List<ActivityDTO> result = new List<ActivityDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var listActivity = projectContext.Activity.Where(x => x.ProcessID == processID);
                foreach (var item in listActivity)
                    result.Add(ToActivityDTO(item, withDetails));
            }
            return result;
        }
        public ActivityDTO GetActivity(int ActivitysID, bool withDetails)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var Activitys = projectContext.Activity.First(x => x.ID == ActivitysID);
                return ToActivityDTO(Activitys, withDetails);
            }
        }
        public ActivityDTO ToActivityDTO(Activity item, bool withDetails)
        {
            ActivityDTO result = new ActivityDTO();
            result.ID = item.ID;
            result.Name = item.Name;
            result.ActivityType = (ActivityType)item.ActivityTypeID;
            result.ProcessID = item.ProcessID;
            result.Description = item.Description;

            if (withDetails)
            {
                BizTarget bizTarget = new BizTarget();
                BizRoleType bizRoleType = new BizRoleType();
                foreach (var target in item.ActivityTarget)
                {
                    ActivityTargetDTO at = new ActivityTargetDTO();
                    at.ID = target.ID;
                    at.TargetType = (TargetType)target.TargetType;
                    foreach (var role in target.ActivityTarget_RoleType)
                    {
                        at.RoleTypes.Add(bizRoleType.ToRoleTypeDTO(role.RoleType));
                    }
                    result.Targets.Add(at);
                }
            }

            return result;
        }
        public List<ActivityType> GetActivityTypes()
        {
            return Enum.GetValues(typeof(ActivityType)).Cast<ActivityType>().ToList();
        }
        public int UpdateActivitys(ActivityDTO message)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbActivity = projectContext.Activity.FirstOrDefault(x => x.ID == message.ID);
                if (dbActivity == null)
                    dbActivity = new DataAccess.Activity();
                dbActivity.ID = message.ID;
                dbActivity.Name = message.Name;
                dbActivity.ActivityTypeID = Convert.ToInt16(message.ActivityType);
                dbActivity.ProcessID = message.ProcessID;
                dbActivity.Description = message.Description;

                while (dbActivity.ActivityTarget.Any())
                {
                    while (dbActivity.ActivityTarget.First().ActivityTarget_RoleType.Any())
                        projectContext.ActivityTarget_RoleType.Remove(dbActivity.ActivityTarget.First().ActivityTarget_RoleType.First());
                    projectContext.ActivityTarget.Remove(dbActivity.ActivityTarget.First());

                }
                foreach (var msgActivityTarget in message.Targets)
                {
                    var dbActivityTarget = new ActivityTarget();
                    dbActivityTarget.TargetType = (Int16)msgActivityTarget.TargetType;
                    foreach (var role in msgActivityTarget.RoleTypes)
                    {
                        dbActivityTarget.ActivityTarget_RoleType.Add(new  ActivityTarget_RoleType() {  RoleTypeID = role.ID });
                    }
                    dbActivity.ActivityTarget.Add(dbActivityTarget);
                }

                if (dbActivity.ID == 0)
                    projectContext.Activity.Add(dbActivity);
                projectContext.SaveChanges();
                return dbActivity.ID;
            }
        }
    }
}
