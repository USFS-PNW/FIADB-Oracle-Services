using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.EntityClient;
using Devart.Data.Oracle;
using FcsClassLibrary;
using System.Configuration;
using System.Windows.Forms;


namespace FIADB.Oracle
{
    public class Services
    {
        public int m_intError=0;
        public string m_strError = "";
        public Tree m_oTree = null;
        
        private EntityConnection _oEntityConnection = null;
        private OracleConnection _oOracleConnection = null;
        private FCS_Entities _oFCSEntities = null;
        public const int ERROR_MONITOR = -1;
        public const int ERROR_CONNECTION = -2;
        public const int ERROR_ENTIIES = -3;
        public const int ERROR_ADDRECORD = -4;
        public const int ERROR_GETVOLUMES = -5;
        public const int ERROR_SQLQUERY = -6;
        public EntityConnection EntityConnection
        {
            get { return _oEntityConnection; }
        }
        public OracleConnection OracleConnection
        {
            get { return _oOracleConnection; }
        }
        public FCS_Entities FCSEntities
        {
            get { return _oFCSEntities; }
            set { _oFCSEntities = value; }
        }
        
        
        public Services()
        {
            try
            {
                m_intError = 0; m_strError = "";
                // Turn on OracleMonitor to view executed DDL & DML statements on the server (ensure dbMonitor is running FIRST).
                //OracleMonitor monitor = new OracleMonitor() { IsActive = true };
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
               // MessageBox.Show("start");
                m_intError = 0; m_strError = "";
                //for (int x = 0; x <= ConfigurationManager.ConnectionStrings.Count - 1; x++)
                //{
                    // MessageBox.Show(ConfigurationManager.ConnectionStrings[x].ConnectionString);
                //}
                _oEntityConnection = new EntityConnection(ConfigurationManager.ConnectionStrings["FCS_Entities_ConnectionString"].ConnectionString);
                
                //string strConn = "metadata=res://*/FcsDataModel.csdl|res://*/FcsDataModel.ssdl|res://*/FcsDataModel.msl;provider=Devart.Data.Oracle;provider connection string=';User Id=FCS;Password=fcs;Server=localhost;Direct=True;Sid=XE'";
                //_oEntityConnection = new EntityConnection(strConn);
                                
                //MessageBox.Show("have entity connection");
                _oOracleConnection = new OracleConnection(_oEntityConnection.StoreConnection.ConnectionString);
               // MessageBox.Show("have oracle connection");
                InstantiateNewFCSEntity();
                SQLQuery("DELETE FROM biosum_volume");
                m_oTree = new Tree();
                m_oTree.ReferenceServices = this;
                //if (m_oTree == null) MessageBox.Show("m_oTree instantiation failed");
            }
            catch (Exception err)
            {
                MessageBox.Show("!!error!! FIADBOracleServices.Start " + err.Message);
                m_intError = ERROR_CONNECTION;
                m_strError = "Oracle Services Failed To Start\r\n-------------------------\r\n" + err.Message;
            }

        }
        public void InstantiateNewFCSEntity()
        {
            try
            {
                //MessageBox.Show("instantiate fcs entity");
                _oFCSEntities = new FCS_Entities();
                //if (_oFCSEntities == null) MessageBox.Show("_oFVSEntities never initialized");
               
            }
            catch (Exception err)
            {
                MessageBox.Show("!!error!! FIADBOracleServices.InstatiateNewFCSEntity " + err.Message);
                m_intError = ERROR_ENTIIES;
                m_strError = err.Message;
            }
        }
        private void SQLQuery(string p_strSql)
        {
            try
            {
                //MessageBox.Show("execute _oFCSEntities.Database.ExecuteSqlCommand(" + p_strSql + ")");
                _oFCSEntities.Database.ExecuteSqlCommand(p_strSql);
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
            private FIADB.Oracle.Services _oServices=null;
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
            public FIADB.Oracle.Services ReferenceServices
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
                    _aBiosumTreeRecord.VOL_LOC_GRP = p_oInputRecord.Vol_Loc_Grp;
                    ReferenceServices.FCSEntities.BIOSUM_VOLUMEs.Add(_aBiosumTreeRecord);
                 

                                      
                    _BiosumTreeInputRecord_Collection.Add(p_oInputRecord);
                }
                catch (Exception err)
                {
                    MessageBox.Show("!!ERROR!! FIADBOracle.Services.Tree.AddBiosumRecord " + err.Message);
                    ReferenceServices.m_intError = FIADB.Oracle.Services.ERROR_ADDRECORD;
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

                        ReferenceServices.FCSEntities.SaveChanges();
                        ReferenceServices.FCSEntities.Dispose();
                        ReferenceServices.FCSEntities = null;

                        ReferenceServices.InstantiateNewFCSEntity();
                        ReferenceServices.FCSEntities.COMP_BIOSUM_VOLS_BY_CURSOR();
                        //populate the collection with volume data
                        for (x = 0; x <= BiosumTreeInputRecordCollection.Count - 1; x++)
                        {


                            var TreeRecord = ReferenceServices.FCSEntities.BIOSUM_VOLUMEs.Find(BiosumTreeInputRecordCollection.Item(x).TRE_CN);
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

                    ReferenceServices.FCSEntities.Dispose();
                    ReferenceServices.FCSEntities = null;
                     
                }
                catch (Exception err)
                {
                    ReferenceServices.m_intError = FIADB.Oracle.Services.ERROR_GETVOLUMES;
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