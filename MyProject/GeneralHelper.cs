using AutoMapper;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject
{
    public class GeneralHelper
    {
        public static string GetCatalogName(string serverName, string databaseName)
        {
            return serverName + "\\" + databaseName;
        }
        public static Tuple< string,string> GetServerNameDatabaseName(string catalog)
        {
            return new Tuple<string,string>(catalog.Split('\\')[0] ,catalog.Split('\\')[1]);
        }
        internal static List<T> CreateListFromSingleObject<T>(T item)
        {
            List<T> list = new List<T>();
            list.Add(item);
            return list;
        }

        public static RelationshipDTO ToRelationshipDTO(Relationship item)
        {
            RelationshipDTO rItem = new RelationshipDTO();
            rItem.ID = item.ID;
            rItem.Name = item.Name;
            rItem.PairRelationshipID = item.RelationshipID;
            if (item.Relationship2 != null)
                rItem.PairRelationship = item.Relationship2.Name;
            rItem.Alias = item.Alias;
            rItem.Entity1 = item.TableDrivedEntity.Name;
            rItem.Entity2 = item.TableDrivedEntity1.Name;
            rItem.RelationshipColumns = "";
            foreach (var relColumn in item.RelationshipColumns)
                rItem.RelationshipColumns += (rItem.RelationshipColumns == "" ? "" : ",") + "(PK)" + item.TableDrivedEntity.Name + "." + relColumn.Column.Name + ">(FK)" + item.TableDrivedEntity1.Name + "." + relColumn.Column1.Name;

            rItem.TypeEnum = GetRelationshipType(item);
            rItem.TypeStr = rItem.TypeEnum.ToString();
            rItem.DataEntryEnabled = item.DataEntryEnabled;
            rItem.SearchEnabled = item.SearchEnabled;
            rItem.ViewEnabled = item.ViewEnabled;
            rItem.Enabled = item.Enabled;
            return rItem;
        }

        public static Enum_RelationshipType GetRelationshipType(Relationship relationship)
        {
            if (relationship.RelationshipType != null)
            {
                if (relationship.RelationshipType.OneToManyRelationshipType != null)
                    return Enum_RelationshipType.OneToMany;
                else if (relationship.RelationshipType.ManyToOneRelationshipType != null)
                    return Enum_RelationshipType.ManyToOne;
                else if (relationship.RelationshipType.ExplicitOneToOneRelationshipType != null)
                    return Enum_RelationshipType.ExplicitOneToOne;
                else if (relationship.RelationshipType.ImplicitOneToOneRelationshipType != null)
                    return Enum_RelationshipType.ImplicitOneToOne;
                else if (relationship.RelationshipType.SuperToSubRelationshipType != null)
                    return Enum_RelationshipType.SuperToSub;
                else if (relationship.RelationshipType.SubToSuperRelationshipType != null)
                    return Enum_RelationshipType.SubToSuper;
                else if (relationship.RelationshipType.UnionToSubUnionRelationshipType != null && relationship.RelationshipType.UnionToSubUnionRelationshipType.UnionRelationshipType.UnionHoldsKeys)
                    return Enum_RelationshipType.UnionToSubUnion_UnionHoldsKeys;
                else if (relationship.RelationshipType.SubUnionToUnionRelationshipType != null && relationship.RelationshipType.SubUnionToUnionRelationshipType.UnionRelationshipType.UnionHoldsKeys)
                    return Enum_RelationshipType.SubUnionToUnion_UnionHoldsKeys;
                else if (relationship.RelationshipType.UnionToSubUnionRelationshipType != null && !relationship.RelationshipType.UnionToSubUnionRelationshipType.UnionRelationshipType.UnionHoldsKeys)
                    return Enum_RelationshipType.UnionToSubUnion_SubUnionHoldsKeys;
                else if (relationship.RelationshipType.SubUnionToUnionRelationshipType != null && !relationship.RelationshipType.SubUnionToUnionRelationshipType.UnionRelationshipType.UnionHoldsKeys)
                    return Enum_RelationshipType.SubUnionToUnion_SubUnionHoldsKeys;
                else
                    return Enum_RelationshipType.None;
                // else if (relationship.RelationshipType.OneToManyRelationshipType != null)
                //    return Enum_RelationshipType.;
                //else if (relationship.RelationshipType.OneToManyRelationshipType != null)
                //    return Enum_RelationshipType.;
                //else if (relationship.RelationshipType.OneToManyRelationshipType != null)
                //    return Enum_RelationshipType.;
            }
            else
                return Enum_RelationshipType.None;

        }
        public static ColumnDTO ToColumnDTO(Column item)
        {
            ColumnDTO rItem = new ColumnDTO();
            rItem.ID = item.ID;
            rItem.Name = item.Name;
            rItem.Alias = item.Alias;
            rItem.IsNull = item.IsNull == true;
            rItem.IsMandatory = item.IsMandatory == true;
            return rItem;
        }

        public static ManyToOne ToManyToOne(RelationshipDTO rItem, ManyToOneRelationshipType manyToOneRelationshipType)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<RelationshipDTO, ManyToOne>());
            var result = AutoMapper.Mapper.Map<RelationshipDTO, ManyToOne>(rItem);
            result.IsOneSideTransferable = manyToOneRelationshipType.RelationshipType.IsOtherSideTransferable;
            result.IsOneSideCreatable = manyToOneRelationshipType.RelationshipType.IsOtherSideCreatable;
            result.IsOneSideMadatory = manyToOneRelationshipType.RelationshipType.IsOtherSideMandatory;
            result.IsOneSideDirectlyCreatable = manyToOneRelationshipType.RelationshipType.IsOtherSideDirectlyCreatable;

            return result;
        }


    }


}
