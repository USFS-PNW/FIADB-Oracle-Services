using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.EntityClient;
using FcsClassLibrary;
using System.Configuration;
using System.Windows.Forms;
using Oracle.ADO;
using System.Data.OracleClient;
//using ODP;
//using Oracle.DataAccess;
//using Oracle.DataAccess.Client;



namespace FIADBOracle
{
    public class Services
    {
        public int m_intError=0;
        public string m_strError = "";
        public Tree m_oTree = null;
        
        public const int ERROR_MONITOR = -1;
        public const int ERROR_CONNECTION = -2;
        public const int ERROR_ENTIIES = -3;
        public const int ERROR_ADDRECORD = -4;
        public const int ERROR_GETVOLUMES = -5;
        public const int ERROR_SQLQUERY = -6;

        public static bool FS_NETWORK_AVAILABLE { get; set; }

        private FCSEntities _FCSEntities;
        public FCSEntities FCSEntities
        {
            get { return _FCSEntities; }
            set { _FCSEntities = value; }
        }
        
     
        
        public Services()
        {
            try
            {
                m_intError = 0; m_strError = "";
                
            }
            catch (Exception e)
            {
                m_intError = ERROR_MONITOR;
                m_strError = "Oracle Services Error\r\n-------------------------\r\n" + e.Message;
            }
           

        }
        public void Start()
        {
            try
            {
              
                m_intError = 0; m_strError = "";
                _FCSEntities = new FcsClassLibrary.FCSEntities();
                if (!FS_NETWORK_AVAILABLE) _FCSEntities.DeleteAllRecords_BIOSUM_VOLUME();
                m_oTree = new Tree();
                m_oTree.ReferenceServices = this;
            }
            catch (Exception err)
            {
                MessageBox.Show("!!error!! FIADBOracleServices.Start " + err.Message);
                m_intError = ERROR_CONNECTION;
                m_strError = "Oracle Services Failed To Start\r\n-------------------------\r\n" + err.Message;
            }

        }
        
        public void SQLQuery(string p_strSql)
        {
            try
            {
                _FCSEntities.SQLNonQuery(p_strSql);
            }
            catch (Exception err)
            {
                MessageBox.Show("!!error!! FIADBOracleServices.SQLQuery " + err.Message);
                m_intError = ERROR_SQLQUERY;
                m_strError = "Oracle Services SQL Query Error\r\n-----------------------------------\r\n" + err.Message;
            }

        }
        public class Tree
        {

            private BIOSUM_VOLUME _aBiosumTreeRecord=null;
            //private BIOSUM_VOLUME _aBiosumTreeRecord = new BIOSUM_VOLUME();
            //private FIADBOracle.Services _oServices=null;
            private FIADBOracle.Services _oServices = null;
            private BiosumTreeInputRecord _BiosumTreeInputRecord = null;
            private BiosumTreeInputRecord_Collection _BiosumTreeInputRecord_Collection = null;

            public BIOSUM_VOLUME BiosumTreeRecord
            {
                get { return _aBiosumTreeRecord; }
                set { _aBiosumTreeRecord = value; }
            }
            public BiosumTreeInputRecord BiosumTreeInputSingleRecord
            {
                get { return _BiosumTreeInputRecord; }
                set { _BiosumTreeInputRecord = value; }
            }
            public BiosumTreeInputRecord_Collection BiosumTreeInputRecordCollection
            {
                get { return _BiosumTreeInputRecord_Collection; }
                set { _BiosumTreeInputRecord_Collection = value; }
            }
            public FIADBOracle.Services ReferenceServices
            {
                get { return _oServices; }
                set { _oServices = value; }
            }
            public enum GetVolumesModeValues
            {
                InsertRowTrigger,
                SQLUpdate,
                CursorRowUpdate,
            }
            private GetVolumesModeValues _oGetVolumesMode = GetVolumesModeValues.SQLUpdate;
            public GetVolumesModeValues GetVolumesMode
            {
                get { return _oGetVolumesMode; }
                set { _oGetVolumesMode = value; }
            }
            public Tree()
            {
               // MessageBox.Show("tree constructor");
            }
            public void InstantiateNewBiosumTreeInputRecord()
            {
                _BiosumTreeInputRecord = new BiosumTreeInputRecord();
                if (_BiosumTreeInputRecord_Collection == null) _BiosumTreeInputRecord_Collection = new BiosumTreeInputRecord_Collection();
            }
            public void InstantiateNewBiosumTreeInputRecordCollection()
            {
                _BiosumTreeInputRecord_Collection = new BiosumTreeInputRecord_Collection();
            }
           

           
             
