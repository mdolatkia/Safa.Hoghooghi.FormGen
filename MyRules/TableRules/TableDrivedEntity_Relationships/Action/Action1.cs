using DataAccess;
using ModelEntites;
using MyDataManagerBusiness;
using MyRuleEngine;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRules.TableRules.TableDrivedEntity_Relationships.Action
{
    public class Action1 : IAction
    {
        public ActionResultEnum Execute(object[] objects)
        {
            //فقط بروی مدل دیتابیسی خصوصیات را ست میکند. عملیات سیو باید بیرون از این انجام شود
            //var context = ObjectExtractor.Extract<MyProjectEntities>(objects);
            //var entity = ObjectExtractor.Extract<TableDrivedEntity>(objects);

            //ModelDataHelper dataHelper = new ModelDataHelper();
            //Tuple<ISARelationship, TableDrivedEntity, List<TableDrivedEntity>> iSARelationship = null;
            //foreach (var relationship in entity.Relationship.Where(x => x.Relationship2 == null && x.RelationshipType == null))
            //{
            //    var relationInfo = dataHelper.GetRelationshipsInfo(relationship.ID);
            //    bool isaCondition = false;
            //    if ((relationInfo.FKRelatesOnPrimaryKey))
            //    {
            //        isaCondition = true;
            //    }

            //    if (isaCondition)
            //    {
            //        Relationship reverseRelationship = dataHelper.GetReverseRelationship(relationship);

            //        relationship.RelationshipType = new RelationshipType();
            //        reverseRelationship.RelationshipType = new RelationshipType();
            //        relationship.Enabled = true;
            //        reverseRelationship.Enabled = true;
            //        if (iSARelationship == null)
            //        {
            //            iSARelationship = new Tuple<ISARelationship, TableDrivedEntity, List<TableDrivedEntity>>(new ISARelationship(), relationship.TableDrivedEntity, new List<TableDrivedEntity>());

            //        }

            //        relationship.RelationshipType.SuperToSubRelationshipType = new SuperToSubRelationshipType();
            //        relationship.RelationshipType.IsOtherSideCreatable = true;
            //        relationship.RelationshipType.SuperToSubRelationshipType.ISARelationship = iSARelationship.Item1;
            //        relationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.SuperToSub);

            //        reverseRelationship.RelationshipType.SubToSuperRelationshipType = new SubToSuperRelationshipType();
            //        reverseRelationship.RelationshipType.IsOtherSideCreatable = true;
            //        reverseRelationship.RelationshipType.SubToSuperRelationshipType.ISARelationship = iSARelationship.Item1;
            //        reverseRelationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.SubToSuper);

            //        iSARelationship.Item3.Add(relationship.TableDrivedEntity1);
            //    }

            //    else if (relationInfo.RelationType == RelationType.OnePKtoOneFK)
            //    {//implicit explicit
            //        Relationship reverseRelationship = dataHelper.GetReverseRelationship(relationship);

            //        relationship.RelationshipType = new RelationshipType();
            //        reverseRelationship.RelationshipType = new RelationshipType();

            //        relationship.Enabled = true;
            //        reverseRelationship.Enabled = true;


            //        relationship.RelationshipType.ImplicitOneToOneRelationshipType = new ImplicitOneToOneRelationshipType();
            //        relationship.RelationshipType.IsOtherSideCreatable = true;
            //        relationship.RelationshipType.IsOtherSideMandatory = relationInfo.AllPrimarySideHasFkSideData;
            //        relationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.ImplicitOneToOne);

            //        reverseRelationship.RelationshipType.ExplicitOneToOneRelationshipType = new ExplicitOneToOneRelationshipType();
            //        reverseRelationship.RelationshipType.IsOtherSideCreatable = true;
            //        reverseRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.FKColumnIsMandatory;
            //        reverseRelationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.ExplicitOneToOne);
            //    }
            //    else if (relationInfo.RelationType == RelationType.OnePKtoManyFK)
            //    {
            //        Relationship reverseRelationship = dataHelper.GetReverseRelationship(relationship);

            //        relationship.RelationshipType = new RelationshipType();
            //        reverseRelationship.RelationshipType = new RelationshipType();

            //        relationship.Enabled = true;
            //        reverseRelationship.Enabled = true;

            //        relationship.RelationshipType.OneToManyRelationshipType = new OneToManyRelationshipType();
            //        relationship.RelationshipType.IsOtherSideMandatory = relationInfo.AllPrimarySideHasFkSideData;
            //        relationship.RelationshipType.IsOtherSideCreatable = true;
            //        relationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.OneToMany);

            //        reverseRelationship.RelationshipType.ManyToOneRelationshipType = new ManyToOneRelationshipType();
            //        reverseRelationship.RelationshipType.IsOtherSideCreatable = true;
            //        reverseRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.FKColumnIsMandatory;
            //        reverseRelationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.ManyToOne);
            //    }
            //}

            //if (iSARelationship != null)
            //{
            //    dataHelper.SetISA_RelationshipProperties(iSARelationship);
            //}
            return ActionResultEnum.Successful;
        }

        public event EventHandler<ActionInfoArg> ActionEvent;
    }


}
