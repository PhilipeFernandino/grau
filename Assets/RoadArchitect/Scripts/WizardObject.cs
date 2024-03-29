﻿using UnityEngine;


namespace RoadArchitect
{
    public class WizardObject
    {
        public Texture2D thumb;
        public string thumbString;
        public string displayName;
        public string desc;
        public bool isDefault;
        public bool isBridge;
        public string fileName;
        public string FullPath;
        public int sortID = 0;


        public string ConvertToString()
        {
            WizardObjectLibrary WOL = new WizardObjectLibrary();
            WOL.LoadFrom(this);
            return RootUtils.GetString<WizardObjectLibrary>(WOL);
        }


        public void LoadDataFromWOL(WizardObjectLibrary _wizardObjLib)
        {
            thumbString = _wizardObjLib.thumbString;
            displayName = _wizardObjLib.displayName;
            desc = _wizardObjLib.desc;
            isDefault = _wizardObjLib.isDefault;
            fileName = _wizardObjLib.fileName;
            isBridge = _wizardObjLib.isBridge;
        }


        public static WizardObject LoadFromLibrary(string _path)
        {
            string tData = System.IO.File.ReadAllText(_path);
            string[] tSep = new string[2];
            tSep[0] = RoadUtility.FileSepString;
            tSep[1] = RoadUtility.FileSepStringCRLF;
            string[] tSplit = tData.Split(tSep, System.StringSplitOptions.RemoveEmptyEntries);
            int tSplitCount = tSplit.Length;
            WizardObjectLibrary WOL = null;
            for (int i = 0; i < tSplitCount; i++)
            {
                WOL = WizardObject.WizardObjectLibrary.WOLFromData(tSplit[i]);
                if (WOL != null)
                {
                    WizardObject WO = new WizardObject();
                    WO.LoadDataFromWOL(WOL);
                    return WO;
                }
            }
            return null;
        }


        [System.Serializable]
        public class WizardObjectLibrary
        {
            public string thumbString;
            public string displayName;
            public string desc;
            public bool isDefault;
            public bool isBridge;
            public string fileName;


            public void LoadFrom(WizardObject _wizardObj)
            {
                thumbString = _wizardObj.thumbString;
                displayName = _wizardObj.displayName;
                desc = _wizardObj.desc;
                isDefault = _wizardObj.isDefault;
                fileName = _wizardObj.fileName;
                isBridge = _wizardObj.isBridge;
            }


            public static WizardObjectLibrary WOLFromData(string _data)
            {
                try
                {
                    WizardObjectLibrary WOL = RootUtils.LoadData<WizardObjectLibrary>(ref _data);
                    return WOL;
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
