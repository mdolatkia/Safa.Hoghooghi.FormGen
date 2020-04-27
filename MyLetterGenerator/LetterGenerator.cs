using MyDataSearchManagerBusiness;
using MyFormulaFunctionStateFunctionLibrary;
using MyModelManager;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;
using ModelEntites;
using MyRelationshipDataManager;

namespace MyLetterGenerator
{
    public class LetterGenerator
    {
        RelationshipDataManager relationshipDataManager = new RelationshipDataManager();
        FormulaFunctionHandler formulaHandler = new FormulaFunctionHandler();
        BizLetterTemplate bizLetterTemplate = new BizLetterTemplate();
        SearchRequestManager searchProcessor = new SearchRequestManager();
        public byte[] GenerateLetter(int letterTemplateID, List<EntityInstanceProperty> keyProperties, DR_Requester requester)
        {
            var letterTemplete = bizLetterTemplate.GetMainLetterTepmplate(requester, letterTemplateID);
            DP_SearchRepository searchDataItem = new DP_SearchRepository(letterTemplete.TableDrivedEntityID);
            foreach (var property in keyProperties)
                searchDataItem.Phrases.Add(new SearchProperty() { ColumnID = property.ColumnID, Value = property.Value });

            DP_DataView dataviewItem = null;
            //سکوریتی داده اعمال میشود
            //////DR_SearchFullDataRequest request = new DR_SearchFullDataRequest(requester,searchDataItem);
            //////var searchResult = searchProcessor.Process(request);
            //////if (searchResult.Result == Enum_DR_ResultType.SeccessfullyDone)
            //////    dataItem = searchResult.ResultDataItems.FirstOrDefault();
            //////else if (searchResult.Result == Enum_DR_ResultType.ExceptionThrown)
            //////    throw (new Exception(searchResult.Message));

            DR_SearchViewRequest request = new DR_SearchViewRequest(requester, searchDataItem);
            request.EntityViewID = letterTemplete.EntityListViewID;
            var searchResult = searchProcessor.Process(request);
            if (searchResult.Result == Enum_DR_ResultType.SeccessfullyDone)
                dataviewItem = searchResult.ResultDataItems.FirstOrDefault();
            else if (searchResult.Result == Enum_DR_ResultType.ExceptionThrown)
                throw (new Exception(searchResult.Message));


            //var dataItem = searchProcessor.GetDataItemsByListOFSearchProperties(requester, searchDataItem).FirstOrDefault();


            string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var fileName = Guid.NewGuid() + "." + letterTemplete.FileExtension;
            string filePath = path + @"\\" + fileName;
            System.IO.File.WriteAllBytes(filePath, letterTemplete.Content);

            Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document doc = new Microsoft.Office.Interop.Word.Document();

            doc = word.Documents.Open(filePath);
            doc.Activate();
            word.Visible = true;
            //foreach (Microsoft.Office.Interop.Word.ContentControl cc in doc.ContentControls)
            //{
            //    //cc.
            //    //MessageBox.Show(cc.Tag);
            //    //MessageBox.Show(cc.Title);
            //}
            //List<Microsoft.Office.Interop.Word.Field> letterFields = new List<Microsoft.Office.Interop.Word.Field>();
            //foreach (Microsoft.Office.Interop.Word.Field field in doc.Fields)
            //    letterFields.Add(field);

            GenerateFieleds(doc, dataviewItem, letterTemplete, requester);


            //   doc.SaveAs2(@"N:\mehler\Ausgefuellt.docx");
            doc.Save();
            doc.Close();
            word.Quit();

            return System.IO.File.ReadAllBytes(filePath);

        }