            public void AddBiosumRecord(BiosumTreeInputRecord p_oInputRecord)
            {
                try
                {
                    
                       

                    _aBiosumTreeRecord = new BIOSUM_VOLUME();
                    _aBiosumTreeRecord.TRE_CN = p_oInputRecord.TRE_CN;
                    _aBiosumTreeRecord.PLT_CN = p_oInputRecord.PLT_CN;
                    _aBiosumTreeRecord.CND_CN = p_oInputRecord.CND_CN;
                    _aBiosumTreeRecord.STATECD = p_oInputRecord.StateCd;
                    _aBiosumTreeRecord.COUNTYCD = p_oInputRecord.CountyCd;
                    _aBiosumTreeRecord.PLOT = p_oInputRecord.Plot;
                    _aBiosumTreeRecord.INVYR = p_oInputRecord.InvYr;
                    _aBiosumTreeRecord.TREE = p_oInputRecord.Tree;
                    _aBiosumTreeRecord.SPCD = p_oInputRecord.SpCd;
                    _aBiosumTreeRecord.DIA = p_oInputRecord.DBH;
                    _aBiosumTreeRecord.HT = p_oInputRecord.Ht;
                    _aBiosumTreeRecord.ACTUALHT = p_oInputRecord.ActualHt;
                    _aBiosumTreeRecord.CULL = p_oInputRecord.Cull;
                    _aBiosumTreeRecord.ROUGHCULL = p_oInputRecord.RoughCull;
                    _aBiosumTreeRecord.STATUSCD = (short)p_oInputRecord.StatusCd;
                    _aBiosumTreeRecord.TREECLCD = p_oInputRecord.TreeClCd;
                    _aBiosumTreeRecord.CR = p_oInputRecord.CR;
                    _aBiosumTreeRecord.DECAYCD = p_oInputRecord.DecayCd;
                    _aBiosumTreeRecord.TOTAGE = p_oInputRecord.TotalAge;
                    _aBiosumTreeRecord.VOL_LOC_GRP = p_oInputRecord.Vol_Loc_Grp;
                    //START: ADDED BIOSUM_VOLUME COLUMNS
                    _aBiosumTreeRecord.SITREE = p_oInputRecord.SiTree;
                    _aBiosumTreeRecord.WDLDSTEM = p_oInputRecord.WoodlandStem;
                    _aBiosumTreeRecord.UPPER_DIA = p_oInputRecord.UpperDia;
                    _aBiosumTreeRecord.UPPER_DIA_HT = p_oInputRecord.UpperDiaHt;
                    _aBiosumTreeRecord.CENTROID_DIA = p_oInputRecord.CentroidDia;
                    _aBiosumTreeRecord.CENTROID_DIA_HT_ACTUAL = p_oInputRecord.CentroidDiaHtActual;
                    _aBiosumTreeRecord.SAWHT = p_oInputRecord.SawHt;
                    _aBiosumTreeRecord.HTDMP = p_oInputRecord.HtDmp;
                    _aBiosumTreeRecord.BOLEHT = p_oInputRecord.BoleHt;
                    _aBiosumTreeRecord.CULLCF = p_oInputRecord.CullCf;
                    _aBiosumTreeRecord.CULL_FLD = p_oInputRecord.CullFld;
                    _aBiosumTreeRecord.CULLDEAD = p_oInputRecord.CullDead;
                    _aBiosumTreeRecord.CULLFORM = p_oInputRecord.CullForm;
                    _aBiosumTreeRecord.CULLMSTOP = p_oInputRecord.CullMStop;
                    _aBiosumTreeRecord.CFSND = p_oInputRecord.CfSnd;
                    _aBiosumTreeRecord.BFSND = p_oInputRecord.BfSnd;
                    _aBiosumTreeRecord.PRECIPITATION = p_oInputRecord.Precipitation;
                    _aBiosumTreeRecord.BALIVE = p_oInputRecord.BaLive;
                    _aBiosumTreeRecord.DIAHTCD = p_oInputRecord.DiaHtCd;
                    _aBiosumTreeRecord.STANDING_DEAD_CD = p_oInputRecord.StandingDeadCd;
                    _aBiosumTreeRecord.VOLCFSND_CALC = p_oInputRecord.VolcfsndCalc;
                    _aBiosumTreeRecord.DRYBIO_BOLE_CALC = p_oInputRecord.DrybioBoleCalc;
                    _aBiosumTreeRecord.DRYBIO_TOP_CALC = p_oInputRecord.DrybioTopCalc;
                    _aBiosumTreeRecord.DRYBIO_SAPLING_CALC = p_oInputRecord.DrybioSaplingCalc;
                    _aBiosumTreeRecord.DRYBIO_WDLD_SPP_CALC = p_oInputRecord.DrybioWdldSppCalc;
                    _aBiosumTreeRecord.CULLBF = p_oInputRecord.CullBf;
                    _aBiosumTreeRecord.SUBP = p_oInputRecord.Subp;
                    _aBiosumTreeRecord.FORMCL = p_oInputRecord.FormCl;
                    //END: ADDED BIOSUM_VOLUME COLUMNS

                    if (ReferenceServices.FCSEntities.BIOSUM_VOLUME_LIST == null) ReferenceServices.FCSEntities.BIOSUM_VOLUME_LIST = new List<BIOSUM_VOLUME>();
                    ReferenceServices.FCSEntities.BIOSUM_VOLUME_LIST.Add(_aBiosumTreeRecord);
                 

                                      
                    _BiosumTreeInputRecord_Collection.Add(p_oInputRecord);
                }
                catch (Exception err)
                {
                    MessageBox.Show("!!ERROR!! FIADBOracle.Services.Tree.AddBiosumRecord " + err.Message);
                    ReferenceServices.m_intError = FIADBOracle.Services.ERROR_ADDRECORD;
                    ReferenceServices.m_strError = "Oracle Services Add Biosum Record Error\r\n--------------------------------------\r\n" + err.Message;
                }


            }
            public void GetBiosumVolumes()
            {
               
                int x;
                try
                {

                    if (_oGetVolumesMode == GetVolumesModeValues.InsertRowTrigger)
                    {

                        ReferenceServices.FCSEntities.InsertInto_BIOSUM_VOLUMEStable
                            (ReferenceServices.FCSEntities.BIOSUM_VOLUME_LIST);

                        if (FIADBOracle.Services.FS_NETWORK_AVAILABLE == false)
                            ReferenceServices.FCSEntities.COMP_BIOSUM_VOLS_BY_CURSOR();

                        if (ReferenceServices.FCSEntities.BIOSUM_VOLUME_LIST == null)
                            ReferenceServices.FCSEntities.BIOSUM_VOLUME_LIST = new List<BIOSUM_VOLUME>();

                        ReferenceServices.FCSEntities.BIOSUM_VOLUME_LIST =
                            ReferenceServices.FCSEntities.ExecuteSelectSQL_CreateBIOSUM_VOLUMEList("SELECT * FROM " + FCSOracle.FCSSchema + ".BIOSUM_VOLUME");

                        //populate the collection with volume data
                        for (x = 0; x <= BiosumTreeInputRecordCollection.Count - 1; x++)
                        {


                            var TreeRecord = ReferenceServices.FCSEntities.BIOSUM_VOLUME_LIST.Where(a=>a.TRE_CN == BiosumTreeInputRecordCollection.Item(x).TRE_CN).Select(a=>a).FirstOrDefault();
                            if (TreeRecord != null)
                            {
                                if (TreeRecord.VOLCFGRS_CALC != null)
                                    BiosumTreeInputRecordCollection.Item(x).VOLCFGRS = (double)TreeRecord.VOLCFGRS_CALC;
                                else
                                    BiosumTreeInputRecordCollection.Item(x).VOLCFGRS = -1;

                                if (TreeRecord.VOLCSGRS_CALC != null)
                                    BiosumTreeInputRecordCollection.Item(x).VOLCSGRS = (double)TreeRecord.VOLCSGRS_CALC;
                                else
                                    BiosumTreeInputRecordCollection.Item(x).VOLCSGRS = -1;

                                if (TreeRecord.VOLCFNET_CALC != null)
                                    BiosumTreeInputRecordCollection.Item(x).VOLCFNET = (double)TreeRecord.VOLCFNET_CALC;
                                else
                                    BiosumTreeInputRecordCollection.Item(x).VOLCFNET = -1;

                                if (TreeRecord.DRYBIOT_CALC != null)
                                    BiosumTreeInputRecordCollection.Item(x).DRYBIOT = (double)TreeRecord.DRYBIOT_CALC;
                                else
                                    BiosumTreeInputRecordCollection.Item(x).DRYBIOT = -1;

                                if (TreeRecord.DRYBIOM_CALC != null)
                                    BiosumTreeInputRecordCollection.Item(x).DRYBIOM = (double)TreeRecord.DRYBIOM_CALC;
                                else
                                    BiosumTreeInputRecordCollection.Item(x).DRYBIOM = -1;


                                if (TreeRecord.VOLTSGRS_CALC != null)
                                    BiosumTreeInputRecordCollection.Item(x).VOLTSGRS = (double)TreeRecord.VOLTSGRS_CALC;
                                else
                                    BiosumTreeInputRecordCollection.Item(x).VOLTSGRS = -1;

                            }


                        }
                    }
                    else if (_oGetVolumesMode == GetVolumesModeValues.SQLUpdate)
                    {
                        ReferenceServices.FCSEntities.COMP_BIOSUM_VOLS_BY_UPDATE();
                    }
                     
                }
                catch (Exception err)
                {
                    ReferenceServices.m_intError = FIADBOracle.Services.ERROR_GETVOLUMES;
                    ReferenceServices.m_strError = "Oracle Compilation Error\r\n-----------------------------\r\n" + err.Message;
                }
               


               
            }

