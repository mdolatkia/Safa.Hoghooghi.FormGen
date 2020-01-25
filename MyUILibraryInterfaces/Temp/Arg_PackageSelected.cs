using ModelEntites;
using MyUILibrary.EntityArea;
using System;
using System.Collections.Generic;
namespace MyUILibrary
{ 
	public class Arg_PackageSelected : EventArgs
 {


		public  Arg_PackageSelected()
		{
			 Packages = new List<TableDrivedEntityDTO>();
		}


		public List<TableDrivedEntityDTO> Packages;


		//public I_EditEntityArea SourceEditEntityArea;

	}

}