        private void GenerateFieleds(Document doc, DP_DataView dataItem, LetterTemplateDTO letterTemplete, DR_Requester requester)
        {
            var fileds = GetFields(doc);
            //ParagraphFormat paragraphFormat = null;
            GenerateValues(doc, dataItem, letterTemplete, fileds.Item1, fileds.Item2, requester, "main", null);
            //PrepareDocForGenerate(null, fileds.Item1, fileds.Item2);
            //GenerateValues(null, fileds.Item1, fileds.Item2, dataItem, requester);

            //foreach (var relationshipField in fileds.Item2)
            //{
            //    var bizField = letterTemplete.RelationshipFields.FirstOrDefault(x => x.FieldName == relationshipField.FieldName);
            //    if (bizField != null)
            //    {
            //        if (bizField.RelationshipID != 0)
            //        {
            //            var searchRepository = relationshipDataManager.GetSearchDataItemByRelationship(dataItem, bizField.RelationshipID, requester);
            //            var relatedDataItems = searchProcessor.GetDataItemsByListOFSearchProperties(requester, searchRepository);
            //            if (relatedDataItems.Any())
            //            {
            //                foreach (var item in relatedDataItems)
            //                {
            //                    CopyRange(relationshipField, item);
            //                }
            //            }
            //            RemoveRange(relationshipField);
            //        }
            //    }
            //}
        }

        private void GenerateValues(Document doc, DP_DataView dataviewItem, LetterTemplateDTO letterTemplete, List<LetterTemplatePlainFieldDTO> plainFields, List<LetterTemplateRelationshipFieldDTO> relationshipFields, DR_Requester requester, string parentTail, ParagraphFormat paragraphFormat)
        {
            //int start;
            //int end;
            //if (parentTail == "")
            //{
            //    start = 0;
            //    end = 1000000;
            //}
            //else
            //{
            //    start = parentRange.Item1;
            //    end = parentRange.Item2;
            //}
            foreach (var plainField in plainFields)
            {
                if (plainField.tmpParentTail == parentTail)
                {
                    var bizField = letterTemplete.PlainFields.FirstOrDefault(x => x.FieldName == plainField.FieldName);
                    if (bizField != null)
                    {

                        if (bizField.EntityListViewColumnsID != 0)
                        {


                            //درست شود
                            var property = dataviewItem.Properties.FirstOrDefault(x => x.RelativeName == bizField.EntityListViewColumns.RelativeColumnName);
                            if (property != null)
                                (plainField.LetterField as Field).Result.Text = property.Value==null?"":property.Value.ToString();

                        }
                        else if (bizField.FormulaID != 0)
                        {
                            DP_DataRepository dataRepository = new DP_DataRepository(dataviewItem.TargetEntityID, dataviewItem.TargetEntityAlias);
                            dataRepository.DataView = dataviewItem;
                            foreach (var key in dataviewItem.Properties.Where(x => x.IsKey))
                            {
                                dataRepository.AddProperty(new ColumnDTO() { ID = key.ColumnID }, key.Value);
                            }
                            var parameterValue = formulaHandler.CalculateFormula(bizField.FormulaID, dataRepository, requester);
                            if (parameterValue.Result != null)
                                (plainField.LetterField as Field).Result.Text = parameterValue.Result.ToString();
                        }
                    }
                }
            }

            foreach (var relationshipField in relationshipFields)
            {
                var bizField = letterTemplete.RelationshipFields.FirstOrDefault(x => x.FieldName == relationshipField.FieldName);
                if (bizField != null)
                {
                    //List<EntityInstanceProperty> columnValues = new List<EntityInstanceProperty>();
                    //foreach (var property in dataviewItem.KeyProperties)
                    //    columnValues.Add(property);
                    //var searchRepository = relationshipDataManager.GetSearchDataItemByRelationship(dataItem, bizField.RelationshipID, requester);
                    //var relatedDataItems = searchProcessor.GetDataItemsByListOFSearchProperties(requester, searchRepository);
                    List<DP_DataView> relatedDataItems = null;
                    //سکوریتی داده اعمال میشود
                    RelationshipTailDataManager relationshipTailDataManager = new RelationshipTailDataManager();
                    var searchDataItem = relationshipTailDataManager.GetTargetSearchItemFromRelationshipTail(dataviewItem, bizField.RelationshipTail);

                    DR_SearchViewRequest request = new DR_SearchViewRequest(requester, searchDataItem);
                    request.EntityViewID = bizField.PartialLetterTemplate.EntityListViewID;
                    var searchResult = searchProcessor.Process(request);
                    if (searchResult.Result == Enum_DR_ResultType.SeccessfullyDone)
                        relatedDataItems = searchResult.ResultDataItems;
                    else if (searchResult.Result == Enum_DR_ResultType.ExceptionThrown)
                        throw (new Exception(searchResult.Message));

                    //DR_SearchByRelationshipTailRequest request = new DR_SearchByRelationshipTailRequest(requester);
                    //request.FirstRelationshipFirstSideKeyColumns = columnValues;
                    //request.RelationshipTail = bizField.RelationshipTail;
                    //var process = searchProcessor.Process(request);
                    //if (process.Result == Enum_DR_ResultType.SeccessfullyDone)
                    //    relatedDataItems = process.ResultDataItems;
                    //else if (process.Result == Enum_DR_ResultType.ExceptionThrown)
                    //    throw (new Exception(process.Message));
                    if (relatedDataItems.Any())
                    {
                        int itemIndex = 0;
                        foreach (var item in relatedDataItems)
                        {
                            itemIndex++;

                            var copyItem = Copy(doc, relationshipField.StartLetterField as Field, relationshipField.EndLetterField as Field, bizField.IsRow, ref paragraphFormat); ;
                            if (copyItem != null)
                            {
                                var tail = (parentTail == "" ? "" : parentTail + ",") + relationshipField.FieldName;
                                var fields = GetFields(doc, copyItem, tail);
                                GenerateValues(doc, item, bizField.PartialLetterTemplate, fields.Item1, fields.Item2, requester, tail, paragraphFormat);
                            }
                        }
                    }
                    ClearItem(doc, relationshipField.StartLetterField as Field, relationshipField.EndLetterField as Field, bizField.IsRow);
                }
            }
        }

