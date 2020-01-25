using ModelEntites;
using MyCodeFunctionLibrary;
using MyDataManagerBusiness;

using MyFormulaFunctionStateFunctionLibrary;
using MyInterfaces;
using MyModelManager;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyDataItemManager;
using MyLetterGenerator;
using ProxyLibrary.Request;

namespace MyFormulaManagerService
{
    public class LetterManagerService
    {
        BizLetter bizLetter = new BizLetter();
        BizLetterTemplate bizLetterTemplate = new BizLetterTemplate();
        LetterGenerator letterGenerator = new LetterGenerator();
        public List<LetterDTO> GetLetters(DR_Requester requester, List<int> dataitemIDS)
        {
            return bizLetter.GetLetters(requester, dataitemIDS);
        }
        public LetterDeleteResult DeleteLetter(DR_Requester requester, int letterID)
        {
            return bizLetter.DeleteLetter(requester, letterID);
        }

        public LetterResult UpdateLetter(LetterDTO letter, DR_Requester requester)
        {
            return bizLetter.UpdateLetter(letter, requester);
        }

        public List<MainLetterTemplateDTO> GetMainLetterTemplates(DR_Requester requester, int entityID)
        {
            return bizLetterTemplate.GetMainLetterTemplates(requester, entityID);
        }
        public MainLetterTemplateDTO GetLetterTemplate(DR_Requester requester, int letterTemplateID)
        {
            return bizLetterTemplate.GetMainLetterTepmplate(requester, letterTemplateID);
        }

        public List<LetterTypeDTO> GetLetterTypes()
        {
            return bizLetter.GetLetterTypes();
        }

        public LetterDTO GetLetter(DR_Requester requester, int letterID, bool withDetails)
        {
            return bizLetter.GetLetter(requester, letterID, withDetails);
        }

        public byte[] GenereateLetter(int letterTemplateID, List<EntityInstanceProperty> keyProperties, DR_Requester requester)
        {
            return letterGenerator.GenerateLetter(letterTemplateID, keyProperties, requester);
        }

        public LetterSettingDTO GetLetterSettings()
        {
            return bizLetterTemplate.GetLetterSetting(true);
        }

        public bool ConvertLetterToExternal(int letterID, string externalCode)
        {
            return bizLetter.ConvertLetterToExternal(letterID, externalCode);
        }

        public List<LetterRelationshipTailDTO> GetLetterRelationshipTails(DR_Requester dR_Requester, int entityID)
        {
            return bizLetterTemplate.GetLetterRelationshipTails(dR_Requester, entityID, true);
        }

        public List<LetterDTO> SearchLetters(DR_Requester dR_Requester, string singleFilterValue)
        {
            return bizLetter.SearchLetters(dR_Requester, singleFilterValue);
        }


        //public FormulaDTO GetFormula(int formulaID)
        //{
        //    BizFormula bizFormula = new BizFormula();
        //    return bizFormula.GetFormula(formulaID);
        //}

    }
}
