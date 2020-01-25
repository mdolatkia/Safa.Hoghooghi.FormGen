using DataAccess;
using ModelEntites;
using ProxyLibrary.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModelManager
{
    public class BizTarget
    {
        public List<TargetType> GetTargetTypes()
        {
            return Enum.GetValues(typeof(TargetType)).Cast<TargetType>().ToList();
        }

        //public List<TargetDTO> GetTargets()
        //{
        //    List<TargetDTO> result = new List<TargetDTO>();
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var listTarget = projectContext.Target;
        //        foreach (var item in listTarget)
        //            result.Add(ToTargetDTO(item));
        //    }
        //    return result;
        //}
        //public TargetDTO GetTarget(int TargetsID)
        //{
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var Targets = projectContext.Target.First(x => x.ID == TargetsID);
        //        return ToTargetDTO(Targets);
        //    }
        //}
        //public TargetDTO ToTargetDTO(Target item)
        //{
        //    TargetDTO result = new TargetDTO();
        //    result.ID = item.ID;
        //    result.Name = item.Name;


        //    return result;
        //}
        //public void UpdateTargets(TargetDTO message)
        //{
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var dbTarget = projectContext.Target.FirstOrDefault(x => x.ID == message.ID);
        //        if (dbTarget == null)
        //            dbTarget = new DataAccess.Target();
        //        dbTarget.ID = message.ID;
        //        dbTarget.Name = message.Name;

        //        if (dbTarget.ID == 0)
        //            projectContext.Target.Add(dbTarget);
        //        projectContext.SaveChanges();
        //    }
        //}
    }
}