        private void ClearItem(Document doc, Field startField, Field endField, bool isRow)
        {
            if (isRow)
            {
                var startFieldRange = doc.Range(startField.Result.Start, startField.Result.End);
                startFieldRange.Select();
                doc.Application.Selection.Delete();
                var endFieldRange = doc.Range(endField.Result.Start, endField.Result.End);
                endFieldRange.Select();
                doc.Application.Selection.Delete();

                var mainRange = doc.Range(startField.Result.Start, endField.Result.End);
                mainRange.Select();
                var tables = mainRange.Tables;
                Table table = null;
                Row row = null;
                if (tables.Count > 0)
                {
                    foreach (Table tt in tables)
                    {
                        table = tt;
                        break;
                    }
                    if (table != null)
                    {
                        int rowIndex = 1;
                        foreach (Row rr in table.Rows)
                        {
                            if (rowIndex == 2)
                            {
                                row = rr;
                                break;
                            }
                            rowIndex++;
                        }
                    }
                }
                if (table != null && row != null)
                {
                    row.Delete();
                    //table.Rows.Delete()
                    //   var tmpRow = doc.Range(row.Range.Start, row.Range.End);
                    //tmpRow.Select();
                    //doc.Application.Selection.Delete();
                }
            }
            else
            {
                var mainRange = doc.Range(startField.Result.Start, endField.Result.End);
                mainRange.Select();
                mainRange.Application.Selection.Delete();
            }
        }





