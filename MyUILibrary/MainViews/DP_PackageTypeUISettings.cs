using CommonDefinitions.UISettings;
using System;
using System.Collections.Generic;
namespace DataManager.DataPackage {
	public class DP_PackageTypeUISettings {


		public  DP_PackageTypeUISettings()
		{
		//	 ItemSetting = new List<DataManager.DataPackage.DataPackageUISetting.DPUI_ItemSetting>();
		}


		public Guid ID;


		public bool IsMainType;


        //public DataManager.DataPackage.DataPackageUISetting.DPUI_TypeSettings TypeUISetting;


		public int Priority;


		public IntracionMode IntractionMode;


		public DataMode DataMode;


        //public List<DataManager.DataPackage.DataPackageUISetting.DPUI_ItemSetting> ItemSetting;


        public bool TemporaryDisplayViewRequired { get; set; }
    }

}
