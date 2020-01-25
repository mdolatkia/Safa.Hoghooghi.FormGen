
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Paper_MetadataManagement
{
    //public class BizRelationshipConverter
    //{
    //    BizRelationship bizRelationship = new BizRelationship();
    //    BizISARelationship bizISARelationship = new BizISARelationship();
    //    BizUnionRelationship bizUnionRelationship = new BizUnionRelationship();
    //    public void ConvertRelationship(RelationshipDTO relationship, Enum_RelationshipType targetRaltionshipType)
    //    {

         
    //        var relationshinInfo = dataHelper.GetRelationshipsInfo(relationship);

    //        if (relationship.TypeEnum == Enum_RelationshipType.OneToMany
    //            || targetRaltionshipType == Enum_RelationshipType.ManyToOne)
    //        {
    //            if (relationshinInfo.RelationType == RelationType.OnePKtoManyFK && relationshinInfo.FKHasData)
    //            {
    //                throw new Exception("بعلت وجود ارتباط یک به چند بین داده های دو جدول امکان تبدیل وجود ندارد");
    //            }
    //        }

    //        if (targetRaltionshipType == Enum_RelationshipType.OneToMany
    //            || targetRaltionshipType == Enum_RelationshipType.ManyToOne
    //            || targetRaltionshipType == Enum_RelationshipType.ImplicitOneToOne
    //            || targetRaltionshipType == Enum_RelationshipType.ExplicitOneToOne
    //            )
    //        {
    //            bizRelationship.ConvertRelationship(relationship, targetRaltionshipType);
    //        }
    //        else if (targetRaltionshipType == Enum_RelationshipType.SuperToSub)
    //        {
    //            // OneToMany , ImplicitOneToOne , UnionToSubUnion_SubUnionHoldsKeys , SubUnionToUnion_UnionHoldsKeys
    //            var isaRelationships = bizISARelationship.GetISARelationshipsByEntityID(relationship.EntityID1);
    //            ISARelationshipCreateOrSelect(isaRelationships, relationship, targetRaltionshipType);
    //        }
    //        else if (targetRaltionshipType == Enum_RelationshipType.SubToSuper)
    //        {
    //            // ManyToOne , ExplicitOneToOne , UnionToSubUnion_UnionHoldsKeys , SubUnionToUnion_SubUnionHoldsKeys
    //            var isaRelationships = bizISARelationship.GetISARelationshipsByEntityID(relationship.EntityID2);
    //            ISARelationshipCreateOrSelect(isaRelationships, relationship, targetRaltionshipType);
    //        }
    //        else if (targetRaltionshipType == Enum_RelationshipType.SubUnionToUnion_UnionHoldsKeys)
    //        {
    //            // OneToMany , ImplicitOneToOne , SuperToSub , UnionToSubUnion_SubUnionHoldsKeys
    //            var unionRelationships = bizUnionRelationship.GetUnionRelationshipsBySuperUnionEntity(relationship.EntityID2, true);
    //            UnionRelationshipCreateOrSelect(unionRelationships, true, relationship, targetRaltionshipType);
    //        }
    //        else if (targetRaltionshipType == Enum_RelationshipType.SubUnionToUnion_SubUnionHoldsKeys)
    //        {
    //            // ManyToOne , ExplicitOneToOne , SuperToSub , UnionToSubUnion_UnionHoldsKeys
    //            var unionRelationships = bizUnionRelationship.GetUnionRelationshipsBySuperUnionEntity(relationship.EntityID2, false);
    //            UnionRelationshipCreateOrSelect(unionRelationships, false, relationship, targetRaltionshipType);
    //        }
    //        else if (targetRaltionshipType == Enum_RelationshipType.UnionToSubUnion_SubUnionHoldsKeys)
    //        {
    //            // OneToMany , ImplicitOneToOne , SuperToSub , SubUnionToUnion_UnionHoldsKeys
    //            var unionRelationships = bizUnionRelationship.GetUnionRelationshipsBySuperUnionEntity(relationship.EntityID1, true);
    //            UnionRelationshipCreateOrSelect(unionRelationships, true, relationship, targetRaltionshipType);
    //        }
    //        else if (targetRaltionshipType == Enum_RelationshipType.UnionToSubUnion_UnionHoldsKeys)
    //        {
    //            // ManyToOne , ExplicitOneToOne , SuperToSub , SubUnionToUnion_SubUnionHoldsKeys
    //            var unionRelationships = bizUnionRelationship.GetUnionRelationshipsBySuperUnionEntity(relationship.EntityID1, false);
    //            UnionRelationshipCreateOrSelect(unionRelationships, false, relationship, targetRaltionshipType);
    //        }

    //        //else if (relationship.TypeEnum == Enum_RelationshipType.ManyToOne)
    //        //{
    //        //    if (bizRelationship.RelationshipHasManyData(relationship))
    //        //    {
    //        //        throw new Exception("بعلت وجود ارتباط یک به چند بین داده های دو جدول امکان تبدیل وجود ندارد");
    //        //    }
    //        //    if (targetRaltionshipType == Enum_RelationshipType.ExplicitOneToOne)
    //        //        bizRelationship.ConvertManyToOneToExplicit(relationship);
    //        //    else if (targetRaltionshipType == Enum_RelationshipType.SubToSuper)
    //        //    {
    //        //        var isaRelationships = bizISARelationship.GetISARelationships(relationship.EntityID2);
    //        //        ISARelationshipCreateOrSelect(isaRelationships, relationship, targetRaltionshipType);
    //        //    }
    //        //    else if (targetRaltionshipType == Enum_RelationshipType.UnionToSubUnion_UnionHoldsKeys)
    //        //    {
    //        //        var unionRelationships = bizUnionRelationship.GetUnionRelationships(relationship.EntityID1, true);
    //        //        UnionRelationshipCreateOrSelect(unionRelationships, true, relationship, targetRaltionshipType);
    //        //    }
    //        //    else if (targetRaltionshipType == Enum_RelationshipType.SubUnionToUnion_SubUnionHoldsKeys)
    //        //    {
    //        //        var unionRelationships = bizUnionRelationship.GetUnionRelationships(relationship.EntityID1, false);
    //        //        UnionRelationshipCreateOrSelect(unionRelationships, false, relationship, targetRaltionshipType);
    //        //    }

    //        //}

    //    }
    //    private void ISARelationshipCreateOrSelect(List<ISARelationshipDTO> list, RelationshipDTO relationship, Enum_RelationshipType targetRaltionshipType)
    //    {
    //        frmISARelationshipCreateSelect frm = new frmISARelationshipCreateSelect(list);
    //        frm.ISARelationshipSelected += (sender, e) => frm_ISARelationshipSelected(e.ISARelationship, relationship, targetRaltionshipType);
    //        frm.ShowDialog();
    //    }


    //    void frm_ISARelationshipSelected(ISARelationshipDTO isaRelationshipDTO, RelationshipDTO relationship, Enum_RelationshipType targetRaltionshipType)
    //    {
    //        bizRelationship.ConvertRelationship(relationship, targetRaltionshipType, isaRelationshipDTO.ID);
    //    }

    //    private void UnionRelationshipCreateOrSelect(List<UnionRelationshipDTO> list, bool unionHoldsKeys, RelationshipDTO relationship, Enum_RelationshipType targetRaltionshipType)
    //    {
    //        frmUnionRelationshipCreateSelect frm = new frmUnionRelationshipCreateSelect(list, unionHoldsKeys);
    //        frm.UnionRelationshipSelected += (sender, e) => frm_UnionRelationshipSelected(e.UnionRelationship, relationship, targetRaltionshipType);
    //        frm.ShowDialog();
    //    }
    //    void frm_UnionRelationshipSelected(UnionRelationshipDTO unionRelationshipDTO, RelationshipDTO relationship, Enum_RelationshipType targetRaltionshipType)
    //    {
    //        bizRelationship.ConvertRelationship(relationship, targetRaltionshipType, 0, unionRelationshipDTO.ID);
    //    }



    //    public List<Enum_RelationshipType> GetRelationshipConvertOptions(RelationshipDTO relationship)
    //    {
    //        List<Enum_RelationshipType> result = new List<Enum_RelationshipType>();
    //        if (relationship.TypeEnum == Enum_RelationshipType.OneToMany)
    //        {
    //            result.Add(Enum_RelationshipType.ImplicitOneToOne);
    //            result.Add(Enum_RelationshipType.SuperToSub);
    //            result.Add(Enum_RelationshipType.UnionToSubUnion_SubUnionHoldsKeys);
    //            result.Add(Enum_RelationshipType.SubUnionToUnion_UnionHoldsKeys);
    //        }
    //        else if (relationship.TypeEnum == Enum_RelationshipType.ImplicitOneToOne)
    //        {
    //            result.Add(Enum_RelationshipType.OneToMany);
    //            result.Add(Enum_RelationshipType.SuperToSub);
    //            result.Add(Enum_RelationshipType.UnionToSubUnion_SubUnionHoldsKeys);
    //            result.Add(Enum_RelationshipType.SubUnionToUnion_UnionHoldsKeys);
    //        }
    //        else if (relationship.TypeEnum == Enum_RelationshipType.SuperToSub)
    //        {
    //            result.Add(Enum_RelationshipType.OneToMany);
    //            result.Add(Enum_RelationshipType.ImplicitOneToOne);
    //            result.Add(Enum_RelationshipType.UnionToSubUnion_SubUnionHoldsKeys);
    //            result.Add(Enum_RelationshipType.SubUnionToUnion_UnionHoldsKeys);
    //        }
    //        else if (relationship.TypeEnum == Enum_RelationshipType.UnionToSubUnion_SubUnionHoldsKeys)
    //        {
    //            result.Add(Enum_RelationshipType.OneToMany);
    //            result.Add(Enum_RelationshipType.ImplicitOneToOne);
    //            result.Add(Enum_RelationshipType.SuperToSub);
    //            result.Add(Enum_RelationshipType.SubUnionToUnion_UnionHoldsKeys);
    //        }
    //        else if (relationship.TypeEnum == Enum_RelationshipType.SubUnionToUnion_UnionHoldsKeys)
    //        {
    //            result.Add(Enum_RelationshipType.OneToMany);
    //            result.Add(Enum_RelationshipType.ImplicitOneToOne);
    //            result.Add(Enum_RelationshipType.SuperToSub);
    //            result.Add(Enum_RelationshipType.UnionToSubUnion_SubUnionHoldsKeys);
    //        }
    //        else if (relationship.TypeEnum == Enum_RelationshipType.ManyToOne)
    //        {
    //            result.Add(Enum_RelationshipType.ExplicitOneToOne);
    //            result.Add(Enum_RelationshipType.SubToSuper);
    //            result.Add(Enum_RelationshipType.UnionToSubUnion_UnionHoldsKeys);
    //            result.Add(Enum_RelationshipType.SubUnionToUnion_SubUnionHoldsKeys);
    //        }
    //        else if (relationship.TypeEnum == Enum_RelationshipType.ExplicitOneToOne)
    //        {
    //            result.Add(Enum_RelationshipType.ManyToOne);
    //            result.Add(Enum_RelationshipType.SubToSuper);
    //            result.Add(Enum_RelationshipType.UnionToSubUnion_UnionHoldsKeys);
    //            result.Add(Enum_RelationshipType.SubUnionToUnion_SubUnionHoldsKeys);
    //        }
    //        else if (relationship.TypeEnum == Enum_RelationshipType.SubToSuper)
    //        {
    //            result.Add(Enum_RelationshipType.ManyToOne);
    //            result.Add(Enum_RelationshipType.ExplicitOneToOne);
    //            result.Add(Enum_RelationshipType.UnionToSubUnion_UnionHoldsKeys);
    //            result.Add(Enum_RelationshipType.SubUnionToUnion_SubUnionHoldsKeys);
    //        }
    //        else if (relationship.TypeEnum == Enum_RelationshipType.UnionToSubUnion_UnionHoldsKeys)
    //        {
    //            result.Add(Enum_RelationshipType.ManyToOne);
    //            result.Add(Enum_RelationshipType.ExplicitOneToOne);
    //            result.Add(Enum_RelationshipType.SubToSuper);
    //            result.Add(Enum_RelationshipType.SubUnionToUnion_SubUnionHoldsKeys);
    //        }
    //        else if (relationship.TypeEnum == Enum_RelationshipType.SubUnionToUnion_SubUnionHoldsKeys)
    //        {
    //            result.Add(Enum_RelationshipType.ManyToOne);
    //            result.Add(Enum_RelationshipType.ExplicitOneToOne);
    //            result.Add(Enum_RelationshipType.SubToSuper);
    //            result.Add(Enum_RelationshipType.UnionToSubUnion_UnionHoldsKeys);
    //        }
    //        return result;
    //    }
    //}


}
