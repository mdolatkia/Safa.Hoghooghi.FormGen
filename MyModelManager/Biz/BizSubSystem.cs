using DataAccess;
using ModelEntites;
using MyGeneralLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModelManager
{
    public class BizSubSystem
    {
        public List<SubSystemDTO> GetAllSubSystems()
        {
            List<SubSystemDTO> result = new List<SubSystemDTO>();
            using (var context = new MyProjectEntities())
            {
                foreach (var item in context.SubSystems)
                {
                    result.Add(ToSubSystemDTO(item));
                }
            }
            return result;
        }

        private SubSystemDTO ToSubSystemDTO(SubSystems item)
        {
            SubSystemDTO result = new SubSystemDTO();
            result.ID = item.ID;
            result.Name = item.Name;
            result.Description = item.Description;
            return result;
        }

        public List<ObjectDTO> GetAllSubSystemsObjectDTO()
        {
            List<ObjectDTO> result = new List<ObjectDTO>();
            using (var context = new MyProjectEntities())
            {
                foreach (var item in context.SubSystems)
                {
                    //if (item.Name == "Archive")
                    //    result.Add(ToObjectDTO(DatabaseObjectCategory.Archive, item.ID, "آرشیو"));
                }
            }
            return result;
        }

        private ObjectDTO ToObjectDTO(DatabaseObjectCategory objectCategory, int objectIdentity, string title)
        {
            ObjectDTO result = new ObjectDTO();
            result.ObjectCategory = objectCategory;
            result.ObjectIdentity = objectIdentity;
            result.Title = title;
            return result;
        }
    }

}
