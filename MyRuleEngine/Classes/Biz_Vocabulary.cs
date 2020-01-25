using MyRuleEngine.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRuleEngine
{
    public class Biz_Vocabulary
    {
        public static T GetVocabulary<T>(string vocabularyName, params object[] objects)
        {
            using (var context = new MyRuleEngineEntities())
            {
                var dbVocabulary = context.Vocabulary.Where(x => x.Name == vocabularyName).FirstOrDefault();
                if (dbVocabulary != null)
                {

                    var vocabularyInstance = ReflectionHelper.GetClassInstance(dbVocabulary.AssemblyInfo.Name, dbVocabulary.AssemblyInfo.Path, dbVocabulary.ClassName);
                    if (vocabularyInstance != null)
                    {
                        if (ReflectionHelper.ImplementsInterface(vocabularyInstance, typeof(IVocabulary)))
                        {
                            var result = ((IVocabulary)vocabularyInstance).GetVocabulary(objects);
                            if (result.GetType() == typeof(T))
                                return (T)result;
                        }
                        else
                            throw (new Exception("Vocabulary class '" + dbVocabulary.ClassName + "' is not of type IVocabulary"));
                    }
                    else
                        throw (new Exception("Vocabulary class '" + dbVocabulary.ClassName + "' colud not be found in assembly '" + dbVocabulary.AssemblyInfo.Name + "'"));
                }
                else
                    throw (new Exception("Vocabulary in not defined!"));
            }

            return default(T);
        }

    }
}