        private Range Copy(Document doc, Field startField, Field endField, bool isRow, ref ParagraphFormat paragraphFormat)
        {

            //var startField = relationshipField.StartLetterField as Field;
            //var endField = relationshipField.EndLetterField as Field;
            //startField.Result.Text = "";
            //endField.Result.Text = "";
            //var startText = startField.Result.Text;
            //var endText = endField.Result.Text;
            //int ii = 0;



            if (isRow)
            {
                //int ii = 0;

                var mainRange = doc.Range(startField.Result.Start, endField.Result.End);
                //////mainRange.Select();
                var tables = mainRange.Tables;

                Table table = null;
                Row row = null;
                if (tables.Count > 0)
                {
                    foreach (Table tt in tables)
                    {
                        table = tt;
                        break;
                    }
                    if (table != null)
                    {
                        int rowIndex = 1;
                        foreach (Row rr in table.Rows)
                        {
                            if (rowIndex == 2)
                            {
                                row = rr;
                                break;
                            }
                            rowIndex++;
                        }
                    }
                }
                if (table != null && row != null)
                {
                    Row newRow = table.Rows.Add();
                    int cellindex = 0;
                    foreach (Cell cell in row.Cells)
                    {
                        Cell newRowCell = null;
                        int tmpIndex = 0;
                        foreach (Cell tmpCell in newRow.Cells)
                        {
                            if (tmpIndex == cellindex)
                            {
                                newRowCell = tmpCell;
                                break;
                            }
                            tmpIndex++;
                        }
                        var cellrange = doc.Range(cell.Range.Start, cell.Range.End);
                        cellrange.MoveEnd(WdUnits.wdCharacter, -1);
                        //////cellrange.Select();
                        newRowCell.Range.InsertXML(cellrange.XML);
                        newRowCell.Range.ParagraphFormat = cell.Range.ParagraphFormat.Duplicate;
                        cellindex++;
                    }
                    var finalRange = doc.Range(newRow.Range.Start, newRow.Range.End - 1);
                    //////nextRange.Select();
                    //////nextRange.InsertXML(mainRange.XML);

                    //////nextRange.Select();
                    //foreach (Field field in nextRange.Fields)
                    //{
                    //    if (field.Result.Text == startField.Result.Text)
                    //        field.Result.Text = "";
                    //    if (field.Result.Text == endField.Result.Text)
                    //        field.Result.Text = "";
                    //}

                    return finalRange;
                }
            }
            else
            {
                int startTempRange = startField.Result.Start;
                int endTempRange = endField.Result.End;
                var tempRange = doc.Range(startTempRange, endTempRange);
                var aa = tempRange.Text;
                paragraphFormat = tempRange.ParagraphFormat.Duplicate;
                //////tempRange.Select();
                var bb = doc.Application.Selection.Text;
                var mainRange = doc.Range(startTempRange, endTempRange);
                mainRange.MoveStart(WdUnits.wdCharacter, startField.Result.Text.Length);
                //////mainRange.Select();
                mainRange.MoveEnd(WdUnits.wdCharacter, -1 * (startField.Result.Text.Length - 2));
                //////mainRange.Select();
                var cc = doc.Application.Selection.Text;
                var start = 0;


                //بیخیال حذف یا افزودن اینتر شدم چون رفتار مایکروسافت ورد در اینزرت کردن ایکس ام ال کاملا غیر قابل پیش بینی است
                //برای همه خود اینزرت ایکس ام ال یک اینتر میگذارد


                //if (nextLine)
                //{
                //var breakstart = endTempRange + 1;
                //var breakRange = doc.Range(breakstart, breakstart);
                //breakRange.InsertBreak(WdBreakType.wdLineBreak);
                //start = breakstart + 1;
                //}
                //else
                start = endTempRange + 1;
                //ظاهرا خود اینزرت ایکس ام ال اگر بعدش اینتر نباشد خودش یدونه میزاره
                //start = endTempRange + 1;
                var length = (mainRange.End + 1) - (mainRange.Start - 1);


                //var nextRange = doc.Range(start, start + 1);
                //nextRange.Select();
                //bool willPutReturn = true;
                //if (nextRange.Text == "\r")
                //    willPutReturn = false;

                var finaRange = doc.Range(start, start);
                //////finaRange.Select();
                finaRange.InsertXML(mainRange.XML);

                finaRange.ParagraphFormat = paragraphFormat;
                finaRange.SetRange(start, start + length);
                //////finaRange.Select();

                //if (!nextLine)
                //{
                //    //if (willPutReturn)
                //    //{
                //    var possibleReturn = doc.Range(finaRange.End, finaRange.End + 1);
                //    possibleReturn.Select();
                //    if (possibleReturn.Text != null)
                //        if (possibleReturn.Text == "\r" || possibleReturn.Text.Trim() == "\r\r")
                //        {
                //            possibleReturn.Select();
                //            possibleReturn.Text = "";
                //        }
                //    //}
                //}


                //foreach (Field field in nextRange.Fields)
                //{
                //    if (field.Result.Text == startField.Result.Text)
                //        field.Result.Text = "";
                //    if (field.Result.Text == endField.Result.Text)
                //        field.Result.Text = "";

                //}
                return finaRange;
            }
            return null;
        }