            public class BiosumTreeInputRecord
            {
                private int _intRecordId = -1;
                public int RecordId
                {
                    get { return _intRecordId; }
                    set { _intRecordId = value; }
                }
                private string _strTRE_CN = "";
                public string TRE_CN
                {
                    get { return _strTRE_CN; }
                    set { _strTRE_CN = value; }
                }
                private string _strPLT_CN = "";
                public string PLT_CN
                {
                    get { return _strPLT_CN; }
                    set { _strPLT_CN = value; }
                }
                private string _strCND_CN = "";
                public string CND_CN
                {
                    get { return _strCND_CN; }
                    set { _strCND_CN = value; }
                }
                private int _intStateCd=-1;
                public int StateCd
                {
                    get { return _intStateCd; }
                    set { _intStateCd = value; }
                }
                private int _intCountyCd=-1;
                public int CountyCd
                {
                    get { return _intCountyCd; }
                    set { _intCountyCd = value; }
                }
                private int _intPlot=-1;
                public int Plot
                {
                    get { return _intPlot; }
                    set { _intPlot = value; }
                }
                private int _intInvYr=-1;
                public int InvYr
                {
                    get { return _intInvYr; }
                    set { _intInvYr = value; }
                }
                private int _intTree = -1;
                public int Tree
                {
                    get { return _intTree; }
                    set { _intTree = value; }
                }
                private string _strVolLocGrp = "";
                public string Vol_Loc_Grp
                {
                    get { return _strVolLocGrp; }
                    set { _strVolLocGrp = value; }
                }
                private int _intSpCd = -1;
                public int SpCd
                {
                    get { return _intSpCd; }
                    set { _intSpCd = value; }
                }

                private double _dblDBH = -1;
                public double DBH
                {
                    get { return _dblDBH; }
                    set { _dblDBH = value; }
                }
                private int _intHt = -1;
                public int Ht
                {
                    get { return _intHt; }
                    set { _intHt = value; }
                }
                private int _intActualHt = -1;
                public int ActualHt
                {
                    get { return _intActualHt; }
                    set { _intActualHt = value; }
                }
                private int _intCR = -1;
                public int CR
                {
                    get { return _intCR; }
                    set { _intCR = value; }
                }
                private int _intTreeClCd = -1;
                public int TreeClCd
                {
                    get { return _intTreeClCd; }
                    set { _intTreeClCd = value; }
                }
                private int _intCull=0;
                public int Cull
                {
                    get { return _intCull; }
                    set { _intCull = value; }
                }
                private int _intRoughCull = 0;
                public int RoughCull
                {
                    get { return _intRoughCull; }
                    set { _intRoughCull = value; }
                }
                private int _intStatusCd = 0;
                public int StatusCd
                {
                    get { return _intStatusCd; }
                    set { _intStatusCd = value; }
                }
                private int _intDecayCd = -1;
                public int DecayCd
                {
                    get { return _intDecayCd; }
                    set { _intDecayCd = value; }
                }
                private double? _dblTotalAge = -1;
                public double? TotalAge
                {
                    get { return _dblTotalAge; }
                    set { _dblTotalAge = value; }
                }
                //START: ADDED BIOSUM_VOLUME COLUMNS
                private int? _intSiTree;
                public int? SiTree
                {
                    get { return _intSiTree; }
                    set { _intSiTree = value; }
                }
                private int? _intWoodlandStem;
                public int? WoodlandStem
                {
                    get { return _intWoodlandStem; }
                    set { _intWoodlandStem = value; }
                }
                private double? _dblUpperDia;
                public double? UpperDia
                {
                    get { return _dblUpperDia; }
                    set { _dblUpperDia = value; }
                }
                private double? _dblUpperDiaHt;
                public double? UpperDiaHt
                {
                    get { return _dblUpperDiaHt; }
                    set { _dblUpperDiaHt = value; }
                }
                private double? _dblCentroidDia;
                public double? CentroidDia
                {
                    get { return _dblCentroidDia; }
                    set { _dblCentroidDia = value; }
                }
                private double? _dblCentroidDiaHtActual;
                public double? CentroidDiaHtActual
                {
                    get { return _dblCentroidDiaHtActual; }
                    set { _dblCentroidDiaHtActual = value; }
                }
                private int? _intSawHt;
                public int? SawHt
                {
                    get { return _intSawHt; }
                    set { _intSawHt = value; }
                }
                private double? _dblHtDmp;
                public double? HtDmp
                {
                    get { return _dblHtDmp; }
                    set { _dblHtDmp = value; }
                }
                private int? _intBoleHt;
                public int? BoleHt
                {
                    get { return _intBoleHt; }
                    set { _intBoleHt = value; }
                }
                private int? _intCullCf;
                public int? CullCf
                {
                    get { return _intCullCf; }
                    set { _intCullCf = value; }
                }
                private int? _intCullFld;
                public int? CullFld
                {
                    get { return _intCullFld; }
                    set { _intCullFld = value; }
                }
                private int? _intCullDead;
                public int? CullDead
                {
                    get { return _intCullDead; }
                    set { _intCullDead = value; }
                }
                private int? _intCullForm;
                public int? CullForm
                {
                    get { return _intCullForm; }
                    set { _intCullForm = value; }
                }
                private int? _intCullMStop;
                public int? CullMStop
                {
                    get { return _intCullMStop; }
                    set { _intCullMStop = value; }
                }
                private int? _intCfSnd;
                public int? CfSnd
                {
                    get { return _intCfSnd; }
                    set { _intCfSnd = value; }
                }
                private int? _intBfSnd;
                public int? BfSnd
                {
                    get { return _intBfSnd; }
                    set { _intBfSnd = value; }
                }
                private double? _dblPrecipitation;
                public double? Precipitation
                {
                    get { return _dblPrecipitation; }
                    set { _dblPrecipitation = value; }
                }
                private double? _dblBaLive;
                public double? BaLive
                {
                    get { return _dblBaLive; }
                    set { _dblBaLive = value; }
                }