        private Field GetFieldByValue(Document doc, string startText)
        {

            foreach (Field item in doc.Fields)
            {
                if (item.Result.Text == startText)
                    return item;
            }
            return null;
        }

        //private void PrepareDocForGenerate(LetterTemplateRelationshipFieldDTO parent, List<LetterTemplatePlainFieldDTO> plainFields, List<LetterTemplateRelationshipFieldDTO> relationshipFields)
        //{
        //    string key = "";
        //    if (parent == null)
        //        key = "main";
        //    else
        //    {
        //        var guid = Guid.NewGuid();
        //        key = guid.ToString();
        //        parent.tmpGuid = guid;
        //    }
        //    foreach (var plainField in plainFields)
        //    {
        //        if (plainField.ColumnID != 0)
        //        {
        //            (plainField.LetterField as Field).Result.Text = key + "_" + "col" + "_" + plainField.ColumnID;
        //        }
        //        else if (plainField.ParameterID != 0)
        //        {
        //            (plainField.LetterField as Field).Result.Text = key + "_" + "prm" + "_" + plainField.ParameterID;
        //        }

        //    }
        //    foreach (var relationshipField in relationshipFields)
        //    {
        //        PrepareDocForGenerate(relationshipField, relationshipField.InternalLetterTemplate.PlainFields, relationshipField.InternalLetterTemplate.RelationshipFields);
        //    }
        //}
        //private List<Replace> CopyRange(Document doc, Guid guid, LetterTemplateRelationshipFieldDTO relationshipField, DP_DataRepository item, List<Replace> list)
        //{



        //}

        //private void GenerateFieleds(DP_DataRepository dataItem, LetterTemplateDTO letterTemplete, List<Field> letterFields, bool v, Tuple<int, int> parentRange)
        //{
        //    foreach (var field in letterTemplete.PlainFields)
        //    {
        //        var letterField = letterFields.FirstOrDefault(x => x.Name == field.FieldName);
        //        if (letterField != null)
        //        {
        //            if (field.ColumnID != 0)
        //            {
        //                var column = dataItem.Properties.FirstOrDefault(x => x.ColumnID == field.ColumnID);
        //                if (column != null)
        //                    letterField.Result.Text = column.Value;
        //            }
        //            else if (field.ParameterID != 0)
        //            {
        //                var parameterValue = formulaHandler.CalculateFormula(field.ParameterID, new List<DP_DataRepository>() { dataItem }, dataItem, requester);
        //                if (parameterValue != null)
        //                    letterField.Result.Text = parameterValue.ToString();
        //            }
        //        }
        //    }
        //    int start;
        //    int end;
        //    if (v)
        //    {
        //        start = 0;
        //        end = 1000000;
        //    }
        //    else
        //    {
        //        start = parentRange.Item1;
        //        end = parentRange.Item2;
        //    }
        //    foreach (var field in letterTemplete.RelationshipFields)
        //    {
        //        var letterField = letterFields.Where(x => x.Name == field.FieldName);
        //        if (letterField != null)
        //        {