                public int? DiaHtCd { get; set; }
                public int? StandingDeadCd { get; set; }
                public double? VolcfsndCalc { get; set; }
                public double? DrybioBoleCalc { get; set; }
                public double? DrybioTopCalc { get; set; }
                public double? DrybioSaplingCalc { get; set; }
                public double? DrybioWdldSppCalc { get; set; }
                public int? CullBf { get; set; }
                public byte? Subp { get; set; }
                public byte? FormCl { get; set; }

                //END: ADDED BIOSUM_VOLUME COLUMNS

                private double _dblVOLCFGRS = 0;
                public double VOLCFGRS
                {
                    get { return _dblVOLCFGRS; }
                    set { _dblVOLCFGRS = value; }
                }
                private double _dblVOLCSGRS = 0;
                public double VOLCSGRS
                {
                    get { return _dblVOLCSGRS; }
                    set { _dblVOLCSGRS = value; }
                }
                private double _dblVOLCFNET = 0;
                public double VOLCFNET
                {
                    get { return _dblVOLCFNET; }
                    set { _dblVOLCFNET = value; }
                }
                private double _dblDRYBIOT = 0;
                public double DRYBIOT
                {
                    get { return _dblDRYBIOT; }
                    set { _dblDRYBIOT = value; }
                }
                private double _dblDRYBIOM = 0;
                public double DRYBIOM
                {
                    get { return _dblDRYBIOM; }
                    set { _dblDRYBIOM = value; }
                }
                private double _dblVOLTSGRS = 0;
                public double VOLTSGRS
                {
                    get { return _dblVOLTSGRS; }
                    set { _dblVOLTSGRS = value; }
                }
            }
            public void RemoveAllBiomassInputRecordsFromCollection()
            {
                for (int x = _BiosumTreeInputRecord_Collection.Count - 1; x >= 0 ; x--)
                    _BiosumTreeInputRecord_Collection.Remove(x);
            }
            public class BiosumTreeInputRecord_Collection : System.Collections.CollectionBase
            {
                public int m_intError = 0;
                public string m_strError = "";
                public BiosumTreeInputRecord_Collection()
                {
                    //
                    // TODO: Add constructor logic here
                    //
                }

                public void Add(BiosumTreeInputRecord m_oBiosumTreeInputRecord)
                {
                    // vérify if object is not already in
                    if (this.List.Contains(m_oBiosumTreeInputRecord))
                        throw new InvalidOperationException();

                    // adding it
                    this.List.Add(m_oBiosumTreeInputRecord);

                    // return collection
                    //return this;
                }
                public void Remove(int index)
                {
                    m_intError = 0;
                    m_strError = "";
                    // Check to see if there is a widget at the supplied index.
                    if (index > Count - 1 || index < 0)
                    // If no widget exists, a messagebox is shown and the operation 
                    // is canColumned.
                    {
                        //System.Windows.Forms.MessageBox.Show("Index not valid!");
                        m_intError = -1;
                        m_strError = "Index  " + index.ToString().Trim() + " not valid";
                    }
                    else
                    {
                        List.RemoveAt(index);
                    }
                }
                public BiosumTreeInputRecord Item(int Index)
                {
                    // The appropriate item is retrieved from the List object and
                    // explicitly cast to the Widget type, then returned to the 
                    // caller.
                    return (BiosumTreeInputRecord)List[Index];
                }

            }

        }

        
    }
    

}