        //        }
        //    }
        //}
        private Tuple<List<LetterTemplatePlainFieldDTO>, List<LetterTemplateRelationshipFieldDTO>> GetFields(Document doc, Range range, string tail)
        {//range.Fields??????? میشه استفاده کرد
            var allFormFileds = GetAllFields(doc, range);
            var plainFields = new List<LetterTemplatePlainFieldDTO>();
            var relationshipFields = new List<LetterTemplateRelationshipFieldDTO>();
            SetRelatoinshipTree(null, plainFields, relationshipFields, allFormFileds, tail);
            return new Tuple<List<LetterTemplatePlainFieldDTO>, List<LetterTemplateRelationshipFieldDTO>>(plainFields, relationshipFields);
        }
        private List<Tuple<string, Field>> GetAllFields(Document doc, Range range = null)
        {
            List<Tuple<string, Field>> allFormFileds = new List<Tuple<string, Field>>();
            if (range == null)
            {
                foreach (Microsoft.Office.Interop.Word.Field field in doc.Fields)
                {
                    if (field.Result.Text != null)
                        allFormFileds.Add(new Tuple<string, Field>(field.Result.Text.ToLower().Replace("»", "").Replace("«", "").Trim(), field));
                }
            }
            else
            {
                foreach (Microsoft.Office.Interop.Word.Field field in doc.Fields)
                {
                    if (field.Result.Text != null)
                        if (field.Result.Start > range.Start && field.Result.End < range.End)
                            allFormFileds.Add(new Tuple<string, Field>(field.Result.Text.ToLower().Replace("»", "").Replace("«", "").Trim(), field));
                }
            }
            return allFormFileds;
        }
        public Tuple<List<LetterTemplatePlainFieldDTO>, List<LetterTemplateRelationshipFieldDTO>> GetFields(Document doc)
        {
            var allFormFileds = GetAllFields(doc);
            var plainFields = new List<LetterTemplatePlainFieldDTO>();
            var relationshipFields = new List<LetterTemplateRelationshipFieldDTO>();
            SetRelatoinshipTree(null, plainFields, relationshipFields, allFormFileds, "main");
            return new Tuple<List<LetterTemplatePlainFieldDTO>, List<LetterTemplateRelationshipFieldDTO>>(plainFields, relationshipFields);
        }

        private void SetRelatoinshipTree(LetterTemplateRelationshipFieldDTO parent, List<LetterTemplatePlainFieldDTO> plaintFields, List<LetterTemplateRelationshipFieldDTO> relationshipFields, List<Tuple<string, Field>> allFormFileds, string parentTail = "")
        {
            BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
            int start;
            int end;

            if (parent == null)
            {
                start = 0;
                end = 1000000;
            }
            else
            {
                start = parent.tmpConsumedRange.Item1;
                end = parent.tmpConsumedRange.Item2;
            }
            //TableDrivedEntityDTO entity = null;
            //if (parent == null)
            //    entity = defaultEntity;
            //else if (parent.InternalLetterTemplate.TableDrivedEntityID != 0)
            //    entity = bizTableDrivedEntity.GetTableDrivedEntity(parent.InternalLetterTemplate.TableDrivedEntityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);

            foreach (var field in allFormFileds.Where(x => x.Item1.StartsWith("rel_") && x.Item1.EndsWith("_start") && x.Item2.Result.Start > start))
            {
                var splt = field.Item1.Split('_');
                var fieldName = "";
                if (splt.Count() > 2)
                    fieldName = splt[0] + "_" + splt[1] + "_" + splt[2];
                else
                    fieldName = splt[0] + "_" + splt[1];
                var endField = GetRelationshipFieldEnd(field.Item1, allFormFileds);
                if (endField != null)
                    if (ItemIsInRangeOf(endField, end))
                    {
                        if (!ItemIsInRangeOfChilds(field.Item2, relationshipFields))
                        {
                            var newField = new LetterTemplateRelationshipFieldDTO();
                            newField.FieldName = fieldName;
                            newField.tmpConsumedRange = new Tuple<int, int>(field.Item2.Result.End, endField.Result.Start);
                            newField.StartLetterField = field.Item2;
                            newField.EndLetterField = endField;
                            //////var relName = field.Name.Split('_')[1];
                            //////var relationship = entity.Relationships.FirstOrDefault(x => x.Entity2 == relName);
                            //////if (relationship != null)
                            //////{
                            //////    newField.RelationshipID = relationship.ID;
                            //////    newField.InternalLetterTemplate.TableDrivedEntityID = relationship.EntityID2;
                            //////    //SetLetterRelationshipTemplates(newField);
                            //////    //SetRelationshipFiltered(newField);
                            //////    var firstTemplate = newField.tmpInternalLetterTemplates.FirstOrDefault();
                            //////    if (firstTemplate != null)
                            //////        newField.InternalLetterTemplate.ID = firstTemplate.ID;
                            //////}
                            var tail = (parentTail == "" ? "" : parentTail + ",") + fieldName;

                            SetRelatoinshipTree(newField, newField.PartialLetterTemplate.PlainFields, newField.PartialLetterTemplate.RelationshipFields, allFormFileds, parentTail);
                            relationshipFields.Add(newField);
                        }
                    }
            }
            foreach (var field in allFormFileds.Where(x => !x.Item1.StartsWith("rel_")))// (x.Item1.StartsWith("col_") || x.Item1.StartsWith("prm_")) && x.Item2.Result.Start > start))
            {
                var splt = field.Item1.Split('_');
                var fieldName = field.Item1;// splt[0] + "_" + splt[1];

                if (ItemIsInRangeOf(field.Item2, end))
                {
                    if (!ItemIsInRangeOfChilds(field.Item2, relationshipFields))
                    {

                        var newField = new LetterTemplatePlainFieldDTO();
                        newField.FieldName = fieldName;
                        newField.LetterField = field.Item2;
                        newField.tmpParentTail = parentTail;
                        //if (fieldName.ToLower().StartsWith("col_"))
                        //{

                        //    //اینجا جای ست گردن مقادیر پیش فرض نیست.باید در همان فرم انجام شود
                        //    //////if (entity != null)
                        //    //////{
                        //    //////    var columnName = field.Name.Split('_')[1];
                        //    //////    var column = entity.Columns.FirstOrDefault(x => x.Name == columnName);
                        //    //////    if (column != null)
                        //    //////        newField.ColumnID = column.ID;
                        //    //////}
                        //}
                        //else if (fieldName.ToLower().StartsWith("prm_"))
                        //{
                        //    //اینجا جای ست گردن مقادیر پیش فرض نیست.باید در همان فرم انجام شود
                        //    //////int entityID = 0;
                        //    //////if (parent == null)
                        //    //////    entityID = EntityID;
                        //    //////else
                        //    //////    entityID = parent.InternalLetterTemplate.TableDrivedEntityID;
                        //    //////if (entityID != 0)
                        //    //////{
                        //    //////    var paramters = bizFormula.GetFormulaParameters(entityID);
                        //    //////    var parameterName = field.Name.Split('_')[1];
                        //    //////    var parameter = paramters.FirstOrDefault(x => x.Name == parameterName);
                        //    //////    if (parameter != null)
                        //    //////        newField.ParameterID = parameter.ID;
                        //    //////}
                        //}

                        plaintFields.Add(newField);
                    }
                }
            }


        }

        private bool ItemIsInRangeOfChilds(Field field, List<LetterTemplateRelationshipFieldDTO> templateFields)
        {
            foreach (var item in templateFields.Where(x => x.FieldName.ToLower().StartsWith("rel_")))
            {
                if (field.Result.Start >= item.tmpConsumedRange.Item1
                    && field.Result.End <= item.tmpConsumedRange.Item2)
                    return true;
                var childsResult = ItemIsInRangeOfChilds(field, item.PartialLetterTemplate.RelationshipFields);
                if (childsResult)
                    return true;
            }
            return false;
        }

        private bool ItemIsInRangeOf(Field endField, int end)
        {
            return endField.Result.Start < end;
        }

        private Field GetRelationshipFieldEnd(string fieldName, List<Tuple<string, Field>> allFormFileds)
        {
            var endName = fieldName.Replace("_start", "_end");
            return GetField(endName, allFormFileds);
        }

        private Field GetField(string endName, List<Tuple<string, Field>> allFormFileds)
        {
            foreach (var item in allFormFileds)
                if (item.Item1 == endName)
                    return item.Item2;
            return null;
        }

    }
    public class Sectionss
    {
        public DP_DataRepository DataItem { set; get; }
        public Field StartField { set; get; }
        public Field EndField { set; get; }
        public LetterTemplateRelationshipFieldDTO LetterTemplateRelationshipFieldDTO { set; get; }
        public List<Replace> Replaces { set; get; }

    }
    public class Replace
    {
        public DP_DataRepository DataItem { set; get; }
        public string DataItemKey { set; get; }
        public string SectionKey { set; get; }
        public int ColumnID { set; get; }
        public int ParameterID { set; get; }
    }
}
